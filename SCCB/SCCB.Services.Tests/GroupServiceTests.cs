using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using NUnit.Framework;
using SCCB.DAL.Entities;
using SCCB.Repos.Groups;
using SCCB.Repos.UnitOfWork;
using SCCB.Services.GroupService;

namespace SCCB.Services.Tests
{
    public class GroupServiceTests
    {
        private IGroupService _service;

        private IMapper _mapper;
        private Mock<IGroupRepository> _repositoryMock;
        private Mock<IUnitOfWork> _unitOfWorkMock;

        private Group _newGroup;
        private Group _existingGroup;
        private Group _anotherExistingGroup;

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
            _existingGroup = new Group()
            {
                Id = Guid.NewGuid(),
                Name = "PMI33",
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

            #region setup mocks
            _repositoryMock = new Mock<IGroupRepository>();
            _repositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Guid>())).ReturnsAsync((Group)null);
            _repositoryMock.Setup(repo => repo.FindAsync(_existingGroup.Id)).ReturnsAsync(_existingGroup);
            _repositoryMock.Setup(repo => repo.FindAsync(_anotherExistingGroup.Id)).ReturnsAsync(_anotherExistingGroup);
            _repositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Group>())).Returns(Task.FromResult(Guid.Empty));
            _repositoryMock.Setup(repo => repo.Update(_existingGroup));
            _repositoryMock.Setup(repo => repo.Remove(_existingGroup));
            _repositoryMock.Setup(repo => repo.Update(_anotherExistingGroup));
            _repositoryMock.Setup(repo => repo.Remove(_anotherExistingGroup));
            _repositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Group> { _existingGroup, _anotherExistingGroup });
            _repositoryMock.Setup(repo => repo.FindByIsAcademic(It.IsAny<bool>())).ReturnsAsync((List<Group>)null);
            _repositoryMock.Setup(repo => repo.FindByIsAcademic(_existingGroup.IsAcademic)).ReturnsAsync(new List<Group> { _existingGroup });
            _repositoryMock.Setup(repo => repo.FindByIsAcademic(_anotherExistingGroup.IsAcademic)).ReturnsAsync(new List<Group> { _anotherExistingGroup });

            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _unitOfWorkMock.Setup(uow => uow.Groups).Returns(_repositoryMock.Object);
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
            var result = await _service.Find(_existingGroup.Id);

            Assert.That(
                result.Id,
                Is.EqualTo(_existingGroup.Id));

            Assert.That(
                result.Name,
                Is.EqualTo(_existingGroup.Name));

            Assert.That(
                result.IsAcademic,
                Is.EqualTo(_existingGroup.IsAcademic));
        }

        [Test]
        public async Task GetAll_ExistingGroups_ReturnedGroups()
        {
            var result = await _service.GetAll();

            Assert.That(
                result.First().Id,
                Is.EqualTo(_existingGroup.Id));

            Assert.That(
                result.Last().Id,
                Is.EqualTo(_anotherExistingGroup.Id));

            Assert.That(
                result.Count(),
                Is.EqualTo(2));
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
        public void Find_NotExistingGroup_ArgumentException()
        {
            Assert.That(
                () => _service.Find(_newGroup.Id),
                Throws.ArgumentException.With.Message.EqualTo($"Can not find group with id {_newGroup.Id}"));
        }

        [Test]
        public async Task Remove_ExistingGroup_GroupRepositoryRemoveCalled()
        {
            var groupDto = _mapper.Map<Core.DTO.Group>(_existingGroup);

            await _service.Remove(_existingGroup.Id);

            _repositoryMock.Verify(repo => repo.Remove(It.Is<Group>(group =>
                group.Id == _existingGroup.Id &&
                group.Name == _existingGroup.Name &&
                group.IsAcademic == _existingGroup.IsAcademic)));

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
            groupDto.Id = _existingGroup.Id;

            await _service.Update(groupDto);

            _repositoryMock.Verify(repo => repo.Update(It.Is<Group>(group =>
                group.Id == _existingGroup.Id &&
                group.Name == _existingGroup.Name &&
                group.IsAcademic == _existingGroup.IsAcademic)));

            _unitOfWorkMock.Verify(ouw => ouw.CommitAsync());
        }
    }
}
