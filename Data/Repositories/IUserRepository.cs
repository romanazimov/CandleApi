using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CandleApi.Models;

namespace CandleApi.Data.Repositories
{
    public interface IUserRepository
    {
        // Task<User> FindUserByIdAsync(Guid id);
        Task<bool> CreateNewUser(User user);
        bool VerifyUser(User user);
        ICollection<Order> GetOrders(Guid id);
    }
}