namespace NewsletterSubscriberPublisher.IntegrationTests
{
    [Collection(nameof(TestsCollection))]
    public class Test
    {
        public Test(TestsStartup testsStartup)
        {
            
        }

        [Fact]
        public async Task test2()
        {
           Assert.True(true);
        }

        [Fact]
        public async Task test3()
        {
            Assert.True(true);
        }
    }
}
