using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SCCB.Services.LectorService;
using SCCB.Web.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SCCB.Web.ViewComponents
{
    public class LectorOptionsViewComponent : ViewComponent
    {
        private readonly IMapper _mapper;
        private readonly ILectorService _lectorService;

        public LectorOptionsViewComponent(IMapper mapper, ILectorService lectorService)
        {
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
            _lectorService = lectorService ?? throw new ArgumentException(nameof(lectorService));
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var lectorDtos = await _lectorService.GetAllWithUserInfo();
            var lectorModels = _mapper.Map<IEnumerable<LectorModel>>(lectorDtos);
            return View(lectorModels);
        }
    }
}
