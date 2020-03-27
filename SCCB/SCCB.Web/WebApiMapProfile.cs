using AutoMapper;
using SCCB.Core.DTO;
using SCCB.Web.Models;

namespace SCCB.Web
{
    public class WebApiMapProfile : Profile
    {
        public WebApiMapProfile()
        {
            // Model -> DTO
            CreateMap<SignUpModel, User>();
            CreateMap<ProfileModel, UserProfile>();

            // DTO -> Model
            CreateMap<UserProfile, ProfileModel>();
            CreateMap<User, ProfileModel>();
        }
    }
}
