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
using SCCB.Repos.Lessons;
using SCCB.Services.LessonService;

namespace SCCB.Services.Tests
{
    public class LessonServiceTests
    {
        private ILessonService _service;

        private IMapper _mapper;
        private Mock<ILessonRepository> _repositoryMock;
        private Mock<IUnitOfWork> _unitOfWorkMock;

        private Lesson _newLesson;
        private Lesson _existingLesson;
        private Lesson _anotherExistingLesson;

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
            _existingLesson = new Lesson()
            {
                Id = Guid.NewGuid(),
                Title = "Programming",
                IsDenominator = true,
                IsEnumerator = false,
                Weekday = "Monday",
                GroupId = Guid.NewGuid(),
                ClassroomId = Guid.NewGuid(),
                LectorId = Guid.NewGuid(),
                LessonNumber = "1"
            };

            _anotherExistingLesson = new Lesson()
            {
                Id = Guid.NewGuid(),
                Title = "Algebra",
                IsDenominator = true,
                IsEnumerator = false,
                Weekday = "Friday",
                GroupId = Guid.NewGuid(),
                ClassroomId = Guid.NewGuid(),
                LectorId = Guid.NewGuid(),
                LessonNumber = "2"
            };

            _newLesson = new Lesson()
            {
                Id = Guid.NewGuid(),
                Title = "3D graphics",
                IsDenominator = false,
                IsEnumerator = true,
                Weekday = "Tuesday",
                GroupId = Guid.NewGuid(),
                ClassroomId = Guid.NewGuid(),
                LectorId = Guid.NewGuid(),
                LessonNumber = "3"
            };
            
            #region setup mocks
            _repositoryMock = new Mock<ILessonRepository>();
            _repositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Guid>())).ReturnsAsync((Lesson)null);
            _repositoryMock.Setup(repo => repo.FindAsync(_existingLesson.Id)).ReturnsAsync(_existingLesson);
            _repositoryMock.Setup(repo => repo.FindAsync(_anotherExistingLesson.Id)).ReturnsAsync(_anotherExistingLesson);
            _repositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Lesson>())).Returns(Task.FromResult(new Guid()));
            _repositoryMock.Setup(repo => repo.Update(_existingLesson));
            _repositoryMock.Setup(repo => repo.Remove(_existingLesson));
            _repositoryMock.Setup(repo => repo.Update(_anotherExistingLesson));
            _repositoryMock.Setup(repo => repo.Remove(_anotherExistingLesson));

            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _unitOfWorkMock.Setup(uow => uow.Lessons).Returns(_repositoryMock.Object);
            _unitOfWorkMock.Setup(uow => uow.CommitAsync());
            #endregion

            _service = new LessonService.LessonService(
                _mapper, _unitOfWorkMock.Object);
        }

        [Test]
        public async Task Add_NewLesson_LessonRepositoryAddCalled()
        {
            var lessonDto = _mapper.Map<Core.DTO.Lesson>(_newLesson);

            await _service.Add(lessonDto);

            _repositoryMock.Verify(repo => repo.AddAsync(It.Is<Lesson>(lesson =>
                lesson.Id == _newLesson.Id &&
                lesson.Title == _newLesson.Title &&
                lesson.IsDenominator == _newLesson.IsDenominator &&
                lesson.IsEnumerator == _newLesson.IsEnumerator &&
                lesson.GroupId == _newLesson.GroupId &&
                lesson.LectorId == _newLesson.LectorId &&
                lesson.LessonNumber == _newLesson.LessonNumber &&
                lesson.Weekday == _newLesson.Weekday &&
                lesson.ClassroomId == _newLesson.ClassroomId
            )));

            _unitOfWorkMock.Verify(ouw => ouw.CommitAsync());
        }

        [Test]
        public async Task Find_ExistingLesson_ReturnedLesson()
        {
            var result = await _service.Find(_existingLesson.Id);

            Assert.That(result.Id,
                Is.EqualTo(_existingLesson.Id));

            Assert.That(result.Title,
                Is.EqualTo(_existingLesson.Title));

            Assert.That(result.IsDenominator,
                Is.EqualTo(_existingLesson.IsDenominator));

            Assert.That(result.IsEnumerator,
                Is.EqualTo(_existingLesson.IsEnumerator));

            Assert.That(result.LectorId,
                Is.EqualTo(_existingLesson.LectorId));

            Assert.That(result.GroupId,
                Is.EqualTo(_existingLesson.GroupId));

            Assert.That(result.ClassroomId,
                Is.EqualTo(_existingLesson.ClassroomId));

            Assert.That(result.Weekday,
                Is.EqualTo(_existingLesson.Weekday));

            Assert.That(result.LessonNumber,
                Is.EqualTo(_existingLesson.LessonNumber));

        }

        [Test]
        public void Find_NotExistingLesson_ArgumentException()
        {
            Assert.That(() => _service.Find(_newLesson.Id),
                Throws.ArgumentException.With.Message.EqualTo($"Can not find lesson with id {_newLesson.Id}"));
        }

        [Test]
        public async Task Remove_ExistingLesson_LessonRepositoryRemoveCalled()
        {
            var lessonDto = _mapper.Map<Core.DTO.Lesson>(_existingLesson);

            await _service.Remove(_existingLesson.Id);

            _repositoryMock.Verify(repo => repo.Remove(It.Is<Lesson>(lesson =>
                lesson.Id == _existingLesson.Id &&
                lesson.Title == _existingLesson.Title &&
                lesson.IsDenominator == _existingLesson.IsDenominator &&
                lesson.IsEnumerator == _existingLesson.IsEnumerator &&
                lesson.GroupId == _existingLesson.GroupId &&
                lesson.LectorId == _existingLesson.LectorId &&
                lesson.LessonNumber == _existingLesson.LessonNumber &&
                lesson.Weekday == _existingLesson.Weekday &&
                lesson.ClassroomId == _existingLesson.ClassroomId
            )));

            _unitOfWorkMock.Verify(ouw => ouw.CommitAsync());
        }

        [Test]
        public void Remove_NotExistingLesson_ArgumentException()
        {
            Assert.That(() => _service.Remove(_newLesson.Id),
                Throws.ArgumentException.With.Message.EqualTo($"Can not find lesson with id {_newLesson.Id}"));
        }

        [Test]
        public async Task UpdateLesson_ExistingLessonAndNewLesson_LessonRepositoryUpdateCalled()
        {
            var lessonDto = _mapper.Map<Core.DTO.Lesson>(_newLesson);
            lessonDto.Id = _existingLesson.Id;

            await _service.Update(lessonDto);

            _repositoryMock.Verify(repo => repo.Update(It.Is<Lesson>(lesson =>
                lesson.Id == _existingLesson.Id &&
                lesson.Title == _existingLesson.Title &&
                lesson.IsDenominator == _existingLesson.IsDenominator &&
                lesson.IsEnumerator == _existingLesson.IsEnumerator &&
                lesson.GroupId == _existingLesson.GroupId &&
                lesson.LectorId == _existingLesson.LectorId &&
                lesson.LessonNumber == _existingLesson.LessonNumber &&
                lesson.Weekday == _existingLesson.Weekday &&
                lesson.ClassroomId == _existingLesson.ClassroomId
            )));

            _unitOfWorkMock.Verify(ouw => ouw.CommitAsync());
        }

    }
}
