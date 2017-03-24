using System;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Threading;
using RemObj;

namespace Server
{
    class Server
    {
        static void Main(string[] args)
        {
            RemotingConfiguration.Configure("Server.exe.config", false);
            Console.WriteLine("[Server]: Press return to exit");
            Console.ReadLine();
        }
    }

    public class UserService : MarshalByRefObject, RemObj.IUserService
    {
        private Dictionary<string, string> registeredUsers = new Dictionary<string, string>();
        private List<RemObj.User> onlineUsers =  new List<RemObj.User>();
        public event AlterDelegate alterEvent;

        public int Register(string user, string password)
        {

            if (Exist(user))
            {
                return 2;
            }
            else
            {
                //adiciona user a bd
                registeredUsers.Add(user, password);
                return 0;
            }

        }

        public int Login(string user, string password)
        {
            if (Exist(user))
            {
                if (registeredUsers[user].Equals(password))
                {
                    RemObj.User u = new RemObj.User(user, password);
                    // login sucesso

                    onlineUsers.Add(u);
                    NotifyClients(RemObj.Operation.New, u, null);
                    return 0;
                }
                else
                {
                    //password incorrecta
                    return 1;
                }
            }
            else
            {
                //user nao existe
                return 2;
            }
        }

        public void Logout(string user)
        {
            foreach (var entry in onlineUsers)
            {
                // do something with entry.Value or entry.Key
                if (entry.Name.Equals(user))
                {
                    onlineUsers.Remove(entry);
                    NotifyClients(RemObj.Operation.Remove, entry, null );
                    return;
                }
            }
        }

        public void NotifyClients(RemObj.Operation op, RemObj.User item, string[] remUser)
        {
            if (alterEvent != null)
            {
                Delegate[] invkList = alterEvent.GetInvocationList();

                foreach (RemObj.AlterDelegate handler in invkList)
                {
                    new Thread(() =>
                    {
                        try
                        {
                            handler(op, item, remUser );
                            Console.WriteLine("Invoking event handler on " + item.Name);
                        }
                        catch (Exception)
                        {
                            alterEvent -= handler;
                            Console.WriteLine("Exception: Removed an event handler");
                        }
                    }).Start();
                }
            }
        }

        private Boolean Exist(string user)
        {
            if (registeredUsers.ContainsKey(user))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<string> ListOnlineUsers()
        {
            Console.WriteLine("GetList() called.");
            // list all registeredUsers
            List<string> ret = new List<string>();
            foreach (var entry in onlineUsers)
            {
                // do something with entry.Value or entry.Key
                ret.Add(entry.Name);
            }
            return ret;
        }

        public void SendChatRequest(string user, string remUser, string port)
        {

            foreach (var entry in onlineUsers)
            {
                // do something with entry.Value or entry.Key
                if (entry.Name.Equals(user))
                {
                    string[] rem = new string[2];
                    rem[0] = remUser;
                    rem[1] = port;
                    NotifyClients(RemObj.Operation.Request, entry, rem);
                    return;
                }
            }
        }

        public void AcceptRequest(string user, string me)
        {
            foreach (var entry in onlineUsers)
            {
                if (entry.Name.Equals(user))
                {
                    String[] rm = new string[1];
                    rm[0] = me;
                    NotifyClients(RemObj.Operation.Accept, entry, rm);
                    return;
                }
            }
        }

        public void Print(string m)
        {
            Console.WriteLine(m);
        }
    }
}
