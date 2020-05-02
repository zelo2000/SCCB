using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SCCB.Services.GroupService;
using SCCB.Services.UserService;

namespace SCCB.Web.Controllers
{
    public class GroupsController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IGroupService _groupService;

        public GroupsController(IMapper mapper, IUserService userService, IGroupService groupService)
        {
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
            _userService = userService ?? throw new ArgumentException(nameof(userService));
            _groupService = groupService ?? throw new ArgumentException(nameof(groupService));
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}