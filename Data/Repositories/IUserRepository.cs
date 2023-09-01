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
        //Task<IEnumerable<OrderDto>> GetOrders(Guid id);
        List<OrderDto> GetOrders(Guid id);
        bool CheckAdminStatus(string username);
    }
}