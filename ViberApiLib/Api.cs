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
        public string AuthToken { get; }

        public string BotName { get; }

        public string AvatarPath { get; }

        public Api(string authToken, string botName, string avatarPath)
        {
            AuthToken = authToken;
            BotName = botName;
            AvatarPath = avatarPath;
        }

        public async Task<string> SendMessages(string userId, string text, string trackingData = "")
        {
            var dictPayload = PrepareSendMessagesPayload(message: text, receiver: userId, senderName: BotName, senderAvatar: AvatarPath, trackingData: trackingData);
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
                return $"Failed with status: {values["status"]}, massage: {values["status_message"]}";
            }
            return values["event_types"].ToString();
        }

        public bool VerifySignature(string requestData, string signature)
        {
            return signature == CalculateMessageSignature(requestData);
        }

        public Request ParseRequest(string jsonRequest)
        {
            var factory = new RequstFactory();
            return factory.Create(jsonRequest);
        }

        private Dictionary<string, object> PrepareSendMessagesPayload(string message, string receiver, string senderName, string senderAvatar, string trackingData)
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
            var client = new HttpClient();
            var content = new StringContent(payload, System.Text.Encoding.UTF8, "application/json");
            // post data to Viber API
            var apiResponse = await client.PostAsync(Constants.VIBER_BOT_API_URL + "/" + endPoint, content);
            var response = await apiResponse.Content.ReadAsStringAsync();
            return response;
        }

        private string CalculateMessageSignature(string message)
        {
            var keyByte = new ASCIIEncoding().GetBytes(AuthToken);
            var messageBytes = new ASCIIEncoding().GetBytes(message);

            var hash = new HMACSHA256(keyByte).ComputeHash(messageBytes);
            return string.Concat(Array.ConvertAll(hash, x => x.ToString("x2")));
        }
    }
}
