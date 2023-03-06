using _18_E_LEARN.DataAccess.Data.IRepository;
using _18_E_LEARN.DataAccess.Data.Models.Categories;
using _18_E_LEARN.DataAccess.Data.Models.Courses;
//using _18_E_LEARN.DataAccess.Data.ViewModels.Course;
using _18_E_LEARN.DataAccess.Data.ViewModels.Courses;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _18_E_LEARN.BusinessLogic.Services
{
    public class CourseService
    {
        public ICourseRepository _courseRepository;
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IHostingEnvironment _hostEnvironment;
        private readonly IConfiguration _configuration;

        public CourseService(IMapper mapper, IConfiguration configuration, IHostingEnvironment hostEnvironment, ICourseRepository courseRepository, ICategoryRepository categoryRepository)
        {
            _courseRepository = courseRepository;
            _categoryRepository = categoryRepository;
            _hostEnvironment = hostEnvironment;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<ServiceResponse> GetAllAsync()
        {
            IEnumerable<Course> courses = await _courseRepository.GetAllAsync();
            return new ServiceResponse
            {
                Success = true,
                Message = "All courses were loaded.",
                Payload = courses
            };
        }
        public async Task<ServiceResponse> GetByIdAsync(int id)
        {
            Course course = await _courseRepository.GetByIdAsync(id);
            if (course == null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Nothing was found",
                };
            }
            return new ServiceResponse
            {
                Success = true,
                Message = "A category was found",
                Payload = course
            };
        }

        public async Task<ServiceResponse> Create(AddCourseVM model)
        {
            var categoriesList = await _categoryRepository.GetAllAsync();
            foreach (var category in categoriesList)
            {
                if(category.Id == model.CategoryId)
                {
                    model.CategoryName = category.Name;
                }
            }
            if (model.Files != null)
            {
                string webPath = _hostEnvironment.WebRootPath;
                var files = model.Files;
                string upload = webPath + Settings.ImagePath;
                string fileName = Guid.NewGuid().ToString();
                string extension = Path.GetExtension(files[0].FileName);
                using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }
                model.Image = fileName + extension;
            }
            else
            {
                model.Image = Settings.DefaultCourseImage;
            }

            var mappedCourse = _mapper.Map<AddCourseVM, Course>(model);

            await _courseRepository.CreateAsync(mappedCourse);
            return new ServiceResponse
            {
                Success = true,
                Message = "Course was successfully updated."
            };
        }

        public async Task<ServiceResponse> EditAsync(EditCourseVM model)
        {
            var categoriesList = await _categoryRepository.GetAllAsync();
            foreach (var category in categoriesList)
            {
                if (category.Id == model.CategoryId)
                {
                    model.CategoryName = category.Name;
                }
            }
            var course = await _courseRepository.GetByIdAsync(model.Id);
            if (course == null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Category does not exist.",
                };
            }
            string webPath = _hostEnvironment.WebRootPath;
            string upload = webPath + Settings.ImagePath;
            if (course.Image != null)
            {
                File.Delete(upload + course.Image);
            }
            if (model.Files != null)
            {
                var files = model.Files;
                string fileName = Guid.NewGuid().ToString();
                string extension = Path.GetExtension(files[0].FileName);
                using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }
                model.Image = fileName + extension;
            }
            else
            {
                model.Image = Settings.DefaultCourseImage;
            }
            var mappedCourse = _mapper.Map<EditCourseVM, Course>(model);
            _courseRepository.Update(mappedCourse);

            return new ServiceResponse
            {
                Success = true,
                Message = "Category was updated.",
            };
        }
        public async Task<ServiceResponse> DeleteAsync(int id)
        {
            var result = await _courseRepository.GetByIdAsync(id);
            if (result.Image != null && result.Image != Settings.DefaultCourseImage)
            {
                string webPath = _hostEnvironment.WebRootPath;
                string upload = webPath + Settings.ImagePath;
                File.Delete(upload + result.Image);
            }
            if (result == null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Nothing was found",
                };
            }
            await _courseRepository.DeleteAsync(id);
            return new ServiceResponse
            {
                Success = true,
                Message = "A course was deleted",
                Payload = result
            };
        }
    }
}