using _18_E_LEARN.BusinessLogic.Services;
using _18_E_LEARN.DataAccess.Data.Models.Categories;
using _18_E_LEARN.DataAccess.Data.Models.Courses;
using _18_E_LEARN.DataAccess.Data.ViewModels.Courses;
using _18_E_LEARN.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.DependencyResolver;
using System.Diagnostics;
using System.Dynamic;

namespace _18_E_LEARN.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserService _userService;
        private readonly CategoryService _categoryService;
        private readonly CourseService _courseService;
        public HomeController(ILogger<HomeController> logger, CategoryService categoryService, CourseService courseService)
        {
            _logger = logger;
            _categoryService = categoryService;
            _courseService = courseService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public async Task<IActionResult> Categories()
        {
            var result = await _categoryService.GetAllAsync();
            return View(result.Payload);
        }
        public async Task<IActionResult> Courses(string name)
        {
            if(name != null)
            {
                var courses = await _courseService._courseRepository.GetAllAsync();
                var result = courses.Where(c => c.CategoryName == name).ToList();
                var tupleModel = new Tuple<List<Course>, List<Category>>(result, await _categoryService._categoryRepository.GetAllAsync());
                return View(tupleModel);
            }
            else
            {
                var tupleModel = new Tuple<List<Course>, List<Category>>(await _courseService._courseRepository.GetAllAsync(), await _categoryService._categoryRepository.GetAllAsync());
                return View(tupleModel);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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
    }
}