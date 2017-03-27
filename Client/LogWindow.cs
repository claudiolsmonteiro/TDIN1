using System;
using System.Windows.Forms;
using RemObj;

namespace Client
{
    public partial class LogWindow : Form
    {
        private readonly int port;
        private readonly IUserService userService;

        public LogWindow(int p)
        {
            InitializeComponent();
            userService = (IUserService) R.New(typeof(IUserService)); // get reference to the singleton remote object
            port = p;
            userService.LoadUsers();
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            var log = userService.Login(UsernameTextBox.Text, PasswordTextBox.Text);

            switch (log)
            {
                case 0:
                    //fechar este form e abrir o que tem a lista de users
                    Visible = false;
                    var popup = new ClientListWindow(UsernameTextBox.Text, userService, port);
                    popup.ShowDialog();
                    break;
                case 1:
                    //password errada
                    MessageBox.Show("Wrong password!");
                    break;
                case 2:
                    //username errado
                    MessageBox.Show("Username doesn't exist!");
                    break;
                case 3:
                    MessageBox.Show("This username is already logged in!");
                    break;
                default:
                    break;
            }
        }

        private void RegisterButton_Click(object sender, EventArgs e)
        {
            if (UsernameTextBox.Text.Length < 4 || UsernameTextBox.Text.Length > 10)
            {
                MessageBox.Show("Username must have between 4 and 10 characters", "Invalid Username",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (PasswordTextBox.Text.Length < 4 || PasswordTextBox.Text.Length > 10)
            {
                MessageBox.Show("Password must have between 4 and 10 characters", "Invalid Password",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            var reg = userService.Register(UsernameTextBox.Text, PasswordTextBox.Text);
            switch (reg)
            {
                case 0:
                    //sucesso
                    MessageBox.Show("Success!", "Register", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    break;
                case 2:
                    MessageBox.Show("Username already exists!");
                    break;
                default:
                    break;
            }
        }

        private void LogWindow_Load(object sender, EventArgs e)
        {
        }

        private void UsernameTextBox_TextChanged(object sender, EventArgs e)
        {
        }
    }
}