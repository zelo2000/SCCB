using AutoMapper;

namespace SCCB.Services
{
    public class ServiceMapProfile : Profile
    {
        public ServiceMapProfile()
        {
            CreateMap<Core.DTO.User, DAL.Entities.User>();

            CreateMap<DAL.Entities.User, Core.DTO.User>();
        }
    }
}
