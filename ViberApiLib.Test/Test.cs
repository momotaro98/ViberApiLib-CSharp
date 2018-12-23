using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ViberApiLib.Test
{
    [TestFixture]
    public class Test
    {
        private Api api;

        [SetUp]
        protected void SetUp()
        {
            api = new Api("testAuthToken", "testBotName", "testAvatarPath");
        }

        [Test]
        public void TestSendMessages()
        {
            // Arrange
            var userIdList = new List<string>{"user1", "user2", "user3"};
            var taskList = new List<Task<string>>{};
            // Act
            foreach (var userid in userIdList)
            {
                Task<string> response = api.SendMessages(userid, "text");
                taskList.Add(response);
            }
            // await Task.WhenAll(taskList); // An example usage for async method
            var t = Task.WhenAll(taskList);
            // Assert
            Assert.IsInstanceOf<Task>(t);
        }

        [Test]
        public void TestSubscribedRequest()
        {
            // Arrange
            var reqData = @"{
               ""event"":""subscribed"",
               ""timestamp"":1457764197627,
               ""user"":{
                  ""id"":""01234567890A="",
                  ""name"":""John McClane"",
                  ""avatar"":""http://avatar.example.com"",
                  ""country"":""UK"",
                  ""language"":""en"",
                  ""api_version"":1
               },
               ""message_token"":4912661846655238145
            }";
            // Act
            var req = Api.ParseRequest(reqData.ToString());
            // Assert
            Assert.IsInstanceOf<SubscribedRequest>(req);
            Assert.AreEqual(req.Event, "subscribed");
            Assert.AreEqual(req.TimeStamp, "1457764197627");
            if (req is SubscribedRequest)
            {
                SubscribedRequest dreq = req as SubscribedRequest;
                Assert.AreEqual(dreq.User.Id, "01234567890A=");
                Assert.AreEqual(dreq.User.Name, "John McClane");
                Assert.AreEqual(dreq.User.Avatar, "http://avatar.example.com");
                Assert.AreEqual(dreq.User.Country, "UK");
                Assert.AreEqual(dreq.User.Language, "en");
                Assert.AreEqual(dreq.User.ApiVersion, "1");
            }
        }

        [Test]
        public void TestUnsubscribedRequest()
        {
            // Arrange
            var reqData = @"{
               ""event"":""unsubscribed"",
               ""timestamp"":1457764197627,
               ""user_id"":""01234567890A="",
               ""message_token"":4912661846655238145
            }";
            // Act
            var req = Api.ParseRequest(reqData.ToString());
            // Assert
            Assert.IsInstanceOf<UnsubscribedRequest>(req);
            Assert.AreEqual(req.Event, "unsubscribed");
            Assert.AreEqual(req.TimeStamp, "1457764197627");
            if (req is UnsubscribedRequest)
            {
                UnsubscribedRequest dreq = req as UnsubscribedRequest;
                Assert.AreEqual(dreq.UserId, "01234567890A=");
            }
        }

        [Test]
        public void TestMessageRequest()
        {
            // Arrange
            var reqData = @"{
                ""event"":""message"",
                ""timestamp"":1457764197627,
                ""message_token"":4912661846655238145,
                ""sender"":{
                    ""id"":""01234567890A="",
                    ""name"":""John McClane"",
                    ""avatar"":""http://avatar.example.com"",
                    ""country"":""UK"",
                    ""language"":""en"",
                    ""api_version"":1
                },
                ""message"":{
                ""type"":""text"",
                ""text"":""a message to the service"",
                ""media"":""http://example.com"",
                ""location"":{
                    ""lat"":50.76891,
                    ""lon"":6.11499
                },
                ""tracking_data"":""tracking data""
                }
            }";
            // Act
            var req = Api.ParseRequest(reqData.ToString());
            // Assert
            Assert.IsInstanceOf<MessageRequest>(req);
            Assert.AreEqual(req.Event, "message");
            Assert.AreEqual(req.TimeStamp, "1457764197627");
            if (req is MessageRequest)
            {
                MessageRequest dreq = req as MessageRequest;

                Assert.AreEqual(dreq.User.Id, "01234567890A=");
                Assert.AreEqual(dreq.User.Name, "John McClane");
                Assert.AreEqual(dreq.User.Avatar, "http://avatar.example.com");
                Assert.AreEqual(dreq.User.Country, "UK");
                Assert.AreEqual(dreq.User.Language, "en");
                Assert.AreEqual(dreq.User.ApiVersion, "1");

                Assert.IsInstanceOf<TextMessage>(dreq.Message);
                Assert.AreEqual(dreq.Message.Type, "text");
                Assert.AreEqual(dreq.Message.TrackingData, "tracking data");
                if (dreq.Message is TextMessage)
                {
                    TextMessage dmessage = dreq.Message as TextMessage;
                    Assert.AreEqual(dmessage.Text, "a message to the service");
                }

            }
        }

        [Test]
        public void TestSeenRequest()
        {
            // Arrange
            var reqData = @"{
                ""event"":""seen"",
                ""timestamp"":1457764197627,
                ""message_token"":4912661846655238145,
                ""user_id"":""01234567890A=""
            }";
            // Act
            var req = Api.ParseRequest(reqData.ToString());
            // Assert
            Assert.IsInstanceOf<SeenRequest>(req);
            Assert.AreEqual(req.Event, "seen");
            Assert.AreEqual(req.TimeStamp, "1457764197627");
            if (req is SeenRequest)
            {
                SeenRequest dreq = req as SeenRequest;
                Assert.AreEqual(dreq.UserId, "01234567890A=");
            }
        }

        [Test]
        public void TestConversationStartedRequest()
        {
            // Arrange
            var reqData = @"{
                ""event"":""conversation_started"",
                ""timestamp"":1457764197627,
                ""message_token"":4912661846655238145,
                ""type"":""open"",
                ""context"":""context information"",
                ""user"":{
                    ""id"":""01234567890A="",
                    ""name"":""John McClane"",
                    ""avatar"":""http://avatar.example.com"",
                    ""country"":""UK"",
                    ""language"":""en"",
                    ""api_version"":1
                },
                ""subscribed"":false
            }";
            // Act
            var req = Api.ParseRequest(reqData.ToString());
            // Assert
            Assert.IsInstanceOf<ConversationStartedRequest>(req);
            Assert.AreEqual(req.Event, "conversation_started");
            Assert.AreEqual(req.TimeStamp, "1457764197627");
            if (req is ConversationStartedRequest)
            {
                ConversationStartedRequest dreq = req as ConversationStartedRequest;
                Assert.AreEqual(dreq.User.Id, "01234567890A=");
                Assert.AreEqual(dreq.User.Name, "John McClane");
                Assert.AreEqual(dreq.User.Avatar, "http://avatar.example.com");
                Assert.AreEqual(dreq.User.Country, "UK");
                Assert.AreEqual(dreq.User.Language, "en");
                Assert.AreEqual(dreq.User.ApiVersion, "1");
            }
        }

        [Test]
        public void TestDeliveredRequest()
        {
            // Arrange
            var reqData = @"{
                ""event"":""delivered"",
                ""timestamp"":1457764197627,
                ""message_token"":4912661846655238145,
                ""user_id"":""01234567890A=""
            }";
            // Act
            var req = Api.ParseRequest(reqData.ToString());
            // Assert
            Assert.IsInstanceOf<DeliveredRequest>(req);
            Assert.AreEqual(req.Event, "delivered");
            Assert.AreEqual(req.TimeStamp, "1457764197627");
            if (req is DeliveredRequest)
            {
                DeliveredRequest dreq = req as DeliveredRequest;
                Assert.AreEqual(dreq.UserId, "01234567890A=");
            }
        }

        [Test]
        public void TestFailedRequest()
        {
            // Arrange
            var reqData = @"{
                ""event"":""failed"",
                ""timestamp"":1457764197627,
                ""message_token"":4912661846655238145,
                ""user_id"":""01234567890A="",
                ""desc"":""failure description""
            }";
            // Act
            var req = Api.ParseRequest(reqData.ToString());
            // Assert
            Assert.IsInstanceOf<FailedRequest>(req);
            Assert.AreEqual(req.Event, "failed");
            Assert.AreEqual(req.TimeStamp, "1457764197627");
            if (req is FailedRequest)
            {
                FailedRequest dreq = req as FailedRequest;
                Assert.AreEqual(dreq.UserId, "01234567890A=");
            }
        }

        [Test]
        public void TestWebhookRequest()
        {
            // Arrange
            var reqData = @"{
                ""event"":""webhook"",
                ""timestamp"":1457764197627,
                ""message_token"":""241256543215""
            }";
            // Act
            var req = Api.ParseRequest(reqData.ToString());
            // Assert
            Assert.IsInstanceOf<WebhookRequest>(req);
            Assert.AreEqual(req.Event, "webhook");
            Assert.AreEqual(req.TimeStamp, "1457764197627");
        }

        [Test]
        public void TestRequestThrowsException()
        {
            // Arrange
            // reqData is missing "event" element.
            var reqData = @"{
               ""timestamp"":1457764197627,
               ""user"":{
                  ""id"":""01234567890A="",
                  ""name"":""John McClane"",
                  ""avatar"":""http://avatar.example.com"",
                  ""country"":""UK"",
                  ""language"":""en"",
                  ""api_version"":1
               },
               ""message_token"":4912661846655238145
            }";
            // Act
            try
            {
                var req = Api.ParseRequest(reqData.ToString());
                // Assert
                Assert.Fail();
            } // Assert
            catch (KeyNotFoundException ex)
            {
                Assert.IsTrue(ex.Message.Contains("Necessary key of Viber request, \"event\" is not in the request payload."));
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestMessageThrowsException()
        {
            // Arrange
            // reqData is missing "type" in "message".
            var reqData = @"{
                ""event"":""message"",
                ""timestamp"":1457764197627,
                ""message_token"":4912661846655238145,
                ""sender"":{
                    ""id"":""01234567890A="",
                    ""name"":""John McClane"",
                    ""avatar"":""http://avatar.example.com"",
                    ""country"":""UK"",
                    ""language"":""en"",
                    ""api_version"":1
                },
                ""message"":{
                    ""text"":""a message to the service"",
                    ""media"":""http://example.com"",
                    ""location"":{
                        ""lat"":50.76891,
                        ""lon"":6.11499
                },
                ""tracking_data"":""tracking data""
                }
            }";
            // Act
            try
            {
                var req = Api.ParseRequest(reqData.ToString());
                // Assert
                Assert.Fail();
            } // Assert
            catch (KeyNotFoundException ex)
            {
                Assert.IsTrue(ex.Message.Contains("Necessary key of Viber message, \"type\" is not in the request payload."));
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }
    }
}
