using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SCCB.Services.LessonService;
using SCCB.Web.Models;

namespace SCCB.Web.ViewComponents
{
    public class LessonsForDayViewComponent : ViewComponent
    {
        private readonly IMapper _mapper;
        private readonly ILessonService _lessonService;

        public LessonsForDayViewComponent(IMapper mapper, ILessonService lessonService)
        {
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
            _lessonService = lessonService ?? throw new ArgumentException(nameof(lessonService));
        }

        public async Task<IViewComponentResult> InvokeAsync(Guid groupId, string weekday)
        {
            var lessonDtos = await _lessonService.FindByGroupIdAndWeekday(groupId, weekday);
            return View(lessonDtos);
        }
    }
}
