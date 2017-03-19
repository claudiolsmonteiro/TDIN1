using System;
using System.Runtime.Remoting;
using System.Windows.Forms;

namespace Client
{
    class Client
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            
            RemotingConfiguration.Configure("Client.exe.config", false);
            RemObj.RemObj rem = new RemObj.RemObj();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(rem));
        }
    }
}
