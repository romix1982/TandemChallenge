# TandemChallenge
Tandem Technical Test

## Message Board API
Create a REST API web service that allows clients to post and retrieve messages from a message board. The messages can be stored in-memory rather than a datastore. The challenge involves supporting two scenarios which are outlined below:

## Scenario 1 - POST message
A client can post messages to a REST API endpoint on the web service.
* The request must include a userId (string) and a message (string)
* The request body properties should be case insensitve
* The response body should include the newly created message resource (application/json)
* The message resource must include a messageId (unique string), userId (string), message (string) and createdDate(ISO 8601)

## Scenario 2 - GET messages (for given userId)
A client can get messages for a specified userId from a REST API endpoint on the web service.

* The request must support a case-insensitive userId querystring parameter
* The response body should be a result object with a collection of message resources for the specified userId ordered by createdDate in descending order.
* If a userId does not exist or has no messages the collection should be empty.
* No pagination is necessary
* The message resource must include a messageId (unique string), userId (string), message (string) and createdDate(ISO 8601)

# Solution
I have decided to resolve this challenge using Web API 2 which is an ideal platform for building RESTful services.
In the solution, you can find 2 projects:
* TandemChallenge which resolves the challenge
* TandemChallenge.Tests which contains  all the unit tests

## TandemChallenge Project
Into this project, you can find the model class Message with all the properties that were needed in the Models folder.

```
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

```


The MessageController handles incoming HTTP requests and sends a response back to the caller. Also, you can find the class MessagesContext, which implements IMessagesContext Interface, in order to remove that hard-coded dependency with the controller.

```
public class MessageController : ApiController
    {
        private IMessagesContext db = new MessagesContext();

        public MessageController() { }

        public MessageController(IMessagesContext context)
        {
            db = context;
        }
        
```
The PostMessage method resolves the scenario 1 and the GetMessage with userId parameter Method resolves the scenario 2. 
```
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

 public IHttpActionResult GetMessages(string userID)
{
  IEnumerable<Message> messages = db.Messages.Where(x => x.UserId.ToLower() == userID.ToLower()).OrderByDescending(x=> x.CreatedDate);

    if (messages == null)
    {
        return NotFound();
    }

    return Ok(messages);

}

```
In additional, the solution is ready to use Entity Framework as a ORM.

## Tandem Challenge Web API in action

In the next image, you can see a Post response using Finddler Tool.
![alt text](https://github.com/romix1982/acucafe/blob/master/AcuView.png)

in the following image, you can see a Get response which shows all messages saved.
![alt text](https://github.com/romix1982/acucafe/blob/master/AcuView.png)

And in this last image, you can see some Get response with the userId “Simon” as a parameter

![alt text](https://github.com/romix1982/acucafe/blob/master/AcuView.png)

About Security, the solution should implement  SSL or token authentication to resolve the actual vulnerability.

## TandemChallenge.Tests Project 
In this project, you can find the TestMessageController class that contains all the possible test methods in order to check the Web API functionality.
I have also build a mock dataSet class and a mock context, which were necessaries for the full test of the service. 

```
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
```




