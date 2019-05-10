
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using TandemChallenge.Models;

namespace TandemChallenge.Controllers
{
    public class MessageController : ApiController
    {
        private IMessagesContext db = new MessagesContext();

        public MessageController() { }

        public MessageController(IMessagesContext context)
        {
            db = context;
        }

        // GET: api/Message
        public IHttpActionResult GetMessages()
        {
            IEnumerable<Message> messages = db.Messages.OrderByDescending(x => x.CreatedDate);
            if (messages == null)
            {
                return NotFound();
            }

            return Ok(messages);
        }

        // GET: api/Message/userid

        public IHttpActionResult GetMessages(string userID)
        {
            IEnumerable<Message> messages = db.Messages.Where(x => x.UserId.ToLower() == userID.ToLower()).OrderByDescending(x=> x.CreatedDate);

            if (messages == null)
            {
                return NotFound();
            }

            return Ok(messages);

        }

        

        // POST: api/Message/user

        public IHttpActionResult PostMessage(string userID, string message)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var msg = new Message(userID.ToLower(), message);
            db.Messages.Add(msg);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (MessageExists(msg.MessageId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = msg.MessageId }, msg);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MessageExists(string id)
        {
            return db.Messages.Count(e => e.MessageId == id) > 0;
        }
    }
}