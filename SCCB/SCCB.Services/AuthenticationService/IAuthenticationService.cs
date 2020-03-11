using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SCCB.Services.AuthenticationService
{
    public interface IAuthenticationService
    {
        Task<ClaimsPrincipal> LogIn(string email, string password);

        Task<Guid> CreateUser(string email, string password);
    }
}
