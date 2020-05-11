using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using NUnit.Framework;
using SCCB.DAL.Entities;
using SCCB.Repos.Bookings;
using SCCB.Repos.UnitOfWork;
using SCCB.Services.BookingService;

namespace SCCB.Services.Tests
{
    public class BookingServiceTests
    {
        private IBookingService _service;

        private IMapper _mapper;
        private Mock<IBookingRepository> _repositoryMock;
        private Mock<IUnitOfWork> _unitOfWorkMock;

        private Booking _existingBooking;
        private Booking _anotherExistingBooking;

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
                Date = new DateTime(2020, 11, 20),
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
                IsApproved = true,
                UserId = Guid.NewGuid(),
                ClassroomId = Guid.NewGuid(),
                GroupId = Guid.NewGuid(),
            };

            _newBooking = new Booking()
            {
                Description = "BordGames",
                Date = new DateTime(2020, 6, 20),
                LessonNumber = 6,
                IsApproved = true,
                UserId = Guid.NewGuid(),
                ClassroomId = Guid.NewGuid(),
                GroupId = Guid.NewGuid(),
            };

            #region setup mocks
            _repositoryMock = new Mock<IBookingRepository>();
            _repositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Lesson>())).Returns(Task.FromResult(Guid.Empty));

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
                booking.ClassromId == _newBooking.ClassromId)));

            _unitOfWorkMock.Verify(ouw => ouw.CommitAsync());
        }
    }
}