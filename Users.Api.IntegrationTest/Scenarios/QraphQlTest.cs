using FluentAssertions;
using GraphQL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Users.Api.IntegrationTest.Fixtures;
using Users.Core.Models;
using Xunit;

namespace Users.Api.IntegrationTest.Scenarios
{
    [Collection("SystemCollection")]
    public class QraphQlTests : IDisposable
    {
        private readonly TestContext _context;

        public QraphQlTests(TestContext context)
        {
            _context = context;
        }

        [Fact]
        public async Task GetAllUsersTest()
        {
            User user1 = new User()
            {
                Email = "test@abv.bg",
                GivvenName = "Peter",
                FamilyName = "Johnson"
            };

            User user2 = new User()
            {
                Email = "tes2t@abv.bg",
                GivvenName = "Alex",
                FamilyName = "Talles"
            };


            //Setup db data
            await _context.DbContext.Users.AddAsync(user1);
            await _context.DbContext.Users.AddAsync(user2);
            await _context.DbContext.SaveChangesAsync();

            //Api call
            var query = new GraphQLQuery()
            {
                Query = "{users {id,givvenName,familyName,email,created}}"
            };
            string postData = JsonConvert.SerializeObject(query);
            var response = await _context.Client.PostAsync("/graphql", new StringContent(postData, Encoding.UTF8, "application/json"));

            //Check status code
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            //Parse response
            string responseStr = await response.Content.ReadAsStringAsync();
            GraphListResponse graphQlResponse = JsonConvert.DeserializeObject<GraphListResponse>(responseStr);

            //Compare response data and original data
            graphQlResponse.Data.users.Count.Should().Be(2);
            user1.Should().Equals(graphQlResponse.Data.users[0]);
            user2.Should().Equals(graphQlResponse.Data.users[1]);
        }

        [Fact]
        public async Task GetUserByIdTest()
        {
            User user = new User()
            {
                Email = "test@abv.bg",
                GivvenName = "Peter",
                FamilyName = "Johnson"
            };

            //Setup db data
            await _context.DbContext.Users.AddAsync(user);
            await _context.DbContext.SaveChangesAsync();

            //Api call
            var query = new GraphQLQuery()
            {
                Query = $"{{user(id: \"{user.Id}\") {{id,givvenName,familyName,email,created}} }}"
            };
            string postData = JsonConvert.SerializeObject(query);
            var response = await _context.Client.PostAsync("/graphql", new StringContent(postData, Encoding.UTF8, "application/json"));

            //Check status code
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            //Parse response
            string responseStr = await response.Content.ReadAsStringAsync();
            GraphUserResponse graphQlResponse = JsonConvert.DeserializeObject<GraphUserResponse>(responseStr);

            //Compare response data and original data
            user.Should().Equals(graphQlResponse.Data.user);
        }

        public void Dispose()
        {
            //Cleanup database after every test
            _context.DbContext.Users.RemoveRange(_context.DbContext.Users);
            _context.DbContext.SaveChanges();
        }
    }

    class GraphListResponse
    {
        public QueryListResult Data { get; set; }
    }

    class QueryListResult
    {
        public List<User> users { get; set; }
    }

    class GraphUserResponse
    {
        public QueryUserResult Data { get; set; }
    }

    class QueryUserResult
    {
        public User user { get; set; }
    }
}
