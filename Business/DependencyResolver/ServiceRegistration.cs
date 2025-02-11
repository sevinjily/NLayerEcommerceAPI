using Business.Abstract;
using Business.Concrete;
using Business.Utilities.Storage.Abstract;
using Business.Utilities.Storage.Concrete;
using Business.Validations.FluentValidation;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Business.DependencyResolver
{
    public static class ServiceRegistration
    {
        public static void AddBusinessService(this IServiceCollection services)
        {
            services.AddScoped<AppDbContext>();
            services.AddScoped<ICategoryService,CategoryManager>();
            services.AddScoped<ICategoryDAL, EFCategoryDAL>();

            services.AddScoped<ISubCategoryService,SubCategoryManager>();
            services.AddScoped<ISubCategoryDAL, EFSubCategoryDAL>();

            services.AddScoped<IAuthService,AuthManager>();
            services.AddScoped<IRoleService, RoleManager>();

            services.AddScoped<IProductService, ProductManager>();
            services.AddScoped<IProductDAL,EFProductDAL>();

            ValidatorOptions.Global.LanguageManager = new CustomLanguageManager();

            services.AddIdentity<AppUser, AppRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
        }
        public static void AddStorageService<T>(this IServiceCollection services)
            where T:Storage,IStorage
        {

            services.AddScoped<IStorage,T>();
        } 
    }
}
