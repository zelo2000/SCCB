using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SCCB.Services.GroupService;
using SCCB.Web.Models;

namespace SCCB.Web.ViewComponents
{
    /// <summary>
    /// View component for groups list.
    /// </summary>
    public class GroupOptionsViewComponent : ViewComponent
    {
        private readonly IGroupService _groupService;

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupOptionsViewComponent"/> class.
        /// </summary>
        /// <param name="groupService">Group service.</param>
        public GroupOptionsViewComponent(IGroupService groupService)
        {
            _groupService = groupService ?? throw new ArgumentException(nameof(groupService));
        }

        /// <summary>
        /// Method for calling partial view.
        /// </summary>
        /// <param name="groupId">Group identifier.</param>
        /// <returns>IViewComponentResult.</returns>
        public async Task<IViewComponentResult> InvokeAsync(Guid? groupId)
        {
            var groups = await _groupService.GetAllAcademic();
            var model = new GroupOptionsModel()
            {
                GroupId = groupId,
                Groups = groups,
            };

            return View(model);
        }
    }
}
