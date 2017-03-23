using System;
using System.Collections.Generic;
using System.Windows.Forms;
using RemObj;

namespace Client
{
    public partial class ClientListWindow : Form
    {
        private string localUserName;
        private List<string> items;
        event AlterDelegate alterEvent;
        AlterEventRepeater evRepeater;
        delegate ListViewItem LVAddDelegate(ListViewItem lvItem);
        delegate void LVRemoveDelegate(ListViewItem lvItem);
        private RemObj.IUserService rObj;

        public ClientListWindow(string n, RemObj.IUserService r)
        {
            InitializeComponent();
            localUserName = n;
            rObj = r;
            items = rObj.ListOnlineUsers();
            evRepeater = new AlterEventRepeater();
            evRepeater.alterEvent += new AlterDelegate(DoAlterations);
            rObj.alterEvent += new AlterDelegate(evRepeater.Repeater);
        }

        public ListViewItem GetItemtoDelete(string ClientName)
        {
            ListViewItem listviewitem = null;
            ListViewItem[] listItems;

            if (InvokeRequired)
            {
                listItems = (ListViewItem[])Invoke((MethodInvoker)delegate ()
                {
                    foreach (ListViewItem i in ClientList.Items)
                    {
                        listviewitem = i;
                        if (ClientName == listviewitem.Text)
                            break;
                    }
                });
            }
            else
            {
                foreach (ListViewItem i in ClientList.Items)
                {
                    listviewitem = i;
                    if (ClientName == listviewitem.Text)
                        break;
                }
            }
            return listviewitem;
        }

        public void DoAlterations(Operation op, User item)
        {
            LVAddDelegate lvAdd;
            ListViewItem lvItem;
            switch (op)
            {
                case Operation.New:
                    lvAdd = new LVAddDelegate(ClientList.Items.Add);
                    lvItem = new ListViewItem(new string[] { item.Name });
                    BeginInvoke(lvAdd, new object[] { lvItem });
                    break;

                case Operation.Remove:
                    ListViewItem listviewitem = new ListViewItem();
                    listviewitem = GetItemtoDelete(item.Name);
                    if (listviewitem != null)
                    {
                        if (InvokeRequired)
                        {
                            Invoke((MethodInvoker)delegate () { ClientList.Items.Remove(listviewitem); });
                            Invoke((MethodInvoker)delegate () { ClientList.Refresh(); });
                        }
                        else
                        {
                            ClientList.Items.Remove(listviewitem);
                            ClientList.Refresh();
                        }
                    }
                    break;

                case Operation.Request:
                    if (item.Name.Equals(localUserName))
                    {
                        var chatRequest = new ChatRequestWindow(item.Name, rObj);
                        chatRequest.ShowDialog();
                    }
                    break;
            }
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewHitTestInfo info = ClientList.HitTest(e.X, e.Y);
            ListViewItem item = info.Item;

            if (item != null)
            {
                // send chat request
                //MessageBox.Show("The selected Item Name is: " + item.Text);
                rObj.SendChatRequest(item.Text);
            }
            else
            {
                this.ClientList.SelectedItems.Clear();
                MessageBox.Show("No Item is selected");
            }
        }

        private void ClientWindow_Load(object sender, EventArgs e)
        {
            foreach (string name in items)
            {
                if (name != localUserName)
                {
                    ListViewItem lvItem = new ListViewItem(new string[] { name });
                    ClientList.Items.Add(lvItem);
                }
            }
        }

        private void ClientWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            rObj.Logout(localUserName);
            this.alterEvent -= new AlterDelegate(evRepeater.Repeater);
            evRepeater.alterEvent -= new AlterDelegate(DoAlterations);
            Application.Exit();

        }

        private void LogoutButton_Click(object sender, EventArgs e)
        {
            rObj.Logout(localUserName);
            this.alterEvent -= new AlterDelegate(evRepeater.Repeater);
            evRepeater.alterEvent -= new AlterDelegate(DoAlterations);
            Application.Exit();
        }
    }
}
