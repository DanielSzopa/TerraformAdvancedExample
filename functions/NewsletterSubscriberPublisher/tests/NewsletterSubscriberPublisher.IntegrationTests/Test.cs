using DotNet.Testcontainers.Builders;

namespace NewsletterSubscriberPublisher.IntegrationTests
{
    public class Test
    {
        [Fact]
        public async Task test2()
        {
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
                    { "AzureWebJobsSecretStorageType", "files" }
                })
                .WithPortBinding("6666", "6666")
                .WithResourceMapping(new FileInfo("host.json"), "/azure-functions-host/Secrets")
                .WithWaitStrategy(Wait.ForUnixContainer().UntilHttpRequestIsSucceeded(r => r.ForPort(6666)))
                .Build();

            await container.StartAsync();

            var test = "";
        }
    }
}
