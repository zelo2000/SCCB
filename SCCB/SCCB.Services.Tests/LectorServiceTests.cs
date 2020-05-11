using AutoMapper;
using Moq;
using SCCB.DAL.Entities;
using SCCB.Repos.Lectors;
using SCCB.Repos.UnitOfWork;
using SCCB.Services.LectorService;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;

namespace SCCB.Services.Tests
{
    public class LectorServiceTests
    {
        private ILectorService _service;

        private IMapper _mapper;
        private Mock<ILectorRepository> _repositoryMock;
        private Mock<IUnitOfWork> _unitOfWorkMock;

        private Lector _newLector;
        private Lector _existingLector;
        private Lector _anotherExistingLector;
        private Core.DTO.LessonTime _lessonTime;
        private Core.DTO.LessonTime _anotherLessonTime;


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
            _newLector = new Lector()
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Position = "Аспірант",
            };

            _existingLector = new Lector()
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Position = "Доцент",
            };

            _anotherExistingLector = new Lector()
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Position = "Доцент",
            };

            _lessonTime = new Core.DTO.LessonTime()
            {
                Weekday = "Monday",
                LessonNumber = 1,
                IsNumerator = true,
                IsDenominator = false,
            };

            _anotherLessonTime = new Core.DTO.LessonTime()
            {
                IsNumerator = true,
                IsDenominator = false,
            };

            #region setup mocks
            _repositoryMock = new Mock<ILectorRepository>();
            _repositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Guid>())).ReturnsAsync((Lector)null);
            _repositoryMock.Setup(repo => repo.FindAsync(_existingLector.Id)).ReturnsAsync(_existingLector);
            _repositoryMock.Setup(repo => repo.FindAsync(_anotherExistingLector.Id)).ReturnsAsync(_anotherExistingLector);
            _repositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Lector>())).Returns(Task.FromResult(Guid.Empty));
            _repositoryMock.Setup(repo => repo.Update(_existingLector));
            _repositoryMock.Setup(repo => repo.Remove(_existingLector));
            _repositoryMock.Setup(repo => repo.Update(_anotherExistingLector));
            _repositoryMock.Setup(repo => repo.Remove(_anotherExistingLector));
            _repositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Lector> { _existingLector, _anotherExistingLector });

            _repositoryMock.Setup(repo => repo.GetAllWithUserInfoAsync()).ReturnsAsync(new List<Lector> { _existingLector, _anotherExistingLector });

            _repositoryMock.Setup(repo => repo.FindFreeLectors(_lessonTime)).ReturnsAsync(new List<Lector> { _existingLector, _anotherExistingLector});
            _repositoryMock.Setup(repo => repo.FindFreeLectors(_anotherLessonTime)).ReturnsAsync(new List<Lector>());

            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _unitOfWorkMock.Setup(uow => uow.Lectors).Returns(_repositoryMock.Object);
            _unitOfWorkMock.Setup(uow => uow.CommitAsync());
            #endregion

            _service = new LectorService.LectorService(
                _mapper, _unitOfWorkMock.Object);
        }

        [Test]
        public async Task GetAllWithUserInfo_ReturnedAllLectorsWithUserInfo()
        {
            _existingLector.User = new User()
            { 
                Id = _existingLector.UserId,
                FirstName = "Ivan",
                LastName = "Ivanov",
                Email = "Ivan@gmail.com",
                Role = "Lector",
            };

            _anotherExistingLector.User = new User()
            {
                Id = _anotherExistingLector.UserId,
                FirstName = "Petro",
                LastName = "Petrov",
                Email = "petro@gmail.com",
                Role = "Lector",
            };

            var result = await _service.GetAllWithUserInfo();

            Assert.That(
                result.First().Id,
                Is.EqualTo(_existingLector.Id));

            Assert.That(
                result.First().UserFirstName,
                Is.EqualTo(_existingLector.User.FirstName));

            Assert.That(
                result.First().UserLastName,
                Is.EqualTo(_existingLector.User.LastName));

            Assert.That(
                result.Last().Id,
                Is.EqualTo(_anotherExistingLector.Id));

            Assert.That(
                result.Last().UserFirstName,
                Is.EqualTo(_anotherExistingLector.User.FirstName));

            Assert.That(
                result.Last().UserLastName,
                Is.EqualTo(_anotherExistingLector.User.LastName));
        }

        [Test]
        public async Task FindFreeLectors_LessonTime_ListOfFreeLectors()
        {
            var result = await _service.FindFreeLectors(_lessonTime);

            Assert.That(
                result.First().Id,
                Is.EqualTo(_existingLector.Id));

            Assert.That(
                result.Last().Id,
                Is.EqualTo(_anotherExistingLector.Id));

            Assert.That(
                result.Count(),
                Is.EqualTo(2));
        }

        [Test]
        public async Task FindFreeLectors_NotFullLessonTime_EmptyListOfLectors()
        {
            var result = await _service.FindFreeLectors(_anotherLessonTime);

            Assert.That(
                result.Count(),
                Is.EqualTo(0));
        }
    }
}
