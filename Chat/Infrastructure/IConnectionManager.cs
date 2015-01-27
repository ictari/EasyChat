using System;
using System.Collections.Generic;
namespace Chat.Infrastructure
{
    public interface IConnectionManager
    {
        bool Add(string key, string connectionId, string username);
        bool Exist(string key);
        IEnumerable<string> GetConnections(string key);
        User GetUser(string key);
        IEnumerable<User> GetUsers();
        int KeyCount { get; }
        bool Remove(string key, string connectionId);
    }
}
