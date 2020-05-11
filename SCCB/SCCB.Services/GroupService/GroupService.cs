using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SCCB.Core.Constants;
using SCCB.Core.DTO;
using SCCB.Repos.UnitOfWork;

namespace SCCB.Services.GroupService
{
    /// <summary>
    /// Group service.
    /// </summary>
    public class GroupService : IGroupService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupService"/> class.
        /// </summary>
        /// <param name="mapper">Mapper instance.</param>
        /// <param name="unitOfWork">UnitOfWork instance.</param>
        public GroupService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        /// <inheritdoc/>
        public async Task<Guid> Add(Group groupDto)
        {
            var group = _mapper.Map<DAL.Entities.Group>(groupDto);
            var id = await _unitOfWork.Groups.AddAsync(group);

            var ownerToGroup = new DAL.Entities.UsersToGroups
            {
                UserId = groupDto.OwnerId,
                GroupId = id,
                IsUserOwner = true,
            };
            _unitOfWork.Groups.AddUser(ownerToGroup);

            await _unitOfWork.CommitAsync();

            return id;
        }

        /// <inheritdoc/>
        public async Task<Group> Find(Guid id)
        {
            var group = await FindGroupEntity(id);
            var groupDto = _mapper.Map<Group>(group);
            return groupDto;
        }

        /// <inheritdoc/>
        public async Task Remove(Guid id)
        {
            var group = await FindGroupEntity(id);
            _unitOfWork.Groups.Remove(group);
            await _unitOfWork.CommitAsync();
        }

        /// <inheritdoc/>
        public async Task Update(Group groupDto)
        {
            var group = await FindGroupEntity(groupDto.Id);

            group.Name = groupDto.Name;
            group.IsAcademic = groupDto.IsAcademic;

            _unitOfWork.Groups.Update(group);
            await _unitOfWork.CommitAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Group>> GetAll()
        {
            var groups = await _unitOfWork.Groups.GetAllAsync();
            var groupsDto = _mapper.Map<List<Core.DTO.Group>>(groups);
            return groupsDto;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Group>> GetAllAcademic()
        {
            var groups = await _unitOfWork.Groups.FindByIsAcademic(true);
            var groupsDto = _mapper.Map<List<Core.DTO.Group>>(groups);
            return groupsDto;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Group>> FindByOption(string option)
        {
            IEnumerable<DAL.Entities.Group> groups;

            if (option == GroupOptions.All)
            {
                groups = await _unitOfWork.Groups.GetAllAsync();
            }
            else if (option == GroupOptions.Academic)
            {
                groups = await _unitOfWork.Groups.FindByIsAcademic(true);
            }
            else if (option == GroupOptions.UserDefined)
            {
                groups = await _unitOfWork.Groups.FindByIsAcademic(false);
            }
            else
            {
                throw new ArgumentException($"Wrong option {option}");
            }

            var groupDtos = _mapper.Map<IEnumerable<Group>>(groups);
            return groupDtos;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Group>> FindNotAcademic(Guid userId, bool isUserOwner)
        {
            var groups = await _unitOfWork.Groups.FindNotAcademic(userId, isUserOwner);
            var groupDtos = _mapper.Map<IEnumerable<Group>>(groups);
            return groupDtos;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<UserProfile>> FindUsersInGroup(Guid groupId)
        {
            var users = await _unitOfWork.Users.FindByGroupId(groupId);
            var userDtos = _mapper.Map<IEnumerable<UserProfile>>(users);
            return userDtos;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<UserProfile>> FindUsersNotInGroup(Guid groupId)
        {
            var allUsers = await _unitOfWork.Users.GetAllAsync();
            var usersInGroup = await _unitOfWork.Users.FindByGroupId(groupId);
            var usersNotInGroup = allUsers.Except(usersInGroup);
            var userDtos = _mapper.Map<IEnumerable<UserProfile>>(usersNotInGroup);
            return userDtos;
        }

        /// <inheritdoc/>
        public async Task<bool> CheckOwnership(Guid userId, Guid groupId)
        {
            var ownerId = await _unitOfWork.Groups.GetOwner(groupId);
            return userId == ownerId;
        }

        /// <inheritdoc/>
        public async Task<Guid> AddUser(Guid userId, Guid groupId)
        {
            var userToGroup = new DAL.Entities.UsersToGroups
            {
                UserId = userId,
                GroupId = groupId,
                IsUserOwner = false,
            };
            var id = _unitOfWork.Groups.AddUser(userToGroup);
            await _unitOfWork.CommitAsync();
            return id;
        }

        /// <inheritdoc/>
        public async Task RemoveUser(Guid userId, Guid groupId)
        {
            var userToGroup = await _unitOfWork.Groups.FindUserToGroup(userId, groupId);
            _unitOfWork.Groups.RemoveUser(userToGroup);
            await _unitOfWork.CommitAsync();
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
