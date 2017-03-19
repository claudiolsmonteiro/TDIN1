using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RemObj;

namespace Client
{
    public partial class Popup : Form
    {
        private string localuser;
        private List<string> items;
        event AlterDelegate alterEvent;
        AlterEventRepeater evRepeater;
        delegate ListViewItem LVAddDelegate(ListViewItem lvItem);
        delegate void LVRemoveDelegate(ListViewItem lvItem);
        private RemObj.RemObj rObj;

        public Popup(string n, RemObj.RemObj r)
        {
            InitializeComponent();
            localuser = n;
            rObj = r;
            items = rObj.ListOnlineUsers();
            evRepeater = new AlterEventRepeater();
            evRepeater.alterEvent += new AlterDelegate(DoAlterations);
            rObj.alterEvent += new AlterDelegate(evRepeater.Repeater);
        }
        public ListViewItem GetItemtoDelete(string ClientName)
        {
            ListViewItem listviewitem = new ListViewItem();
            for (int i = 0; i < ClientList.Items.Count; i++)
            {
                listviewitem = ClientList.Items[i];
                if (ClientName == listviewitem.Text)
                {
                    return listviewitem;
                }
            }
            return null;
        }
        public void DoAlterations(Operation op, User item)
        {
            LVAddDelegate lvAdd;
            LVRemoveDelegate lvRem;
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
                        }
                        else
                        {
                            ClientList.Items.Remove(listviewitem);
                        }
                    }
                    ClientList.Refresh();

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
                MessageBox.Show("The selected Item Name is: " + item.Text);
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
                if (name != localuser)
                {
                    ListViewItem lvItem = new ListViewItem(new string[] { name });
                    ClientList.Items.Add(lvItem);
                }
            }
        }
        private void ClientWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            rObj.Logout(localuser);
            this.alterEvent -= new AlterDelegate(evRepeater.Repeater);
            evRepeater.alterEvent -= new AlterDelegate(DoAlterations);

        }

        private void LogoutButton_Click(object sender, EventArgs e)
        {
            rObj.Logout(localuser);
            this.alterEvent -= new AlterDelegate(evRepeater.Repeater);
            evRepeater.alterEvent -= new AlterDelegate(DoAlterations);
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
