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
using AutoMapper;

namespace SCCB.Services.Tests
{
    public class AuthenticationServiceTests
    {
        private IMapper _mapper;

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
                FirstName = "Firstname",
                LastName = "Lastname",
                Email = "registered@gmail.com",
                PasswordHash = passwordProcessor.GetPasswordHash(_registeredUserPassword),
                Role = "Student"
            };

            _newUser = new User()
            {
                FirstName = "Firstname",
                LastName = "Lastname",
                Email = "new@gmail.com",
                PasswordHash = passwordProcessor.GetPasswordHash(_newUserPassword),
                Role = "Student"
            };

            #region setup mocks
            _repositoryMock = new Mock<IUserRepository>();
            _repositoryMock.Setup(repo => repo.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((User)null);
            _repositoryMock.Setup(repo => repo.FindByEmailAsync(_registeredUser.Email)).ReturnsAsync(_registeredUser);
            _repositoryMock.Setup(repo => repo.Add(It.IsAny<User>())).Returns<User>(x => x);

            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _unitOfWorkMock.Setup(uow => uow.Users).Returns(_repositoryMock.Object);
            _unitOfWorkMock.Setup(uow => uow.CommitAsync());
            #endregion

            var serviceMapProfile = new ServiceMapProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(serviceMapProfile));
            _mapper = new Mapper(configuration);

            _service = new AuthenticationService.AuthenticationService(
                _mapper, _unitOfWorkMock.Object, hashGenerationSetting);
        }

        [Test]
        public async Task LogIn_RegisteredUser_ClaimsPrincipal()
        {
            var result = await _service.LogIn(_registeredUser.Email, _registeredUserPassword);

            Assert.That(result.Claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).SingleOrDefault(),
                Is.EqualTo(_registeredUser.Email));

            Assert.That(result.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).SingleOrDefault(),
                Is.EqualTo(_registeredUser.Role));

            Assert.That(result.Claims.Where(c => c.Type == "FirstName").Select(c => c.Value).SingleOrDefault(),
                Is.EqualTo(_registeredUser.FirstName));
            
            Assert.That(result.Claims.Where(c => c.Type == "LastName").Select(c => c.Value).SingleOrDefault(),
                Is.EqualTo(_registeredUser.LastName));

            _repositoryMock.Verify(repo => repo.FindByEmailAsync(_registeredUser.Email));
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
            var userDto = _mapper.Map<Core.DTO.User>(_registeredUser);

            Assert.That(() => _service.CreateUser(userDto),
                Throws.ArgumentException.With.Message.EqualTo("User already exists"));
        }

        [Test]
        public async Task CreateUser_NewUser_UserRepositoryAddCalled()
        {
            var userDto = _mapper.Map<Core.DTO.User>(_newUser);

            await _service.CreateUser(userDto);

            _repositoryMock.Verify(repo => repo.Add(It.Is<User>(user => 
                user.FirstName == userDto.FirstName &&
                user.LastName == userDto.LastName &&
                user.Email == userDto.Email &&
                user.PasswordHash == userDto.PasswordHash
            )));

            _unitOfWorkMock.Verify(ouw => ouw.CommitAsync());
        }
    }
}