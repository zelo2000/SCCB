using NUnit.Framework;
using Moq;
using SCCB.Services.AuthenticationService;
using SCCB.DAL.Entities;
using SCCB.Repos.Users;
using SCCB.Repos.UnitOfWork;
using SCCB.Core.Settings;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace SCCB.Services.Tests
{
    public class AuthenticationServiceTests
    {
        private IAuthenticationService _service;

        private User _registeredUser;

        private User _newUser;

        [OneTimeSetUp]
        public void Setup()
        {
            _registeredUser = new User()
            {
                Email = "registered@gmail.com",
                PasswordHash = "Pa$$word",
                Role = "Student"
            };

            _newUser = new User()
            {
                Email = "new@gmail.com",
                PasswordHash = "Pa@@word",
                Role = "Student"
            };

            #region setup mocks
            var repositoryMock = new Mock<IUserRepository>();
            repositoryMock.Setup(repo => repo.FindByEmail(It.IsAny<string>())).ReturnsAsync((User)null);
            repositoryMock.Setup(repo => repo.FindByEmail(_registeredUser.Email)).ReturnsAsync(_registeredUser);
            repositoryMock.Setup(repo => repo.Add(It.IsAny<User>())).Returns<User>(x => x);

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(uow => uow.Users).Returns(repositoryMock.Object);

            var hashGenerationSetting = Options.Create(new HashGenerationSetting()
            {
                Salt = "EWEM9nXVuQHIWiBzPOEj9A==",
                IterationCount = 10000,
                BytesNumber = 32
            });
            #endregion

            _service = new AuthenticationService.AuthenticationService(unitOfWorkMock.Object, hashGenerationSetting);
        }

        //TODO: Write some tests
        [Test]
        public async Task Test1()
        {
            Assert.Pass();
        }
    }
}