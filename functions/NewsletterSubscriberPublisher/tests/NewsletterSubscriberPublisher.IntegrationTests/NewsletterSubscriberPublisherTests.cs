using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Bogus;
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
        private readonly Faker _faker = new Faker();
        private readonly QueueClient _queueClient = SubscribersQueueClient.Create();

        public NewsletterSubscriberPublisherTests(TestsStartup testsStartup)
        {
            
        }

        [Fact]
        public async Task HttpTrigger_WhenSentEmailIsValid_ShouldReturn200Ok_And_MessageShouldBeCreatedInQueue()
        {
            //arrange
            var client = NewsletterSubscriberPublisherClient.Create();
            var email = _faker.Person.Email;
            var request = RequestMessage.BuildPost(email);
             
            //act
            var response = await client.SendAsync(request);

            //assert
            QueueMessage message = await _queueClient.ReceiveMessageAsync();

            using var scope = new AssertionScope();
            message.MessageText.Should().Be(email);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task test3()
        {
            Assert.True(true);
        }

        public Task InitializeAsync() => Task.CompletedTask;

        public async Task DisposeAsync()
        {
            //Make sure that queue is clear before execute next test
            await _queueClient.DeleteIfExistsAsync();
        }
    }
}
