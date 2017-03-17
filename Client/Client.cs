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
            RemObj c = (RemObj)RemotingServices.Connect(typeof(RemObj), "tcp://localhost:9000/RemObj/ListClients");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
