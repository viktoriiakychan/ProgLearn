using _18_E_LEARN.DataAccess.Data.Context;
using _18_E_LEARN.DataAccess.Data.IRepository;
using _18_E_LEARN.DataAccess.Data.Models.Categories;
using _18_E_LEARN.DataAccess.Data.Models.Courses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _18_E_LEARN.DataAccess.Data.Repository
{
    public class CourseRepository : ICourseRepository
    {
        public async Task<List<Course>> GetAllAsync()
        {
            using (var _context = new AppDbContext())
            {
                List<Course> courses = await _context.Courses.ToListAsync();
                return courses;
            }
        }

        public async Task<Course> GetByIdAsync(int id)
        {
            using (var _context = new AppDbContext())
            {
                Course result = await _context.Courses.FirstOrDefaultAsync(c => c.Id == id);
                return result;
            }
        }

        public async Task<Course> GetByNameAsync(string name)
        {
            using (var _context = new AppDbContext())
            {
                Course result = await _context.Courses.FirstOrDefaultAsync(c => c.Name == name);
                return result;
            }
        }
        public async Task AddAsync(Course course)
        {
            using (var _context = new AppDbContext()) // to close all connections
            {
                await _context.Courses.AddAsync(course);
                _context.SaveChanges();
            }
        }
        public async Task CreateAsync(Course model)
        {
            using (var _context = new AppDbContext())
            {
                await _context.Courses.AddAsync(model);
                await _context.SaveChangesAsync();
            }
        }

        public string Update(Course course)
        {

            using (var _context = new AppDbContext())
            {
                var result = _context.Courses.Update(course);
                _context.SaveChanges();
                return result.State.ToString();
            }
        }
        public async Task DeleteAsync(int id)
        {
            using (var _context = new AppDbContext()) // to close all connections
            {
                _context.Courses.Remove(await GetByIdAsync(id));
                _context.SaveChanges();
            }
        }
    }
}
