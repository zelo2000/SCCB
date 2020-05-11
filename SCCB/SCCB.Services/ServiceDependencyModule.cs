using Autofac;
using SCCB.Services.AuthenticationService;
using SCCB.Services.BookingService;
using SCCB.Services.ClassroomService;
using SCCB.Services.EmailService;
using SCCB.Services.GroupService;
using SCCB.Services.LectorService;
using SCCB.Services.LessonService;
using SCCB.Services.UserService;

namespace SCCB.Services
{
    public class ServiceDependencyModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AuthenticationService.AuthenticationService>().As<IAuthenticationService>();
            builder.RegisterType<UserService.UserService>().As<IUserService>();
            builder.RegisterType<EmailService.EmailService>().As<IEmailService>();
            builder.RegisterType<LessonService.LessonService>().As<ILessonService>();
            builder.RegisterType<GroupService.GroupService>().As<IGroupService>();
            builder.RegisterType<ClassroomService.ClassroomService>().As<IClassroomService>();
            builder.RegisterType<LectorService.LectorService>().As<ILectorService>();
            builder.RegisterType<BookingService.BookingService>().As<IBookingService>();
        }
    }
}
