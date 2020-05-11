using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SCCB.Core.Constants;
using SCCB.Core.DTO;
using SCCB.Services.BookingService;
using SCCB.Services.ClassroomService;
using SCCB.Services.GroupService;
using SCCB.Services.LessonService;
using SCCB.Services.UserService;
using SCCB.Web.Models;
using SCCB.Web.ViewComponents;

namespace SCCB.Web.Controllers
{
    [Authorize(Policy = Policies.AdminOnly)]
    public class AdminController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILessonService _lessonService;
        private readonly IGroupService _groupService;
        private readonly IUserService _userService;
        private readonly IClassroomService _classroomService;
        private readonly IBookingService _bookingService;

        public AdminController(IMapper mapper, ILessonService lessonService, IGroupService groupService,
            IUserService userService, IClassroomService classroomService, IBookingService bookingService)
        {
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
            _lessonService = lessonService ?? throw new ArgumentException(nameof(lessonService));
            _groupService = groupService ?? throw new ArgumentException(nameof(groupService));
            _userService = userService ?? throw new ArgumentException(nameof(userService));
            _classroomService = classroomService ?? throw new ArgumentException(nameof(classroomService));
            _bookingService = bookingService ?? throw new ArgumentException(nameof(bookingService));
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var id = Guid.Parse(User.FindFirst(ClaimKeys.Id).Value);
            var user = await _userService.Find(id);
            var profileModel = _mapper.Map<ProfileModel>(user);
            return View(profileModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(ProfileModel profileModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userDto = _mapper.Map<UserProfile>(profileModel);
                    userDto.Id = Guid.Parse(User.FindFirst(ClaimKeys.Id).Value);
                    await _userService.UpdateProfile(userDto);
                    return View(profileModel);
                }
                catch (ArgumentException e)
                {
                    ViewBag.Error = e.Message;
                    return View(profileModel);
                }
            }
            else
            {
                return View(profileModel);
            }
        }

        [HttpGet]
        public IActionResult EditSchedule(Guid? groupId)
        {
            var model = new ScheduleEditModel() { GroupId = groupId };
            return View(model);
        }

        [HttpGet]
        public IActionResult LessonsForDay(Guid groupId, string weekday)
        {
            return ViewComponent("LessonsForDay", new { groupId, weekday });
        }

        [HttpGet]
        public IActionResult FreeClassrooms(string weekday, int? number, bool isNumerator, bool isDenominator)
        {
            return ViewComponent(
                typeof(ClassroomOptionsViewComponent),
                new LessonTime
                {
                    Weekday = weekday,
                    LessonNumber = number,
                    IsNumerator = isNumerator,
                    IsDenominator = isDenominator,
                });
        }

        [HttpGet]
        public IActionResult FreeLectors(string weekday, int? number, bool isNumerator, bool isDenominator)
        {
            return ViewComponent(
                typeof(LectorOptionsViewComponent),
                new LessonTime
                {
                    Weekday = weekday,
                    LessonNumber = number,
                    IsNumerator = isNumerator,
                    IsDenominator = isDenominator,
                });
        }

        [HttpGet]
        public IActionResult AddLesson(Guid groupId, string weekday)
        {
            var model = new LessonModel()
            {
                GroupId = groupId,
                Weekday = weekday,
            };
            return PartialView("_AddLessonPartial", model);
        }

