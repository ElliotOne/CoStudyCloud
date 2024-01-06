using CoStudyCloud.Core.Models;

namespace CoStudyCloud.Core.Repositories
{
    /// <summary>
    /// Represents Document repository interface
    /// </summary>
    public interface IDocumentRepository
    {
        /// <summary>
        /// Get all documents with the status indicating whether its owned by the given user
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>All documents with the ownership status</returns>
        Task<IEnumerable<DocumentWithOwnerStatus>> GetDocumentsWithOwnerStatus(string userId);

        /// <summary>
        /// Add a new document
        /// </summary>
        /// <param name="document">Document</param>
        Task Add(Document document);
    }
}
