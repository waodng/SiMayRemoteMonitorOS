using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SiMay.Platform.Windows
{
    public class DesktopCapture : IDisposable
    {
        [DllImport("user32.dll")]
        static extern IntPtr GetDC(IntPtr ptr);
        [DllImport("gdi32.dll")]
        static extern int GetDeviceCaps(
        IntPtr hdc, // handle to DC  
                        int nIndex // index of capability  
                        );
        [DllImport("User32.dll")]
        private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        const int HORZRES = 8;
        const int VERTRES = 10;
        const int LOGPIXELSX = 88;
        const int LOGPIXELSY = 90;
        const int DESKTOPVERTRES = 117;
        const int DESKTOPHORZRES = 118;

        public Size DesktopSize
            => _desktopSize;

        private Size _desktopSize
        {
            get
            {
                var hdc = GetDC(IntPtr.Zero);
                int screenHeight = GetDeviceCaps(hdc, DESKTOPVERTRES);
                int screenWidth = GetDeviceCaps(hdc, DESKTOPHORZRES);
                ReleaseDC(IntPtr.Zero, hdc);
                return new Size(screenWidth % 2 == 0 ? screenWidth : screenWidth - 1, screenHeight % 2 == 0 ? screenHeight : screenHeight - 1);
            }
        }

        private Graphics _graphic { get; set; }
        private Bitmap _originBitmap;
        private Rectangle _screenBounds;

        private int _screenIndex = Screen.AllScreens.ToList().IndexOf(Screen.PrimaryScreen);
        public int ScreenIndex
        {
            get
            {
                return _screenIndex;
            }
            set
            {
                _screenBounds = Screen.AllScreens[value].Bounds;
                _screenIndex = value;
            }
        }

        public DesktopCapture()
        {
            _screenBounds = Screen.PrimaryScreen.Bounds;
            _originBitmap = new Bitmap(_desktopSize.Width, _desktopSize.Height);
            _graphic = Graphics.FromImage(_originBitmap);
        }

        public Bitmap Capture()
        {
            _graphic.CopyFromScreen(_screenBounds.X, _screenBounds.Y, 0, 0, new Size(_desktopSize.Width, _desktopSize.Height));
            return _originBitmap;
        }

        public void Dispose()
        {
            _graphic.Dispose();
            _originBitmap.Dispose();
        }
    }
}
