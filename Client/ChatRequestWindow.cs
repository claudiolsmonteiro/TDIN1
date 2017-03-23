using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class ChatRequestWindow : Form
    {
        string remoteUserName;
        private RemObj.IUserService rObj;

        public ChatRequestWindow(string name, RemObj.IUserService r)
        {
            InitializeComponent();
            remoteUserName = name;
            rObj = r;
            this.ChatTextBox.Text = "" + remoteUserName + " wants to start a conversation with you!";
        }

        private void AcceptRequest(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DenyRequest(object sender, EventArgs e)
        {
            this.Close();
            //
        }

        private void ChatReqWindow_Load(object sender, EventArgs e)
        {
            this.ChatTextBox.Text = "" + remoteUserName + " wants to start a conversation with you!";
        }
    }
}
