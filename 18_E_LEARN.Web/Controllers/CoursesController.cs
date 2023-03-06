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
//using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace _18_E_LEARN.Web.Controllers
{
    public class CoursesController : Controller
    {
        private readonly IMapper _mapper;
        private readonly CategoryService _categoryService;
        private readonly CourseService _courseService;

        public CoursesController(IMapper mapper, CourseService courseService, CategoryService categoryService)
        {
            _mapper = mapper;
            _courseService = courseService;
            _categoryService = categoryService; 
        }
        public async Task<IActionResult> Index()
        {
            var result = await _courseService.GetAllAsync();
            return View(result.Payload);
        }
        public async Task<IActionResult> Edit(int id)
        {
            await LoadCategories();
            var result = await _courseService.GetByIdAsync(id);
            if (result.Success)
            {
                return View(result.Payload);
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditCourseVM model)
        {
            var validator = new EditCourseValidation();
            var validationResult = await validator.ValidateAsync(model);
            if (validationResult.IsValid)
            {
                await _courseService.EditAsync(model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
        public async Task<IActionResult> Add()
        {
            await LoadCategories();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddCourseVM model)
        {
            var validator = new AddCourseValidation();
            var validationresult = await validator.ValidateAsync(model);
            if (validationresult.IsValid)
            {
                if (model.Files != null)
                {
                    model.Files = HttpContext.Request.Form.Files;
                }

                await _courseService.Create(model);
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        private async Task LoadCategories()
        {
            var result = await _categoryService.GetAllAsync();
            ViewBag.CategoryList = new SelectList(
                (System.Collections.IEnumerable)result.Payload,
                nameof(Category.Id),
                nameof(Category.Name)
                );
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Category model) // ***
        {
            await _courseService.DeleteAsync(model.Id);
            return RedirectToAction(nameof(Index));
        }
    }
}