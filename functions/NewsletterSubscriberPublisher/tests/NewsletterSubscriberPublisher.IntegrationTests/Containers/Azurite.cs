using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Images;
using DotNet.Testcontainers.Networks;

namespace NewsletterSubscriberPublisher.IntegrationTests.Containers;

internal class Azurite
{

    private Azurite() {}

    internal static int BlobPort => 9777;
    internal static int QueuePort => 9778;
    internal static int TablePort => 9779;
       
    internal static string? Ip { get; private set; }

    private static async Task<IFutureDockerImage> CreateImageAsync()
    {
        var azuriteDicrectory = Path.GetFullPath(Path.Combine(CommonDirectoryPath.GetSolutionDirectory().DirectoryPath, "../"));
        var image = new ImageFromDockerfileBuilder()
            .WithName("azurite_publisher_integration_tests")
            .WithDockerfileDirectory(azuriteDicrectory)
            .WithDockerfile("Dockerfile")
            .Build();

        await image.CreateAsync();

        return image;
    }

    internal static async Task<IContainer> CreateContainerAsync(INetwork network)
    {
        var image = await CreateImageAsync();
        var container =  new ContainerBuilder()
                .WithName("azurite_publisher_integration_tests")
                .WithImage(await CreateImageAsync())
                .WithCommand("--blobPort", BlobPort.ToString(), "--queuePort", QueuePort.ToString(), "--tablePort", TablePort.ToString())
                .WithPortBinding(BlobPort, BlobPort)
                .WithPortBinding(QueuePort, QueuePort)
                .WithPortBinding(TablePort, TablePort)
                .WithNetwork(network)
                .WithWaitStrategy(Wait.ForUnixContainer()
                .UntilPortIsAvailable(BlobPort)
                .UntilPortIsAvailable(QueuePort)
                .UntilPortIsAvailable(TablePort))
                .Build();

        await container.StartAsync();

        Ip = container.IpAddress;

        return container;
    }
}
