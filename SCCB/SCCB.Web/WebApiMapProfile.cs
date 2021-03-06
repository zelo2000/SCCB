﻿using AutoMapper;
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
            CreateMap<BookingModel, Booking>();
            CreateMap<UserModel, User>();
            CreateMap<GroupModel, Group>();

            // DTO -> Model
            CreateMap<UserProfile, ProfileModel>();
            CreateMap<User, ProfileModel>();
            CreateMap<Lesson, LessonModel>();
            CreateMap<Booking, BookingModel>();
            CreateMap<User, UserModel>();
            CreateMap<Group, GroupModel>();
        }
    }
}
