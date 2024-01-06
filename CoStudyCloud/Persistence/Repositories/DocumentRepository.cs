using CoStudyCloud.Core.Models;
using CoStudyCloud.Core.Repositories;
using Google.Cloud.Spanner.Data;

namespace CoStudyCloud.Persistence.Repositories
{
    /// <summary>
    /// Represents the default implementation of Document repository
    /// </summary>
    public class DocumentRepository : IDocumentRepository
    {
        private readonly IConfiguration _configuration;

        public DocumentRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<DocumentWithOwnerStatus>> GetDocumentsWithOwnerStatus(string userId)
        {
            using var connection = new SpannerConnection(_configuration.GetConnectionString("SpannerConnection"));
            await connection.OpenAsync();

            string query = @"
                SELECT 
                    d.Id, d.StudyGroupId, d.UploaderUserId, d.Title, d.FileName, d.FileUrl, d.CreateDate,
                    sg.Title AS StudyGroupTitle,
                    u.FirstName AS UploaderUserFirstName, u.LastName AS UploaderUserLastName,
                    CASE WHEN d.UploaderUserId = @UserId THEN TRUE ELSE FALSE END AS IsOwned
                FROM 
                    Documents d
                JOIN 
                    StudyGroups sg ON d.StudyGroupId = sg.Id
                JOIN 
                    Users u ON d.UploaderUserId = u.Id";

            using var command = new SpannerCommand(query, connection);
            command.Parameters.Add("UserId", SpannerDbType.String).Value = userId;

            var documentsWithStatus = new List<DocumentWithOwnerStatus>();

            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var documentWithStatus = new DocumentWithOwnerStatus
                {
                    Id = reader[nameof(DocumentWithOwnerStatus.Id)]?.ToString(),
                    StudyGroupId = reader[nameof(DocumentWithOwnerStatus.StudyGroupId)]?.ToString(),
                    UploaderUserId = reader[nameof(DocumentWithOwnerStatus.UploaderUserId)]?.ToString(),
                    Title = reader[nameof(DocumentWithOwnerStatus.Title)]?.ToString(),
                    FileName = reader[nameof(DocumentWithOwnerStatus.FileName)]?.ToString(),
                    FileUrl = reader[nameof(DocumentWithOwnerStatus.FileUrl)]?.ToString(),
                    CreateDate = (DateTime)reader[nameof(DocumentWithOwnerStatus.CreateDate)],
                    StudyGroupTitle = reader[nameof(DocumentWithOwnerStatus.StudyGroupTitle)]?.ToString(),
                    UploaderUserFirstName = reader[nameof(DocumentWithOwnerStatus.UploaderUserFirstName)]?.ToString(),
                    UploaderUserLastName = reader[nameof(DocumentWithOwnerStatus.UploaderUserLastName)]?.ToString(),
                    IsOwned = reader.GetFieldValue<bool>("IsOwned")
                };

                documentsWithStatus.Add(documentWithStatus);
            }

            return documentsWithStatus;
        }

        public async Task<Document?> GetById(string documentId)
        {
            using var connection = new SpannerConnection(_configuration.GetConnectionString("SpannerConnection"));
            await connection.OpenAsync();

            string query = @"
                SELECT 
                    Id, StudyGroupId, UploaderUserId, Title, FileName, FileUrl, CreateDate
                FROM 
                    Documents
                WHERE 
                    Id = @DocumentId";

            using var command = new SpannerCommand(query, connection);
            command.Parameters.Add("DocumentId", SpannerDbType.String).Value = documentId;

            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Document
                {
                    Id = reader[nameof(Document.Id)]?.ToString(),
                    StudyGroupId = reader[nameof(Document.StudyGroupId)]?.ToString(),
                    UploaderUserId = reader[nameof(Document.UploaderUserId)]?.ToString(),
                    Title = reader[nameof(Document.Title)]?.ToString(),
                    FileName = reader[nameof(Document.FileName)]?.ToString(),
                    FileUrl = reader[nameof(Document.FileUrl)]?.ToString(),
                    CreateDate = (DateTime)reader[nameof(Document.CreateDate)],
                };
            }

            return null;
        }

        public async Task Add(Document document)
        {
            using var connection = new SpannerConnection(_configuration.GetConnectionString("SpannerConnection"));
            await connection.OpenAsync();

            string query = @"
                INSERT INTO Documents (
                    StudyGroupId,
                    UploaderUserId,
                    Title,
                    FileName,
                    FileUrl,
                    CreateDate
                ) VALUES (
                    @StudyGroupId,
                    @UploaderUserId,
                    @Title,
                    @FileName,
                    @FileUrl,
                    @CreateDate
                )";

            using var command = new SpannerCommand(query, connection);

            command.Parameters.Add(nameof(Document.StudyGroupId), SpannerDbType.String).Value = document.StudyGroupId;
            command.Parameters.Add(nameof(Document.UploaderUserId), SpannerDbType.String).Value = document.UploaderUserId;
            command.Parameters.Add(nameof(Document.Title), SpannerDbType.String).Value = document.Title;
            command.Parameters.Add(nameof(Document.FileName), SpannerDbType.String).Value = document.FileName;
            command.Parameters.Add(nameof(Document.FileUrl), SpannerDbType.String).Value = document.FileUrl;
            command.Parameters.Add(nameof(Document.CreateDate), SpannerDbType.Timestamp).Value = document.CreateDate;

            await command.ExecuteNonQueryAsync();
        }

        public async Task Delete(string documentId)
        {
            using var connection = new SpannerConnection(_configuration.GetConnectionString("SpannerConnection"));
            await connection.OpenAsync();

            string query = @"
                DELETE FROM 
                    Documents
                WHERE 
                    Id = @DocumentId";

            using var command = new SpannerCommand(query, connection);
            command.Parameters.Add("DocumentId", SpannerDbType.String).Value = documentId;

            await command.ExecuteNonQueryAsync();
        }
    }
}
