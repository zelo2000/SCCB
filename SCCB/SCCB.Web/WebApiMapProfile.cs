using AutoMapper;
using SCCB.Core.DTO;
using SCCB.Web.Models;

namespace SCCB.Web
{
    public class WebApiMapProfile : Profile
    {
        public WebApiMapProfile()
        {
            CreateMap<SignUpModel, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password));
        }
    }
}
