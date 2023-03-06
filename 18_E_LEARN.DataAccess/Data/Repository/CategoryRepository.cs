using _18_E_LEARN.DataAccess.Data.Context;
using _18_E_LEARN.DataAccess.Data.IRepository;
using _18_E_LEARN.DataAccess.Data.Models.Categories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _18_E_LEARN.DataAccess.Data.Repository
{
    public class CategoryRepository : ICategoryRepository // implementing 
    {
        public async Task<List<Category>> GetAllAsync() // just GetALl, no GetAllCategories
        {
            using (var _context = new AppDbContext()) // to close all connections
            {
                List<Category> categories = await _context.Categories.ToListAsync();
                return categories;
            }
        }
        //public void EditCategory(Category category)
        //{
        //    //using (var _context = new AppDbContext()) // to close all connections
        //    //{
        //    //    _context.Categories.First(c => c.Id == category.Id).Name = category.Name;
        //    //    _context.SaveChanges();
        //    //}
        //    using (var _context = new AppDbContext())
        //    {
        //        var result = _context.Categories.Update(category);
        //        _context.SaveChanges();
        //        return result.State.ToString();
        //    }
        //}
        public async Task<Category> GetByIdAsync(int id)
        {
            using (var _context = new AppDbContext())
            {
                Category result = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
                return result;
            }
        }
        public string Update(Category category)
        {
            using (var _context = new AppDbContext())
            {
                var result = _context.Categories.Update(category);
                _context.SaveChanges();
                return result.State.ToString();
            }
        }
        public async Task DeleteAsync(int id)
        {
            using (var _context = new AppDbContext()) // to close all connections
            {
                _context.Categories.Remove(await GetByIdAsync(id));
                _context.SaveChanges();
            }
        }
        public async Task AddAsync(Category category)
        {
            using (var _context = new AppDbContext()) // to close all connections
            {
                await _context.Categories.AddAsync(category);
                _context.SaveChanges();
            }
        }
        public async Task<Category> GetByNameAsync(string name)
        {
            using (var _context = new AppDbContext())
            {
                Category result = await _context.Categories.FirstOrDefaultAsync(c => c.Name == name);
                return result;
            }
        }
    }
}
