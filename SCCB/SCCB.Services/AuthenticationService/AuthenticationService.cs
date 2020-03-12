using Microsoft.AspNetCore.Authentication.Cookies;
using SCCB.DAL.Entities;
using SCCB.Repos.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SCCB.Core.Settings;
using SCCB.Core.Helpers;

namespace SCCB.Services.AuthenticationService
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly PasswordProcessor passwordProcessor;

        public AuthenticationService(IUnitOfWork unitOfWork, IOptions<HashGenerationSetting> hashGenerationSetting)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentException(nameof(unitOfWork));
            passwordProcessor = new PasswordProcessor(hashGenerationSetting.Value);
        }

        public async Task CreateUser(string email, string password, string role)
        {
            var user = await _unitOfWork.Users.FindByEmail(email);

            if (user == null)
            {
                user = new User()
                {
                    Email = email,
                    PasswordHash = passwordProcessor.GetPasswordHash(password),
                    Role = role
                };

                _unitOfWork.Users.Add(user);
                await _unitOfWork.CommitAsync();
            }
            else
            {
                throw new ArgumentException("User already exists");
            }
        }

        public async Task<ClaimsPrincipal> LogIn(string email, string password)
        {
            var user = await _unitOfWork.Users.FindByEmail(email);

            if (user != null && user.PasswordHash == passwordProcessor.GetPasswordHash(password))
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
