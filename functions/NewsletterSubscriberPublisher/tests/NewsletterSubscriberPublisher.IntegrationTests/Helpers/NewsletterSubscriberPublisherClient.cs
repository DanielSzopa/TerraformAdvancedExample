using NewsletterSubscriberPublisher.IntegrationTests.Containers;

namespace NewsletterSubscriberPublisher.IntegrationTests.Helpers
{
    internal class NewsletterSubscriberPublisherClient
    {
        private static HttpClient? _httpClient;

        private NewsletterSubscriberPublisherClient() { }

        internal static HttpClient Create()
        {
            if(_httpClient is null)
            {
                _httpClient = new HttpClient()
                {
                    BaseAddress = new Uri($"http://localhost:{Function.FuncPort}"),
                };
            }

            return _httpClient;
        }
    }
}
