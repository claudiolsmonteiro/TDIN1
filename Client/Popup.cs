using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RemObj;

namespace Client
{
    public partial class Popup : Form
    {
        private List<string> items;
        event AlterDelegate alterEvent;
        AlterEventRepeater evRepeater;
        delegate ListViewItem LVAddDelegate(ListViewItem lvItem);
        delegate void ChCommDelegate(User item);
        private RemObj.RemObj rObj;
        public Popup(RemObj.RemObj r)
        {

            InitializeComponent();
            rObj = r;
            items = rObj.ListOnlineUsers();
            evRepeater = new AlterEventRepeater();
            evRepeater.alterEvent += new AlterDelegate(DoAlterations);
            rObj.alterEvent += new AlterDelegate(evRepeater.Repeater);
        }
        public void DoAlterations(Operation op, User item)
        {
            LVAddDelegate lvAdd;
            ChCommDelegate chComm;

            switch (op)
            {
                case Operation.New:
                    lvAdd = new LVAddDelegate(listView1.Items.Add);
                    ListViewItem lvItem = new ListViewItem(new string[] { item.Name });
                    BeginInvoke(lvAdd, new object[] { lvItem });
                    break;
            }
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ClientWindow_Load(object sender, EventArgs e)
        {
            foreach (string name in items)
            {
                ListViewItem lvItem = new ListViewItem(new string[] { name });
                listView1.Items.Add(lvItem);
            }
        }
        private void ClientWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.alterEvent -= new AlterDelegate(evRepeater.Repeater);
            evRepeater.alterEvent -= new AlterDelegate(DoAlterations);
        }
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
