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
        /// Get document by its id
        /// </summary>
        /// <param name="documentId">Document id</param>
        /// <returns>Document with the given id or null</returns>
        Task<Document?> GetById(string documentId);

        /// <summary>
        /// Add a new document
        /// </summary>
        /// <param name="document">Document</param>
        Task Add(Document document);

        /// <summary>
        /// Delete an existing document
        /// </summary>
        /// <param name="documentId">Document id</param>
        Task Delete(string documentId);
    }
}
