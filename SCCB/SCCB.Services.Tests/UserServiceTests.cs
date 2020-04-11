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
        private readonly string _registeredUserPassword = "Pa$$word";
        private readonly string _newUserPassword = "Pa@@word";

        private IUserService _service;

        private IMapper _mapper;
        private IOptions<HashGenerationSetting> _hashGenerationSetting;

        private Mock<IUserRepository> _repositoryMock;
        private Mock<IUnitOfWork> _unitOfWorkMock;

        private User _registeredUser;
        private User _newUser;
        private User _anotherRegisteredUser;

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
            };

            _newUser = new User()
            {
                Id = Guid.NewGuid(),
                FirstName = "Firstname",
                LastName = "Lastname",
                Email = "new@gmail.com",
                PasswordHash = passwordProcessor.GetPasswordHash(_newUserPassword),
                Role = Roles.Student,
            };

            _anotherRegisteredUser = new User()
            {
                Id = Guid.NewGuid(),
                FirstName = "Firstname",
                LastName = "Lastname",
                Email = "anotherregistered@gmail.com",
                PasswordHash = passwordProcessor.GetPasswordHash(_newUserPassword),
                Role = Roles.Student,
            };

            #region setup mocks
            _repositoryMock = new Mock<IUserRepository>();
            _repositoryMock.Setup(repo => repo.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((User)null);
            _repositoryMock.Setup(repo => repo.FindByEmailAsync(_registeredUser.Email)).ReturnsAsync(_registeredUser);
            _repositoryMock.Setup(repo => repo.FindByEmailAsync(_anotherRegisteredUser.Email)).ReturnsAsync(_anotherRegisteredUser);
            _repositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Guid>())).ReturnsAsync((User)null);
            _repositoryMock.Setup(repo => repo.FindAsync(_registeredUser.Id)).ReturnsAsync(_registeredUser);
            _repositoryMock.Setup(repo => repo.FindAsync(_anotherRegisteredUser.Id)).ReturnsAsync(_anotherRegisteredUser);
            _repositoryMock.Setup(repo => repo.AddAsync(It.IsAny<User>())).Returns(Task.FromResult(Guid.Empty));
            _repositoryMock.Setup(repo => repo.Update(_registeredUser));
            _repositoryMock.Setup(repo => repo.Remove(_registeredUser));
            _repositoryMock.Setup(repo => repo.Update(_anotherRegisteredUser));
            _repositoryMock.Setup(repo => repo.Remove(_anotherRegisteredUser));

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
                user.PasswordHash == _newUser.PasswordHash)));

            _unitOfWorkMock.Verify(ouw => ouw.CommitAsync());
        }

        [Test]
        public void Add_RegisteredUser_ArgumentException()
        {
            var userDto = _mapper.Map<Core.DTO.User>(_registeredUser);

            Assert.That(
                () => _service.Add(userDto),
                Throws.ArgumentException.With.Message.EqualTo($"User with email {userDto.Email} already exists"));
        }

        [Test]
        public async Task Find_RegisteredUser_ReturnedUser()
        {
            var result = await _service.Find(_registeredUser.Id);

            Assert.That(
                result.Id,
                Is.EqualTo(_registeredUser.Id));

            Assert.That(
                result.FirstName,
                Is.EqualTo(_registeredUser.FirstName));

            Assert.That(
                result.LastName,
                Is.EqualTo(_registeredUser.LastName));

            Assert.That(
                result.Role,
                Is.EqualTo(_registeredUser.Role));

            Assert.That(
                result.Email,
                Is.EqualTo(_registeredUser.Email));

            Assert.That(
                result.Password,
                Is.EqualTo(_registeredUser.PasswordHash));
        }

        [Test]
        public void Find_NotRegisteredUser_ArgumentException()
        {
            Assert.That(
                () => _service.Find(_newUser.Id),
                Throws.ArgumentException.With.Message.EqualTo($"Can not find user with id {_newUser.Id}"));
        }

        [Test]
        public async Task Remove_RegisteredUser_UserRepositoryRemoveCalled()
        {
            var userDto = _mapper.Map<Core.DTO.User>(_registeredUser);

            await _service.Remove(_registeredUser.Id);

            _repositoryMock.Verify(repo => repo.Remove(It.Is<User>(user =>
                user.Id == _registeredUser.Id &&
                user.FirstName == _registeredUser.FirstName &&
                user.LastName == _registeredUser.LastName &&
                user.Email == _registeredUser.Email &&
                user.PasswordHash == _registeredUser.PasswordHash)));

            _unitOfWorkMock.Verify(ouw => ouw.CommitAsync());
        }

        [Test]
        public void Remove_NotRegisteredUser_ArgumentException()
        {
            Assert.That(
                () => _service.Remove(_newUser.Id),
                Throws.ArgumentException.With.Message.EqualTo($"Can not find user with id {_newUser.Id}"));
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
                user.Email == _newUser.Email)));

            _unitOfWorkMock.Verify(ouw => ouw.CommitAsync());
        }

        [Test]
        public void UpdateProfileByEmail_RegisteredUser_ArgumentException()
        {
            var userDto = _mapper.Map<Core.DTO.UserProfile>(_registeredUser);
            userDto.Email = _anotherRegisteredUser.Email;

            Assert.That(
                () => _service.UpdateProfile(userDto),
                Throws.ArgumentException.With.Message.EqualTo($"User with email {userDto.Email} already exists"));
        }

        [Test]
        public async Task UpdateByEmail_RegisteredUserEmailAndNewUser_UserRepositoryUpdateCalled()
        {
            var userDto = _mapper.Map<Core.DTO.User>(_newUser);
            userDto.Id = _registeredUser.Id;

            await _service.Update(userDto);

            _repositoryMock.Verify(repo => repo.Update(It.Is<User>(user =>
                user.Id == _registeredUser.Id &&
                user.FirstName == _newUser.FirstName &&
                user.LastName == _newUser.LastName &&
                user.Email == _newUser.Email)));

            _unitOfWorkMock.Verify(ouw => ouw.CommitAsync());
        }

        [Test]
        public void UpdateByEmail_RegisteredUserAndAnotherRegisteredUser_ArgumentException()
        {
            var userDto = _mapper.Map<Core.DTO.User>(_registeredUser);
            userDto.Email = _anotherRegisteredUser.Email;

            Assert.That(
                () => _service.Update(userDto),
                Throws.ArgumentException.With.Message.EqualTo($"User with email {userDto.Email} already exists"));
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
                user.PasswordHash == _newUser.PasswordHash)));

            _unitOfWorkMock.Verify(ouw => ouw.CommitAsync());
        }

        [Test]
        public void UpdatePassword_WrongPassword_ArgumentException()
        {
            Assert.That(
                () => _service.UpdatePassword(_registeredUser.Id, "WrongPass", _newUserPassword),
                Throws.ArgumentException.With.Message.EqualTo("Wrong password"));
        }

        [Test]
        public void UpdatePassword_WrongId_ArgumentException()
        {
            Assert.That(
                () => _service.UpdatePassword(_newUser.Id, _newUserPassword, _registeredUserPassword),
                Throws.ArgumentException.With.Message.EqualTo($"Can not find user with id {_newUser.Id}"));
        }
    }
}
