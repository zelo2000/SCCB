using System;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using NUnit.Framework;
using SCCB.DAL.Entities;
using SCCB.Repos.Classrooms;
using SCCB.Repos.UnitOfWork;
using SCCB.Services.ClassroomService;

namespace SCCB.Services.Tests
{
    public class ClassroomServiceTests
    {
        private IClassroomService _service;

        private IMapper _mapper;
        private Mock<IClassroomRepository> _repositoryMock;
        private Mock<IUnitOfWork> _unitOfWorkMock;

        private Classroom _newClassroom;
        private Classroom _existingClassroom;
        private Classroom _anotherExistingClassroom;

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
            _existingClassroom = new Classroom()
            {
                Id = Guid.NewGuid(),
                Number = "117",
                Building = "Main",
            };

            _anotherExistingClassroom = new Classroom()
            {
                Id = Guid.NewGuid(),
                Number = "439",
                Building = "Main",
            };

            _newClassroom = new Classroom()
            {
                Id = Guid.NewGuid(),
                Number = "14",
                Building = "Geographic",
            };

            #region setup mocks
            _repositoryMock = new Mock<IClassroomRepository>();
            _repositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Guid>())).ReturnsAsync((Classroom)null);
            _repositoryMock.Setup(repo => repo.FindAsync(_existingClassroom.Id)).ReturnsAsync(_existingClassroom);
            _repositoryMock.Setup(repo => repo.FindAsync(_anotherExistingClassroom.Id)).ReturnsAsync(_anotherExistingClassroom);
            _repositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Classroom>())).Returns(Task.FromResult(Guid.Empty));
            _repositoryMock.Setup(repo => repo.Update(_existingClassroom));
            _repositoryMock.Setup(repo => repo.Remove(_existingClassroom));
            _repositoryMock.Setup(repo => repo.Update(_anotherExistingClassroom));
            _repositoryMock.Setup(repo => repo.Remove(_anotherExistingClassroom));

            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _unitOfWorkMock.Setup(uow => uow.Classrooms).Returns(_repositoryMock.Object);
            _unitOfWorkMock.Setup(uow => uow.CommitAsync());
            #endregion

            _service = new ClassroomService.ClassroomService(
                _mapper, _unitOfWorkMock.Object);
        }

        [Test]
        public async Task Add_NewClassroom_ClassroomRepositoryAddCalled()
        {
            var classroomDto = _mapper.Map<Core.DTO.Classroom>(_newClassroom);

            await _service.Add(classroomDto);

            _repositoryMock.Verify(repo => repo.AddAsync(It.Is<Classroom>(classroom =>
                classroom.Id == _newClassroom.Id &&
                classroom.Number == _newClassroom.Number &&
                classroom.Building == _newClassroom.Building)));

            _unitOfWorkMock.Verify(ouw => ouw.CommitAsync());
        }

        [Test]
        public async Task Find_ExistingClassroom_ReturnedClassroom()
        {
            var result = await _service.Find(_existingClassroom.Id);

            Assert.That(
                result.Id,
                Is.EqualTo(_existingClassroom.Id));

            Assert.That(
                result.Number,
                Is.EqualTo(_existingClassroom.Number));

            Assert.That(
                result.Building,
                Is.EqualTo(_existingClassroom.Building));
        }

        [Test]
        public void Find_NotExistingClassroom_ArgumentException()
        {
            Assert.That(
                () => _service.Find(_newClassroom.Id),
                Throws.ArgumentException.With.Message.EqualTo($"Can not find classroom with id {_newClassroom.Id}"));
        }

        [Test]
        public async Task Remove_ExistingClassroom_ClassroomRepositoryRemoveCalled()
        {
            var classroomDto = _mapper.Map<Core.DTO.Classroom>(_existingClassroom);

            await _service.Remove(_existingClassroom.Id);

            _repositoryMock.Verify(repo => repo.Remove(It.Is<Classroom>(classroom =>
                classroom.Id == _existingClassroom.Id &&
                classroom.Number == _existingClassroom.Number &&
                classroom.Building == _existingClassroom.Building)));

            _unitOfWorkMock.Verify(ouw => ouw.CommitAsync());
        }

        [Test]
        public void Remove_NotExistingClassroom_ArgumentException()
        {
            Assert.That(
                () => _service.Remove(_newClassroom.Id),
                Throws.ArgumentException.With.Message.EqualTo($"Can not find classroom with id {_newClassroom.Id}"));
        }

        [Test]
        public async Task UpdateClassroom_ExistingClassroomAndNewClassroom_ClassroomRepositoryUpdateCalled()
        {
            var classroomDto = _mapper.Map<Core.DTO.Classroom>(_newClassroom);
            classroomDto.Id = _existingClassroom.Id;

            await _service.Update(classroomDto);

            _repositoryMock.Verify(repo => repo.Update(It.Is<Classroom>(classroom =>
                classroom.Id == _existingClassroom.Id &&
                classroom.Number == _existingClassroom.Number &&
                classroom.Building == _existingClassroom.Building)));

            _unitOfWorkMock.Verify(ouw => ouw.CommitAsync());
        }
    }
}
