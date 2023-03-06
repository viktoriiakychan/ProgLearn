﻿using _18_E_LEARN.BusinessLogic.Services;
using _18_E_LEARN.DataAccess.Data.Context;
using _18_E_LEARN.DataAccess.Data.Models.Categories;
using _18_E_LEARN.DataAccess.Data.Models.Courses;
using _18_E_LEARN.DataAccess.Data.Models.User;
using _18_E_LEARN.DataAccess.Data.ViewModels.Courses;
using _18_E_LEARN.DataAccess.Data.ViewModels.User;
using _18_E_LEARN.DataAccess.Validation.Categories;
using _18_E_LEARN.DataAccess.Validation.Courses;
using _18_E_LEARN.DataAccess.Validation.User;
using AutoMapper;
using FluentValidation;
//using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace _18_E_LEARN.Web.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IMapper _mapper;
        private readonly UserService _userService;
        public AdminController(UserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Users()
        {
            var user = await _userService.GetAllUsers();
            if (user.Success)
            {
                return View(user.Payload);
            }
            return View();
        }
        public async Task<IActionResult> Profile()
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userService.GetUserProfileAsync(userId);
            if (user.Success)
            {
                return View(user.Payload);
            }
            return View();
        }
        [AllowAnonymous]
        public IActionResult SignIn()
        {
            var user = HttpContext.User.Identity.IsAuthenticated;
            if (user)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SignIn(LoginUserVM model)
        {
            var validator = new LoginUserValidation();
            var validationResult = await validator.ValidateAsync(model);
            if (validationResult.IsValid)
            {
                var result = await _userService.LoginUserAsync(model);
                if (result.Success)
                {
                    return RedirectToAction("Index", "Admin");
                }
                ViewBag.AuthError = result.Message;
                return View(model);
            }
            return View(model);
        }

        [AllowAnonymous]
        public IActionResult SignUp()
        {
            var user = HttpContext.User.Identity.IsAuthenticated;
            if (user)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SignUp(RegisterUserVM model)
        {
            var validator = new RegisterUserValidation();
            var validationResult = await validator.ValidateAsync(model);
            if (validationResult.IsValid)
            {
                var result = await _userService.RegisterUserAsync(model);
                if (result.Success)
                {
                    return RedirectToAction("SignIn", "Admin");
                }
                ViewBag.AuthError = result.Message;
                return View(model); // error
            }
            ViewBag.AuthError = validationResult.Errors.First();
            return View(model);

        }
        public async Task<IActionResult> LogOut()
        {
            var result = await _userService.LogoutUserAsync();
            if (result.Success)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var result = await _userService.ConfirmEmailAsync(userId, token);
            if (result.Success)
            {
                return RedirectToAction("ConfirmEmailPage", "Admin");
            }
            return View();
        }

        [AllowAnonymous]
        public IActionResult ConfirmEmailPage()
        {
            return View();
        }

        [HttpPost]
        // [ValidateAntiForgeryToken] // secure 
        public async Task<IActionResult> UserSettings(UpdateProfileVM model)
        {
            var validator = new UpdateProfileValidation();
            var validationresult = await validator.ValidateAsync(model);
            if (validationresult.IsValid)
            {
                var result = await _userService.UpdateProfileAsync(model);
                if (result.Success)
                {
                    return RedirectToAction("SignIn", "Admin");
                }
                ViewBag.AuthError = result.Message;
                return View(model);

            }
            return View(model);
        }

        public async Task<IActionResult> UserSettings()
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _userService.GetUserForSettingsAsync(userId);
            if (result.Success)
            {
                return View(result.Payload);
            }
            return View();
        }
        public async Task<IActionResult> EditUser(string id)
        {
            var result = await _userService.GetUserIdForEditingAsync(id);
            if (result.Success)
            {
                return View(result.Payload);
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserVM model)
        {
            var validator = new EditUserValidation();
            var validationresult = await validator.ValidateAsync(model);
            if (validationresult.IsValid)
            {
                var result = await _userService.EditUserAsync(model);
                if (result.Success)
                {
                    return RedirectToAction("Users", "Admin");
                }
                ViewBag.AuthError = result.Message;
                return View(model);

            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(AppUser model) // ***
        {
            await _userService.DeleteAsync(model.Id);
            return RedirectToAction("Users", "Admin");
        }

        [HttpPost]
        public async Task<IActionResult> BlockUser(AppUser model) // ***
        {
            await _userService.BlockAsync(model.Id);
            return RedirectToAction("Users", "Admin");
        }
        //[HttpPost]
        public async Task<IActionResult> UnblockUser(AppUser model) // ***
        {
            await _userService.UnblockAsync(model.Id);
            return RedirectToAction("Users", "Admin");
        }
    }
}