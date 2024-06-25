using DotNet.Testcontainers.Builders;
using NewsletterSubscriberPublisher.IntegrationTests.Containers;

namespace NewsletterSubscriberPublisher.IntegrationTests
{
    public class Test
    {
        [Fact]
        public async Task test2()
        {

            var container = new ContainersStartup();
            await container.StartAsync();
            var test = "";
        }
    }
}
