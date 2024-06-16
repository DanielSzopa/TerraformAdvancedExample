using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using NewsletterSubscriberPublisher.Models;

namespace NewsletterSubscriberPublisher
{
    public class NewsletterSubscriberPublisher
    {
        private readonly ILogger<NewsletterSubscriberPublisher> _logger;

        public NewsletterSubscriberPublisher(ILogger<NewsletterSubscriberPublisher> logger)
        {
            _logger = logger;
        }

        [Function("Subscribe")]
        [QueueOutput(Constants.SubscribersQueue, Connection = "SubscribersQueueConnection")]
        public async Task<string> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req, CancellationToken cancellationToken)
        {
            _logger.LogInformation("C# HTTP trigger function start processing a request.");
            var dto = await req.ReadFromJsonAsync<SubscribeMessageDto>(cancellationToken);
            var email = new Email(dto.Email);
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return email;
        }
    }
}
