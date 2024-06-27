using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Images;
using DotNet.Testcontainers.Networks;
using NewsletterSubscriberPublisher.IntegrationTests.Helpers;

namespace NewsletterSubscriberPublisher.IntegrationTests.Containers;

internal class Function
{
    internal static int FuncPort => 6666;

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

        var container = new ContainerBuilder()
                .WithName("newlettersubscriberpublisher_integration_tests")
                .WithImage(image)
                .WithEnvironment(new Dictionary<string, string>()
                {
                    { "ASPNETCORE_ENVIRONMENT","Development" },
                    { "ASPNETCORE_HTTP_PORTS",$"{FuncPort}" },
                    { "ASPNETCORE_URLS", $"http://+:{FuncPort}" },
                    { "AzureWebJobsSecretStorageType", "files" },
                    { "SubscribersQueueConnection", SubscribersQueueConnectionBuilder.Build(azuriteIp, blobPort, queuePort, tablePort) }
                })
                .WithPortBinding(FuncPort, FuncPort)
                .WithResourceMapping(new FileInfo("host.json"), "/azure-functions-host/Secrets")
                .WithNetwork(network)
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(FuncPort))
                .Build();

        await container.StartAsync();

        return container;
    }
}
