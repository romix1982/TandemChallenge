using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using TandemChallenge.Controllers;
using TandemChallenge.Models;
using System.Linq;
using System;
using System.Web.Http.Routing;

namespace TandemChallenge.Tests
{
    [TestClass]
    public class TestMessageController
    {


        [TestMethod]
        public void PostMessage_ShouldReturnSameMessage()
        {
            var controller = new MessageController(new TestMessageContext());


            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            var result =
                controller.PostMessage("Tom", "Message from Tom") as CreatedAtRouteNegotiatedContentResult<Message>;



            Assert.IsNotNull(result);
            Assert.AreEqual(result.RouteName, "DefaultApi");
            Assert.AreEqual(result.RouteValues["id"], result.Content.MessageId);
            Assert.IsNotNull(result.Content);
            Assert.AreEqual(result.Content.UserMessage, "Message from Tom");
        }


        [TestMethod]
        public void GetMessage_ShouldReturnMessagesFromThisUserId()
        {
            var context = new TestMessageContext();
            context.Messages.Add(GetMessagesDemo());
            context.Messages.Add(GetMessagesDemo());

            var controller = new MessageController(context);
            var result = controller.GetMessages("Tom") as IHttpActionResult;
            var contentResult = result as OkNegotiatedContentResult<IEnumerable<Message>>;

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<IEnumerable<Message>>));
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(2, contentResult.Content.Count<Message>()); 
            

        }

        [TestMethod]
        public void GetMessage_ShouldReturnAllMessage()
        {
            var context = new TestMessageContext();
            context.Messages.Add(new Message("Romina", "Message 1"));
            context.Messages.Add(new Message("Romina", "Message 2"));
            context.Messages.Add(new Message("Tom", "Message 1"));

            var controller = new MessageController(context);
            var result = controller.GetMessages() as TestMessageDbSet;

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Local.Count);
        }


        Message GetMessagesDemo()
        {
            return new Message("Tom", "Message 1");
        }
    }
}
