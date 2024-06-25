using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Images;
using DotNet.Testcontainers.Networks;

namespace NewsletterSubscriberPublisher.IntegrationTests.Containers;

internal class Function
{
    private Function() { }
    private static async Task<IFutureDockerImage> CreateImageAsync()
    {
        var directory = Path.Combine(CommonDirectoryPath.GetSolutionDirectory().DirectoryPath, "NewsletterSubscriberPublisher\\NewsletterSubscriberPublisher");
        var image = new ImageFromDockerfileBuilder()
            .WithName("newlettersubscriberpublisher_integration_tests")
            .WithDockerfileDirectory(directory)
            .WithDockerfile("Dockerfile")
            .WithBuildArgument("BUILD_CONFIGURATION", "Release")
            .Build();

        await image.CreateAsync();

        return image;
    }

    internal static async Task<IContainer> CreateContainerAsync(INetwork network, string azuriteIp, int blobPort, int queuePort, int tablePort)
    {
        var image = await CreateImageAsync();

        int funcPort = 6666;

        var container = new ContainerBuilder()
                .WithName("newlettersubscriberpublisher_integration_tests")
                .WithImage(image)
                .WithEnvironment(new Dictionary<string, string>()
                {
                    { "ASPNETCORE_ENVIRONMENT","Development" },
                    { "ASPNETCORE_HTTP_PORTS",$"{funcPort}" },
                    { "ASPNETCORE_URLS", $"http://+:{funcPort}" },
                    { "AzureWebJobsSecretStorageType", "files" },
                    { "SubscribersQueueConnection", $"AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;DefaultEndpointsProtocol=http;BlobEndpoint=http://{azuriteIp}:{blobPort}/devstoreaccount1;QueueEndpoint=http://{azuriteIp}:{queuePort}/devstoreaccount1;TableEndpoint=http://{azuriteIp}:{tablePort}/devstoreaccount1;" }
                })
                .WithPortBinding(funcPort, funcPort)
                .WithResourceMapping(new FileInfo("host.json"), "/azure-functions-host/Secrets")
                .WithNetwork(network)
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(funcPort))
                .Build();

        await container.StartAsync();

        return container;
    }
}
