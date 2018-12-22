using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ViberApiLib
{
    public class MessageFactory
    {
        private static readonly List<string> MessageTypeList
            = new List<string>
        {
        Constants.RICH_MEDIA,
        Constants.STICKER,
        Constants.URL,
        Constants.LOCATION,
        Constants.CONTACT,
        Constants.FILE,
        Constants.TEXT,
        Constants.PICTURE,
        Constants.VIDEO,
        Constants.KEYBOARD
        };

        public static Message Create(string jsonString)
        {
            var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonString);

            if (!values.ContainsKey("type"))
            {
                throw new KeyNotFoundException("Necessary key of Viber message, \"type\" is not in the request payload.");
            }

            var typeStr = values["type"].ToString();

            if (!MessageTypeList.Contains(typeStr))
            {
                throw new Exception("\"type\" key's value of the Viber message, " + typeStr + " is unknown.");
            }

            switch (typeStr)
            {
                case Constants.TEXT:
                    return new TextMessage(values);
                default: // TODO : Add more message types of Viber
                    break;
            }
            return null; // never reach
        }
    }

    public class Message
    {
        public string Type { get; }

        public string TrackingData { get; }

        public Message(IReadOnlyDictionary<string, object> dict)
        {
            Type = dict["type"].ToString();
            TrackingData = dict["tracking_data"].ToString();
        }
    }

    public class TextMessage : Message
    {
        public string Text { get; }

        public TextMessage(IReadOnlyDictionary<string, object> dict) : base(dict)
        {
            Text = dict["text"].ToString();
        }
    }
}
