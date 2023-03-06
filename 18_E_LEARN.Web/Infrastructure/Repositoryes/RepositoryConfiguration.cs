﻿using _18_E_LEARN.DataAccess.Data.IRepository;
using _18_E_LEARN.DataAccess.Data.Repository;

namespace _18_E_LEARN.Web.Infrastructure.Repositoryes
{
    public class RepositoryConfiguration
    {
        public static void Config(IServiceCollection services)
        {
            // Add category repository
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICourseRepository, CourseRepository>();
        }
    }
}
