using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Remoting;
using System.Threading;
using RemObj;

namespace Server
{
    internal class Server
    {
        private static void Main(string[] args)
        {
            RemotingConfiguration.Configure("Server.exe.config", false);
            Console.WriteLine("[Server]: Press return to exit");
            Console.ReadLine();
        }
    }

    public class UserService : MarshalByRefObject, IUserService
    {
        private readonly List<User> onlineUsers = new List<User>();
        private readonly Dictionary<string, string> registeredUsers = new Dictionary<string, string>();
        private bool loaded;
        public event AlterDelegate alterEvent;


        public void LoadUsers()
        {
            if (!loaded)
            {
                var path = AppDomain.CurrentDomain.BaseDirectory + "Users.txt";

                if (!File.Exists(path))
                {
                    File.Create(path).Close();
                    
                }
                else
                {
                    var text = File.ReadAllLines(path);
                    var usernames = new List<string>();
                    var passwords = new List<string>();

                    for (var i = 0; i < text.Length; i++)
                    {
                        registeredUsers.Add(text[i], text[i + 1]);
                        i++;
                    }
                }
                loaded = true;
            }
        }

        public int Register(string user, string password)
        {
            if (Exist(user))
                return 2;
            //adiciona user a bd
            var path = AppDomain.CurrentDomain.BaseDirectory + "Users.txt";

            using (var sw = new StreamWriter(path, true))
            {
                sw.WriteLine(user);
                sw.WriteLine(password);
                sw.Close();
            }

            registeredUsers.Add(user, password);
            return 0;
        }

        public int Login(string user, string password)
        {
            if (Exist(user))
                if (isLogged(user))
                {
                    return 3;
                }
                else if (registeredUsers[user].Equals(password))
                {
                    var u = new User(user, password);
                    // login sucesso

                    onlineUsers.Add(u);
                    var l = new List<User>();
                    l.Add(u);
                    NotifyClients(Operation.New, l, null);
                    return 0;
                }

                else
                {
                    //password incorrecta
                    return 1;
                }
            return 2;
        }

        public void Logout(string user)
        {
            foreach (var entry in onlineUsers)
                if (entry.Name.Equals(user))
                {
                    onlineUsers.Remove(entry);
                    var l = new List<User>();
                    l.Add(entry);
                    NotifyClients(Operation.Remove, l, null);
                    return;
                }
        }

        public void NotifyClients(Operation op, List<User> item, string[] remUser)
        {
            if (alterEvent != null)
            {
                var invkList = alterEvent.GetInvocationList();

                foreach (AlterDelegate handler in invkList)
                    new Thread(() =>
                    {
                        try
                        {
                            handler(op, item, remUser);
                            Console.WriteLine("Invoking event handler on " + item[0].Name);
                        }
                        catch (Exception)
                        {
                            alterEvent -= handler;
                            Console.WriteLine("Exception: Removed an event handler");
                        }
                    }).Start();
            }
        }

        public List<string> ListOnlineUsers()
        {
            Console.WriteLine("GetList() called.");
            // list all registeredUsers
            var ret = new List<string>();
            foreach (var entry in onlineUsers)
                ret.Add(entry.Name);
            return ret;
        }

        public void SendChatRequest(string target, string me, string myport)
        {
            foreach (var entry in onlineUsers)
                if (entry.Name.Equals(target))
                {
                    var rem = new string[3];
                    rem[0] = me;
                    rem[1] = myport;
                    rem[2] = target;
                    var l = new List<User>();
                    l.Add(entry);
                    NotifyClients(Operation.Request, l, rem);
                    return;
                }
        }

        public void SendMultipleChatRequest(List<string> targets, string me, string myport)
        {
            var l = new List<User>();
            var rem = new string[3];
            foreach (var entry in onlineUsers)
                if (targets.Contains(entry.Name))
                {
                    rem[0] = me;
                    rem[1] = myport;
                    rem[2] = "mult";
                    l.Add(entry);
                }
            NotifyClients(Operation.Request, l, rem);
        }

        public void AcceptRequest(string user, string me, string t)
        {
            foreach (var entry in onlineUsers)
                if (entry.Name.Equals(user))
                {
                    var rm = new string[2];
                    rm[0] = me;
                    rm[1] = t;
                    var l = new List<User>();
                    l.Add(entry);
                    NotifyClients(Operation.Accept, l, rm);
                    return;
                }
        }

        public void DenyRequest(string user, string me)
        {
            foreach (var entry in onlineUsers)
                if (entry.Name.Equals(user))
                {
                    var rm = new string[1];
                    rm[0] = me;
                    var l = new List<User>();
                    l.Add(entry);
                    NotifyClients(Operation.Reject, l, rm);
                    return;
                }
        }

        public void Print(string m)
        {
            Console.WriteLine(m);
        }

        private bool Exist(string user)
        {
            if (registeredUsers.ContainsKey(user))
                return true;
            return false;
        }

        private bool isLogged(string user)
        {
            if (ListOnlineUsers().Contains(user))
                return true;
            return false;
        }
    }
}