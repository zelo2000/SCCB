using AutoMapper;

namespace SCCB.Services
{
    public class ServiceMapProfile : Profile
    {
        public ServiceMapProfile()
        {
            // DTO -> Entities
            CreateMap<Core.DTO.User, DAL.Entities.User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password));
            CreateMap<Core.DTO.UserProfile, DAL.Entities.User>();
            CreateMap<Core.DTO.Lesson, DAL.Entities.Lesson>();
            CreateMap<Core.DTO.Group, DAL.Entities.Group>();
            CreateMap<Core.DTO.Classroom, DAL.Entities.Classroom>();
            CreateMap<Core.DTO.Lector, DAL.Entities.Lector>();

            // Entities -> DTO
            CreateMap<DAL.Entities.User, Core.DTO.User>()
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.PasswordHash));
            CreateMap<DAL.Entities.User, Core.DTO.UserProfile>();
            CreateMap<DAL.Entities.Lesson, Core.DTO.Lesson>();
            CreateMap<DAL.Entities.Group, Core.DTO.Group>();
            CreateMap<DAL.Entities.Classroom, Core.DTO.Classroom>();
            CreateMap<DAL.Entities.Lector, Core.DTO.Lector>();
        }
    }
}
