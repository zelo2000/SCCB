using System;
using System.Threading.Tasks;
using SCCB.Core.DTO;

namespace SCCB.Services.UserService
{
    /// <summary>
    /// User service interface.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Add user.
        /// </summary>
        /// <param name="userDto">User.</param>
        /// <returns>Task.</returns>
        Task Add(User userDto);

        /// <summary>
        /// Update all user's properties.
        /// </summary>
        /// <param name="userDto">Updated user data.</param>
        /// <returns>Task.</returns>
        Task Update(User userDto);

        /// <summary>
        /// Update user's email, firstname and lastname.
        /// </summary>
        /// <param name="userDto">Updated user data.</param>
        /// <returns>Task.</returns>
        Task UpdateProfile(UserProfile userDto);

        /// <summary>
        /// Update password.
        /// </summary>
        /// <param name="id">User's id.</param>
        /// <param name="oldPassword">Old password.</param>
        /// <param name="newPassword">New Password.</param>
        /// <returns>Task.</returns>
        Task UpdatePassword(Guid id, string oldPassword, string newPassword);

        /// <summary>
        /// Remove user.
        /// </summary>
        /// <param name="id">User's id.</param>
        /// <returns>Task.</returns>
        Task Remove(Guid id);

        /// <summary>
        /// Find user.
        /// </summary>
        /// <param name="id">User's id.</param>
        /// <returns>User.</returns>
        Task<User> Find(Guid id);
    }
}
