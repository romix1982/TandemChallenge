using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TandemChallenge.Models;

namespace TandemChallenge.Tests
{
    class TestMessageContext : IMessagesContext
    {
        public TestMessageContext()
        {
            this.Messages = new TestMessageDbSet();
        }

        public DbSet<Message> Messages { get; set; }

        public int SaveChanges()
        {
            return 0;
        }

        public void MarkAsModified(Message item) { }
        public void Dispose() { }
    }
}
