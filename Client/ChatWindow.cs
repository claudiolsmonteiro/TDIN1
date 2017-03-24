using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using RemObj;

namespace Client
{
    public partial class ChatWindow : Form
    {
        private string localUsername, remoteUsername;
        private int port;
        private List<string> items;
        event ChatDelegate alterEventChat;
        ChatEventRepeater evRepeater;
        //delegate ListViewItem LVAddDelegate(ListViewItem lvItem);
        private RemObj.IChat chatService;

        public ChatWindow(String owner, int port, string local, string rem)
        {
            InitializeComponent();

            switch (owner)
            {
                case "OWN":
                    RemotingConfiguration.RegisterWellKnownServiceType(typeof(ChatService), "Chat", WellKnownObjectMode.Singleton);  // register my remote object for servic
                    chatService = (ChatService)RemotingServices.Connect(typeof(ChatService), "tcp://localhost:" + port.ToString() + "/Chat");    // connect to the registered my remote object here
                    break;

                case "REMOTE":
                    chatService = (ChatService)RemotingServices.Connect(typeof(ChatService), "tcp://localhost:" + port.ToString() + "/Chat");
                    break;
            }
            localUsername = local;
            remoteUsername = rem;
            this.Text = remoteUsername;

            evRepeater = new ChatEventRepeater();
            evRepeater.alterEventChat += new ChatDelegate(DoAlterations);
            chatService.alterEventChat += new ChatDelegate(evRepeater.Repeater);
        }

        private void SendMessage(object sender, EventArgs e)
        {
            chatService.SendMessage(localUsername, this.MessageBox.Text);
        }


        public void DoAlterations(ChatOperation op, string user, string message)
        {
            switch (op)
            {
                case ChatOperation.NewMsg:
                    String msg;
                    if (localUsername == user)
                        msg = "Me: " + message + Environment.NewLine;
                    else
                    {
                        msg = user + ": " + message + Environment.NewLine;
                    }
                    if (InvokeRequired)
                    {
                        Invoke((MethodInvoker)delegate () { ChatBox.AppendText(msg); });
                        Invoke((MethodInvoker)delegate () { ChatBox.Refresh(); });
                    }
                    else
                    {
                        ChatBox.AppendText(msg);
                        ChatBox.Refresh();
                    }
                    break;
                case ChatOperation.CloseChat:
                    if (InvokeRequired)
                    {
                        if (message == localUsername)
                        {
                            Invoke((MethodInvoker)delegate () { evRepeater.alterEventChat -= new ChatDelegate(DoAlterations); });
                            Invoke((MethodInvoker)delegate () { chatService.alterEventChat -= new ChatDelegate(evRepeater.Repeater); });
                            Invoke((MethodInvoker)delegate () { Close(); });
                        }
                    }
                    else
                    {
                        if (message == localUsername)
                        {
                            evRepeater.alterEventChat -= new ChatDelegate(DoAlterations);
                            chatService.alterEventChat -= new ChatDelegate(evRepeater.Repeater);
                            Close();
                        }
                    }
                    break;

            }
        }

        private void ChatWindow_Load(object sender, EventArgs e)
        {

        }

        private void ChatWindow_Closed(object sender, FormClosingEventArgs e)
        {
            chatService.CloseChat(localUsername, remoteUsername);
            this.alterEventChat -= new ChatDelegate(DoAlterations);
            evRepeater.alterEventChat -= new ChatDelegate(evRepeater.Repeater);

        }
    }











    public class ChatService : MarshalByRefObject, IChat
    {
        public event RemObj.ChatDelegate alterEventChat;

        public void SendMessage(string user, string message)
        {
            NotifyClients(RemObj.ChatOperation.NewMsg, user, message);
        }

        public void CloseChat(string me, string other)
        {
            NotifyClients(RemObj.ChatOperation.CloseChat, me, other);
        }

        public void NotifyClients(RemObj.ChatOperation op, string user, string message)
        {
            if (alterEventChat != null)
            {
                Delegate[] invkList = alterEventChat.GetInvocationList();

                foreach (RemObj.ChatDelegate handler in invkList)
                {
                    new Thread(() =>
                    {
                        try
                        {
                            handler(op, user, message);
                            // Console.WriteLine("Invoking event handler on " + item.Name);
                        }
                        catch (Exception)
                        {
                            alterEventChat -= handler;
                            //Console.WriteLine("Exception: Removed an event handler");
                        }
                    }).Start();
                }
            }
        }
    }
}
