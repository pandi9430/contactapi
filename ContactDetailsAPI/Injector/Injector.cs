using ContactDetailsAPI.Models;
using ContactDetailsAPI.Repository;
using ContactDetailsAPI.Service;
using UserDetailsAPI.Repository;
using UserDetailsAPI.Service;

namespace ContactDetailsAPI.Injector
{
    public static class Injector
    {
        public static void DependenceyInjection(IServiceCollection services)
        {
            //Auth
            services.AddScoped<IAuthRepository<LoginDTO>, AuthService>();

            //Authentication
            services.AddScoped<TokenService>();

            //Forgot Password
            services.AddScoped<IForgotRepository<ForgotPassword,string>, ForgotPasswordService>();

            //User Master
            services.AddScoped<IUserRepository<User, int, ResetPasswordFilter, RegisterFilter, UserStatusFilter>, UserService>();

            //Conatct Master
            services.AddScoped<IContactRepository<Contact, int>, ContactService>();
        }
    }
}
