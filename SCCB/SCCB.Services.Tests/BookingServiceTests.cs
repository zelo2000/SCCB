using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SCCB.Core.Constants;
using SCCB.Core.Helpers;
using SCCB.Core.Settings;
using SCCB.DAL.Entities;
using SCCB.Repos.Bookings;
using SCCB.Repos.UnitOfWork;
using SCCB.Services.BookingService;

namespace SCCB.Services.Tests
{
    public class BookingServiceTests
    {
        private IBookingService _service;

        private readonly string _existingUserPassword = "Pa$$word";
        private IOptions<HashGenerationSetting> _hashGenerationSetting;

        private IMapper _mapper;
        private Mock<IBookingRepository> _repositoryMock;
        private Mock<IUnitOfWork> _unitOfWorkMock;

        private Booking _existingBooking;
        private Booking _anotherExistingBooking;
        private Booking _newBooking;
        private Booking _oneMoreExistingBooking;
        private User _existingUser;
        private Classroom _existingClassroom;
        private Group _existingGroup;

        private DateTime dateTime = DateTime.Parse("12.12.2020");

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
            _existingBooking = new Booking()
            {
                Description = "Mafia",
                Date = new DateTime(2020, 12, 12),
                LessonNumber = 5,
                IsApproved = true,
                UserId = Guid.NewGuid(),
                ClassroomId = Guid.NewGuid(),
                GroupId = Guid.NewGuid(),
            };

            _anotherExistingBooking = new Booking()
            {
                Description = "Meeting",
                Date = new DateTime(2020, 9, 5),
                LessonNumber = 7,
                IsApproved = false,
                UserId = Guid.NewGuid(),
                ClassroomId = Guid.NewGuid(),
                GroupId = Guid.NewGuid(),
            };

            _newBooking = new Booking()
            {
                Description = "BordGames",
                Date = new DateTime(2020, 6, 20),
                LessonNumber = 6,
                IsApproved = false,
                UserId = Guid.NewGuid(),
                ClassroomId = Guid.NewGuid(),
                GroupId = Guid.NewGuid(),
            };

            _oneMoreExistingBooking = new Booking()
            {
                Description = "Meeting",
                Date = new DateTime(2020, 12, 12),
                LessonNumber = 7,
                IsApproved = false,
                User = _existingUser,
                Classroom = _existingClassroom,
                Group = _existingGroup,
            };

            _hashGenerationSetting = Options.Create(new HashGenerationSetting()
            {
                Salt = "EWEM9nXVuQHIWiBzPOEj9A==",
                IterationCount = 10000,
                BytesNumber = 32,
            });
            var passwordProcessor = new PasswordProcessor(_hashGenerationSetting.Value);

            _existingUser = new User()
            {
                Id = Guid.NewGuid(),
                FirstName = "Firstname",
                LastName = "Lastname",
                Email = "registered@gmail.com",
                PasswordHash = passwordProcessor.GetPasswordHash(_existingUserPassword),
                Role = Roles.Student,
            };

            _existingClassroom = new Classroom()
            {
                Id = Guid.NewGuid(),
                Number = "117",
                Building = "Main",
            };

            _existingGroup = new Group()
            {
                Id = Guid.NewGuid(),
                Name = "PMI33",
                IsAcademic = true,
            };

            #region setup mocks
            _repositoryMock = new Mock<IBookingRepository>();
            _repositoryMock.Setup(repo => repo.FindAsync(_existingBooking.Id)).ReturnsAsync(_existingBooking);
            _repositoryMock.Setup(repo => repo.FindAsync(_anotherExistingBooking.Id)).ReturnsAsync(_anotherExistingBooking);
            _repositoryMock.Setup(repo => repo.FindAsync(_newBooking.Id)).ReturnsAsync(_newBooking);
            _repositoryMock.Setup(repo => repo.AddAsync(_newBooking)).ReturnsAsync(_newBooking.Id);
            _repositoryMock.Setup(repo => repo.AddAsync(_existingBooking)).ReturnsAsync(_existingBooking.Id);
            _repositoryMock.Setup(repo => repo.Remove(_existingBooking));
            _repositoryMock.Setup(repo => repo.Remove(_anotherExistingBooking));
            _repositoryMock.Setup(repo => repo.Update(_anotherExistingBooking));
            _repositoryMock.Setup(repo => repo.FindBookingsWithIncludedInfo(dateTime, _oneMoreExistingBooking.LessonNumber, _existingClassroom.Id)).ReturnsAsync(new List<Booking> { _oneMoreExistingBooking });
            _repositoryMock.Setup(repo => repo.FindBookingsByCreator(_existingUser.Id)).ReturnsAsync(new List<Booking> { _existingBooking });
            _repositoryMock.Setup(repo => repo.FindBookingsByMember(_existingUser.Id)).ReturnsAsync(new List<Booking> { _anotherExistingBooking });


            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _unitOfWorkMock.Setup(uow => uow.Bookings).Returns(_repositoryMock.Object);
            _unitOfWorkMock.Setup(uow => uow.CommitAsync());
            #endregion

