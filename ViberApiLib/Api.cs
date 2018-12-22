using System;
using System.Text;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ViberApiLib
{
    public class Api
    {
        private string _authToken;
        public string AuthToken
        {
            get { return _authToken; }
        }

        private string _botName;
        public string BotName
        {
            get { return _botName; }
        }

        private string _avatarPath;
        public string AvatarPath
        {
            get { return _avatarPath; }
        }

        public Api(string authToken, string botName, string avatarPath)
        {
            _authToken = authToken;
            _botName = botName;
            _avatarPath = avatarPath;
        }

        public async Task<string> SendMessages(string userId, string text, string trackingData = "")
        {
            var dictPayload = prepareSendMessagesPayload(message: text, receiver: userId, senderName: BotName, senderAvatar: AvatarPath, trackingData: trackingData);
            var payload = JsonConvert.SerializeObject(dictPayload);
            var response = await PostRequest(Constants.SEND_MESSAGE, payload);
            return response;
        }

        public async Task<string> SetWebhook(string url, List<string> event_types = null)
        {
            var dictPayload = new Dictionary<string, object>()
        {
            { "auth_token", AuthToken},
            { "url", url},
        };
            if (event_types != null && event_types.Count > 0)
            {
                dictPayload.Add("event_types", event_types);
            }
            var payload = JsonConvert.SerializeObject(dictPayload);
            var result = await PostRequest(Constants.SET_WEBHOOK, payload);
            var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(result);
            if (values["status"].ToString() != "0")
            {
                return string.Format("Failed with status: {0}, massage: {1}", values["status"], values["status_message"]);
            }
            return values["event_types"].ToString();
        }

        public bool VerifySignature(string requestData, string signature)
        {
            return signature == calculateMessageSignature(requestData);
        }

        public Request ParseRequest(string jsonRequest)
        {
            RequstFactory factory = new RequstFactory();
            return factory.Create(jsonRequest);
        }

        private Dictionary<string, object> prepareSendMessagesPayload(string message, string receiver, string senderName, string senderAvatar, string trackingData)
        {
            return new Dictionary<string, object>()
        {
            { "auth_token", AuthToken},
            { "receiver", receiver},
            { "min_api_version", 1},
            { "sender", new Dictionary<string, object>(){
                    { "name", senderName},
                    { "avatar", senderAvatar},
                }
            },
            { "tracking_data", trackingData},
            { "type", "text"},
            {"text", message},
        };
        }

        public async Task<string> PostRequest(string endPoint, string payload)
        {
            HttpClient client = new HttpClient();
            StringContent content = new StringContent(payload, System.Text.Encoding.UTF8, "application/json");
            // post data to Viber API
            HttpResponseMessage apiResponse = await client.PostAsync(Constants.VIBER_BOT_API_URL + "/" + endPoint, content);
            string response = await apiResponse.Content.ReadAsStringAsync();
            return response;
        }

        private string calculateMessageSignature(string message)
        {
            byte[] keyByte = new ASCIIEncoding().GetBytes(AuthToken);
            byte[] messageBytes = new ASCIIEncoding().GetBytes(message);

            byte[] hashmessage = new HMACSHA256(keyByte).ComputeHash(messageBytes);
            return string.Concat(Array.ConvertAll(hashmessage, x => x.ToString("x2")));
        }
    }
}
