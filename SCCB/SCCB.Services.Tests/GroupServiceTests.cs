using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SCCB.Core.Helpers;
using SCCB.Core.Settings;
using SCCB.DAL.Entities;
using SCCB.Repos.Groups;
using SCCB.Repos.UnitOfWork;
using SCCB.Repos.Users;
using SCCB.Services.GroupService;

namespace SCCB.Services.Tests
{
    public class GroupServiceTests
    {
        private readonly string _existingUserPassword = "Pa$$word";
        private readonly string _anotherExistingUserPassword = "Pa@@word";

        private IGroupService _service;

        private IMapper _mapper;
        private IOptions<HashGenerationSetting> _hashGenerationSetting;

        private Mock<IGroupRepository> _repositoryMock;
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IUnitOfWork> _unitOfWorkMock;

        private Group _newGroup;
        private Group _existingGroup1;
        private Group _existingGroup2;
        private Group _anotherExistingGroup;
        private Guid _userId;
        private User _existingUser;
        private User _anotherExistingUser;
        private UsersToGroups _usersToGroups;

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

            _existingGroup1 = new Group()
            {
                Id = Guid.NewGuid(),
                Name = "PMI33",
                IsAcademic = false,
            };

            _existingGroup2 = new Group()
            {
                Id = Guid.NewGuid(),
                Name = "AdditionMath",
                IsAcademic = false,
            };

            _anotherExistingGroup = new Group()
            {
                Id = Guid.NewGuid(),
                Name = "PMI31",
                IsAcademic = true,
            };

            _newGroup = new Group()
            {
                Id = Guid.NewGuid(),
                Name = "PMI32",
                IsAcademic = true,
            };

            _existingUser = new User()
            {
                Id = Guid.NewGuid(),
                FirstName = "Ivan",
                LastName = "Ivanov",
                Email = "ivan@gmail.com",
                PasswordHash = passwordProcessor.GetPasswordHash(_existingUserPassword),
                Role = "Student",
            };

            _anotherExistingUser = new User()
            {
                Id = Guid.NewGuid(),
                FirstName = "Petro",
                LastName = "Petrov",
                Email = "petro@gmail.com",
                PasswordHash = passwordProcessor.GetPasswordHash(_anotherExistingUserPassword),
                Role = "Lector",
            };

            _userId = Guid.NewGuid();

            _usersToGroups = new UsersToGroups()
            {
                Id = Guid.NewGuid(),
                UserId = _existingUser.Id,
                GroupId = _anotherExistingGroup.Id,
                IsUserOwner = true,
            };

