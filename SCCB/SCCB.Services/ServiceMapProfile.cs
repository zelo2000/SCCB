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

            // Entities -> DTO
            CreateMap<DAL.Entities.User, Core.DTO.User>()
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.PasswordHash));
            CreateMap<DAL.Entities.User, Core.DTO.UserProfile>();
        }
    }
}
