using System;
using System.Collections.Generic;
using System.Linq;
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
        private Core.DTO.LessonTime _existingLessonTime;
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

            _existingLessonTime = new Core.DTO.LessonTime()
            {
               Weekday = "Monday",
               LessonNumber = "1",
               IsDenominator = false,
               IsNumerator = true,
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
            _repositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Classroom> { _existingClassroom, _anotherExistingClassroom });
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

        [Test]
        public async Task GetAll_ExistingClassrooms_ReturnedClassrooms()
        {
            var result = await _service.GetAll();

            Assert.That(
                result.First().Id,
                Is.EqualTo(_existingClassroom.Id));

            Assert.That(
                result.Last().Id,
                Is.EqualTo(_anotherExistingClassroom.Id));

            Assert.That(
                result.Count(),
                Is.EqualTo(2));
        }

        [Test]
        public async Task GetAllGroupedByBuilding_ExistingClassrooms_ReturnedListOfClassrooms()
        {
            var result = await _service.GetAllGroupedByBuilding();

            Assert.That(
              result.Keys.Count(),
              Is.EqualTo(1));

            Assert.That(
              result.Values.Count(),
              Is.EqualTo(1));

            Assert.That(
              result.Values.First().Count,
              Is.EqualTo(2));

            Assert.That(
                result.Keys.First(),
                Is.EqualTo(_existingClassroom.Building));

            Assert.That(
                result.Values.First().First().Id,
                Is.EqualTo(_existingClassroom.Id));

            Assert.That(
                result.Keys.Last(),
                Is.EqualTo(_existingClassroom.Building));

            Assert.That(
                result.Values.Last().First().Id,
                Is.EqualTo(_existingClassroom.Id));

            Assert.That(
                result.Keys.First(),
                Is.EqualTo(_anotherExistingClassroom.Building));

            Assert.That(
                result.Values.First().Last().Id,
                Is.EqualTo(_anotherExistingClassroom.Id));

            Assert.That(
                result.Keys.Last(),
                Is.EqualTo(_anotherExistingClassroom.Building));

            Assert.That(
                result.Values.Last().Last().Id,
                Is.EqualTo(_anotherExistingClassroom.Id));
        }

        [Test]
        public async Task FindFreeClassrooms_GroupedByBuilding_ExistingClassrooms_ReturnedClassrooms()
        {
            var result = await _service.FindFreeClassroomsGroupedByBuilding(_existingLessonTime);

            Assert.That(
                result.FirstOrDefault,
                Is.EqualTo(_existingLessonTime.Weekday));



        }

        [Test]
        public async Task Find_ClassroomEntity_ReturnedClassroom()
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
    }
}
