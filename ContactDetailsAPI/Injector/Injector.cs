using ContactDetailsAPI.Data;
using ContactDetailsAPI.Models;
using ContactDetailsAPI.Repository;
using ContactDetailsAPI.Service;
using DB.Common.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace ContactDetailsAPI.Injector
{
    public static class Injector
    {

        public static void DependenceyInjection(IServiceCollection services)
        {

            //Authentication
            services.AddScoped<IAuthRepository, AuthService>();
            services.AddScoped<ISqlDataAccessRepository, SqlDataAccessRepository>();
            services.AddScoped<TokenService>();

            //Conatct Master
            services.AddScoped<ISqlDataAccessRepository, SqlDataAccessRepository>();
            services.AddScoped<IContactRepository<Contact, int>, ContactService>();
        }
    }
}
