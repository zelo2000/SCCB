using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SCCB.Core.Constants;
using SCCB.Core.DTO;
using SCCB.Services.BookingService;
using SCCB.Web.Models;
using System;
using System.Threading.Tasks;

namespace SCCB.Web.Controllers
{
    /// <summary>
    /// Booking controller.
    /// </summary>
    public class BookingController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IBookingService _bookingService;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookingController"/> class.
        /// </summary>
        /// <param name="mapper">Mapper.</param>
        /// <param name="bookingService">Booking service.</param>
        public BookingController(IMapper mapper, IBookingService bookingService)
        {
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
            _bookingService = bookingService ?? throw new ArgumentException(nameof(bookingService));
        }

        /// <summary>
        /// Get method for calling personal booking page.
        /// </summary>
        /// <returns>IActionResult.</returns>
        [HttpGet]
        public IActionResult Personal()
        {
            return View();
        }

        /// <summary>
        /// Get method for calling booking create page.
        /// </summary>
        /// <returns>IActionResult.</returns>
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(BookingModel model)
        {
            if (ModelState.IsValid)
            {
                var modelDto = _mapper.Map<Booking>(model);
                modelDto.UserId = Guid.Parse(User.FindFirst(ClaimKeys.Id).Value);
                await _bookingService.Add(modelDto);
                return View();
            }
            else
            {
                return View(model);
            }
        }
    }
}
