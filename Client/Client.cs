using System;
using System.Collections;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Serialization.Formatters;
using System.Windows.Forms;

namespace Client
{
    internal static class Client
    {
        [STAThread]
        private static void Main()
        {
            IDictionary props = new Hashtable();
            props["port"] = 0; // let the system choose a free port
            var serverProvider = new BinaryServerFormatterSinkProvider();
            serverProvider.TypeFilterLevel = TypeFilterLevel.Full;
            var clientProvider = new BinaryClientFormatterSinkProvider();
            var chan = new TcpChannel(props, clientProvider, serverProvider); // instantiate the channel
            ChannelServices.RegisterChannel(chan, false); // register the channel

            var data = (ChannelDataStore) chan.ChannelData;
            var port = new Uri(data.ChannelUris[0]).Port; // get the port
            RemotingConfiguration.Configure("Client.exe.config", false);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LogWindow(port));
        }
    }
}