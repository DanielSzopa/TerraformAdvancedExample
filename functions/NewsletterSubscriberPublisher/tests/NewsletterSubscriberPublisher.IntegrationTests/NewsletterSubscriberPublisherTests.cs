using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using FluentAssertions;
using FluentAssertions.Execution;
using NewsletterSubscriberPublisher.IntegrationTests.Helpers;
using NewsletterSubscriberPublisher.IntegrationTests.Setup;
using System.Net;

namespace NewsletterSubscriberPublisher.IntegrationTests
{
    [Collection(nameof(TestsCollection))]
    public class NewsletterSubscriberPublisherTests : IAsyncLifetime
    {
        private readonly QueueClient _queueClient = SubscribersQueueClient.Create();

        public NewsletterSubscriberPublisherTests(TestsStartup testsStartup)
        {
            
        }

        [Theory]
        [InlineData("test123@gmail.com")]
        [InlineData("0.1@test.pl")]
        public async Task HttpTrigger_WhenSentEmailIsValid_ShouldReturn200Ok_And_MessageShouldBeCreatedInQueue(string email)
        {
            //arrange
            var client = NewsletterSubscriberPublisherClient.Create();
            var request = RequestMessage.BuildPost(email);
             
            //act
            var response = await client.SendAsync(request);

            //assert
            QueueMessage message = await _queueClient.ReceiveMessageAsync();

            using var scope = new AssertionScope();
            message.MessageText.Should().Be(email);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("test.com")]
        [InlineData("test.test.com")]
        public async Task HttpTrigger_WhenSentEmailIsInvalid_ShouldReturn400BadRequest_And_MessageShouldNotBeCreatedInQueue(string invalidEmail)
        {
            //arrange
            var client = NewsletterSubscriberPublisherClient.Create();
            var request = RequestMessage.BuildPost(invalidEmail);

            //act
            var response = await client.SendAsync(request);

            //assert
            bool queueExistence = await _queueClient.ExistsAsync();
            using var scope = new AssertionScope();
            queueExistence.Should().BeFalse();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        public Task InitializeAsync() => Task.CompletedTask;

        public async Task DisposeAsync()
        {
            //Make sure that queue is clear before execute next test
            await _queueClient.DeleteIfExistsAsync();
        }
    }
}
