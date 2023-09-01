using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using CandleApi.Models;

namespace CandleApi.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration) {
            _configuration = configuration;
        }

        public string GenerateToken(string username, bool isAdmin)
        {
            // Create claims for the user
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, isAdmin ? "Administrator" : "User"),
                // You can add additional claims like roles, permissions, etc.
            };

            string? jwtKey = _configuration["JwtSettings:Key"];

            SymmetricSecurityKey securityKey = string.IsNullOrEmpty(jwtKey)
                ? new SymmetricSecurityKey(Encoding.UTF8.GetBytes("backup_candle_key"))
                : new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

            var key = securityKey;
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1), // Token expiration time
                signingCredentials: creds);
            
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}