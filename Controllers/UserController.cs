using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CandleApi.Data;
using CandleApi.Data.Repositories;
using CandleApi.Models;
using CandleApi.Services;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace CandleApi.Controllers
{
    [Route("[controller]")]
    public class UserController : Controller
    {
        // private readonly AppDbContext _appDbContext;
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        // public UserController(AppDbContext appDbContext, IUserRepository userRepository, ITokenService tokenService) { 
        //     _appDbContext = appDbContext;
        //     _userRepository = userRepository;
        //     _tokenService = tokenService;
        // }

        public UserController(IUserRepository userRepository, ITokenService tokenService) { 
            // _appDbContext = appDbContext;
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest("Invalid user data.");
            }

            bool created = await _userRepository.CreateNewUser(user);

            // return await _userRepository.CreateNewUser(user);

            // string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);

            // User newUser = new()
            // {
            //     Username = user.Username,
            //     Password = hashedPassword
            // };

            // _appDbContext.Users.Add(newUser);
            // _appDbContext.SaveChanges();
            
            if (!created) {
                return BadRequest("User already exists.");
            }

            return Ok("User has been created");
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] User user)
        {
            // if (user == null) {
            //     return BadRequest("Invalid username or password.");
            // }

            // User selectedUser = _appDbContext.Users.FirstOrDefault(u => u.Username == user.Username);

            // if (selectedUser == null)
            // {
            //     return BadRequest("Invalid username or password.");
            // }

            // bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(user.Password, selectedUser.Password);

            // bool userFound = _userRepository.CheckUsernameExistsAsync(user.Username);
            // bool isPasswordCorrect = false;
            // if (userFound) {
            //     isPasswordCorrect = _userRepository.VerifyUser(user);
            // }

            // if (isPasswordCorrect) {
            //     var token = _tokenService.GenerateToken(user.Username);
            //     return Ok(new {message = "Login successful.", token});
            // } else {
            //     return BadRequest("Invalid username or password.");
            // }

            if (user != null && _userRepository.VerifyUser(user)) {
                var token = _tokenService.GenerateToken(user.Username);
                return Ok(new {message = "Login successful.", token});
            } else {
                return BadRequest("Invalid username or password.");
            }
        }

        [HttpGet("Test")]
        [Authorize]
        public IActionResult Test() {
            return Ok("It worked");
        }

        // [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        // public IActionResult Error()
        // {
        //     return View("Error!");
        // }

        /// <summary>
        /// Retrieves all orders from a specific user
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("Orders/{id}")]
        public IActionResult GetOrdersByUser(Guid id)
        {
            
            // bool userFound = _userRepository.CheckUsernameExistsAsync(username);
            // if (userFound) {
            //     _userRepository.GetOrdersByUser(username);
            // }
            // var user = _appDbContext.Users.FirstOrDefault(u => u.Username == username);
            // if (user == null) {
            //     return BadRequest("Invalid username or password.");
            // }

            // var orders = _appDbContext.Orders
            //     .Where(o => o.UserId == user.Id)
            //     .Include(o => o.OrderItems) // Include the OrderItems collection
            //         .ThenInclude(oi => oi.Item) // Include the associated Item for each OrderItem
            //     .Select(o => new
            //     {
            //         o.OrderId,
            //         o.OrderDate,
            //         // ... other order properties

            //         OrderItems = o.OrderItems.Select(oi => new
            //         {
            //             oi.OrderItemId,
            //             oi.Quantity,
            //             Item = new
            //             {
            //                 oi.Item.ItemId,
            //                 oi.Item.Name,
            //                 // ... other item properties
            //             }
            //         }).ToList()
            //     })
            //     .ToList();

            // return Ok(orders);
            // var orders = _userRepository.GetOrders(id);
            return Ok(_userRepository.GetOrders(id));
        }
    }
}