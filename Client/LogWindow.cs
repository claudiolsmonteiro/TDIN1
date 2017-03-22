using System;
using System.Collections;
using System.Runtime.Remoting;
using System.Windows.Forms;
using RemObj;

namespace Client
{
    public partial class LogWindow : Form
    {
        IUserService userService;
        int port;

        public LogWindow(int p)
        {
            InitializeComponent();
            userService = (IUserService)R.New(typeof(IUserService));  // get reference to the singleton remote object
            port = p;
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            int log = userService.Login(this.UsernameTextBox.Text, this.PasswordTextBox.Text);

            switch (log)
            {
                case 0:
                    //fechar este form e abrir o que tem a lista de users
                    this.Visible = false;
                    var popup = new ClientListWindow(this.UsernameTextBox.Text, userService);
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

            if (this.UsernameTextBox.Text.Length < 4 || this.UsernameTextBox.Text.Length > 10)
            {
                MessageBox.Show("Username must have between 4 and 10 characters", "Invalid Username", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (this.PasswordTextBox.Text.Length < 4 || this.PasswordTextBox.Text.Length > 10)
            {
                MessageBox.Show("Password must have between 4 and 10 characters", "Invalid Password", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            int reg = userService.Register(this.UsernameTextBox.Text, this.PasswordTextBox.Text);
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

    class R
    {
        private static IDictionary wellKnownTypes;

        public static object New(Type type)
        {
            if (wellKnownTypes == null)
                InitTypeCache();
            WellKnownClientTypeEntry entry = (WellKnownClientTypeEntry)wellKnownTypes[type];
            if (entry == null)
                throw new RemotingException("Type not found!");
            return Activator.GetObject(type, entry.ObjectUrl);
        }

        public static void InitTypeCache()
        {
            Hashtable types = new Hashtable();
            foreach (WellKnownClientTypeEntry entry in RemotingConfiguration.GetRegisteredWellKnownClientTypes())
            {
                if (entry.ObjectType == null)
                    throw new RemotingException("A configured type could not be found!");
                types.Add(entry.ObjectType, entry);
            }
            wellKnownTypes = types;
        }
    }
}
