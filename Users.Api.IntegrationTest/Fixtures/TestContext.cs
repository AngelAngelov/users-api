using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using Users.Data;

namespace Users.Api.IntegrationTest.Fixtures
{
    public class TestContext
    {
        public HttpClient Client { get; set; }
        public UsersContext DbContext { get; set; }
        private TestServer _server { get; set; }

        public TestContext()
        {
            SetupClient();
        }

        private void SetupClient()
        {
            //Load test configuration file
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

            WebHostBuilder webHostBuilder = new WebHostBuilder();
            //pass the test config to the host builder
            webHostBuilder.UseConfiguration(configuration);
            //use Users.Api Startup
            webHostBuilder.UseStartup<Startup>();

            _server = new TestServer(webHostBuilder);
            Client = _server.CreateClient();


            var optionsBuilder = new DbContextOptionsBuilder<UsersContext>();
            optionsBuilder.UseSqlite(configuration.GetConnectionString("SQLite"));
            DbContext = new UsersContext(optionsBuilder.Options);
        }
    }
}
