using FFmpeg.AutoGen;
using SiMay.Basic;
using SiMay.Core;
using SiMay.Core.Standard;
using SiMay.ModelBinder;
using SiMay.Net.SessionProvider;
using SiMay.Platform.Windows;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SiMay.Service.Core
{
    [ApplicationName(ApplicationNameConstant.REMOTE_DESKTOP)]
    public sealed unsafe class RemoteDesktopService : ApplicationRemoteServiceBase
    {
        private AVCodec* _pCodec;
        private AVCodecContext* _pCodecContext;

        private IntPtr _convertedFrameBufferPtr;
        private byte_ptrArray4 _dstData;
        private int_array4 _dstLinesize;
        private SwsContext* _pConvertContext;

        private bool _systemAuthor = AppConfiguration.GetApplicationConfiguration<AppConfiguration>().StartParameter.SystemPermission;

        /// <summary>
        /// 依赖的dll
        /// </summary>
        private readonly string[] _denpendentDlls = new string[] {
            "avcodec-58.dll",
            "avdevice-58.dll",
            "avfilter-7.dll",
            "avformat-58.dll",
            "avutil-56.dll",
            "postproc-55.dll",
            "swresample-3.dll",
            "swscale-5.dll"
        };

        private bool _encodingCancel = false;
        private DesktopCapture _desktopCapture = new DesktopCapture();
        public override void SessionClosed()
        {
            //释放编码器
            ffmpeg.avcodec_close(_pCodecContext);
            ffmpeg.av_free(_pCodecContext);
            ffmpeg.av_free(_pCodec);

            //释放转换器
            Marshal.FreeHGlobal(_convertedFrameBufferPtr);
            ffmpeg.sws_freeContext(_pConvertContext);
        }

        public unsafe override void SessionInited(SessionProviderContext session)
        {
            ffmpeg.av_log_set_level(ffmpeg.AV_LOG_VERBOSE);

            av_log_set_callback_callback logCallback = (p0, level, format, vl) =>
            {
                if (level > ffmpeg.av_log_get_level()) return;

                var lineSize = 1024;
                var lineBuffer = stackalloc byte[lineSize];
                var printPrefix = 1;
                ffmpeg.av_log_format_line(p0, level, format, vl, lineBuffer, lineSize, &printPrefix);
                var line = Marshal.PtrToStringAnsi((IntPtr)lineBuffer);
                LogHelper.DebugWriteLog(line);
            };

            ffmpeg.av_log_set_callback(logCallback);
        }

        [PacketHandler(MessageHead.S_DESKTOP_INIT_INFO)]
        public DesktopInitializeInfoPacket GetInitializeInfor(SessionProviderContext session)
        {
            var currentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location.IsNullOrEmpty() ? Application.ExecutablePath : Assembly.GetExecutingAssembly().Location);
            return new DesktopInitializeInfoPacket()
            {
                DpiX = ScreenDpiHelper.ScaleX,
                DpiY = ScreenDpiHelper.ScaleY,
                Height = _desktopCapture.DesktopSize.Height,
                Width = _desktopCapture.DesktopSize.Width,
                PrimaryScreenIndex = _desktopCapture.ScreenIndex,
                Monitors = Screen.AllScreens.Select(c => new MonitorItem()
                {
                    DeviceName = c.DeviceName,
                    Primary = c.Primary
                }).ToArray(),
                DependentDlls = _denpendentDlls.Where(c => !File.Exists(Path.Combine(currentDirectory, c))).ToArray(),
                CurrentDirectory = currentDirectory
            };
        }

        [PacketHandler(MessageHead.S_DESKTOP_START_PUSH)]
        public unsafe void StartPushStream(SessionProviderContext session)
        {
            var arguments = session.GetMessageEntity<SetVideoArgumentsPacket>();
            var originPixelFormat = AVPixelFormat.AV_PIX_FMT_RGB24;
            var destinationPixelFormat = AVPixelFormat.AV_PIX_FMT_YUV420P;

            var codecId = AVCodecID.AV_CODEC_ID_H264;
            _pCodec = ffmpeg.avcodec_find_encoder(codecId);
            if (_pCodec == null)
                throw new InvalidOperationException("Codec not found.");

            LogHelper.DebugWriteLog($"desktop width:{_desktopCapture.DesktopSize.Width} && height:{_desktopCapture.DesktopSize.Height}");

            _pCodecContext = ffmpeg.avcodec_alloc_context3(_pCodec);
            _pCodecContext->width = _desktopCapture.DesktopSize.Width;
            _pCodecContext->height = _desktopCapture.DesktopSize.Height;
            _pCodecContext->time_base = new AVRational
            {
                num = 1,
                den = arguments.FPS
            };
            _pCodecContext->pix_fmt = AVPixelFormat.AV_PIX_FMT_YUV420P;
            foreach (var arg in arguments.Arguments)
            {
                var optionName = arg.Split('=')[0];
                var value = arg.Split('=')[1];
                ffmpeg.av_opt_set(_pCodecContext->priv_data, optionName, value, 0);
            }
            ffmpeg.avcodec_open2(_pCodecContext, _pCodec, null).ThrowExceptionIfError();

            LogHelper.DebugWriteLog("avcodec_open2 OK");

            _pConvertContext = ffmpeg.sws_getContext(_desktopCapture.DesktopSize.Width, _desktopCapture.DesktopSize.Height, originPixelFormat,
            _desktopCapture.DesktopSize.Width,
            _desktopCapture.DesktopSize.Height, destinationPixelFormat,
            ffmpeg.SWS_FAST_BILINEAR, null, null, null);
            if (_pConvertContext == null)
                throw new ApplicationException("Could not initialize the conversion context.");

            var convertedFrameBufferSize = ffmpeg.av_image_get_buffer_size(destinationPixelFormat, _desktopCapture.DesktopSize.Width, _desktopCapture.DesktopSize.Height, 1);
            _convertedFrameBufferPtr = Marshal.AllocHGlobal(convertedFrameBufferSize);
            _dstData = new byte_ptrArray4();
            _dstLinesize = new int_array4();

            ffmpeg.av_image_fill_arrays(ref _dstData, ref _dstLinesize, (byte*)_convertedFrameBufferPtr, destinationPixelFormat, _desktopCapture.DesktopSize.Width, _desktopCapture.DesktopSize.Height, 1);
            Task.Factory.StartNew(() =>
            {
                while (!_encodingCancel)
                {
                    if (_systemAuthor)
                        Win32Interop.SwitchToInputDesktop();

                    byte[] sourceBitmapData = default;
                    var waitEncoderBitmap = _desktopCapture.Capture();
                    var bitmapData = waitEncoderBitmap.LockBits(new Rectangle(Point.Empty, waitEncoderBitmap.Size), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                    try
                    {
                        var length = bitmapData.Stride * bitmapData.Height;
                        sourceBitmapData = new byte[length];
                        Marshal.Copy(bitmapData.Scan0, sourceBitmapData, 0, length);
                    }
                    finally
                    {
                        waitEncoderBitmap.UnlockBits(bitmapData);
                    }

                    fixed (byte* pBitmapData = sourceBitmapData)
                    {
                        var waittoYUVFrame = new AVFrame
                        {
                            data = new byte_ptrArray8 { [0] = pBitmapData },
                            linesize = new int_array8 { [0] = sourceBitmapData.Length / waitEncoderBitmap.Height },
                            height = waitEncoderBitmap.Height
                        };
                        this.internalEncodingPlushData(FramergbToYuv(waittoYUVFrame));
                    }
                }
            }, TaskCreationOptions.LongRunning);
        }

        private AVFrame FramergbToYuv(AVFrame waitConvertYUVFrame)
        {
            ffmpeg.sws_scale(_pConvertContext, waitConvertYUVFrame.data, waitConvertYUVFrame.linesize, 0, waitConvertYUVFrame.height, _dstData, _dstLinesize);

            var data = new byte_ptrArray8();
            data.UpdateFrom(_dstData);
            var linesize = new int_array8();
            linesize.UpdateFrom(_dstLinesize);

            ffmpeg.av_frame_unref(&waitConvertYUVFrame);

            return new AVFrame
            {
                data = data,
                linesize = linesize,
                width = _desktopCapture.DesktopSize.Width,
                height = _desktopCapture.DesktopSize.Height
            };
        }

        private void internalEncodingPlushData(AVFrame convertedFrame)
        {
            var pPacket = ffmpeg.av_packet_alloc();
            try
            {
                int error;
                do
                {
                    ffmpeg.avcodec_send_frame(_pCodecContext, &convertedFrame).ThrowExceptionIfError();
                    error = ffmpeg.avcodec_receive_packet(_pCodecContext, pPacket);
                } while (error == ffmpeg.AVERROR(ffmpeg.EAGAIN));

                error.ThrowExceptionIfError();

                byte[] buffer = new byte[pPacket->size];
                Marshal.Copy(new IntPtr(pPacket->data), buffer, 0, pPacket->size);
                CurrentSession.SendTo(MessageHead.C_DESKTOP_STREAM, buffer);
            }
            finally
            {
                ffmpeg.av_frame_unref(&convertedFrame);
                ffmpeg.av_packet_unref(pPacket);
            }
        }
    }
}
