using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SCCB.Core.Constants;
using SCCB.Core.DTO;
using SCCB.Core.Helpers;
using SCCB.Core.Settings;
using SCCB.Repos.UnitOfWork;
using SCCB.Services.EmailService;

namespace SCCB.Services.AuthenticationService
{
    /// <summary>
    /// Authentication service.
    /// </summary>
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly PasswordProcessor passwordProcessor;
        private readonly IEmailService _emailService;
        private readonly ILogger<AuthenticationService> _log;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationService"/> class.
        /// </summary>
        /// <param name="mapper">Mapper instance.</param>
        /// <param name="unitOfWork">UnitOfWork instance.</param>
        /// <param name="hashGenerationSetting">HashGenerationSetting instance.</param>
        /// <param name="emailService">EmailService instance.</param>
        /// <param name="log">Logger.</param>
        public AuthenticationService(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IOptions<HashGenerationSetting> hashGenerationSetting,
            IEmailService emailService,
            ILogger<AuthenticationService> log)
        {
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
            _unitOfWork = unitOfWork ?? throw new ArgumentException(nameof(unitOfWork));
            passwordProcessor = new PasswordProcessor(hashGenerationSetting.Value);
            _emailService = emailService ?? throw new ArgumentException(nameof(emailService));
            _log = log ?? throw new ArgumentException(nameof(log));
        }

        /// <inheritdoc />
        public async Task CreateUser(Core.DTO.User userDto)
        {
            var user = await _unitOfWork.Users.FindByEmailAsync(userDto.Email);

            if (user == null)
            {
                user = _mapper.Map<DAL.Entities.User>(userDto);
                user.PasswordHash = passwordProcessor.GetPasswordHash(userDto.Password);
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
                    new Claim(ClaimKeys.Id, user.Id.ToString()),
                    new Claim(ClaimKeys.FirstName, user.FirstName),
                    new Claim(ClaimKeys.LastName, user.LastName),
                    new Claim(ClaimKeys.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role),
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                return new ClaimsPrincipal(claimsIdentity);
            }

            throw new ArgumentException("Wrong email or password");
        }

        /// <inheritdoc />
        public async Task SendChangePasswordEmail(string email)
        {
            try
            {
                var user = await _unitOfWork.Users.FindByEmailAsync(email);

                if (user != null)
                {
                    var resetToken = Guid.NewGuid().ToString();
                    user.ChangePasswordToken = resetToken;
                    user.ExpirationChangePasswordTokenDate = DateTime.Now.AddHours(24);
                    await _unitOfWork.CommitAsync();

                    _emailService.SendChangePasswordEmail(new EmailWithToken
                    {
                        EmailAddress = email,
                        Token = resetToken,
                    });
                }
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
            }
        }

        /// <inheritdoc />
        public async Task ChangeForgottenPassword(string token, string password)
        {
            var user = await _unitOfWork.Users.GetAsync(x => x.ChangePasswordToken == token &&
                                          x.ExpirationChangePasswordTokenDate >= DateTime.UtcNow);

            if (user == null)
            {
                throw new AccessViolationException("Token not found or expired");
            }

            var passwordHash = passwordProcessor.GetPasswordHash(password);
            user.PasswordHash = passwordHash;
            user.ChangePasswordToken = null;
            user.ExpirationChangePasswordTokenDate = null;

            _unitOfWork.Users.Update(user);

            await _unitOfWork.CommitAsync();
        }
    }
}
