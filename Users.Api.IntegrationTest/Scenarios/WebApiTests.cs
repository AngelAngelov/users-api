using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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
    public class WebApiTests : IDisposable
    {
        private readonly TestContext _context;

        private User _testUser;

        public WebApiTests(TestContext context)
        {
            _context = context;
            this._testUser = new User()
            {
                Id = Guid.NewGuid(),
                GivvenName = "John",
                FamilyName = "Doe",
                Email = "jonny_doe@gmail.com",
                Created = DateTime.Now
            };
        }

        [Fact]
        public async Task GetAllUser()
        {
            //Setup db data
            await _context.DbContext.Users.AddAsync(_testUser);
            await _context.DbContext.SaveChangesAsync();

            //Api call
            HttpResponseMessage response = await _context.Client.GetAsync("/api/user/all");

            //Check response status code
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            string responseStr = await response.Content.ReadAsStringAsync();
            responseStr.Should().NotBeEmpty();

            List<User> result = JsonConvert.DeserializeObject<List<User>>(responseStr);

            //Check if returned list length is one, as we have one user in db
            result.Should().NotBeNull();
            result.Count.Should().Equals(1);
        }

        [Fact]
        public async Task GetUserById()
        {
            //Setup db data
            await _context.DbContext.Users.AddAsync(_testUser);
            await _context.DbContext.SaveChangesAsync();

            //Api call
            HttpResponseMessage response = await _context.Client.GetAsync($"/api/user/{_testUser.Id}");

            //Check response status code
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            string responseStr = await response.Content.ReadAsStringAsync();
            responseStr.Should().NotBeEmpty();

            User result = JsonConvert.DeserializeObject<User>(responseStr);

            //Check if returned user equals original user
            result.Should().NotBeNull();
            result.Should().Equals(_testUser);
        }

        [Fact]
        public async Task EditUser()
        {
            //Setup db data
            await _context.DbContext.Users.AddAsync(_testUser);
            await _context.DbContext.SaveChangesAsync();

            _testUser.GivvenName = "Kozmi";
            _testUser.FamilyName = "Motti";

            //Api call
            string postData = JsonConvert.SerializeObject(_testUser);
            var response = await _context.Client.PutAsync("/api/user", new StringContent(postData, Encoding.UTF8, "application/json"));

            //Check Status code
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            string responseStr = await response.Content.ReadAsStringAsync();
            User editedUser = JsonConvert.DeserializeObject<User>(responseStr);

            //Check returned user names
            editedUser.GivvenName.Should().Be("Kozmi");
            editedUser.FamilyName.Should().Be("Motti");

            //Check db user names
            User dbUser = await _context.DbContext.Users.FindAsync(editedUser.Id);

            editedUser.GivvenName.Should().Equals(dbUser.GivvenName);
            editedUser.FamilyName.Should().Equals(dbUser.FamilyName);
        }

        [Fact]
        public async Task CreateUser()
        {
            UserCreate newUser = new UserCreate()
            {
                Email = "test@abv.bg",
                GivvenName = "Peter",
                FamilyName = "Johnson"
            };

            //Api call
            string postData = JsonConvert.SerializeObject(newUser);
            var response = await _context.Client.PostAsync("/api/user", new StringContent(postData, Encoding.UTF8, "application/json"));

            //Check status code
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            string responseStr = await response.Content.ReadAsStringAsync();
            User createdUser = JsonConvert.DeserializeObject<User>(responseStr);

            //Check if user props are filled
            createdUser.Id.Should().NotBe(default(Guid));
            createdUser.Created.Should().HaveValue();

            User dbUser = await _context.DbContext.Users.FindAsync(createdUser.Id);

            //Check if returned user is same as db user
            dbUser.Should().NotBeNull();
            dbUser.Should().Equals(createdUser);
        }

        [Fact]
        public async Task DeleteUser()
        {
            //Setup db data
            await _context.DbContext.Users.AddAsync(_testUser);
            await _context.DbContext.SaveChangesAsync();

            //Api call
            var response = await _context.Client.DeleteAsync($"/api/user/{_testUser.Id}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            //Make sure user does not exist in db
            bool exist = _context.DbContext.Users.Any(x => x.Id == _testUser.Id);
            exist.Should().BeFalse();
        }

        public void Dispose()
        {
            //Cleanup database after every test
            _context.DbContext.Users.RemoveRange(_context.DbContext.Users);
            _context.DbContext.SaveChanges();
        }
    }
}
