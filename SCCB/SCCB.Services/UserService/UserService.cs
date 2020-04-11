using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Options;
using SCCB.Core.DTO;
using SCCB.Core.Helpers;
using SCCB.Core.Settings;
using SCCB.Repos.UnitOfWork;

namespace SCCB.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly PasswordProcessor _passwordProcessor;

        public UserService(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IOptions<HashGenerationSetting> hashGenerationSetting)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _passwordProcessor = new PasswordProcessor(hashGenerationSetting.Value);
        }

        /// <inheritdoc />
        public async Task Add(Core.DTO.User userDto)
        {
            var user = await _unitOfWork.Users.FindByEmailAsync(userDto.Email);

            if (user == null)
            {
                user = _mapper.Map<DAL.Entities.User>(userDto);
                user.PasswordHash = _passwordProcessor.GetPasswordHash(userDto.Password);
                await _unitOfWork.Users.AddAsync(user);
                await _unitOfWork.CommitAsync();
            }
            else
            {
                throw new ArgumentException($"User with email {userDto.Email} already exists");
            }
        }

        /// <inheritdoc />
        public async Task<Core.DTO.User> Find(Guid id)
        {
            var user = await FindUserEntity(id);
            var userDto = _mapper.Map<Core.DTO.User>(user);
            return userDto;
        }

        /// <inheritdoc />
        public async Task Remove(Guid id)
        {
            var user = await FindUserEntity(id);
            _unitOfWork.Users.Remove(user);
            await _unitOfWork.CommitAsync();
        }

        /// <inheritdoc/>
        public async Task Update(User userDto)
        {
            var user = await FindUserEntity(userDto.Id);

            if (user.Email == userDto.Email || await CheckIfEmailAllowed(userDto.Email))
            {
                user.Email = userDto.Email;
                user.FirstName = userDto.FirstName;
                user.LastName = userDto.LastName;
                user.Role = userDto.Role;
                user.PasswordHash = _passwordProcessor.GetPasswordHash(userDto.Password);

                _unitOfWork.Users.Update(user);
                await _unitOfWork.CommitAsync();
            }
            else
            {
                throw new ArgumentException($"User with email {userDto.Email} already exists");
            }
        }

        /// <inheritdoc />
        public async Task UpdateProfile(Core.DTO.UserProfile userDto)
        {
            var user = await FindUserEntity(userDto.Id);

            if (user.Email == userDto.Email || await CheckIfEmailAllowed(userDto.Email))
            {
                user.Email = userDto.Email;
                user.FirstName = userDto.FirstName;
                user.LastName = userDto.LastName;

                _unitOfWork.Users.Update(user);
                await _unitOfWork.CommitAsync();
            }
            else
            {
                throw new ArgumentException($"User with email {userDto.Email} already exists");
            }
        }

        /// <inheritdoc />
        public async Task UpdatePassword(Guid id, string oldPassword, string newPassword)
        {
            var user = await FindUserEntity(id);

            if (user.PasswordHash == _passwordProcessor.GetPasswordHash(oldPassword))
            {
                user.PasswordHash = _passwordProcessor.GetPasswordHash(newPassword);
                _unitOfWork.Users.Update(user);
                await _unitOfWork.CommitAsync();
            }
            else
            {
                throw new ArgumentException("Wrong password");
            }
        }

        /// <summary>
        /// Helper function that checks if user with specified email exists.
        /// </summary>
        /// <param name="id">User's id.</param>
        /// <returns>User if exists.</returns>
        private async Task<DAL.Entities.User> FindUserEntity(Guid id)
        {
            var user = await _unitOfWork.Users.FindAsync(id);

            if (user != null)
            {
                return user;
            }
            else
            {
                throw new ArgumentException($"Can not find user with id {id}");
            }
        }

        /// <summary>
        /// Checks if email is not already taken.
        /// </summary>
        /// <param name="email">Email.</param>
        /// <returns>False if email is already taken, otherwise true.</returns>
        private async Task<bool> CheckIfEmailAllowed(string email)
        {
            return await _unitOfWork.Users.FindByEmailAsync(email) == null;
        }
    }
}
