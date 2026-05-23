using System;
using System.Windows.Forms;

namespace GFCDevOpsUtility
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            // Sets high DPI mode for crisp UI fonts on high-resolution displays (modern .NET)
            Application.SetHighDpiMode(HighDpiMode.SystemAware);

            Application.Run(new MainForm());
        }
    }
}
