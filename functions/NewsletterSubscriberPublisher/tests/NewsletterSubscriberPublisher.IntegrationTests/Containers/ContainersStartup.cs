namespace NewsletterSubscriberPublisher.IntegrationTests.Containers
{
    internal class ContainersStartup
    {
        internal async Task StartAsync()
        {
            var network = await Network.CreateAsync();
            var azurite = await Azurite.CreateContainerAsync(network);
            var func = await Function.CreateContainerAsync(network, azuriteIp: azurite.IpAddress, blobPort: Azurite.BlobPort, queuePort: Azurite.QueuePort, tablePort: Azurite.TablePort);
        }
    }
}
