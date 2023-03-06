using _18_E_LEARN.DataAccess.Data.Models.Courses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _18_E_LEARN.DataAccess.Data.IRepository
{
    public interface ICourseRepository
    {
        Task<List<Course>> GetAllAsync();
        Task<Course> GetByIdAsync(int id);
        Task<Course> GetByNameAsync(string name);

        Task CreateAsync(Course model);
        string Update(Course course);
        Task DeleteAsync(int id);
    }
}
