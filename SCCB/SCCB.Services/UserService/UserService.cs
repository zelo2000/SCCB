﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Options;
using SCCB.Core.Constants;
using SCCB.Core.DTO;
using SCCB.Core.Helpers;
using SCCB.Core.Settings;
using SCCB.Repos.UnitOfWork;

namespace SCCB.Services.UserService
{
    /// <summary>
    /// User service.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly PasswordProcessor _passwordProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="mapper">Mapper instance.</param>
        /// <param name="unitOfWork">UnitOfWork instance.</param>
        /// <param name="hashGenerationSetting">HashGenerationSetting instance.</param>
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
        public async Task Add(User userDto)
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
        public async Task<User> Find(Guid id)
        {
            var user = await FindUserEntity(id);
            var userDto = _mapper.Map<User>(user);
            return userDto;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<User>> FindByRoleWithoutOwnData(string role, Guid id)
        {
            if (role != null)
            {
                var users = await _unitOfWork.Users.FindByRoleWithoutOwnData(role, id);
                var usersDto = _mapper.Map<IEnumerable<User>>(users);
                return usersDto;
            }
            else
            {
                return new List<User>();
            }
        }

        /// <inheritdoc/>
        public async Task<User> FindWithLectorAndStudentInfoById(Guid id)
        {
            var user = await _unitOfWork.Users.FindWithLectorAndStudentInfoById(id);
            var userDto = _mapper.Map<User>(user);

            if (userDto.Role == Roles.Lector)
            {
                userDto.Position = user.Lector.Position;
            }
            else if (userDto.Role == Roles.Student)
            {
                userDto.AcademicGroupId = user.Student.AcademicGroupId;
                userDto.StudentId = user.Student.Id;
            }

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
            var user = await _unitOfWork.Users.FindWithLectorAndStudentInfoById(userDto.Id);
            if (user == null)
            {
                throw new ArgumentException($"Can not find user with id {userDto.Id}");
            }

            if (user.Email == userDto.Email || await CheckIfEmailAllowed(userDto.Email))
            {
                user.Email = userDto.Email;
                user.FirstName = userDto.FirstName;
                user.LastName = userDto.LastName;

                if (userDto.Role == Roles.Lector)
                {
                    var lector = new DAL.Entities.Lector()
                    {
                        UserId = user.Id,
                        Position = userDto.Position,
                    };

                    await _unitOfWork.Lectors.AddAsync(lector);
                    await _unitOfWork.CommitAsync();
                }
                else if (userDto.Role == Roles.Admin)
                {
                    var admin = new DAL.Entities.Admin()
                    {
                        UserId = user.Id,
                    };
                    await _unitOfWork.Admins.AddAsync(admin);
                    await _unitOfWork.CommitAsync();
                }
                else if (userDto.Role == Roles.Student)
                {
                    var student = new DAL.Entities.Student()
                    {
                        Id = userDto.StudentId,
                        UserId = user.Id,
                        AcademicGroupId = userDto.AcademicGroupId.Value,
                    };

                    await _unitOfWork.Students.AddAsync(student);
                    await _unitOfWork.CommitAsync();
                }

                if (user.Role == Roles.Lector)
                {
                    _unitOfWork.Lectors.Remove(user.Lector);
                    await _unitOfWork.CommitAsync();
                }
                else if (user.Role == Roles.Admin)
                {
                    _unitOfWork.Admins.Remove(user.Admin);
                    await _unitOfWork.CommitAsync();
                }
                else if (user.Role == Roles.Student)
                {
                    _unitOfWork.Students.Remove(user.Student);
                    await _unitOfWork.CommitAsync();
                }

                user.Role = userDto.Role;

                _unitOfWork.Users.Update(user);
                await _unitOfWork.CommitAsync();
            }
            else
            {
                throw new ArgumentException($"User with email {userDto.Email} already exists");
            }
        }

        /// <inheritdoc />
        public async Task UpdateProfile(UserProfile userDto)
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
        /// Helper function that checks if user with specified id exists.
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
