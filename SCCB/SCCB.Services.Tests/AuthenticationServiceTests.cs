using AutoMapper;
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
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SCCB.Services.Tests
{
    public class AuthenticationServiceTests
    {
        private IAuthenticationService _service;
        
        private IMapper _mapper;
        private IOptions<HashGenerationSetting> _hashGenerationSetting;

        private Mock<IUserRepository> _repositoryMock;
        private Mock<IUnitOfWork> _unitOfWorkMock;

        private readonly string _registeredUserPassword = "Pa$$word";
        private User _registeredUser;

        private readonly string _newUserPassword = "Pa@@word";
        private User _newUser;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _hashGenerationSetting = Options.Create(new HashGenerationSetting()
            {
                Salt = "EWEM9nXVuQHIWiBzPOEj9A==",
                IterationCount = 10000,
                BytesNumber = 32
            });
            var passwordProcessor = new PasswordProcessor(_hashGenerationSetting.Value);

            _registeredUser = new User()
            {
                Id = new Guid(),
                FirstName = "Firstname",
                LastName = "Lastname",
                Email = "registered@gmail.com",
                PasswordHash = passwordProcessor.GetPasswordHash(_registeredUserPassword),
                Role = Roles.Student
            };

            _newUser = new User()
            {
                FirstName = "Firstname",
                LastName = "Lastname",
                Email = "new@gmail.com",
                PasswordHash = passwordProcessor.GetPasswordHash(_newUserPassword),
                Role = Roles.Student
            };

            var serviceMapProfile = new ServiceMapProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(serviceMapProfile));
            _mapper = new Mapper(configuration);

        }

        [SetUp]
        public void SetUp()
        {
            #region setup mocks
            _repositoryMock = new Mock<IUserRepository>();
            _repositoryMock.Setup(repo => repo.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((User)null);
            _repositoryMock.Setup(repo => repo.FindByEmailAsync(_registeredUser.Email)).ReturnsAsync(_registeredUser);
            _repositoryMock.Setup(repo => repo.AddAsync(It.IsAny<User>())).Returns(Task.FromResult(new Guid()));

            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _unitOfWorkMock.Setup(uow => uow.Users).Returns(_repositoryMock.Object);
            _unitOfWorkMock.Setup(uow => uow.CommitAsync());
            #endregion

            _service = new AuthenticationService.AuthenticationService(
                _mapper, _unitOfWorkMock.Object, _hashGenerationSetting);
        }

        [Test]
        public async Task LogIn_RegisteredUser_ClaimsPrincipal()
        {
            var result = await _service.LogIn(_registeredUser.Email, _registeredUserPassword);

            Assert.That(result.Claims.Where(c => c.Type == ClaimKeys.Email).Select(c => c.Value).SingleOrDefault(),
                Is.EqualTo(_registeredUser.Email));

            Assert.That(result.Claims.Where(c => c.Type == ClaimKeys.Role).Select(c => c.Value).SingleOrDefault(),
                Is.EqualTo(_registeredUser.Role));

            Assert.That(result.Claims.Where(c => c.Type == ClaimKeys.FirstName).Select(c => c.Value).SingleOrDefault(),
                Is.EqualTo(_registeredUser.FirstName));

            Assert.That(result.Claims.Where(c => c.Type == ClaimKeys.LastName).Select(c => c.Value).SingleOrDefault(),
                Is.EqualTo(_registeredUser.LastName));

            _repositoryMock.Verify(repo => repo.FindByEmailAsync(_registeredUser.Email));
        }

        [Test]
        public void LogIn_NotRegistered_ArgumentException()
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
            var userDto = _mapper.Map<Core.DTO.User>(_registeredUser);

            Assert.That(() => _service.CreateUser(userDto),
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
                user.PasswordHash == _newUser.PasswordHash
            )));

            _unitOfWorkMock.Verify(ouw => ouw.CommitAsync());
        }
    }
}