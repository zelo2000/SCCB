using AutoMapper;
using SCCB.Core.DTO;
using SCCB.Web.Models;

namespace SCCB.Web
{
    /// <summary>
    /// Web API map profile.
    /// </summary>
    public class WebApiMapProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebApiMapProfile"/> class.
        /// </summary>
        public WebApiMapProfile()
        {
            // Model -> DTO
            CreateMap<SignUpModel, User>();
            CreateMap<ProfileModel, UserProfile>();
            CreateMap<LessonModel, Lesson>();
            CreateMap<GroupModel, Group>();
            CreateMap<ClassroomModel, Classroom>();
            CreateMap<LectorModel, Lector>()
                .ForMember(dest => dest.UserFirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.UserLastName, opt => opt.MapFrom(src => src.LastName));
            CreateMap<BookingModel, Booking>();

            // DTO -> Model
            CreateMap<UserProfile, ProfileModel>();
            CreateMap<User, ProfileModel>();
            CreateMap<Lesson, LessonModel>();
            CreateMap<Group, GroupModel>();
            CreateMap<Classroom, ClassroomModel>();
            CreateMap<Lector, LectorModel>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.UserFirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.UserLastName));
            CreateMap<Booking, BookingModel>();
        }
    }
}
