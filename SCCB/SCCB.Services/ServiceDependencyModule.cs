using Autofac;
using SCCB.Services.AuthenticationService;
using SCCB.Services.EmailService;

namespace SCCB.Services
{
    public class ServiceDependencyModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AuthenticationService.AuthenticationService>().As<IAuthenticationService>();
            builder.RegisterType<EmailService.EmailService>().As<IEmailService>();
        }
    }
}
