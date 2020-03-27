using Autofac;
using SCCB.Services.AuthenticationService;
using SCCB.Services.UserService;
using SCCB.Services.EmailService;
>>>>>>>>> Temporary merge branch 2

namespace SCCB.Services
{
    public class ServiceDependencyModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AuthenticationService.AuthenticationService>().As<IAuthenticationService>();
            builder.RegisterType<UserService.UserService>().As<IUserService>();
            builder.RegisterType<EmailService.EmailService>().As<IEmailService>();
        }
    }
}
