using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemObj
{
    [Serializable]
    public class User
    {

        private string name;
        private string password;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public User()
        {
            name = "";
            password = "";
        }

        public User(string user, string pass)
        {
            name = user;
            password = pass;
        }
    }

    public enum Operation { New, Remove };

    public delegate void AlterDelegate(Operation op, User item);

    public class AlterEventRepeater : MarshalByRefObject
    {
        public event AlterDelegate alterEvent;

        public override object InitializeLifetimeService()
        {
            return null;
        }

        public void Repeater(Operation op, User item)
        {
            if (alterEvent != null)
                alterEvent(op, item);
        }
    }
}