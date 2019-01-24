using Microsoft.EntityFrameworkCore;
using Users.Core.Interfaces;
using Users.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Users.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UsersContext _db;

        public UserRepository(UsersContext db)
        {
            _db = db;
        }

        public async Task<User> Get(Guid id)
        {
            return await _db.Users.FindAsync(id);
        }

        public IQueryable<User> All()
        {
            return _db.Users;
        }

        public async Task<User> Add(UserCreate user)
        {
            bool exist = await _db.Users.AnyAsync(p => p.Email == user.Email);

            if (exist)
            {
                throw new Exception("user with same Email already exist.");
            }

            User dbUser = new User()
            {
                GivvenName = user.GivvenName,
                FamilyName = user.FamilyName,
                Email = user.Email,
                Created = DateTime.Now
            }; 

            await _db.Users.AddAsync(dbUser);
            await _db.SaveChangesAsync();
            return dbUser;
        }

        public async Task<User> Edit(User user)
        {
            User dbUser = await _db.Users.FindAsync(user.Id);

            if (dbUser == null)
            {
                throw new NullReferenceException("user not found.");
            }

            dbUser.GivvenName = user.GivvenName;
            dbUser.FamilyName = user.FamilyName;
            dbUser.Email = user.Email;

            await _db.SaveChangesAsync();
            return dbUser;
        }

        public async Task Remove(Guid id)
        {
            _db.Users.Remove(_db.Users.Find(id));
            await _db.SaveChangesAsync();
        }
    }
}
