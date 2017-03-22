using System.Collections.Generic;

namespace RemObj
{
    public interface IUserService
    {
        event RemObj.AlterDelegate alterEvent;

        int Register(string user, string password);
        int Login(string user, string password);
        void Logout(string user);
        void NotifyClients(Operation op, User item);
        List<string> ListOnlineUsers();
        void SendChatRequest(string user);
        //void SendChatRequest(string[] user);
        //void chatReject(string user);
    }

    public interface IChat
    {

    }
}