using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using SCCB.Core.Constants;
using SCCB.Core.Helpers;
using SCCB.Core.Settings;
using SCCB.Repos.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SCCB.Services.AuthenticationService
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly PasswordProcessor passwordProcessor;

        public AuthenticationService(IMapper mapper, IUnitOfWork unitOfWork,
            IOptions<HashGenerationSetting> hashGenerationSetting)
        {
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
            _unitOfWork = unitOfWork ?? throw new ArgumentException(nameof(unitOfWork));
            passwordProcessor = new PasswordProcessor(hashGenerationSetting.Value);
        }

        /// <inheritdoc />
        public async Task CreateUser(Core.DTO.User userDto)
        {
            var user = await _unitOfWork.Users.FindByEmailAsync(userDto.Email);

            if (user == null)
            {
                userDto.PasswordHash = passwordProcessor.GetPasswordHash(userDto.PasswordHash);
                user = _mapper.Map<DAL.Entities.User>(userDto);
                user.Role = Roles.NotApprovedUser;
                await _unitOfWork.Users.AddAsync(user);
                await _unitOfWork.CommitAsync();
            }
            else
            {
                throw new ArgumentException("User already exists");
            }
        }

        /// <inheritdoc />
        public async Task<ClaimsPrincipal> LogIn(string email, string password)
        {
            var user = await _unitOfWork.Users.FindByEmailAsync(email);

            if (user != null && user.PasswordHash == passwordProcessor.GetPasswordHash(password))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimKeys.FirstName, user.FirstName),
                    new Claim(ClaimKeys.LastName, user.LastName),
                    new Claim(ClaimKeys.Email, user.Email),
                    new Claim(ClaimKeys.Role, user.Role)
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                return new ClaimsPrincipal(claimsIdentity);
            }

            throw new ArgumentException("Wrong email or password");
        }
    }
}
