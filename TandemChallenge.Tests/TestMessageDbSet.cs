using System.Data.Entity;
using System.Linq;
using TandemChallenge.Models;

namespace TandemChallenge.Tests
{
    class TestMessageDbSet : DbSetMock<Message>
    {
        public override Message Find(params object[] keyValues)
        {
            return this.SingleOrDefault(message => message.MessageId == (string)keyValues.Single());
        }
    }
}