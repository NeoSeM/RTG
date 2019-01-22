using Microsoft.VisualBasic.ApplicationServices;
using RTRss.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RTRss
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            new Startup().Run(args);
        }
    }

    public class Startup : WindowsFormsApplicationBase
    {
        protected override void OnCreateSplashScreen()
        {
            SplashScreen = new frmSplash();
        }

        protected override void OnCreateMainForm()
        {
            MainForm = new Form1();
        }
    }
}
