using AutoMapper;
using SCCB.Core.DTO;
using SCCB.Repos.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SCCB.Services.BookingService
{
    public class BookingService : IBookingService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public BookingService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
            _unitOfWork = unitOfWork ?? throw new ArgumentException(nameof(unitOfWork));
        }

        /// <inheritdoc/>
        public async Task Add(Booking bookingDto)
        {
            var booking = _mapper.Map<DAL.Entities.Booking>(bookingDto);
            await _unitOfWork.Bookings.AddAsync(booking);
            await _unitOfWork.CommitAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<BookingWithIncludedInfo>> FindBookingsWithIncludedInfo(DateTime? date, int? lessonNumber, Guid? classroomId)
        {
            var bookings = await _unitOfWork.Bookings.FindBookingsWithIncludedInfo(date, lessonNumber, classroomId);
            var bookingDtos = _mapper.Map<IEnumerable<BookingWithIncludedInfo>>(bookings);
            return bookingDtos;
        }
    }
}
