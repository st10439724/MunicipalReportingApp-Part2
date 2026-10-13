using System;
using System.Windows.Forms;
using MunicipalReportingApp.Forms;

namespace MunicipalReportingApp
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // Standard WinForms bootstrap.(just dont get it at all // like that)
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new MainMenuForm());
        }
    }
}
