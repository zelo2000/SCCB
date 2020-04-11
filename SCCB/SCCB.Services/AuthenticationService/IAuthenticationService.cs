using System.Security.Claims;
using System.Threading.Tasks;
using SCCB.Core.DTO;

namespace SCCB.Services.AuthenticationService
{
    public interface IAuthenticationService
    {
        /// <summary>
        /// Log in user.
        /// </summary>
        /// <param name="email">Email.</param>
        /// <param name="password">Password.</param>
        /// <returns>User claims.</returns>
        Task<ClaimsPrincipal> LogIn(string email, string password);

        /// <summary>
        /// Create user.
        /// </summary>
        /// <param name="userDto">User.</param>
        /// <returns>Task.</returns>
        Task CreateUser(User userDto);

        /// <summary>
        /// Send change password email.
        /// </summary>
        /// <param name="email">User email address.</param>
        /// <returns>Task.</returns>
        Task SendChangePasswordEmail(string email);

        /// <summary>
        /// Change forgotten password.
        /// </summary>
        /// <param name="token">Change token.</param>
        /// <param name="password">New password.</param>
        /// <returns>Task.</returns>
        Task ChangeForgottenPassword(string token, string password);
    }
}
