using System;
using System.Windows.Forms;
using System.Diagnostics;

static class Program
{
    [STAThread]
    static void Main()
    {
        try
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
        catch
        {
            Process.GetCurrentProcess().Kill();
            return;
        }
    }
}