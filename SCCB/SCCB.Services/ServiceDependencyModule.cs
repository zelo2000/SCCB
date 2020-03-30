using Autofac;
using SCCB.Services.AuthenticationService;
using SCCB.Services.LessonService;
using SCCB.Services.EmailService;
using SCCB.Services.LessonService;

namespace SCCB.Services
{
    public class ServiceDependencyModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AuthenticationService.AuthenticationService>().As<IAuthenticationService>();
            builder.RegisterType<LessonService.UserService>().As<IUserService>();
            builder.RegisterType<EmailService.EmailService>().As<IEmailService>();
            builder.RegisterType<LessonService.LessonService>().As<ILessonService>();
        }
    }
}
