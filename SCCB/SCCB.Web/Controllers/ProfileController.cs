using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SCCB.Web.Models;
using SCCB.Services.UserService;
using SCCB.Services.AuthenticationService;
using SCCB.Core.Constants;
using SCCB.Core.DTO;

namespace SCCB.Web.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IAuthenticationService _authService;

        public ProfileController(IMapper mapper, IUserService userService, IAuthenticationService authService)
        {
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
            _userService = userService ?? throw new ArgumentException(nameof(userService));
            _authService = authService ?? throw new ArgumentException(nameof(authService));
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var id = Guid.Parse(User.FindFirst(ClaimKeys.Id).Value);
            var user = await _userService.Find(id);
            var profileModel = _mapper.Map<ProfileModel>(user);
            return View(profileModel);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(ProfileModel profileModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userDto = _mapper.Map<UserProfile>(profileModel);
                    userDto.Id = Guid.Parse(User.FindFirst(ClaimKeys.Id).Value);
                    await _userService.UpdateProfile(userDto);
                    return RedirectToAction("Index", "Home");
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
    }
}