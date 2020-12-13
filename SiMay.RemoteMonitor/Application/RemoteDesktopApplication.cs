using FFmpeg.AutoGen;
using SiMay.Basic;
using SiMay.Platform.Windows;
using SiMay.RemoteControls.Core;
using SiMay.RemoteMonitor.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SiMay.RemoteMonitor.Application
{
    [OnTools]
    [Rank(5)]
    [ApplicationName("远程桌面")]
    [AppResourceName("ScreenManager")]
    public partial class RemoteDesktopApplication : Form, IApplication
    {
        private IntPtr _convertedFrameBufferPtr;
        private byte_ptrArray4 _dstData;
        private int_array4 _dstLinesize;
        private unsafe SwsContext* _pConvertContext;

        private unsafe AVCodecContext* _pDecodecContext;
        private Size _destinationSize;
        private unsafe AVCodec* _pDecodec;
        private int _fps = 25;

        private string[] _arguments = new string[]
        {
            "preset=veryfast",
            "tune=zerolatency"
        };

        //private Graphics _deskGraphics;

        [ApplicationAdapterHandler]
        public RemoteDesktopAdapterHandler RemoteDesktopAdapterHandler { get; set; }

        public RemoteDesktopApplication()
        {
            InitializeComponent();
        }

        public void ContinueTask(ApplicationBaseAdapterHandler handler)
        {
        }

        public void SessionClose(ApplicationBaseAdapterHandler handler)
        {
        }

        public async void Start()
        {
            var initializeInfo = await RemoteDesktopAdapterHandler.GetInitializeInfor();
            if (!initializeInfo.IsNull() && initializeInfo.DependentDlls.Length > 0)
            {
                await BasicApplication.OpenFileTransportApplication(RemoteDesktopAdapterHandler.CurrentSession, this, "插件传输【请不要关闭窗口】", initializeInfo.CurrentDirectory, initializeInfo.DependentDlls.Select(c => Path.Combine("ffmpeg", c)).ToArray());
                this.Close();
            }
            else
            {
                var denpentDllDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ffmpeg");
                CommonWin32Api.SetDllDirectory(denpentDllDirectory);

                this.Show();
                unsafe
                {
                    var originPixelFormat = AVPixelFormat.AV_PIX_FMT_YUV420P;
                    var destinationPixelFormat = AVPixelFormat.AV_PIX_FMT_RGB24;

                    var codecId = AVCodecID.AV_CODEC_ID_H264;
                    _pDecodec = ffmpeg.avcodec_find_decoder(codecId);
                    if (_pDecodec == null) throw new InvalidOperationException("Codec not found.");

                    _pDecodecContext = ffmpeg.avcodec_alloc_context3(_pDecodec);
                    _pDecodecContext->width = initializeInfo.Width;
                    _pDecodecContext->height = initializeInfo.Height;
                    _pDecodecContext->time_base = new AVRational { num = 1, den = _fps };
                    _pDecodecContext->pix_fmt = AVPixelFormat.AV_PIX_FMT_YUV420P;
                    foreach (var arg in _arguments)
                    {
                        var optionName = arg.Split('=')[0];
                        var value = arg.Split('=')[1];
                        ffmpeg.av_opt_set(_pDecodecContext->priv_data, optionName, value, 0);
                    }
                    ffmpeg.avcodec_open2(_pDecodecContext, _pDecodec, null).ThrowExceptionIfError();

                    _destinationSize = new Size(initializeInfo.Width, initializeInfo.Height);
                    _pConvertContext = ffmpeg.sws_getContext(initializeInfo.Width, initializeInfo.Height, originPixelFormat,
                            initializeInfo.Width,
                            initializeInfo.Height, destinationPixelFormat,
                            ffmpeg.SWS_FAST_BILINEAR, null, null, null);
                    if (_pConvertContext == null)
                        throw new ApplicationException("Could not initialize the conversion context.");

                    var convertedFrameBufferSize = ffmpeg.av_image_get_buffer_size(destinationPixelFormat, initializeInfo.Width, initializeInfo.Height, 1);
                    _convertedFrameBufferPtr = Marshal.AllocHGlobal(convertedFrameBufferSize);
                    _dstData = new byte_ptrArray4();
                    _dstLinesize = new int_array4();

                    ffmpeg.av_image_fill_arrays(ref _dstData, ref _dstLinesize, (byte*)_convertedFrameBufferPtr, destinationPixelFormat, initializeInfo.Width, initializeInfo.Height, 1);
                }
                this.RemoteDesktopAdapterHandler.VideoStreamEventHandler += RemoteDesktopAdapterHandler_VideoStreamEventHandler;
            }
        }

        private void RemoteDesktopAdapterHandler_VideoStreamEventHandler(byte[] packetData)
        {
            unsafe
            {
                fixed (byte* waitDecodeDataIntprt = packetData)
                {
                    var waitDecodePacket = ffmpeg.av_packet_alloc();
                    waitDecodePacket->data = waitDecodeDataIntprt;
                    waitDecodePacket->size = packetData.Length;

                    var waitDecoderFrame = ffmpeg.av_frame_alloc();
                    int error;
                    do
                    {
                        ffmpeg.avcodec_send_packet(_pDecodecContext, waitDecodePacket);

                        error = ffmpeg.avcodec_receive_frame(_pDecodecContext, waitDecoderFrame);
                    } while (error == ffmpeg.AVERROR(ffmpeg.EAGAIN));
                    ffmpeg.av_packet_unref(waitDecodePacket);

                    var decodeAfterFrame = FrameyuvToRgb(waitDecoderFrame);
                    var bitmap = new Bitmap(_destinationSize.Width, _destinationSize.Height, decodeAfterFrame.linesize[0], PixelFormat.Format24bppRgb, (IntPtr)decodeAfterFrame.data[0]);
                    this.desktopBox.Image = bitmap;
                }
            }
        }

        private unsafe AVFrame FrameyuvToRgb(AVFrame* waitDecoderFrame)
        {
            ffmpeg.sws_scale(_pConvertContext, waitDecoderFrame->data, waitDecoderFrame->linesize, 0, waitDecoderFrame->height, _dstData, _dstLinesize);
            var decodeAfterDatas = new byte_ptrArray8();
            decodeAfterDatas.UpdateFrom(_dstData);
            var linesize = new int_array8();
            linesize.UpdateFrom(_dstLinesize);

            ffmpeg.av_frame_unref(waitDecoderFrame);

            return new AVFrame
            {
                data = decodeAfterDatas,
                linesize = linesize,
                width = _destinationSize.Width,
                height = _destinationSize.Height
            };
        }

        private void RemoteDesktopApplication_FormClosing(object sender, FormClosingEventArgs e)
        {
            unsafe
            {
                ffmpeg.avcodec_close(_pDecodecContext);
                ffmpeg.av_free(_pDecodecContext);
                ffmpeg.av_free(_pDecodec);

                Marshal.FreeHGlobal(_convertedFrameBufferPtr);
                ffmpeg.sws_freeContext(_pConvertContext);
            }

            this.RemoteDesktopAdapterHandler.CloseSession();
        }

        private unsafe void RemoteDesktopApplication_Load(object sender, EventArgs e)
        {
            //this._deskGraphics = this.desktopBox.CreateGraphics();
            this.RemoteDesktopAdapterHandler.StartPullStream(_fps, _arguments);
        }
    }
}
