# ViberApiLib-CSharp

ViberApiLib-CSharp is a library for developing Viber platform with use of its API.

This library is made based on [Viber REST API](https://developers.viber.com/docs/api/rest-bot-api/).

# Viber API

## Vpi class

`using ViberApiLib`

* Api
  * string AuthToken
  * string BotName
  * string AvatarPath
  * string SendMessages(string userId, string text, string trackingData)
  * string SetWebhook(string url, List<string> event_types)
  * Request ParseRequest(string jsonRequest)
  * string PostRequest(string endPoint, string payload)

## Request class

* Kinds of `Request`
  * SubscribedRequest
  * UnsubscribedRequest
  * MessageRequest
  * SeenRequest
  * ConversationStartedRequest
  * DeliveredRequest
  * FailedRequest
  * WebhookRequest

All of `Request`s have `string Event` and `string TimeStamp` property but each `Request` have different properties/methods.

# Usage

To utilize the library, please refer to the test codes in this project.

[Test Codes](https://github.com/momotaro98/ViberApiLib-CSharp/tree/master/ViberApiLib.Test)
