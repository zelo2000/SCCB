using Microsoft.AspNetCore.Mvc;
using SCCB.Core.DTO;
using SCCB.Services.GroupService;
using SCCB.Web.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SCCB.Web.ViewComponents
{
    public class UsersForGroupViewComponent : ViewComponent
    {
        private readonly IGroupService _groupService;

        public UsersForGroupViewComponent(IGroupService groupService)
        {
            _groupService = groupService ?? throw new ArgumentException(nameof(groupService));
        }

        public async Task<IViewComponentResult> InvokeAsync(Guid groupId, bool areMembers)
        {
            if (areMembers)
            {
                var model = new UsersForGroupModel
                {
                    GroupId = groupId,
                    Users = await _groupService.FindUsersInGroup(groupId),
                };
                return View("Members", model);
            }
            else
            {
                var model = new UsersForGroupModel
                {
                    GroupId = groupId,
                    Users = await _groupService.FindUsersNotInGroup(groupId),
                };
                return View("NotMembers", model);
            }
        }
    }
}
