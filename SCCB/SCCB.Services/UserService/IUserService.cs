using SCCB.Core.DTO;
using System;
using System.Threading.Tasks;

namespace SCCB.Services.LessonService
{
    public interface IUserService
    {
        /// <summary>
        /// Add user
        /// </summary>
        /// <param name="userDto">User</param>
        /// <returns>User id</returns>
        Task Add(User userDto);

        /// <summary>
        /// Update all user's properties
        /// </summary>
        /// <param name="userDto">Updated user data</param>
        Task Update(User userDto);

        /// <summary>
        /// Update user's email, firstname and lastname 
        /// </summary>
        /// <param name="userDto">Updated user data</param>
        Task UpdateProfile(UserProfile userDto);

        /// <summary>
        /// Update password
        /// </summary>
        /// <param name="id">User's id</param>
        /// <param name="oldPassword">Old password</param>
        /// <param name="newPassword">New Password</param>
        Task UpdatePassword(Guid id, string oldPassword, string newPassword);

        /// <summary>
        /// Remove user
        /// </summary>
        /// <param name="id">User's id</param>
        Task Remove(Guid id);

        /// <summary>
        /// Find user
        /// </summary>
        /// <param name="id">User's id</param>
        /// <returns>User</returns>
        Task<User> Find(Guid id);
    }
}
