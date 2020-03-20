using AutoMapper;

namespace SCCB.Services
{
    public class ServiceMapProfile : Profile
    {
        public ServiceMapProfile()
        {
            // DTO -> Entities
            CreateMap<Core.DTO.User, DAL.Entities.User>();

            // Entities -> DTO
            CreateMap<DAL.Entities.User, Core.DTO.User>();
        }
    }
}
