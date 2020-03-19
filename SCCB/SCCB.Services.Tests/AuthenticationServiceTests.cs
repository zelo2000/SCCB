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
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Collections.Generic;
using System;

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
        public async Task LogIn_AlreadyRegistered_UserId()
        {
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, _registeredUser.Email),
                    new Claim(ClaimTypes.Role, _registeredUser.Role)
                };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var user =  new ClaimsPrincipal(claimsIdentity);

            var userId = await _service.LogIn(_registeredUser.Email, _registeredUser.PasswordHash);
            Assert.That(user, Is.EqualTo(userId));
        }

        [Test]
        public async Task LogIn_NotRegistered_ThrowException()
        {
            Assert.That(() => Int32.Parse("otherTestUser"), Throws.Exception.TypeOf<ArgumentException>());
        }

       
    }
}