using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Client
{
    public partial class ChatRequestWindow : Form
    {
        string remoteUserName, localUserName;
        private RemObj.IUserService rObj;
        private int port;

        public ChatRequestWindow(string name, RemObj.IUserService r, string p, string localName)
        {
            InitializeComponent();
            remoteUserName = name;
            rObj = r;
            port = Int32.Parse(p);
            localUserName = localName;
            this.ChatTextBox.Text = "" + remoteUserName + " wants to start a conversation with you!";
        }

        private void AcceptRequest(object sender, EventArgs e)
        {
            rObj.AcceptRequest(remoteUserName,localUserName);
            this.Visible = false;
            var chatWindow = new ChatWindow("REMOTE", port, localUserName, remoteUserName);
            chatWindow.ShowDialog();

            this.Close();
        }

        private void DenyRequest(object sender, EventArgs e)
        {

            this.Close();
            //
        }
    }
}
