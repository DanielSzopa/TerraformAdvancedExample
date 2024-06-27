using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Networks;
using NewsletterSubscriberPublisher.IntegrationTests.Containers;

namespace NewsletterSubscriberPublisher.IntegrationTests.Setup
{
    public class TestsStartup : IAsyncLifetime
    {
        private INetwork _network;
        private IContainer _azurite;
        private IContainer _func;

        public async Task InitializeAsync()
        {
            _network = await Network.CreateAsync();
            _azurite = await Azurite.CreateContainerAsync(_network);
            _func = await Function.CreateContainerAsync(_network, azuriteIp: _azurite.IpAddress, blobPort: Azurite.BlobPort, queuePort: Azurite.QueuePort, tablePort: Azurite.TablePort);
        }

        public async Task DisposeAsync()
        {
            await _network.DisposeAsync();
            await _azurite.DisposeAsync();
            await _func.DisposeAsync();
        }
    }
}
