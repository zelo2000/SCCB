using Autofac;
using SCCB.Services.AuthenticationService;

namespace SCCB.Services
{
    public class ServiceDependencyModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AuthenticationService.AuthenticationService>()
                .As<IAuthenticationService>();
        }
    }
}
