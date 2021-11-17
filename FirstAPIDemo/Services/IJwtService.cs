using FirstAPIDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstAPIDemo.Services
{
    public interface IJwtService
    {
        string JwtGen(User user, List<string> userRoles);
    }
}
