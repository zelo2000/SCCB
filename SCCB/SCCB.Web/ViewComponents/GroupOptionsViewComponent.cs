using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SCCB.Services.GroupService;
using SCCB.Web.Models;

namespace SCCB.Web.ViewComponents
{
    public class GroupOptionsViewComponent : ViewComponent
    {
        private readonly IMapper _mapper;
        private readonly IGroupService _groupService;

        public GroupOptionsViewComponent(IMapper mapper, IGroupService groupService)
        {
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
            _groupService = groupService ?? throw new ArgumentException(nameof(groupService));
        }

        public async Task<IViewComponentResult> InvokeAsync(Guid? groupId)
        {
            var classroomDtos = await _groupService.GetAllAcademic();
            var classroomModels = _mapper.Map<IEnumerable<GroupModel>>(classroomDtos);
            var model = new GroupOptionsModel()
            {
                GroupId = groupId,
                Groups = classroomModels,
            };
            return View(model);
        }
    }
}
