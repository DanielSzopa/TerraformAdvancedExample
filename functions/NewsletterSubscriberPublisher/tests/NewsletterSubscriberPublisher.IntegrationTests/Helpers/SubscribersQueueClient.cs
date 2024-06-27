using Azure.Storage.Queues;
using NewsletterSubscriberPublisher.IntegrationTests.Containers;

namespace NewsletterSubscriberPublisher.IntegrationTests.Helpers;

internal class SubscribersQueueClient
{
    private static QueueClient? _queueClient = null;

    private SubscribersQueueClient() { }

    internal static QueueClient Create()
    {
        if(_queueClient is null)
        {
            var connectionString = SubscribersQueueConnectionBuilder.Build("127.0.0.1", Azurite.BlobPort, Azurite.QueuePort, Azurite.TablePort);
            _queueClient = new QueueClient(connectionString, Constants.SubscribersQueue, new() { MessageEncoding = QueueMessageEncoding.Base64 });
        }

        return _queueClient;
    }
}
