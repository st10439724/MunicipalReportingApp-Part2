using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MunicipalReportingApp.Common
{
    
   
    public static class UiHelpers
    {
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        private const int PBM_SETBARCOLOR = 0x409;

        
        public static void MakeRounded(Control control, int radius)
        {
            void Apply()
            {
                if (control.Width <= 0 || control.Height <= 0) return;

                var path = new GraphicsPath();
                var rect = new Rectangle(0, 0, control.Width, control.Height);
                int d = radius * 2;

                path.AddArc(rect.X, rect.Y, d, d, 180, 90);
                path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
                path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
                path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
                path.CloseFigure();

                control.Region = new Region(path);
            }

            Apply();
            control.Resize += (_, _) => Apply();
        }

       
        public static void StyleAsTag(Label label, Color tint, Color text)
        {
            label.AutoSize = true;
            label.BackColor = tint;
            label.ForeColor = text;
            label.Font = AppTheme.TagFont;
            label.Padding = new Padding(10, 4, 10, 4);
            label.TextAlign = ContentAlignment.MiddleCenter;
            MakeRounded(label, 9);
        }

        
        public static void SetProgressBarColor(ProgressBar bar, Color color)
        {
            if (!bar.IsHandleCreated) return;
            SendMessage(bar.Handle, PBM_SETBARCOLOR, IntPtr.Zero, ColorTranslator.ToWin32(color));
        }
    }
}
