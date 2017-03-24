﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
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

    public enum Operation { New, Remove, Request, Accept, Reject };
    public enum ChatOperation { NewMsg, Remove, CloseChat };

    public delegate void AlterDelegate(Operation op, User item, string[] remUser);

    public delegate void ChatDelegate(ChatOperation op, string user, string message); 

    public class AlterEventRepeater : MarshalByRefObject
    {
        public event AlterDelegate alterEvent;

        public override object InitializeLifetimeService()
        {
            return null;
        }

        public void Repeater(Operation op, User item, string[] remUser)
        {
            if (alterEvent != null)
                alterEvent(op, item, remUser);
        }
    }

    public class ChatEventRepeater : MarshalByRefObject
    {
        public event ChatDelegate alterEventChat;

        public override object InitializeLifetimeService()
        {
            return null;
        }

        public void Repeater(ChatOperation op, string user, string message)
        {
            if (alterEventChat != null)
                alterEventChat(op, user, message);
        }
    }

    public class R
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