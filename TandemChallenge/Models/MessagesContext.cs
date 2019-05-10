using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TandemChallenge.Models
{
    public class MessagesContext : DbContext, IMessagesContext
    {
       
        public DbSet<Message> Messages { get; set; }

        public MessagesContext() : base("name=MessagesContext")
        {
        }

        public void MarkAsModified(Message item)
        {
            Entry(item).State = EntityState.Modified;
        }
    }
}
