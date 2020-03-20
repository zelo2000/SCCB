using SCCB.Core.DTO;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SCCB.Services.AuthenticationService
{
    public interface IAuthenticationService
    {
        /// <summary>
        /// Log in user
        /// </summary>
        /// <param name="email">email</param>
        /// <param name="password">password</param>
        /// <returns>User claims</returns>
        Task<ClaimsPrincipal> LogIn(string email, string password);

        /// <summary>
        /// Create user
        /// </summary>
        /// <param name="userDto">User</param>
        Task CreateUser(User userDto);
    }
}