            #region setup mocks
            _repositoryMock = new Mock<IGroupRepository>();
            _repositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Guid>())).ReturnsAsync((Group)null);
            _repositoryMock.Setup(repo => repo.FindAsync(_existingGroup1.Id)).ReturnsAsync(_existingGroup1);
            _repositoryMock.Setup(repo => repo.FindAsync(_existingGroup2.Id)).ReturnsAsync(_existingGroup2);
            _repositoryMock.Setup(repo => repo.FindAsync(_anotherExistingGroup.Id)).ReturnsAsync(_anotherExistingGroup);
            _repositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Group>())).Returns(Task.FromResult(Guid.Empty));
            _repositoryMock.Setup(repo => repo.Update(_existingGroup1));
            _repositoryMock.Setup(repo => repo.Update(_existingGroup2));
            _repositoryMock.Setup(repo => repo.Remove(_existingGroup1));
            _repositoryMock.Setup(repo => repo.Remove(_existingGroup2));
            _repositoryMock.Setup(repo => repo.Update(_anotherExistingGroup));
            _repositoryMock.Setup(repo => repo.Remove(_anotherExistingGroup));
            _repositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Group> { _existingGroup1, _existingGroup2,_anotherExistingGroup });
            _repositoryMock.Setup(repo => repo.FindByIsAcademic(It.IsAny<bool>())).ReturnsAsync((List<Group>)null);
            _repositoryMock.Setup(repo => repo.FindByIsAcademic(_existingGroup1.IsAcademic)).ReturnsAsync(new List<Group> { _existingGroup1, _existingGroup2 });
            _repositoryMock.Setup(repo => repo.FindByIsAcademic(_anotherExistingGroup.IsAcademic)).ReturnsAsync(new List<Group> { _anotherExistingGroup });

            _repositoryMock.Setup(repo => repo.FindNotAcademic(_userId, true)).ReturnsAsync(new List<Group> { _existingGroup1 });
            _repositoryMock.Setup(repo => repo.FindNotAcademic(_userId, false)).ReturnsAsync(new List<Group> { _existingGroup2 });


            _repositoryMock.Setup(repo => repo.GetOwner(_anotherExistingGroup.Id)).ReturnsAsync(_existingUser.Id);

            _repositoryMock.Setup(repo => repo.FindUserToGroup(_existingUser.Id, _anotherExistingUser.Id)).ReturnsAsync(_usersToGroups);
            _repositoryMock.Setup(repo => repo.RemoveUser(_usersToGroups));
            _repositoryMock.Setup(repo => repo.AddUser(It.IsAny<UsersToGroups>())).Returns(Guid.NewGuid);

            _userRepositoryMock = new Mock<IUserRepository>();
            _userRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Guid>())).ReturnsAsync((User)null);
            _userRepositoryMock.Setup(repo => repo.FindByGroupId(_anotherExistingGroup.Id)).ReturnsAsync(new List<User> { _existingUser });
            _userRepositoryMock.Setup(repo => repo.FindByGroupId(_existingGroup1.Id)).ReturnsAsync(new List<User> { _anotherExistingUser });
            _userRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<User> { _existingUser, _anotherExistingUser });

            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _unitOfWorkMock.Setup(uow => uow.Groups).Returns(_repositoryMock.Object);
            _unitOfWorkMock.Setup(uow => uow.Users).Returns(_userRepositoryMock.Object);
            _unitOfWorkMock.Setup(uow => uow.CommitAsync());
            #endregion

            _service = new GroupService.GroupService(
                _mapper, _unitOfWorkMock.Object);
        }

        [Test]
        public async Task Add_NewGroup_GroupRepositoryAddCalled()
        {
            var groupDto = _mapper.Map<Core.DTO.Group>(_newGroup);

            await _service.Add(groupDto);

            _repositoryMock.Verify(repo => repo.AddAsync(It.Is<Group>(group =>
                group.Id == _newGroup.Id &&
                group.Name == _newGroup.Name &&
                group.IsAcademic == _newGroup.IsAcademic)));

            _unitOfWorkMock.Verify(ouw => ouw.CommitAsync());
        }

        [Test]
        public async Task Find_ExistingGroup_ReturnedGroup()
        {
            var result = await _service.Find(_existingGroup1.Id);

            Assert.That(
                result.Id,
                Is.EqualTo(_existingGroup1.Id));

            Assert.That(
                result.Name,
                Is.EqualTo(_existingGroup1.Name));

            Assert.That(
                result.IsAcademic,
                Is.EqualTo(_existingGroup1.IsAcademic));
        }

        [Test]
        public async Task GetAll_ExistingGroups_ReturnedGroups()
        {
            var result = await _service.GetAll();

            Assert.That(
                result.First().Id,
                Is.EqualTo(_existingGroup1.Id));

            Assert.That(
                result.Last().Id,
                Is.EqualTo(_anotherExistingGroup.Id));

            Assert.That(
                result.Count(),
                Is.EqualTo(3));
        }

        [Test]
        public async Task GetAllAcademic_ExistingGroups_ReturnedGroups()
        {
            var result = await _service.GetAllAcademic();

            Assert.That(
                result.First().IsAcademic,
                Is.EqualTo(true));

            Assert.That(
                result.First().Id,
                Is.EqualTo(_anotherExistingGroup.Id));

            Assert.That(
                result.Count(),
                Is.EqualTo(1));
        }

        [Test]
        public async Task FindByOption_OptionAll_ReturnedAllGroups()
        {
            var result = await _service.FindByOption("All");

            Assert.That(
                result.Count(),
                Is.EqualTo(3));

            Assert.That(
                result.First().Id,
                Is.EqualTo(_existingGroup1.Id));

            Assert.That(
               result.First().IsAcademic,
               Is.EqualTo(false));

            Assert.That(
                result.ElementAt(1).Id,
                Is.EqualTo(_existingGroup2.Id));

            Assert.That(
               result.ElementAt(1).IsAcademic,
               Is.EqualTo(false));

            Assert.That(
                result.Last().Id,
                Is.EqualTo(_anotherExistingGroup.Id));

            Assert.That(
               result.Last().IsAcademic,
               Is.EqualTo(true));
        }

        [Test]
        public async Task FindByOption_OptionAcademic_ReturnedAcademicGroups()
        {
            var result = await _service.FindByOption("Academic");

            Assert.That(
                result.Count(),
                Is.EqualTo(1));

            Assert.That(
                result.First().IsAcademic,
                Is.EqualTo(true));

            Assert.That(
                result.First().Id,
                Is.EqualTo(_anotherExistingGroup.Id));
        }

        [Test]
        public async Task FindByOption_OptionUserDefined_ReturnedUserDefinedGroups()
        {
            var result = await _service.FindByOption("UserDefined");

            Assert.That(
                result.Count(),
                Is.EqualTo(2));

            Assert.That(
                result.First().IsAcademic,
                Is.EqualTo(false));

            Assert.That(
                result.Last().IsAcademic,
                Is.EqualTo(false));

            Assert.That(
                result.First().Id,
                Is.EqualTo(_existingGroup1.Id));

            Assert.That(
                result.Last().Id,
                Is.EqualTo(_existingGroup2.Id));
        }

        [Test]
        public void Find_NotExistingGroup_ArgumentException()
        {
            Assert.That(
                () => _service.Find(_newGroup.Id),
                Throws.ArgumentException.With.Message.EqualTo($"Can not find group with id {_newGroup.Id}"));
        }

        [Test]
        public async Task Remove_ExistingGroup_GroupRepositoryRemoveCalled()
        {
            var groupDto = _mapper.Map<Core.DTO.Group>(_existingGroup1);

            await _service.Remove(_existingGroup1.Id);

            _repositoryMock.Verify(repo => repo.Remove(It.Is<Group>(group =>
                group.Id == _existingGroup1.Id &&
                group.Name == _existingGroup1.Name &&
                group.IsAcademic == _existingGroup1.IsAcademic)));

            _unitOfWorkMock.Verify(ouw => ouw.CommitAsync());
        }

        [Test]
        public void Remove_NotExistingGroup_ArgumentException()
        {
            Assert.That(
                () => _service.Remove(_newGroup.Id),
                Throws.ArgumentException.With.Message.EqualTo($"Can not find group with id {_newGroup.Id}"));
        }

        [Test]
        public async Task UpdateGroup_ExistingGroupAndNewGroup_GroupRepositoryUpdateCalled()
        {
            var groupDto = _mapper.Map<Core.DTO.Group>(_newGroup);
            groupDto.Id = _existingGroup1.Id;

            await _service.Update(groupDto);

            _repositoryMock.Verify(repo => repo.Update(It.Is<Group>(group =>
                group.Id == _existingGroup1.Id &&
                group.Name == _existingGroup1.Name &&
                group.IsAcademic == _existingGroup1.IsAcademic)));

            _unitOfWorkMock.Verify(ouw => ouw.CommitAsync());
        }

        [Test]
        public void UpdateGroup_NotExistingGroup_ArgumentException()
        {
            var groupDto = _mapper.Map<Core.DTO.Group>(_newGroup);
            Assert.That(
                () => _service.Update(groupDto),
                Throws.ArgumentException.With.Message.EqualTo($"Can not find group with id {groupDto.Id}"));
        }

        [Test]
        public async Task FindNotAcademic_UserIsOwner_ReturnedGroups()
        {
            var result = await _service.FindNotAcademic(_userId, true);

            Assert.That(
                result.First().IsAcademic,
                Is.EqualTo(false));

            Assert.That(
                result.First().Id,
                Is.EqualTo(_existingGroup1.Id));

            Assert.That(
                result.Count(),
                Is.EqualTo(1));
        }

        [Test]
        public async Task FindNotAcademic_UserIsNotOwner_ReturnedGroups()
        {
            var result = await _service.FindNotAcademic(_userId, false);

            Assert.That(
                result.First().IsAcademic,
                Is.EqualTo(false));

            Assert.That(
                result.First().Id,
                Is.EqualTo(_existingGroup2.Id));

            Assert.That(
                result.Count(),
                Is.EqualTo(1));
        }

        [Test]
        public async Task FindUsersInGroup_ExistingUsers_UsersInGroup()
        {
            var result = await _service.FindUsersInGroup(_anotherExistingGroup.Id);

            Assert.That(
                result.First().Id,
                Is.EqualTo(_existingUser.Id));

            Assert.That(
                result.Count(),
                Is.EqualTo(1));
        }

        [Test]
        public async Task FindUsersNotInGroup_ExistingUsers_UsersNotInGroup()
        {
            var result = await _service.FindUsersNotInGroup(_anotherExistingGroup.Id);

            Assert.That(
                result.First().Id,
                Is.EqualTo(_anotherExistingUser.Id));

            Assert.That(
                result.Count(),
                Is.EqualTo(1));
        }

        [Test]
        public async Task CheckOwnership_ExistingUser_IsOwnerOfGroup()
        {
            var result = await _service.CheckOwnership(_existingUser.Id, _anotherExistingGroup.Id);

            Assert.That(
                result,
                Is.EqualTo(true));
        }

        [Test]
        public async Task AddUser_NewUserToGroup_GroupRepositoryAddUserCalled()
        {
            var result = await _service.AddUser(_existingUser.Id, _anotherExistingGroup.Id);

            _repositoryMock.Verify(repo => repo.AddUser(It.Is<UsersToGroups>(user =>
                user.GroupId == _anotherExistingGroup.Id &&
                user.UserId == _existingUser.Id)));

            Assert.That(
                result,
                Is.Not.EqualTo(Guid.Empty));
        }

        [Test]
        public async Task RemoveUser_ExistingUser_GroupRepositoryRemoveUserCalled()
        {
            await _service.RemoveUser(_existingUser.Id, _anotherExistingGroup.Id);

            _repositoryMock.Verify(repo => repo.RemoveUser(It.IsAny<UsersToGroups>()));

            _unitOfWorkMock.Verify(ouw => ouw.CommitAsync());
        }
    }
}
