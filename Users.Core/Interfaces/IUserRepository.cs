using System;
using System.Linq;
using System.Threading.Tasks;
using Users.Core.Models;

namespace Users.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<User> Get(Guid id);
        IQueryable<User> All();
        Task<User> Add(UserCreate user);
        Task<User> Edit(User user);
        Task Remove(Guid userId);
    }
}
