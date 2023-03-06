using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _18_E_LEARN.DataAccess.Data.ViewModels.Courses
{
    public class AddCourseVM
    {
        //public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Image { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public IFormFileCollection Files { get; set; }

    }
}
