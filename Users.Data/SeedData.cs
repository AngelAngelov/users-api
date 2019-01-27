using System;
using System.Linq;
using Users.Core.Models;

namespace Users.Data
{
    public static class SeedData
    {
        public static void EnsureSeedData(this UsersContext db)
        {
            //Make sure db exist
            //if it does not it is created
            db.Database.EnsureCreated();

            //uncomment this to put some sample data if table Users is empty
            //if (!db.Users.Any())
            //{
            //    db.Users.Add(new User()
            //    {
            //        Id = Guid.NewGuid(),
            //        GivvenName = "John",
            //        FamilyName = "Doe",
            //        Email = "jonny_doe@gmail.com",
            //        Created = DateTime.Now
            //    });

            //    db.SaveChanges();
            //}
        }
    }
}
