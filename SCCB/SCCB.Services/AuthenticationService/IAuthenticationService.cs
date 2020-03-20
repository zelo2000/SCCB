using SCCB.Core.DTO;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SCCB.Services.AuthenticationService
{
    public interface IAuthenticationService
    {
        Task<ClaimsPrincipal> LogIn(string email, string password);

        Task CreateUser(User userDto);
    }
}