            _service = new BookingService.BookingService(
                _mapper, _unitOfWorkMock.Object);
        }

        [Test]
        public async Task Add_NewBooking_BookingRepositoryAddCalled()
        {
            var bookingDto = _mapper.Map<Core.DTO.Booking>(_newBooking);

            await _service.Add(bookingDto);

            _repositoryMock.Verify(repo => repo.AddAsync(It.Is<Booking>(booking =>
                booking.Description == _newBooking.Description &&
                booking.Date == _newBooking.Date &&
                booking.LessonNumber == _newBooking.LessonNumber &&
                booking.IsApproved == _newBooking.IsApproved &&
                booking.UserId == _newBooking.UserId &&
                booking.GroupId == _newBooking.GroupId &&
                booking.ClassroomId == _newBooking.ClassroomId)));

            _unitOfWorkMock.Verify(ouw => ouw.CommitAsync());
        }

        [Test]
        public async Task Approve_Booking_ReturnedBooking()
        {
            var bookingDto = _mapper.Map<Core.DTO.Booking>(_anotherExistingBooking);

            await _service.Approve(_anotherExistingBooking.Id);

            _repositoryMock.Verify(repo => repo.Update(It.Is<Booking>(booking =>
               booking.IsApproved == true)));

            _unitOfWorkMock.Verify(ouw => ouw.CommitAsync());
        }

        [Test]
        public async Task Remove_ExistingBooking_BookingRepositoryRemoveCalled()
        {
            var bookingDto = _mapper.Map<Core.DTO.Booking>(_existingBooking);

            await _service.Remove(_existingBooking.Id);

            _repositoryMock.Verify(repo => repo.Remove(It.Is<Booking>(booking =>
                booking.Description == _newBooking.Description &&
                booking.Date == _newBooking.Date &&
                booking.LessonNumber == _newBooking.LessonNumber &&
                booking.IsApproved == _newBooking.IsApproved &&
                booking.UserId == _newBooking.UserId &&
                booking.GroupId == _newBooking.GroupId &&
                booking.ClassroomId == _newBooking.ClassroomId)));

            _unitOfWorkMock.Verify(ouw => ouw.CommitAsync());
        }

        [Test]
        public async Task Find_BookingsWithIncludedInfo_ReturnedBooking()
        {
            var result = await _service.FindBookingsWithIncludedInfo(dateTime, _oneMoreExistingBooking.LessonNumber, _existingClassroom.Id);

            Assert.That(
                result.First().Date,
                Is.EqualTo(_oneMoreExistingBooking.Date));

            Assert.That(
                result.First().LessonNumber,
                Is.EqualTo(_oneMoreExistingBooking.LessonNumber));

            Assert.That(
                result.First().ClassroomId,
                Is.EqualTo(_oneMoreExistingBooking.ClassroomId));
        }

        [Test]
        public async Task Find_PersonalBookings_ReturnedBooking()
        {
            var result = await _service.FindPersonalBookings(_existingUser.Id);

            Assert.That(
                result.MyBookings.First().Id,
                Is.EqualTo(_existingBooking.Id));

            Assert.That(
                result.MyBookings.Last().Id,
                Is.EqualTo(_existingBooking.Id));

            Assert.That(
                result.MyGroupsBookings.First().Id,
                Is.EqualTo(_anotherExistingBooking.Id));

            Assert.That(
                result.MyGroupsBookings.Last().Id,
                Is.EqualTo(_anotherExistingBooking.Id));
        }
    }
}