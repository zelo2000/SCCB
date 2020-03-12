using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SCCB.Services.AuthenticationService
{
    public interface IAuthenticationService
    {
        Task<ClaimsPrincipal> LogIn(string email, string password);

        Task<ClaimsPrincipal> CreateUser(string email, string password, string role);
    }
}
