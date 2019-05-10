using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TandemChallenge.Models
{
    public class Message
    {
        public string MessageId { get; set; }
        public string UserId { get; set; }
        public string UserMessage { get; set; }
        public DateTime CreatedDate { get; set; }

        public Message()
        {

        }   
            
        public Message(string pUserId, string pMessage)
        {
            MessageId = Guid.NewGuid().ToString();
            UserId = pUserId;
            UserMessage = pMessage;
            CreatedDate = DateTime.UtcNow;
        }
    }
}