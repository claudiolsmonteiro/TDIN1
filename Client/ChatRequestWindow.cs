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
        ClientListWindow previousWindow;
        string remoteUserName;
        private RemObj.IUserService rObj;

        public ChatRequestWindow(string name, ClientListWindow win, RemObj.IUserService r)
        {
            InitializeComponent();
            previousWindow = win;
            remoteUserName = name;
            rObj = r;
            //this.ChatTextBox.Text = "" + remoteUserName + " wants to start a conversation with you!";
        }

        private void AcceptRequest(object sender, EventArgs e)
        {
            previousWindow.Close();
        }

        private void DenyRequest(object sender, EventArgs e)
        {
            this.Close();
            //
        }
    }
}
