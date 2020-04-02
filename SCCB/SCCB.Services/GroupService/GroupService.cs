using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SCCB.Core.DTO;
using SCCB.Core.Helpers;
using SCCB.Core.Settings;
using SCCB.Repos.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCCB.Services.GroupService
{
    public class GroupService : IGroupService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GroupService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task Add(Group groupDto)
        {
            var group = _mapper.Map<DAL.Entities.Group>(groupDto);
            await _unitOfWork.Groups.AddAsync(group);
            await _unitOfWork.CommitAsync();
        }

        public async Task<Group> Find(Guid id)
        {
            var group = await FindGroupEntity(id);
            var groupDto = _mapper.Map<Core.DTO.Group>(group);
            return groupDto;
        }

        public async Task Remove(Guid id)
        {
            var group = await FindGroupEntity(id);
            _unitOfWork.Groups.Remove(group);
            await _unitOfWork.CommitAsync();
        }

        public async Task Update(Group groupDto)
        {
            var group = await FindGroupEntity(groupDto.Id);

            group.Name = groupDto.Name;
            group.IsAcademic = groupDto.IsAcademic;
            

            _unitOfWork.Groups.Update(group);
            await _unitOfWork.CommitAsync();

        }

        public async Task<IEnumerable<Group>>  GetAll()
        {
            var groups = await _unitOfWork.Groups.GetAllAsync();
            var groupsDto = _mapper.Map<List<Core.DTO.Group>>(groups);
            return groupsDto;
        }

        public async Task<IEnumerable<Group>> GetAllAcademic()
        {
            var groups = await _unitOfWork.Groups.FindByIsAcademic(true);
            var groupsDto = _mapper.Map<List<Core.DTO.Group>>(groups);
            return groupsDto;
        }

        private async Task<DAL.Entities.Group> FindGroupEntity(Guid id)
        {
            var group = await _unitOfWork.Groups.FindAsync(id);

            if (group != null)
            {
                return group;
            }
            else
            {
                throw new ArgumentException($"Can not find group with id {id}");
            }
        }
    }
}
