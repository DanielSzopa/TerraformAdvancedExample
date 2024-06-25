using DotNet.Testcontainers.Builders;

namespace NewsletterSubscriberPublisher.IntegrationTests
{
    public class Test
    {
        [Fact]
        public async Task test2()
        {
            var network = new NetworkBuilder()
                .WithName(Guid.NewGuid().ToString())
                .Build();

            await network.CreateAsync();

            var azuriteDicrectory = Path.GetFullPath(Path.Combine(CommonDirectoryPath.GetSolutionDirectory().DirectoryPath, "../"));
            var azuriteImage = new ImageFromDockerfileBuilder()
                .WithName("azurite_integration_tests")
                .WithDockerfileDirectory(azuriteDicrectory)
                .WithDockerfile("Dockerfile")
                .Build();

            await azuriteImage.CreateAsync();

            var azuriteContainer = new ContainerBuilder()
                .WithName("azurite_integration_tests")
                .WithImage(azuriteImage)
                .WithCommand("--blobPort", "9777", "--queuePort", "9778", "--tablePort", "9779")
                .WithPortBinding(9777,9777)
                .WithPortBinding(9778,9778)
                .WithPortBinding(9779,9779)
                .WithNetwork(network)
                .WithWaitStrategy(Wait.ForUnixContainer()
                .UntilPortIsAvailable(9777)
                .UntilPortIsAvailable(9778)
                .UntilPortIsAvailable(9779))
                .Build();

            await azuriteContainer.StartAsync();


            var directory = Path.Combine(CommonDirectoryPath.GetSolutionDirectory().DirectoryPath, "NewsletterSubscriberPublisher\\NewsletterSubscriberPublisher");
            var image = new ImageFromDockerfileBuilder()
                .WithName("newlettersubscriberpublisher_integration_tests")
                .WithDockerfileDirectory(directory)
                .WithDockerfile("Dockerfile")
                .WithBuildArgument("BUILD_CONFIGURATION", "Release")
                .Build();

            await image.CreateAsync();

            var container = new ContainerBuilder()
                .WithName("newlettersubscriberpublisher_integration_tests")
                .WithImage(image)
                .WithEnvironment(new Dictionary<string, string>()
                {
                    { "ASPNETCORE_ENVIRONMENT","Development" },
                    { "ASPNETCORE_HTTP_PORTS","6666" },
                    { "ASPNETCORE_URLS", "http://+:6666" },
                    { "AzureWebJobsSecretStorageType", "files" },
                    { "SubscribersQueueConnection", $"AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;DefaultEndpointsProtocol=http;BlobEndpoint=http://{azuriteContainer.IpAddress}:9777/devstoreaccount1;QueueEndpoint=http://{azuriteContainer.IpAddress}:9778/devstoreaccount1;TableEndpoint=http://{azuriteContainer.IpAddress}:9779/devstoreaccount1;" }
                })
                .WithPortBinding(6666, 6666)
                .WithResourceMapping(new FileInfo("host.json"), "/azure-functions-host/Secrets")
                .WithNetwork(network)
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(6666))
                .Build();

            await container.StartAsync();

            var test = "";
        }
    }
}
