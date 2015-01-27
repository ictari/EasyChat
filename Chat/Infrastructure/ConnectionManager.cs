using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chat.Infrastructure
{
    public class ConnectionManager : IConnectionManager
    {
        private readonly ConcurrentDictionary<string, User> connectionMaps = new ConcurrentDictionary<string, User>();

        public int KeyCount
        {
            get
            {
                return connectionMaps.Count;
            }
        }

        /// <summary>
        /// Add connection to given key, key is created if not exist.
        /// Inserting existent connection key will be ignored silently
        /// </summary>
        /// <param name="key"></param>
        /// <param name="connectionId"></param>
        /// <returns> Returns true if key is new</returns>
        public bool Add(string key, string connectionId, string username)
        {
            var newKey = false;
            var user = connectionMaps.GetOrAdd(key, k =>
            {
                newKey = true;
                return new User { Id = key, Username = username };
            });

            lock (user.Connections)
            {
                user.Connections.Add(connectionId);
            }

            return newKey;
        }

        public IEnumerable<string> GetConnections(string key)
        {
            User user;
            if (connectionMaps.TryGetValue(key, out user))
                return user.Connections;

            return Enumerable.Empty<string>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="connectionId"></param>
        /// <returns>Returns true if existence key also removed </returns>
        public bool Remove(string key, string connectionId)
        {
            User user;

            var removedKey = false;
            if (!connectionMaps.TryGetValue(key, out user))
                return removedKey;

            var connections = user.Connections;
            lock (connections)
            {
                connections.Remove(connectionId);

                if (connections.Count == 0)
                {
                    connectionMaps.TryRemove(key, out user);
                    removedKey = true;
                }
            }

            return removedKey;
        }

        public IEnumerable<User> GetUsers()
        {
            return connectionMaps.Values;
        }

        public User GetUser(string key)
        {
            User user;
            connectionMaps.TryGetValue(key, out user);

            return user;
        }

        public bool Exist(string key)
        {
            return connectionMaps.ContainsKey(key);
        }
    }
}