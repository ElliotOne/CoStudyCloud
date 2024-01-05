using CoStudyCloud.Core.Models;

namespace CoStudyCloud.Core.Repositories
{
    /// <summary>
    /// Represents User repository interface
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns>All users</returns>
        Task<IEnumerable<User>> GetUsers();

        /// <summary>
        /// Get user by its email
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns>User with the given email or null</returns>
        Task<User?> GetByEmail(string email);

        /// <summary>
        /// Add a new user
        /// </summary>
        /// <param name="user">User</param>
        Task Add(User user);

        /// <summary>
        /// Update an existing user
        /// </summary>
        /// <param name="user">Update user</param>
        Task Update(User user);

        /// <summary>
        /// Check whether the user already exists
        /// </summary>
        /// <param name="email">User email</param>
        /// <returns>True if user already exists; otherwise, false</returns>
        Task<bool> Exists(string email);
    }
}
