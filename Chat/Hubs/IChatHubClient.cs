using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chat.Hubs
{
    public interface IChatHubClient
    {
        void SendMessage(string username, string message);

        void Typing(string userId, bool typing);

        void UserConnected(string userId, string username);

        void UserDisconnected(string userId);
    }
}