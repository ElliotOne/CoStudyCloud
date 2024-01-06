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
    }
}
