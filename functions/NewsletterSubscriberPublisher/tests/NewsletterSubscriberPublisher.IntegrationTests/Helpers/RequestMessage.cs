using NewsletterSubscriberPublisher.Models;
using System.Net.Http.Json;

namespace NewsletterSubscriberPublisher.IntegrationTests.Helpers;

internal class RequestMessage
{
    private RequestMessage() { }

    internal static HttpRequestMessage BuildPost(string email)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "api/subscribe")
        {
            Content = JsonContent.Create(new SubscribeMessageDto(email))
        };

        request.Headers.Add("x-functions-key", "test");

        return request;
    }
}
