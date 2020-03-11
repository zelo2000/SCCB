using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SCCB.Core.Settings;
using SCCB.Web.Models;
using System;
using System.Threading.Tasks;

namespace SCCB.Web.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly Services.AuthenticationService.IAuthenticationService _authenticationService;
        private readonly AuthSetting _authSetting;

        public AuthenticationController(Services.AuthenticationService.IAuthenticationService authenticationService,
            IOptions<AuthSetting> authSetting)
        {
            _authenticationService = authenticationService;
            _authSetting = authSetting.Value;
        }

        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(LogInModel logInModel)
        {
            if (ModelState.IsValid)
            {
                var claimsPrinciple = await _authenticationService.LogIn(
                    logInModel.Email, logInModel.Password);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    claimsPrinciple,
                    new AuthenticationProperties
                    {
                        ExpiresUtc = DateTime.UtcNow.AddHours(_authSetting.ExpiredAt)
                    }
                );

                return RedirectToAction("Index", "Home");
            }
            return View(logInModel);
        }
    }
}