using AutoMapper;
using SCCB.Repos.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SCCB.Services.LectorService
{
    class LectorService : ILectorService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public LectorService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
            _unitOfWork = unitOfWork ?? throw new ArgumentException(nameof(unitOfWork));
        }

        public async Task<IEnumerable<Core.DTO.Lector>> GetAllWithUserInfo()
        {
            var lectors = await _unitOfWork.Lectors.GetAllWithUserInfoAsync();
            var lectorDtos = _mapper.Map<IEnumerable<Core.DTO.Lector>>(lectors);
            return lectorDtos;
        }
    }
}
