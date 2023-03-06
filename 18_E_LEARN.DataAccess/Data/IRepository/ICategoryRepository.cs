using _18_E_LEARN.DataAccess.Data.Models.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _18_E_LEARN.DataAccess.Data.IRepository
{
    public interface ICategoryRepository // describe something
    {
        Task<List<Category>> GetAllAsync();
        Task<Category> GetByIdAsync(int id);
        Task<Category> GetByNameAsync(string name);
        string Update(Category category);

        Task DeleteAsync(int id);
        Task AddAsync(Category category);
    }
}
