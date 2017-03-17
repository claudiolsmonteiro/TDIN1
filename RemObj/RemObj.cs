using System;
using System.Data;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Net;
using System.Runtime.Remoting;

public class RemObj : MarshalByRefObject
{
    Dictionary<string, string> users;

    public RemObj()
    {
        users = new Dictionary<string, string>(); // bd de utilizadores
    }

    public void login(string user, string password)
    {
        if (exist(user))
        {
            if (users[user].Equals(password))
            {
                // login sucesso
            }
            else
            {
                //password incorrecta
            }
        }
        else
        {
            //user nao existe
        }
    }
    public void Register(string user, string password)
    {
        
        if (exist(user))
        {
            //user ja existe        
        }
        else
        {
            //adiciona user a bd
            users.Add(user, password);
        }

    }

    private Boolean exist(string user)
    {
        if (users.ContainsKey(user))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public string listUsers()
    {
        // list all users
        return "";
    }
}
