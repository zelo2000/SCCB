using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using SCCB.Core.DTO;
using SCCB.Repos.UnitOfWork;

namespace SCCB.Services.BookingService
{
    /// <inheritdoc/>
    public class BookingService : IBookingService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookingService"/> class.
        /// </summary>
        /// <param name="mapper">Mapper.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        public BookingService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
            _unitOfWork = unitOfWork ?? throw new ArgumentException(nameof(unitOfWork));
        }

        /// <inheritdoc/>
        public async Task Add(Booking bookingDto)
        {
            var booking = _mapper.Map<DAL.Entities.Booking>(bookingDto);
            booking.IsApproved = false;
            await _unitOfWork.Bookings.AddAsync(booking);
            await _unitOfWork.CommitAsync();
        }

        /// <inheritdoc/>
        public async Task Approve(Guid id)
        {
            var booking = await _unitOfWork.Bookings.FindAsync(id);
            booking.IsApproved = true;
            _unitOfWork.Bookings.Update(booking);
            await _unitOfWork.CommitAsync();
        }

        /// <inheritdoc/>
        public async Task Remove(Guid id)
        {
            var booking = await _unitOfWork.Bookings.FindAsync(id);
            _unitOfWork.Bookings.Remove(booking);
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
