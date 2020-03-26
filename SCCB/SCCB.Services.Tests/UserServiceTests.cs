using System;
using System.Threading.Tasks;
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
using SCCB.Services.UserService;

namespace SCCB.Services.Tests
{
    public class UserServiceTests
    {
        private IUserService _service;

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
                Id = Guid.NewGuid(),
                FirstName = "Firstname",
                LastName = "Lastname",
                Email = "registered@gmail.com",
                PasswordHash = passwordProcessor.GetPasswordHash(_registeredUserPassword),
                Role = Roles.Student
            };

            _newUser = new User()
            {
                Id = Guid.NewGuid(),
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
            _repositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Guid>())).ReturnsAsync((User)null);
            _repositoryMock.Setup(repo => repo.FindAsync(_registeredUser.Id)).ReturnsAsync(_registeredUser);
            _repositoryMock.Setup(repo => repo.AddAsync(It.IsAny<User>())).Returns(Task.FromResult(new Guid()));
            _repositoryMock.Setup(repo => repo.Update(_registeredUser));
            _repositoryMock.Setup(repo => repo.Remove(_registeredUser));

            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _unitOfWorkMock.Setup(uow => uow.Users).Returns(_repositoryMock.Object);
            _unitOfWorkMock.Setup(uow => uow.CommitAsync());
            #endregion

            _service = new UserService.UserService(
                _mapper, _unitOfWorkMock.Object, _hashGenerationSetting);
        }

        [Test]
        public async Task Add_NewUser_UserRepositoryAddCalled()
        {
            var userDto = _mapper.Map<Core.DTO.User>(_newUser);
            userDto.Password = _newUserPassword;

            await _service.Add(userDto);

            _repositoryMock.Verify(repo => repo.AddAsync(It.Is<User>(user =>
                user.FirstName == _newUser.FirstName &&
                user.LastName == _newUser.LastName &&
                user.Email == _newUser.Email &&
                user.PasswordHash == _newUser.PasswordHash
            )));

            _unitOfWorkMock.Verify(ouw => ouw.CommitAsync());
        }

        [Test]
        public async Task UpdateProfileByEmail_RegisteredUserEmailAndNewUser_UserRepositoryUpdateCalled()
        {
            var userDto = _mapper.Map<Core.DTO.UserProfile>(_newUser);
            userDto.Id = _registeredUser.Id;

            await _service.UpdateProfile(userDto);

            _repositoryMock.Verify(repo => repo.Update(It.Is<User>(user =>
                user.Id == _registeredUser.Id &&
                user.FirstName == _newUser.FirstName &&
                user.LastName == _newUser.LastName &&
                user.Email == _newUser.Email
            )));

            _unitOfWorkMock.Verify(ouw => ouw.CommitAsync());
        }

        [Test]
        public async Task UpdatePassword_RegisteredUserOldPassword_UserRepositoryUpdateCalled()
        {
            await _service.UpdatePassword(_registeredUser.Id, _registeredUserPassword, _newUserPassword);

            _repositoryMock.Verify(repo => repo.Update(It.Is<User>(user =>
                user.Id == _registeredUser.Id &&
                user.FirstName == _registeredUser.FirstName &&
                user.LastName == _registeredUser.LastName &&
                user.Email == _registeredUser.Email &&
                user.PasswordHash == _newUser.PasswordHash
            )));

            _unitOfWorkMock.Verify(ouw => ouw.CommitAsync());
        }
    }
}
