using CoStudyCloud.Core.Models;

namespace CoStudyCloud.Core.Repositories
{
    /// <summary>
    /// Represents Document repository interface
    /// </summary>
    public interface IDocumentRepository
    {
        /// <summary>
        /// Add a new document
        /// </summary>
        /// <param name="document">Document</param>
        Task Add(Document document);
    }
}
