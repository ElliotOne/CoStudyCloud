using CoStudyCloud.Core.Models;

namespace CoStudyCloud.Core.Repositories
{
    /// <summary>
    /// Represents StudySession repository interface
    /// </summary>
    public interface IStudySessionRepository
    {
        /// <summary>
        /// Add a new study session
        /// </summary>
        /// <param name="studySession">StudySession</param>
        Task Add(StudySession studySession);
    }
}
