using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SCCB.Core.Constants;
using SCCB.Core.DTO;
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

        public AdminController(IMapper mapper, ILessonService lessonService, IGroupService groupService, IUserService userService)
        {
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
            _lessonService = lessonService ?? throw new ArgumentException(nameof(lessonService));
            _groupService = groupService ?? throw new ArgumentException(nameof(groupService));
            _userService = userService ?? throw new ArgumentException(nameof(userService));
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
        public IActionResult FreeClassrooms(string weekday, string number, bool isNumerator, bool isDenominator)
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
        public IActionResult FreeLectors(string weekday, string number, bool isNumerator, bool isDenominator)
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
            var lessonModel = _mapper.Map<ProfileModel>(lessonDto);
            return PartialView("_EditLessonPartial", lessonModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditLesson(LessonModel model)
        {
            if (ModelState.IsValid)
            {
                var lessonDto = _mapper.Map<Lesson>(model);
                await _lessonService.Update(lessonDto);
                return PartialView("_EditLessonPartial");
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
    }
}