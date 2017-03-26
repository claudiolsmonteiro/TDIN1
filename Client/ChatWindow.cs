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
        event ChatDelegate alterEventChat;
        ChatEventRepeater evRepeater;
        private RemObj.IChat chatService;
        bool isOwner;

        public ChatWindow(String owner, int p, string local, string rem)
        {
            switch (owner)
            {
                case "OWN":
                    isOwner = true;
                    RemotingConfiguration.RegisterWellKnownServiceType(typeof(ChatService), "Chat", WellKnownObjectMode.Singleton);  // register my remote object for servic
                    chatService = (ChatService)RemotingServices.Connect(typeof(ChatService), "tcp://localhost:" + p.ToString() + "/Chat");    // connect to the registered my remote object here
                    break;

                case "REMOTE":
                    isOwner = false;
                    chatService = (ChatService)RemotingServices.Connect(typeof(ChatService), "tcp://localhost:" + p.ToString() + "/Chat");
                    break;
            }

            localUsername = local;
            remoteUsername = rem;
            port = p;
            chatService.AddUserInChat(localUsername);

            evRepeater = new ChatEventRepeater();
            evRepeater.alterEventChat += new ChatDelegate(DoAlterations);
            chatService.alterEventChat += new ChatDelegate(evRepeater.Repeater);
            InitializeComponent();

            string title = "";
            foreach(string s in chatService.GetUsersInChat())
            {
                if(!s.Equals(localUsername))
                    title += s + ",";
            }
            this.Text = title;
        }

        private void SendMessage(object sender, EventArgs e)
        {
            chatService.SendMessage(localUsername, this.MsgBox.Text);
            MsgBox.Clear();
        }

        private void SendFile(object sender, EventArgs e)
        {
            OpenFileDialog browse = new OpenFileDialog();
            browse.ShowDialog();
            //chatService.SendMessage(localUsername, this.MsgBox.Text);
            //MsgBox.Clear();
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
                            if(IsDisposed == false)
                            Invoke((MethodInvoker)delegate () {
                                try
                                {
                                    Close();
                                }
                                catch (ObjectDisposedException)
                                {
                                    return;
                                } });
                            else
                            {
                                return;
                            }
                        }
                    }
                    else
                    {
                        if (message == localUsername)
                        {
                            Close();
                        }
                    }
                    break;
                case ChatOperation.NewUser:
                    String title = this.Text;

                    if (!localUsername.Equals(user))
                        title += "," + user;

                    if (InvokeRequired)
                    {
                        Invoke((MethodInvoker)delegate () { this.Text = title; });
                    }
                    else
                    {
                        this.Text = title;
                    }
                    break;
            }
        }

        private void ChatWindow_Load(object sender, EventArgs e)
        {

        }

        private void ChatWindow_Closed(object sender, FormClosingEventArgs e)
        {
            if (chatService.GetUsersInChat().Count() < 3)
            {
                MessageBox.Show("USERS < 3");
                chatService.CloseChat(localUsername, remoteUsername);
            }
            else if (isOwner)
            {
                MessageBox.Show("IsOwner");
                foreach (string s in chatService.GetUsersInChat())
                {
                    if(!s.Equals(localUsername))
                        chatService.CloseChat(localUsername, s);
                }
            }
            else
            {
                MessageBox.Show("SóFechar");
                chatService.RemoveUser(localUsername);
            }
                
        }

        private void ChatWindow_Load_1(object sender, EventArgs e)
        {

        }

        private void ChatWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            alterEventChat -= new ChatDelegate(DoAlterations);
            evRepeater.alterEventChat -= new ChatDelegate(evRepeater.Repeater);
        }
    }











    public class ChatService : MarshalByRefObject, IChat
    {
        public event RemObj.ChatDelegate alterEventChat;
        List<string> usersInChat = new List<string>();

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

        public void AddUserInChat(string u)
        {
            usersInChat.Add(u);
            NotifyClients(RemObj.ChatOperation.NewUser, u, null);
        }

        public List<string> GetUsersInChat()
        {
            return usersInChat;
        }

        public void RemoveUser(string u)
        {
            usersInChat.Remove(u);
        }
    }
}
