using _18_E_LEARN.DataAccess.Data.Models.Categories;
using _18_E_LEARN.DataAccess.Data.Models.Courses;
using _18_E_LEARN.DataAccess.Data.ViewModels.Courses;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _18_E_LEARN.DataAccess.Validation.Courses
{
    public class AddCourseValidation : AbstractValidator<AddCourseVM>
    {
        public AddCourseValidation()
        {
            RuleFor(r => r.Name).NotEmpty();
            RuleFor(r =>r.Description).NotEmpty();
            RuleFor(r=>r.Price).NotEmpty().GreaterThanOrEqualTo(0).LessThanOrEqualTo(1000000);
            RuleFor(r=>r.CategoryId).NotEmpty();

        }
    }
}
