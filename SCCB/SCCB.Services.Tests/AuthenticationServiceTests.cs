using NUnit.Framework;
using Moq;
using SCCB.Services.AuthenticationService;
using SCCB.DAL.Entities;
using SCCB.Repos.Users;
using SCCB.Repos.UnitOfWork;
using SCCB.Core.Settings;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using SCCB.Core.Helpers;
using System.Linq;

namespace SCCB.Services.Tests
{
    public class AuthenticationServiceTests
    {
        private IAuthenticationService _service;

        private Mock<IUserRepository> _repositoryMock;

        private Mock<IUnitOfWork> _unitOfWorkMock;

        private readonly string _registeredUserPassword = "Pa$$word";

        private User _registeredUser;

        private readonly string _newUserPassword = "Pa@@word";

        private User _newUser;

        [OneTimeSetUp]
        public void Setup()
        {
            var hashGenerationSetting = Options.Create(new HashGenerationSetting()
            {
                Salt = "EWEM9nXVuQHIWiBzPOEj9A==",
                IterationCount = 10000,
                BytesNumber = 32
            });

            var passwordProcessor = new PasswordProcessor(hashGenerationSetting.Value);

            _registeredUser = new User()
            {
                Email = "registered@gmail.com",
                PasswordHash = passwordProcessor.GetPasswordHash(_registeredUserPassword),
                Role = "Student"
            };

            _newUser = new User()
            {
                Email = "new@gmail.com",
                PasswordHash = passwordProcessor.GetPasswordHash(_newUserPassword),
                Role = "Student"
            };

            #region setup mocks
            _repositoryMock = new Mock<IUserRepository>();
            _repositoryMock.Setup(repo => repo.FindByEmail(It.IsAny<string>())).ReturnsAsync((User)null);
            _repositoryMock.Setup(repo => repo.FindByEmail(_registeredUser.Email)).ReturnsAsync(_registeredUser);
            _repositoryMock.Setup(repo => repo.Add(It.IsAny<User>())).Returns<User>(x => x);

            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _unitOfWorkMock.Setup(uow => uow.Users).Returns(_repositoryMock.Object);
            _unitOfWorkMock.Setup(uow => uow.CommitAsync());
            #endregion

            _service = new AuthenticationService.AuthenticationService(_unitOfWorkMock.Object, hashGenerationSetting);
        }

        [Test]
        public async Task LogIn_RegisteredUser_ClaimsPrincipal()
        {
            var result = await _service.LogIn(_registeredUser.Email, _registeredUserPassword);

            Assert.That(result.Claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).FirstOrDefault(),
                Is.EqualTo(_registeredUser.Email));

            Assert.That(result.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).FirstOrDefault(),
                Is.EqualTo(_registeredUser.Role));

            _repositoryMock.Verify(repo => repo.FindByEmail(_registeredUser.Email));
        }

        [Test]
        public void LogIn_NotRegistered_ArguemntException()
        {
            Assert.That(() => _service.LogIn(_newUser.Email, _newUser.PasswordHash),
                Throws.ArgumentException.With.Message.EqualTo("Wrong email or password"));
        }

        [Test]
        public void LogIn_WrongPassword_ArgumentException()
        {
            Assert.That(() => _service.LogIn(_registeredUser.Email, "WrongPass"),
                Throws.ArgumentException.With.Message.EqualTo("Wrong email or password"));
        }

        [Test]
        public void CreateUser_RegisteredUser_ArgumentException()
        {
            Assert.That(() => _service.CreateUser(_registeredUser.Email, _registeredUserPassword, _registeredUser.Role),
                Throws.ArgumentException.With.Message.EqualTo("User already exists"));
        }

        [Test]
        public async Task CreateUser_NewUser_UserRepositoryAddCalled()
        {
            await _service.CreateUser(_newUser.Email, _newUserPassword, _newUser.Role);

            _repositoryMock.Verify(repo => repo.Add(It.Is<User>(
                user => user.Email == _newUser.Email &&
                user.PasswordHash == _newUser.PasswordHash &&
                user.Role == _newUser.Role
            )));

            _unitOfWorkMock.Verify(ouw => ouw.CommitAsync());
        }
    }
}