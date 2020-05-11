using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SCCB.Core.Constants;
using SCCB.Core.DTO;
using SCCB.Services.GroupService;
using SCCB.Services.UserService;
using SCCB.Web.Models;

namespace SCCB.Web.Controllers
{
    [Authorize(Policy = Policies.ApprovedUserOnly)]
    public class GroupsController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IGroupService _groupService;

        public GroupsController(IMapper mapper, IUserService userService, IGroupService groupService)
        {
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
            _groupService = groupService ?? throw new ArgumentException(nameof(groupService));
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimKeys.Id).Value);

            var model = new GroupsModel
            {
                OwnedGroups = await _groupService.FindNotAcademic(userId, true),
                MemberGroups = await _groupService.FindNotAcademic(userId, false),
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(GroupModel model)
        {
            if (ModelState.IsValid)
            {
                var group = _mapper.Map<Group>(model);
                group.OwnerId = Guid.Parse(User.FindFirst(ClaimKeys.Id).Value);
                var groupId = await _groupService.Add(group);
                return RedirectToAction("Edit", new { id = groupId });
            }
            else
            {
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimKeys.Id).Value);
            var isOwner = await _groupService.CheckOwnership(userId, id);

            if (isOwner)
            {
                var group = await _groupService.Find(id);
                var model = _mapper.Map<GroupModel>(group);
                return View(model);
            }
            else
            {
                return RedirectToAction("AccessDenied", "Authentication", new { returnUrl = "Groups/Edit" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(GroupModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = Guid.Parse(User.FindFirst(ClaimKeys.Id).Value);
                var isOwner = await _groupService.CheckOwnership(userId, model.Id);

                if (isOwner)
                {
                    var group = _mapper.Map<Group>(model);
                    await _groupService.Update(group);
                    return View(model);
                }
                else
                {
                    return RedirectToAction("AccessDenied", "Authentication", new { returnUrl = "Groups/Edit" });
                }
            }
            else
            {
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(Guid userId, Guid groupId)
        {
            await _groupService.AddUser(userId, groupId);
            return RedirectToAction("Edit", new { id = groupId });
        }

        [HttpDelete]
        public async Task RemoveUser(Guid userId, Guid groupId)
        {
            await _groupService.RemoveUser(userId, groupId);
        }

        [HttpDelete]
        public async Task Remove(Guid id)
        {
            await _groupService.Remove(id);
        }
    }
}