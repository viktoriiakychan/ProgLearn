using _18_E_LEARN.DataAccess.Data.Context;
using _18_E_LEARN.DataAccess.Data.IRepository;
using _18_E_LEARN.DataAccess.Data.Models.Categories;
using _18_E_LEARN.DataAccess.Data.Models.User;
using _18_E_LEARN.DataAccess.Data.ViewModels.User;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _18_E_LEARN.BusinessLogic.Services
{
    public class CategoryService
    {
        public ICategoryRepository _categoryRepository;
        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<ServiceResponse> GetAllAsync()
        {
            List<Category> categories = await _categoryRepository.GetAllAsync();
            return new ServiceResponse
            {
                Success = true,
                Message = "All categories were loaded",
                Payload = categories
            };
        }
        public async Task<ServiceResponse> EditAsync(Category model)
        {
            var category = await _categoryRepository.GetByNameAsync(model.Name);
            if (category != null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Category exists.",
                };
            }
            _categoryRepository.Update(model);

            return new ServiceResponse
            {
                Success = true,
                Message = "Category updated.",
            };
        }
        public async Task<ServiceResponse> GetByIdAsync(int id)
        {
            Category category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
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
                Payload = category
            };
        }
        public async Task<ServiceResponse> DeleteAsync(int id)
        {
            var category = _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Nothing was found",
                };
            }
            await _categoryRepository.DeleteAsync(id);
            return new ServiceResponse
            {
                Success = true,
                Message = "A category was deleted",
                Payload = category
            };
        }
        public async Task<ServiceResponse> AddAsync(Category category)
        {
            await _categoryRepository.AddAsync(category);
            return new ServiceResponse
            {
                Success = true,
                Message = "All categories were loaded"
            };
        }
    }
}
