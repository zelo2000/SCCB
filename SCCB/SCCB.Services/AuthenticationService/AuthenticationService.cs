using SCCB.Repos.UnitOfWork;
using Microsoft.AspNetCore.Authentication.Cookies;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SCCB.Services.AuthenticationService
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuthenticationService (IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentException(nameof(unitOfWork));
        }

        public Task<Guid> CreateUser(string email, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<ClaimsPrincipal> LogIn(string email, string password)
        {
            var user = await _unitOfWork.Users.FindByEmail(email);

            if (user != null && user.PasswordHash == password)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role)
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                return new ClaimsPrincipal(claimsIdentity);
            }

            throw new ArgumentException("Wrong email or password");
        }
    }
}
