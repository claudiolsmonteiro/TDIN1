using System;
using System.Runtime.Remoting;
using System.Windows.Forms;

namespace Server
{
    static class Server
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(RemObj), "ListClients",WellKnownObjectMode.Singleton);
        }
    }
}
