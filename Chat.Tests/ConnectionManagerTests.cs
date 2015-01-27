using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Chat.Infrastructure;
using System.Linq;

namespace Chat.Tests
{
    /// <summary>
    /// Summary description for ConnectionManagerTests
    /// </summary>
    [TestClass]
    public class ConnectionManagerTests
    {
        [TestMethod]
        public void ShouldAddNewKeyAndConnection()
        {
            var manager = new ConnectionManager();

            var userKey = Guid.NewGuid().ToString();
            var connectionId = Guid.NewGuid().ToString();
            var username = "john";
            var isNewUser = manager.Add(userKey, connectionId, username);

            Assert.IsTrue(isNewUser);
            Assert.IsTrue(manager.Exist(userKey));
            var user = manager.GetUser(userKey);

            Assert.AreEqual(user.Id, userKey);
            Assert.IsTrue(user.Connections.Contains(connectionId));

            Assert.AreEqual(user.Username, username);
        }

        [TestMethod]
        public void ShouldRemoveLastConnectionAndKey()
        {
            var manager = new ConnectionManager();

            var userKey = Guid.NewGuid().ToString();
            var connectionId = Guid.NewGuid().ToString();
            var username = "john";
            manager.Add(userKey, connectionId, username);

            var keyRemoved = manager.Remove(userKey, connectionId);
            Assert.IsTrue(keyRemoved);

            var connectionExist = manager.GetConnections(userKey).Contains(connectionId);

            Assert.IsFalse(connectionExist);
        }

        [TestMethod]
        public void ShouldNotDuplicateKeys()
        {
            var manager = new ConnectionManager();

            var userKey = Guid.NewGuid().ToString();
            var connectionId = Guid.NewGuid().ToString();
            var username = "john";
            manager.Add(userKey, connectionId, username);

            var isNewKey = manager.Add(userKey, connectionId, username);
            Assert.IsFalse(isNewKey);
            Assert.AreEqual(manager.KeyCount, 1);

            var connectionCount = manager.GetConnections(userKey).Count();

            Assert.AreEqual(connectionCount, 1);
        }
    }
}
