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

        /// <summary>
        /// Send change password email
        /// </summary>
        /// <param name="email">User email address</param>
        Task SendChangePasswordEmail(string email);

        /// <summary>
        /// Change forgotten password 
        /// </summary>
        /// <param name="token">Change token</param>
        /// <param name="password">New password</param>
        Task ChangeForgottenPassword(string token, string password);
    }
}
