﻿using System;
using System.Collections.Generic;
using System.Threading;

namespace RemObj
{

    public class RemObj : MarshalByRefObject
    {
        public event AlterDelegate alterEvent;
        private Dictionary<string, string> registeredUsers;
        private List<User> onlineUsers;

        public override object InitializeLifetimeService()
        {
            return null;
        }
        public RemObj()
        {
            registeredUsers = new Dictionary<string, string>(); // bd de utilizadores
            onlineUsers = new List<User>(); // lista de utilizadores online
        }

        public int Login(string user, string password)
        {
            if (Exist(user))
            {
                if (registeredUsers[user].Equals(password))
                {
                    User u = new User(user, password);
                    // login sucesso

                    onlineUsers.Add(u);
                    NotifyClients(Operation.New, u);
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

        void NotifyClients(Operation op, User item)
        {
            if (alterEvent != null)
            {
                Delegate[] invkList = alterEvent.GetInvocationList();

                foreach (AlterDelegate handler in invkList)
                {
                    new Thread(() => {
                        try
                        {
                            handler(op, item);
                            Console.WriteLine("Invoking event handler");
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

        public void Logout(string user)
        {
            foreach (var entry in onlineUsers)
            {
                // do something with entry.Value or entry.Key
                if (entry.Name.Equals(user)) { 
                    onlineUsers.Remove(entry);
                    NotifyClients(Operation.Remove, entry);
                    return;
                }
            }
        }

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
    }
}