using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using SCCB.Core.DTO;
using SCCB.Repos.UnitOfWork;

namespace SCCB.Services.LectorService
{
    public class LectorService : ILectorService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public LectorService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
            _unitOfWork = unitOfWork ?? throw new ArgumentException(nameof(unitOfWork));
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Lector>> GetAllWithUserInfo()
        {
            var lectors = await _unitOfWork.Lectors.GetAllWithUserInfoAsync();
            var lectorDtos = _mapper.Map<IEnumerable<Core.DTO.Lector>>(lectors);
            return lectorDtos;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Lector>> FindFreeLectors(LessonTime time)
        {
            if (!string.IsNullOrEmpty(time.Weekday) && time.LessonNumber != null)
            {
                var lectors = await _unitOfWork.Lectors.FindFreeLectors(time);
                var lectorDtos = _mapper.Map<IEnumerable<Lector>>(lectors);
                return lectorDtos;
            }
            else
            {
                return new List<Lector>();
            }
        }
    }
}
