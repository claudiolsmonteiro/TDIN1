using System;
using System.Collections.Generic;
using System.Windows.Forms;
using RemObj;

namespace Client
{
    public partial class ClientListWindow : Form
    {
        private bool chatInitiated;
        private readonly AlterEventRepeater evRepeater;
        private readonly List<string> items;
        private readonly int localPort;
        private readonly string localUserName;
        private int pending;
        private readonly IUserService rObj;

        public ClientListWindow(string n, IUserService r, int p)
        {
            InitializeComponent();
            localUserName = n;
            rObj = r;
            localPort = p;
            items = rObj.ListOnlineUsers();
            chatInitiated = false;
            pending = 0;
            Text += ": " + localUserName;
            evRepeater = new AlterEventRepeater();
            evRepeater.alterEvent += DoAlterations;
            rObj.alterEvent += evRepeater.Repeater;
        }

        private event AlterDelegate alterEvent;

        public ListViewItem GetItemtoDelete(string ClientName)
        {
            ListViewItem listviewitem = null;
            ListViewItem[] listItems;

            if (InvokeRequired)
                listItems = (ListViewItem[])Invoke((MethodInvoker)delegate
              {
                  foreach (ListViewItem i in ClientList.Items)
                  {
                      listviewitem = i;
                      if (ClientName == listviewitem.Text)
                          break;
                  }
              });
            else
                foreach (ListViewItem i in ClientList.Items)
                {
                    listviewitem = i;
                    if (ClientName == listviewitem.Text)
                        break;
                }
            return listviewitem;
        }

        public void DoAlterations(Operation op, List<User> item, string[] remUser)
        {
            LVAddDelegate lvAdd;
            ListViewItem lvItem;
            switch (op)
            {
                case Operation.New:
                    lvAdd = ClientList.Items.Add;
                    lvItem = new ListViewItem(new[] { item[0].Name });
                    BeginInvoke(lvAdd, lvItem);
                    break;

                case Operation.Remove:
                    var listviewitem = new ListViewItem();
                    listviewitem = GetItemtoDelete(item[0].Name);
                    if (listviewitem != null)
                        if (InvokeRequired)
                        {
                            Invoke((MethodInvoker)delegate { ClientList.Items.Remove(listviewitem); });
                            Invoke((MethodInvoker)delegate { ClientList.Refresh(); });
                        }
                        else
                        {
                            ClientList.Items.Remove(listviewitem);
                            ClientList.Refresh();
                        }
                    break;

                case Operation.Request:

                    foreach (var u in item)
                        if (u.Name.Equals(localUserName))
                        {
                            var chatRequest = new ChatRequestWindow(remUser[0], remUser[2], rObj, remUser[1], localUserName);
                            chatRequest.ShowDialog();
                        }
                    break;
                case Operation.Accept:
                    if (item[0].Name.Equals(localUserName))
                    {

                        if (remUser[1].Equals("mult"))
                        {
                            if (!chatInitiated)
                            {
                                chatInitiated = true;
                                var chatWindow = new ChatWindow("OWN", "mult", localPort, localUserName, remUser[0]);
                                chatWindow.ShowDialog();
                            }
                            pending--;
                            if (pending == 0)
                                chatInitiated = false;
                        }
                        else
                        {
                            var chatWindow = new ChatWindow("OWN", remUser[0], localPort, localUserName, remUser[0]);
                        chatWindow.ShowDialog();
                        }

                    }
                    break;
                case Operation.AcceptSingle:
                    if (item[0].Name.Equals(localUserName))
                    {/*
                        
                        pending--;
                        if (pending == 0)
                            chatInitiated = false;
                        if (!chatInitiated)
                        {
                            chatInitiated = true;
                            var chatWindow = new ChatWindow("OWN", localPort, localUserName, remUser[0]);
                            chatWindow.ShowDialog();
                        }
*/
                        
                    }
                    break;
                case Operation.Reject:
                    if (item[0].Name.Equals(localUserName))
                    {
                        pending--;
                        if (pending == 0)
                            chatInitiated = false;
                    }
                    break;
            }
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var info = ClientList.HitTest(e.X, e.Y);
            var item = info.Item;

            if (item != null)
            {
                // send chat request
                rObj.SendChatRequest(item.Text, localUserName, localPort.ToString());
            }
            else
            {
                ClientList.SelectedItems.Clear();
                MessageBox.Show("No Item is selected");
            }
            pending = 1;
        }

        private void InitChatButton_Click(object sender, EventArgs e)
        {
            if (ClientList.SelectedItems.Count == 0)
                return;
            var selectedItems = ClientList.SelectedItems;
            var targets = new List<string>();
            foreach (ListViewItem i in selectedItems)
                targets.Add(i.Text);
            pending = targets.Count;
            rObj.SendMultipleChatRequest(targets, localUserName, localPort.ToString());
        }

        private void ClientWindow_Load(object sender, EventArgs e)
        {
            foreach (var name in items)
                if (name != localUserName)
                {
                    var lvItem = new ListViewItem(new[] { name });
                    ClientList.Items.Add(lvItem);
                }
        }

        private void ClientWindow_FormClosed(object sender, FormClosingEventArgs e)
        {
            rObj.Logout(localUserName);
            alterEvent -= evRepeater.Repeater;
            evRepeater.alterEvent -= DoAlterations;
            Application.Exit();
        }

        private void LogoutButton_Click(object sender, EventArgs e)
        {
            rObj.Logout(localUserName);
            alterEvent -= evRepeater.Repeater;
            evRepeater.alterEvent -= DoAlterations;
            Application.Exit();
        }

        private void ClientList_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private delegate ListViewItem LVAddDelegate(ListViewItem lvItem);
    }
}