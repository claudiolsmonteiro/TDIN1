using System.Collections.Generic;

namespace RemObj
{
    public interface IUserService
    {
        event RemObj.AlterDelegate alterEvent;

        int Register(string user, string password);
        int Login(string user, string password);
        void Logout(string user);
        void NotifyClients(Operation op, User item, string[] remUser);
        List<string> ListOnlineUsers();
        void SendChatRequest(string user,string remUser, string port);
        void AcceptRequest(string user,string me);
        //void SendChatRequest(string[] user);
        //void chatReject(string user);
        void Print(string m);
    }

    public interface IChat
    {
        event RemObj.ChatDelegate alterEventChat;

        void SendMessage(string user, string message);
    }
    
}