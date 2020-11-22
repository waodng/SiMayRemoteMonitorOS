using SiMay.Platform.Windows;
using SiMay.RemoteMonitor.Entitys;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SiMay.RemoteMonitor.UserControls
{
    public class UListView : ListView
    {
        public bool UseWindowsThemStyle { get; set; } = true;

        private const uint WM_CHANGEUISTATE = 0x127;

        private const short UIS_SET = 1;
        private const short UISF_HIDEFOCUS = 0x1;
        private readonly IntPtr _removeDots = new IntPtr((int)UIS_SET << 16 | (int)(short)UISF_HIDEFOCUS);

        private ListViewColumnSorter _sorter = new ListViewColumnSorter();
        public UListView()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            this.ListViewItemSorter = _sorter;
            this.View = View.Details;
            this.FullRowSelect = true;
        }

        protected override void OnColumnClick(ColumnClickEventArgs e)
        {
            base.OnColumnClick(e);

            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == this._sorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                this._sorter.Order = (this._sorter.Order == SortOrder.Ascending)
                    ? SortOrder.Descending
                    : SortOrder.Ascending;
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                this._sorter.SortColumn = e.Column;
                this._sorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            if (!this.VirtualMode)
                this.Sort();
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            if (UseWindowsThemStyle)
            {
                if (PlatformHelper.RunningOnMono) return;

                if (PlatformHelper.VistaOrHigher)
                {
                    // set window theme to explorer
                    Win32Api.SetWindowTheme(this.Handle, "explorer", null);
                }

                if (PlatformHelper.XpOrHigher)
                {
                    // removes the ugly dotted line around focused item
                    Win32Api.SendMessage(this.Handle, WM_CHANGEUISTATE, _removeDots, IntPtr.Zero);
                }
            }
        }





        /// <summary>
        /// 进度条列索引
        /// </summary>
        private int? _progressColumnIndex = -1;
        public int? ProgressColumnIndex
        {
            get
            {
                return _progressColumnIndex;
            }
            set
            {
                _progressColumnIndex = value;
            }
        }

        /// <summary>
        /// 进度条最大值
        /// </summary>
        private int _progressMaximun = 100;
        public int ProgressMaximun
        {
            get
            {
                return _progressMaximun;
            }
        }

        protected override void OnDrawColumnHeader(DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = true;
            base.OnDrawColumnHeader(e);
        }

        protected override void OnDrawSubItem(DrawListViewSubItemEventArgs e)
        {
            if (ProgressColumnIndex.HasValue && e.ColumnIndex == ProgressColumnIndex.Value)
            {
                var item = e.Item.SubItems[ProgressColumnIndex.Value];
                var rect = item.Bounds;

                //绘制进度条
                var g = e.Graphics;
                var progressRect = new Rectangle(rect.X + 1, rect.Y + 3, rect.Width - 2, rect.Height - 5);
                g.DrawRectangle(new Pen(new SolidBrush(Color.Blue), 1), progressRect);

                //绘制进度
                var progressMaxWidth = progressRect.Width - 1;
                var unit = (progressMaxWidth * 1.0) / (_progressMaximun * 100);
                var fValue = float.Parse(item.Text);
                var percent = fValue * unit * 100;
                if (percent >= progressMaxWidth) percent = progressMaxWidth;
                g.FillRectangle(new SolidBrush(Color.Blue), new RectangleF(progressRect.X + 1, progressRect.Y + 1, float.Parse(percent.ToString()), progressRect.Height - 1));

                //绘制进度百分比
                percent = fValue;
                var percentText = string.Format("{0}% ...", Math.Round(percent, 2));
                if (fValue >= _progressMaximun) percentText = "已完成";
                var size = TextRenderer.MeasureText(percentText.ToString(), Font);
                var x = rect.X + (progressRect.Width - size.Width) / 2.0;
                var y = rect.Y + (progressRect.Height - size.Height) / 2.0 + 3;
                g.DrawString(percentText, this.Font, new SolidBrush(Color.Black), float.Parse(x.ToString()), float.Parse(y.ToString()));
            }
            else
            {
                e.DrawDefault = true;
            }
            base.OnDrawSubItem(e);
        }
    }
}
