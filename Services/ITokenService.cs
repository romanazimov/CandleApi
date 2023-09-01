using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CandleApi.Services
{
    public interface ITokenService
    {
        public string GenerateToken(string username, bool isAdmin);
    }
}