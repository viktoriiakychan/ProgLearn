using _18_E_LEARN.BusinessLogic.Services;
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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace _18_E_LEARN.Web.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly IMapper _mapper;
        private readonly CategoryService _categoryService;
        public CategoriesController(IMapper mapper, CategoryService categoryService)
        {
            _mapper = mapper;
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index()
        {
            var result = await _categoryService.GetAllAsync();
            return View(result.Payload);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _categoryService.GetByIdAsync(id);
            if (result.Success)
            {
                return View(result.Payload);
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Category model)
        {
            var validator = new EditCategoryValidation();
            var validationResult = await validator.ValidateAsync(model);
            if (validationResult.IsValid)
            {
                await _categoryService.EditAsync(model);
                return RedirectToAction("Index", "Categories");
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(Category model)
        {
            await _categoryService.DeleteAsync(model.Id);
            return RedirectToAction("Index", "Categories");

        }
        public async Task<IActionResult> Add(int id)
        {
            var result = await _categoryService.GetByIdAsync(id);
            if (result.Success)
            {
                return View(result.Payload);
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(Category model)
        {
            var validator = new EditCategoryValidation();
            var validationResult = await validator.ValidateAsync(model);
            if (validationResult.IsValid)
            {
                _categoryService.AddAsync(model);
                return RedirectToAction("Index", "Categories");
            }
            return View(model);
        }
    }
}