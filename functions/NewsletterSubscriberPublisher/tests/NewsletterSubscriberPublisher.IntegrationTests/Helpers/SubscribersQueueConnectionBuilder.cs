namespace NewsletterSubscriberPublisher.IntegrationTests.Helpers;

internal class SubscribersQueueConnectionBuilder
{
    internal static string Build(string azuriteIp, int blobPort, int queuePort, int tablePort)
    {
        return $"AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;DefaultEndpointsProtocol=http;BlobEndpoint=http://{azuriteIp}:{blobPort}/devstoreaccount1;QueueEndpoint=http://{azuriteIp}:{queuePort}/devstoreaccount1;TableEndpoint=http://{azuriteIp}:{tablePort}/devstoreaccount1;";
    }
}
