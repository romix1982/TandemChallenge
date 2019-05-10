using System;
using System.Data.Entity;


namespace TandemChallenge.Models
{
    public interface IMessagesContext : IDisposable
    {
        DbSet<Message> Messages { get; }
        int SaveChanges();
        void MarkAsModified(Message item);
    }
}
