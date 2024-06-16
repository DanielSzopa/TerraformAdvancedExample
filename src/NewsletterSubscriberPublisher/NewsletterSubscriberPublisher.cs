using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

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
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}
