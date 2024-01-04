using CoStudyCloud.Core.Models;

namespace CoStudyCloud.Core.Repositories
{
    /// <summary>
    /// Represents User repository interface
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Add new user to database
        /// </summary>
        /// <param name="user">User</param>
        public Task Add(User user);

        /// <summary>
        /// Check whether the user already exists
        /// </summary>
        /// <param name="email">User email</param>
        /// <returns>True if user already exists; otherwise, false</returns>
        Task<bool> Exists(string email);
    }
}
