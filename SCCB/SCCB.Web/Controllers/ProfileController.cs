﻿using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SCCB.Core.Constants;
using SCCB.Core.DTO;
using SCCB.Services.GroupService;
using SCCB.Services.UserService;
using SCCB.Web.Models;

namespace SCCB.Web.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IGroupService _groupService;
        private readonly ILogger<ProfileController> _log;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileController"/> class.
        /// </summary>
        /// <param name="mapper">Mapper instance.</param>
        /// <param name="userService">UserService instance.</param>
        /// <param name="groupService">GroupService instance.</param>
        /// <param name="log">Logger instance.</param>
        public ProfileController(
            IMapper mapper,
            IUserService userService,
            IGroupService groupService,
            ILogger<ProfileController> log)
        {
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
            _userService = userService ?? throw new ArgumentException(nameof(userService));
            _groupService = groupService ?? throw new ArgumentException(nameof(groupService));
            _log = log ?? throw new ArgumentException(nameof(userService));
        }

        [Authorize(Policy = Policies.UserOnly)]
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var id = Guid.Parse(User.FindFirst(ClaimKeys.Id).Value);
            var user = await _userService.FindWithLectorAndStudentInfoById(id);
            var profileModel = _mapper.Map<ProfileModel>(user);

            if (user.AcademicGroupId.HasValue)
            {
                var group = await _groupService.Find(user.AcademicGroupId.Value);
                profileModel.AcademicGroupName = group.Name;
            }

            profileModel.StudentId = user.StudentId;
            profileModel.LectorPosition = user.Position;
            return View(profileModel);
        }

        [Authorize(Policy = Policies.UserOnly)]
        [HttpPost]
        public async Task<IActionResult> Edit(ProfileModel profileModel)
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
                catch (Exception e)
                {
                    _log.LogInformation(e.Message);
                }
            }

            return View(profileModel);
        }

        [HttpGet]
        public PartialViewResult ChangePassword()
        {
            return PartialView("_ChangePasswordPartial");
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel passwords)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var id = Guid.Parse(User.FindFirst(ClaimKeys.Id).Value);
                    await _userService.UpdatePassword(id, passwords.OldPassword, passwords.NewPassword);
                    return PartialView("_ChangePasswordPartial");
                }
                catch (ArgumentException e)
                {
                    ModelState.AddModelError("OldPassword", e.Message);
                    return PartialView("_ChangePasswordPartial", passwords);
                }
                catch (Exception e)
                {
                    _log.LogInformation(e.Message);
                }
            }

            return PartialView("_ChangePasswordPartial", passwords);
        }
    }
}