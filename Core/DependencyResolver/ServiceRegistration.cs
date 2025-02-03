using Core.Utilities.Security.Abstract;
using Core.Utilities.Security.Concrete;
using Microsoft.Extensions.DependencyInjection;

namespace Core.DependencyResolver
{
    public static class ServiceRegistration
    {
        public static void AddCoreService(this IServiceCollection services)
        {
            services.AddScoped<ITokenService, TokenManager>();
        }
    }
}
