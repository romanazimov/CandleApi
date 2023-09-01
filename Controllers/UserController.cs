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
        private readonly ILogger<UserController> _logger;
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public UserController(ILogger<UserController> logger, IUserRepository userRepository, ITokenService tokenService) {
            _logger = logger;
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
            
            if (!created) {
                return BadRequest("User already exists.");
            }

            return Ok("User has been created");
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] User user)
        {
            if (user != null && user.Username != null && _userRepository.VerifyUser(user)) {
                bool isAdmin = _userRepository.CheckAdminStatus(user.Username);
                var token = _tokenService.GenerateToken(user.Username, isAdmin);
                return Ok(new {message = "Login successful.", token});
            } else {
                return BadRequest("Invalid username or password.");
            }
        }

        [HttpGet("Test")]
        [Authorize(Roles = "Administrator")]
        public IActionResult Test() {
            return Ok("It worked");
        }


        /// <summary>
        /// Retrieves all orders from a specific user
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("Orders/{id}")]
        public IActionResult GetOrdersByUser(Guid id)
        {
            return Ok(_userRepository.GetOrders(id));
        }
    }
}