        [HttpPost]
        public async Task<IActionResult> AddLesson(LessonModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var lessonDto = _mapper.Map<Lesson>(model);
                    await _lessonService.Add(lessonDto);
                    return PartialView("_AddLessonPartial");
                }
                catch (ArgumentException e)
                {
                    ModelState.AddModelError(string.Empty, e.Message);
                    return PartialView("_AddLessonPartial", model);
                }
            }
            else
            {
                return PartialView("_AddLessonPartial", model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditLesson(Guid id)
        {
            var lessonDto = await _lessonService.Find(id);
            var lessonModel = _mapper.Map<LessonModel>(lessonDto);
            return PartialView("_EditLessonPartial", lessonModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditLesson(LessonModel model)
        {
            if (ModelState.IsValid)
            {
                var lessonDto = _mapper.Map<Lesson>(model);
                await _lessonService.Update(lessonDto);
                return PartialView("_EditLessonPartial", model);
            }
            else
            {
                return PartialView("_EditLessonPartial", model);
            }
        }

        [HttpDelete]
        public async Task RemoveLesson(Guid id)
        {
            await _lessonService.Remove(id);
        }

        /// <summary>
        /// Get method for calling users edit page.
        /// </summary>
        /// <param name="role">User role.</param>
        /// <returns>IActionResult.</returns>
        [HttpGet]
        public async Task<IActionResult> EditUsers(string role = "NotApprovedUser")
        {
            var id = Guid.Parse(User.FindFirst(ClaimKeys.Id).Value);
            var users = await _userService.FindByRoleWithoutOwnData(role, id);
            var model = new EditUsersModel() { Role = role, Users = users };
            return View(model);
        }

        /// <summary>
        /// Get method for calling booking approve page.
        /// </summary>
        /// <param name="date">Date.</param>
        /// <param name="lessonNumber">Lesson number.</param>
        /// <param name="classroomId">Classroom id.</param>
        /// <returns>IActionResult.</returns>
        [HttpGet]
        public async Task<IActionResult> ApproveBooking(DateTime? date, int? lessonNumber, Guid? classroomId)
        {
            var classrooms = await _classroomService.GetAll();
            var model = new BookingFilter
            {
                Date = date,
                LessonNumber = lessonNumber,
                ClassroomId = classroomId,
                Classrooms = new SelectList(classrooms, "Id", "Number"),
            };
            return View(model);
        }

        /// <summary>
        /// Get view component with filtered list of bookings.
        /// </summary>
        /// <param name="date">Date.</param>
        /// <param name="lessonNumber">Lesson number.</param>
        /// <param name="classroomId">Classroom id.</param>
        /// <returns>IActionResult.</returns>
        [HttpGet]
        public IActionResult Bookings(DateTime? date, int? lessonNumber, Guid? classroomId)
        {
            return ViewComponent(typeof(FilteredBookingsViewComponent), new { date, lessonNumber, classroomId });
        }

        /// <summary>
        /// Post method for approving booking with specified id.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <returns>Task.</returns>
        [HttpPost]
        public async Task Approve(Guid id)
        {
            await _bookingService.Approve(id);
        }

        /// <summary>
        /// Delete method for refusing booking with specified id.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <returns>Task.</returns>
        [HttpDelete]
        public async Task Reject(Guid id)
        {
            await _bookingService.Remove(id);
        }

        /// <summary>
        /// Get method for calling user edit modal page.
        /// </summary>
        /// <param name="id">User Identifier.</param>
        /// <returns>IActionResult.</returns>
        [HttpGet]
        public async Task<IActionResult> EditUser(Guid id)
        {
            var userDto = await _userService.FindWithLectorAndStudentInfoById(id);
            var userModel = _mapper.Map<UserModel>(userDto);

            return PartialView("_EditUserPartial", userModel);
        }

        /// <summary>
        /// Post method for user edit.
        /// </summary>
        /// <param name="model">UserModel.</param>
        /// <returns>IActionResult.</returns>
        [HttpPost]
        public async Task<IActionResult> EditUser(UserModel model)
        {
            if (model.Position == null && model.Role == "Lector")
            {
                ModelState.AddModelError("Position", "Lector position is required");
                return PartialView("_EditUserPartial", model);
            }
            else if (model.StudentId == null && model.Role == "Student")
            {
                ModelState.AddModelError("StudentId", "Student Id is required");
                return PartialView("_EditUserPartial", model);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var userDto = _mapper.Map<User>(model);
                    await _userService.Update(userDto);
                    return PartialView("_EditUserPartial", model);
                }
                catch (ArgumentException e)
                {
                    ViewBag.Error = e.Message;
                    return PartialView("_EditUserPartial", model);
                }
            }
            else
            {
                return PartialView("_EditUserPartial", model);
            }
        }

        /// <summary>
        /// Delete user method.
        /// </summary>
        /// <param name="id">User Identifier.</param>
        /// <returns>Task.</returns>
        [HttpDelete]
        public async Task RemoveUser(Guid id)
        {
            await _userService.Remove(id);
        }
    }
}