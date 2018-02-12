using NUnit.Framework;
using System;
using System.Collections.Generic;

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
        public void TestSubscribedRequest()
        {
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
            var req = api.ParseRequest(reqData.ToString());
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
            var reqData = @"{
               ""event"":""unsubscribed"",
               ""timestamp"":1457764197627,
               ""user_id"":""01234567890A="",
               ""message_token"":4912661846655238145
            }";
            var req = api.ParseRequest(reqData.ToString());
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
            var req = api.ParseRequest(reqData.ToString());
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
            var reqData = @"{
                ""event"":""seen"",
                ""timestamp"":1457764197627,
                ""message_token"":4912661846655238145,
                ""user_id"":""01234567890A=""
            }";
            var req = api.ParseRequest(reqData.ToString());
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
            var req = api.ParseRequest(reqData.ToString());
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
            var reqData = @"{
                ""event"":""delivered"",
                ""timestamp"":1457764197627,
                ""message_token"":4912661846655238145,
                ""user_id"":""01234567890A=""
            }";
            var req = api.ParseRequest(reqData.ToString());
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
            var reqData = @"{
                ""event"":""failed"",
                ""timestamp"":1457764197627,
                ""message_token"":4912661846655238145,
                ""user_id"":""01234567890A="",
                ""desc"":""failure description""
            }";
            var req = api.ParseRequest(reqData.ToString());
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
            var reqData = @"{
                ""event"":""webhook"",
                ""timestamp"":1457764197627,
                ""message_token"":""241256543215""
            }";
            var req = api.ParseRequest(reqData.ToString());
            Assert.IsInstanceOf<WebhookRequest>(req);
            Assert.AreEqual(req.Event, "webhook");
            Assert.AreEqual(req.TimeStamp, "1457764197627");
        }

        [Test]
        public void TestRequestThrowsException()
        {
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
            try
            {
                var req = api.ParseRequest(reqData.ToString());
                Assert.Fail();
            }
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
            try
            {
                var req = api.ParseRequest(reqData.ToString());
                Assert.Fail();
            }
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
