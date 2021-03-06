using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SCCB.Core.Constants;
using SCCB.Core.Helpers;
using SCCB.Core.Settings;
using SCCB.DAL.Entities;
using SCCB.Repos.UnitOfWork;
using SCCB.Repos.Users;
using SCCB.Services.AuthenticationService;
using SCCB.Services.EmailService;

namespace SCCB.Services.Tests
{
    public class AuthenticationServiceTests
    {
        private readonly string _registeredUserPassword = "Pa$$word";
        private readonly string _newUserPassword = "Pa@@word";

        private IAuthenticationService _service;

        private IMapper _mapper;
        private IOptions<HashGenerationSetting> _hashGenerationSetting;

        private Mock<IUserRepository> _repositoryMock;
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IEmailService> _emailServiceMock;
        private Mock<ILogger<AuthenticationService.AuthenticationService>> _logMock;

        private User _registeredUser;
        private User _registeredUser2;
        private User _newUser;
        private List<User> _registeredUsers;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var serviceMapProfile = new ServiceMapProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(serviceMapProfile));
            _mapper = new Mapper(configuration);
        }

        [SetUp]
        public void SetUp()
        {
            _hashGenerationSetting = Options.Create(new HashGenerationSetting()
            {
                Salt = "EWEM9nXVuQHIWiBzPOEj9A==",
                IterationCount = 10000,
                BytesNumber = 32,
            });

            var passwordProcessor = new PasswordProcessor(_hashGenerationSetting.Value);

            _registeredUser = new User()
            {
                Id = Guid.NewGuid(),
                FirstName = "Firstname",
                LastName = "Lastname",
                Email = "registered@gmail.com",
                PasswordHash = passwordProcessor.GetPasswordHash(_registeredUserPassword),
                Role = Roles.Student,
                ChangePasswordToken = Guid.NewGuid().ToString(),
                ExpirationChangePasswordTokenDate = DateTime.Now.AddHours(24),
            };

            _registeredUser2 = new User()
            {
                Id = Guid.NewGuid(),
                FirstName = "Firstname2",
                LastName = "Lastname2",
                Email = "registered2@gmail.com",
                PasswordHash = passwordProcessor.GetPasswordHash(_registeredUserPassword),
                Role = Roles.Student,
                ChangePasswordToken = Guid.NewGuid().ToString(),
                ExpirationChangePasswordTokenDate = DateTime.Now.AddHours(-24),
            };

            _registeredUsers = new List<User> { _registeredUser, _registeredUser2 };

            _newUser = new User()
            {
                Id = Guid.NewGuid(),
                FirstName = "Firstname",
                LastName = "Lastname",
                Email = "new@gmail.com",
                PasswordHash = passwordProcessor.GetPasswordHash(_newUserPassword),
                Role = Roles.Student,
            };

            #region setup mocks
            _repositoryMock = new Mock<IUserRepository>();
            _repositoryMock.Setup(repo => repo.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((User)null);
            _repositoryMock.Setup(repo => repo.FindByEmailAsync(_registeredUser.Email)).ReturnsAsync(_registeredUser);
            _repositoryMock.Setup(repo => repo.AddAsync(It.IsAny<User>())).Returns(Task.FromResult(Guid.Empty));
            _repositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync((Expression<Func<User, bool>> expr) => _registeredUsers.AsQueryable().Where(expr).SingleOrDefault());
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _unitOfWorkMock.Setup(uow => uow.Users).Returns(_repositoryMock.Object);
            _unitOfWorkMock.Setup(uow => uow.CommitAsync());

            _emailServiceMock = new Mock<IEmailService>();
            _emailServiceMock.Setup(service => service.SendChangePasswordEmail(It.IsAny<Core.DTO.EmailWithToken>()));

            _logMock = new Mock<ILogger<AuthenticationService.AuthenticationService>>();

            #endregion

            _service = new AuthenticationService.AuthenticationService(
                _mapper, _unitOfWorkMock.Object, _hashGenerationSetting, _emailServiceMock.Object, _logMock.Object);
        }

        [Test]
        public async Task LogIn_RegisteredUser_ClaimsPrincipal()
        {
            var result = await _service.LogIn(_registeredUser.Email, _registeredUserPassword);

            Assert.That(
                result.Claims.Where(c => c.Type == ClaimKeys.Email).Select(c => c.Value).SingleOrDefault(),
                Is.EqualTo(_registeredUser.Email));

            Assert.That(
                result.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).SingleOrDefault(),
                Is.EqualTo(_registeredUser.Role));

            Assert.That(
                result.Claims.Where(c => c.Type == ClaimKeys.FirstName).Select(c => c.Value).SingleOrDefault(),
                Is.EqualTo(_registeredUser.FirstName));

            Assert.That(
                result.Claims.Where(c => c.Type == ClaimKeys.LastName).Select(c => c.Value).SingleOrDefault(),
                Is.EqualTo(_registeredUser.LastName));

            _repositoryMock.Verify(repo => repo.FindByEmailAsync(_registeredUser.Email));
        }

        [Test]
        public void LogIn_NotRegistered_ArgumentException()
        {
            Assert.That(
                () => _service.LogIn(_newUser.Email, _newUser.PasswordHash),
                Throws.ArgumentException.With.Message.EqualTo("Wrong email or password"));
        }

        [Test]
        public void LogIn_WrongPassword_ArgumentException()
        {
            Assert.That(
                () => _service.LogIn(_registeredUser.Email, "WrongPass"),
                Throws.ArgumentException.With.Message.EqualTo("Wrong email or password"));
        }

        [Test]
        public void CreateUser_RegisteredUser_ArgumentException()
        {
            var userDto = _mapper.Map<Core.DTO.User>(_registeredUser);

            Assert.That(
                () => _service.CreateUser(userDto),
                Throws.ArgumentException.With.Message.EqualTo("User already exists"));
        }

        [Test]
        public async Task CreateUser_NewUser_UserRepositoryAddCalled()
        {
            var userDto = _mapper.Map<Core.DTO.User>(_newUser);
            userDto.Password = _newUserPassword;

            await _service.CreateUser(userDto);

            _repositoryMock.Verify(repo => repo.AddAsync(It.Is<User>(user =>
                user.FirstName == _newUser.FirstName &&
                user.LastName == _newUser.LastName &&
                user.Email == _newUser.Email &&
                user.PasswordHash == _newUser.PasswordHash)));

            _unitOfWorkMock.Verify(ouw => ouw.CommitAsync());
        }

        [Test]
        public void ChangeForgottenPassword_WrongToken_AccessViolationException()
        {
            var invalidToken = Guid.NewGuid().ToString();

            Assert.That(
                () => _service.ChangeForgottenPassword(invalidToken, _newUserPassword),
                Throws.Exception.TypeOf<AccessViolationException>().With.Message.EqualTo("Token not found or expired"));
        }

        [Test]
        public void ChangeForgottenPassword_ExpiredToken_AccessViolationException()
        {
            Assert.That(
                () => _service.ChangeForgottenPassword(_registeredUser2.ChangePasswordToken, _newUserPassword),
                Throws.Exception.TypeOf<AccessViolationException>().With.Message.EqualTo("Token not found or expired"));
        }

        [Test]
        public void ChangeForgottenPassword_ValidToken_ChangedPassword()
        {
            _service.ChangeForgottenPassword(_registeredUser.ChangePasswordToken, _newUserPassword);

            _repositoryMock.Verify(repo => repo.Update(It.Is<User>(user =>
                user.Id == _registeredUser.Id &&
                user.PasswordHash == _newUser.PasswordHash)));

            _unitOfWorkMock.Verify(uow => uow.CommitAsync());
        }

    }
}