using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Threading;
using System.Windows.Forms;
using RemObj;

namespace Client
{
    public partial class ChatWindow : Form
    {
        private readonly IChat chatService;
        private readonly ChatEventRepeater evRepeater;
        private readonly bool isOwner;
        private readonly string localUsername;
        private readonly string remoteUsername;
        private int port;

        public ChatWindow(string owner, string type, int p, string local, string rem)
        {
            switch (owner)
            {
                case "OWN":
                    isOwner = true;

                    RemotingConfiguration.RegisterWellKnownServiceType(typeof(ChatService), "Chat/" + type,
                        WellKnownObjectMode.Singleton); // register my remote object for servic
                    chatService =
                        (ChatService)
                        RemotingServices.Connect(typeof(ChatService), "tcp://localhost:" + p + "/Chat/" + type);

                    // connect to the registered my remote object here
                    break;

                case "REMOTE":
                    isOwner = false;
                    chatService =
                        (ChatService)
                        RemotingServices.Connect(typeof(ChatService), "tcp://localhost:" + p + "/Chat/" + type);

                    break;
            }


            localUsername = local;
            remoteUsername = rem;
            port = p;
            chatService.AddUserInChat(localUsername);

            evRepeater = new ChatEventRepeater();
            evRepeater.alterEventChat += DoAlterations;
            chatService.alterEventChat += evRepeater.Repeater;
            InitializeComponent();

            var title = "";
            foreach (var s in chatService.GetUsersInChat())
                if (!s.Equals(localUsername))
                    title += s + ",";

            title = title.Replace(",,", ",");
            title = title.TrimStart(',');
            title = title.TrimEnd(',');
            Text = title;
        }

        private event ChatDelegate alterEventChat;

        private void SendMessage(object sender, EventArgs e)
        {
            chatService.SendMessage(localUsername, MsgBox.Text);
            MsgBox.Clear();
        }



        public void DoAlterations(ChatOperation op, string user, string message)
        {
            switch (op)
            {
                case ChatOperation.NewMsg:
                    string msg;
                    if (localUsername == user)
                        msg = "Me: " + message + Environment.NewLine;
                    else
                        msg = user + ": " + message + Environment.NewLine;
                    if (InvokeRequired)
                    {
                        Invoke((MethodInvoker) delegate { ChatBox.AppendText(msg); });
                        Invoke((MethodInvoker) delegate { ChatBox.Refresh(); });
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
                            if (IsDisposed == false)
                                Invoke((MethodInvoker) delegate
                                {
                                    try
                                    {
                                        Close();
                                    }
                                    catch (ObjectDisposedException)
                                    {
                                    }
                                });
                            else
                                return;
                    }
                    else
                    {
                        if (message == localUsername)
                            Close();
                    }
                    break;
                case ChatOperation.NewUser:
                    var title = Text;

                    if (!localUsername.Equals(user))
                    {
                        title += "," + user;
                        title = title.Replace(",,", ",");
                        title = title.TrimStart(',');
                        title = title.TrimEnd(',');
                    }


                    if (InvokeRequired)
                        Invoke((MethodInvoker) delegate { Text = title; });
                    else
                        Text = title;
                    break;
            }
        }

        private void ChatWindow_Load(object sender, EventArgs e)
        {
        }

        private void ChatWindow_Closed(object sender, FormClosingEventArgs e)
        {
            if (chatService.GetUsersInChat().Count() < 3)
                chatService.CloseChat(localUsername, remoteUsername);
            else if (isOwner)
                foreach (var s in chatService.GetUsersInChat())
                    if (!s.Equals(localUsername))
                        chatService.CloseChat(localUsername, s);
                    else
                        chatService.RemoveUser(localUsername);
        }


        private void ChatWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            chatService.RemoveUser(localUsername);
            alterEventChat -= DoAlterations;
            evRepeater.alterEventChat -= evRepeater.Repeater;
        }
    }


    public class ChatService : MarshalByRefObject, IChat
    {
        private readonly List<string> usersInChat = new List<string>();
        public event ChatDelegate alterEventChat;

        public void SendMessage(string user, string message)
        {
            NotifyClients(ChatOperation.NewMsg, user, message);
        }

        public void CloseChat(string me, string other)
        {
            NotifyClients(ChatOperation.CloseChat, me, other);
        }

        public void AddUserInChat(string u)
        {
            usersInChat.Add(u);
            NotifyClients(ChatOperation.NewUser, u, null);
        }

        public List<string> GetUsersInChat()
        {
            return usersInChat;
        }

        public void RemoveUser(string u)
        {
            usersInChat.Remove(u);
        }

        public void SendFile(string user, byte[] file)
        {
        }

        public void NotifyClients(ChatOperation op, string user, string message)
        {
            if (alterEventChat != null)
            {
                var invkList = alterEventChat.GetInvocationList();

                foreach (ChatDelegate handler in invkList)
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