using Xunit;

namespace Users.Api.IntegrationTest.Fixtures
{
    [CollectionDefinition("SystemCollection")]
    public class TestCollection : ICollectionFixture<TestContext>
    {
    }
}
