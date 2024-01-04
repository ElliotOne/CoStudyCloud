using CoStudyCloud.Core.Models;

namespace CoStudyCloud.Core.Repositories
{
    /// <summary>
    /// Represents StudyGroup repository interface
    /// </summary>
    public interface IStudyGroupRepository
    {
        /// <summary>
        /// Add a new study group
        /// </summary>
        /// <param name="studyGroup">StudyGroup</param>
        public Task Add(StudyGroup studyGroup);
    }
}
