using Microsoft.EntityFrameworkCore;
using CandleApi.Models;
using AutoMapper;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CandleApi.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;

        public UserRepository(AppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        // public bool CheckUsernameExistsAsync(string username)
        // {
        //     return _appDbContext.Users.Any(u => u.Username == username);
        // }


        public async Task<bool> CreateNewUser(User user) {
            bool userExists = _appDbContext.Users.Any(u => u.Username == user.Username);

            if (userExists) {
                return false;
            }

            try {
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);

                User newUser = new()
                {
                    Username = user.Username,
                    Password = hashedPassword,
                    // Orders = new List<Order>()
                };

                _appDbContext.Users.Add(newUser);
                await _appDbContext.SaveChangesAsync();

                return true;
            } catch (Exception ex){
                Console.WriteLine("An exception occured: " + ex.Message);
                throw;
            }
        }

        public bool VerifyUser(User user)
        {
            var selectedUser = _appDbContext.Users.FirstOrDefault(u => u.Username == user.Username);

            if (selectedUser == null)
            {
                return false;
            }

            return BCrypt.Net.BCrypt.Verify(user.Password, selectedUser.Password);
        }

        public List<OrderDto> GetOrders(Guid id)
        {
            //bool userFound = _appDbContext.Users.Any(u => u.Id == id);
            //if (!userFound)
            //{
            //    return new List<Order>();
            //}
            //var user = _appDbContext.Users.FirstOrDefault(u => u.Id == id);

            // eager loading of related data
            var orders = _appDbContext.Orders
                .Where(o => o.UserId == id)
                //.Include(o => o.OrderItems) // Include the OrderItems collection
                //    .ThenInclude(oi => oi.Item) // Include the associated Item for each OrderItem
                //.Select(o => new
                //{
                //    o.OrderId,
                //    o.OrderDate,
                //    // ... other order properties

                //    OrderItems = o.OrderItems.Select(oi => new
                //    {
                //        oi.OrderItemId,
                //        oi.Quantity,
                //        Item = new
                //        {
                //            oi.Item.ItemId,
                //            oi.Item.Name,
                //            // ... other item properties
                //        }
                //    }).ToList()
                //})
                .ToList();

            var orderDtos = orders.Select(o => _mapper.Map<OrderDto>(o)).ToList();

            return orderDtos;
        }



        //public async Task<IEnumerable<OrderDto>> GetOrders(Guid id)
        //{
        //    var orders = await _appDbContext.Orders
        //        .Where(order => order.UserId == id)
        //        .ToListAsync();

        //    return orders.Select(_mapper.Map<OrderDto>);
        //}


        public bool CheckAdminStatus(string username)
        {
            var user = _appDbContext.Users.FirstOrDefault(u => u.Username == username);
            if (user != null)
            {
                return user.IsAdmin;
            }

            return false;
        }
    }
}