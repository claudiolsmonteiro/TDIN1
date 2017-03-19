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
    public partial class Form1 : Form
    {
        private RemObj.RemObj rObj;
        public Form1(RemObj.RemObj r)
        {
            InitializeComponent();
            rObj = r;
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            int log = rObj.Login(this.textBox1.Text, this.textBox2.Text);
            
            switch (log)
            {
                case 0:
                    //fechar este form e abrir o que tem a lista de users
                    MessageBox.Show("Login Sucessful!");
                    this.Visible = false;
                    var popup = new Popup(rObj);
                    popup.Show();
                    break;
                case 1:
                    //password errada
                    MessageBox.Show("Wrong password!");
                    break;
                case 2:
                    //username errado
                    MessageBox.Show("Username doesn't exist!");
                    break;
                default:
                    break;
            }
        }

        private void RegisterButton_Click(object sender, EventArgs e)
        {
           int reg = rObj.Register(this.textBox1.Text, this.textBox2.Text);

            switch (reg)
            {
                case 0:
                    //sucesso
                    MessageBox.Show("Sucess!", "titulo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    break;
                case 2:
                    MessageBox.Show("Username already exists!");
                    break;
                default:
                    break;
            }
        }

    }
}
