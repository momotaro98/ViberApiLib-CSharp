using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ViberApiLib
{
    public class RequstFactory
    {
        private static readonly List<string> EventTypeList
            = new List<string>
        {
        Constants.EVENT_SEEN,
        Constants.EVENT_CONVERSATION_STARTED,
        Constants.EVENT_DELIVERED,
        Constants.EVENT_MESSAGE,
        Constants.EVENT_SUBSCRIBED,
        Constants.EVENT_UNSUBSCRIBED,
        Constants.EVENT_FAILED,
        Constants.EVENT_WEBHOOK
        };

        public Request Create(string jsonRequest)
        {
            var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonRequest);

            if (!values.ContainsKey("event"))
            {
                throw new KeyNotFoundException("Necessary key of Viber request, \"event\" is not in the request payload.");
            }

            var eventTypeStr = values["event"].ToString();

            if (!EventTypeList.Contains(eventTypeStr))
            {
                throw new Exception("\"event\" key's value of the Viber request, " + eventTypeStr + " is unknown.");
            }

            switch (eventTypeStr)
            {
                case Constants.EVENT_SUBSCRIBED:
                    return new SubscribedRequest(values);
                case Constants.EVENT_UNSUBSCRIBED:
                    return new UnsubscribedRequest(values);
                case Constants.EVENT_MESSAGE:
                    return new MessageRequest(values);
                case Constants.EVENT_SEEN:
                    return new SeenRequest(values);
                case Constants.EVENT_CONVERSATION_STARTED:
                    return new ConversationStartedRequest(values);
                case Constants.EVENT_DELIVERED:
                    return new DeliveredRequest(values);
                case Constants.EVENT_FAILED:
                    return new FailedRequest(values);
                case Constants.EVENT_WEBHOOK:
                    return new WebhookRequest(values);
                default:
                    break;
            }
            return null;  // never reach
        }
    }

    public class Request
    {
        public string Event { get; }

        public string TimeStamp { get; }

        public Request(Dictionary<string, object> requestDict)
        {
            Event = requestDict["event"].ToString();
            TimeStamp = requestDict["timestamp"].ToString();
        }
    }

    public class SubscribedRequest : Request
    {
        public UserProfile User { get; set; }

        public SubscribedRequest(Dictionary<string, object> requestDict) : base(requestDict)
        {
            var userStr = requestDict["user"].ToString();
            var userDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(userStr);
            User = new UserProfile(userDict);
        }
    }

    public class UnsubscribedRequest : Request
    {
        public string UserId { get; }

        public UnsubscribedRequest(Dictionary<string, object> requestDict) : base(requestDict)
        {
            UserId = requestDict["user_id"].ToString();
        }
    }

    public class MessageRequest : Request
    {
        public UserProfile User { get; set; }
        public Message Message { get; set; }

        public MessageRequest(Dictionary<string, object> requestDict) : base(requestDict)
        {
            var userStr = requestDict["sender"].ToString();
            var userDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(userStr);
            User = new UserProfile(userDict);

            var messageStr = requestDict["message"].ToString();
            var messageFactory = new MessageFactory();
            Message = MessageFactory.Create(messageStr);
        }
    }

    public class SeenRequest : Request
    {
        public string UserId { get; }

        public SeenRequest(Dictionary<string, object> requestDict) : base(requestDict)
        {
            UserId = requestDict["user_id"].ToString();
        }
    }

    public class ConversationStartedRequest : Request
    {
        public UserProfile User { get; set; }

        public ConversationStartedRequest(Dictionary<string, object> requestDict) : base(requestDict)
        {
            string userStr = requestDict["user"].ToString();
            var userDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(userStr);
            User = new UserProfile(userDict);
        }
    }

    public class DeliveredRequest : Request
    {
        public string UserId { get; }

        public DeliveredRequest(Dictionary<string, object> requestDict) : base(requestDict)
        {
            UserId = requestDict["user_id"].ToString();
        }
    }

    public class FailedRequest : Request
    {
        public string UserId { get; }

        public FailedRequest(Dictionary<string, object> requestDict) : base(requestDict)
        {
            UserId = requestDict["user_id"].ToString();
        }
    }

    public class WebhookRequest : Request
    {
        public WebhookRequest(Dictionary<string, object> requestDict) : base(requestDict)
        {
        }
    }
}
