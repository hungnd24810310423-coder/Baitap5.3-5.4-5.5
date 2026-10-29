
using System;
using System.Windows.Forms;

namespace GUI
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new FrmVatTu());
        }
    }
}
