using CoStudyCloud.Core.Models;

namespace CoStudyCloud.Core.Repositories
{
    /// <summary>
    /// Represents StudySession repository interface
    /// </summary>
    public interface IStudySessionRepository
    {
        /// <summary>
        /// Get all study sessions with the group for the given user
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>All study sessions with the group for the given user</returns>
        Task<IEnumerable<StudySessionWithGroup>> GetStudySessionsWithGroups(string userId);

        /// <summary>
        /// Add a new study session
        /// </summary>
        /// <param name="studySession">StudySession</param>
        Task Add(StudySession studySession);
    }
}
