using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SCCB.Services.ClassroomService;
using SCCB.Web.Models;

namespace SCCB.Web.ViewComponents
{
    public class ClassroomOptionsViewComponent : ViewComponent
    {
        private readonly IMapper _mapper;
        private readonly IClassroomService _classroomService;

        public ClassroomOptionsViewComponent(IMapper mapper, IClassroomService classroomService)
        {
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
            _classroomService = classroomService ?? throw new ArgumentException(nameof(classroomService));
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var classroomsByBuilding = await _classroomService.GetAllGroupedByBuilding();
            return View(classroomsByBuilding);
        }
    }
}
