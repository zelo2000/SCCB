using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SCCB.Services.AuthenticationService
{
    public interface IAuthenticationService
    {
        Task<ClaimsPrincipal> LogIn(string email, string password);

        Task<Guid> CreateUser(string email, string password);
    }
}
