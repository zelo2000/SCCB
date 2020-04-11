using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SCCB.Services.ClassroomService;
using SCCB.Web.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            var classroomDtos = await _classroomService.GetAll();
            var classroomModels = _mapper.Map<IEnumerable<ClassroomModel>>(classroomDtos);
            return View(classroomModels);
        }
    }
}
