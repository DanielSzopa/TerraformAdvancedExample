using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Networks;

namespace NewsletterSubscriberPublisher.IntegrationTests.Containers;

internal class Network
{
    internal static async Task<INetwork> CreateAsync()
    {
        var network = new NetworkBuilder()
                .WithName(Guid.NewGuid().ToString())
                .Build();

        await network.CreateAsync();

        return network;
    }
}
