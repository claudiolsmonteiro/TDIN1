using System.Collections.Generic;

namespace RemObj
{
    public interface IUserService
    {
        event RemObj.AlterDelegate alterEvent;

        int Register(string user, string password);
        int Login(string user, string password);
        void Logout(string user);
        void NotifyClients(Operation op, List<User> item, string[] remUser);
        List<string> ListOnlineUsers();
        void SendChatRequest(string target,string me, string myport);
        void SendMultipleChatRequest(List<string> targets, string me, string myport);
        void AcceptRequest(string user,string me);
        //void SendChatRequest(string[] user);
        //void chatReject(string user);
        void Print(string m);
    }

    public interface IChat
    {
        event RemObj.ChatDelegate alterEventChat;

        void SendMessage(string user, string message);
        void CloseChat(string me, string other);
        void AddUserInChat(string u);
        List<string> GetUsersInChat();
        void RemoveUser(string u);
    }
    
}