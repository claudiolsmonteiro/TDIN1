using System;
using System.Windows.Forms;
using RemObj;

namespace Client
{
    public partial class ChatRequestWindow : Form
    {
        private readonly string localUserName;
        private readonly int port;
        private readonly string remoteUserName;
        private readonly IUserService rObj;
        private readonly string type;

        public ChatRequestWindow(string name, string t, IUserService r, string p, string localName)
        {
            InitializeComponent();
            remoteUserName = name;
            rObj = r;
            port = int.Parse(p);
            localUserName = localName;
            type = t;
            ChatTextBox.Text = "" + remoteUserName + " wants to start a conversation with you!";
        }

        private void AcceptRequest(object sender, EventArgs e)
        {
            rObj.AcceptRequest(remoteUserName, localUserName, type);
            Visible = false;
            var chatWindow = new ChatWindow("REMOTE", type, port, localUserName, remoteUserName);
            chatWindow.ShowDialog();

            Close();
        }

        private void DenyRequest(object sender, EventArgs e)
        {
            rObj.DenyRequest(remoteUserName, localUserName);
            Close();
        }

        private void ChatRequestWindow_Closed(object sender, FormClosingEventArgs e)
        {
            rObj.DenyRequest(remoteUserName, localUserName);
        }
    }
}