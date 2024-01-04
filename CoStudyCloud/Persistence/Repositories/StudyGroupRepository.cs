using CoStudyCloud.Core.Models;
using CoStudyCloud.Core.Repositories;
using Google.Cloud.Spanner.Data;

namespace CoStudyCloud.Persistence.Repositories
{
    /// <summary>
    /// Represents the default implementation of StudyGroup repository
    /// </summary>
    public class StudyGroupRepository : IStudyGroupRepository
    {
        private readonly IConfiguration _configuration;

        public StudyGroupRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task Add(StudyGroup studyGroup)
        {
            // Assuming you have a valid Spanner connection and table schema
            using var connection = new SpannerConnection(_configuration.GetConnectionString("SpannerConnection"));
            connection.Open();

            string query = @"
                INSERT INTO StudyGroups (
                    Title,
                    Description,
                    CreateDate,
                    AdminUserId
                ) VALUES (
                    @Title,
                    @Description,
                    @CreateDate,
                    @AdminUserId
                )";

            using var command = new SpannerCommand(query, connection);

            command.Parameters.Add(nameof(StudyGroup.Title), SpannerDbType.String).Value = studyGroup.Title;
            command.Parameters.Add(nameof(StudyGroup.Description), SpannerDbType.String).Value = studyGroup.Description;
            command.Parameters.Add(nameof(StudyGroup.CreateDate), SpannerDbType.Timestamp).Value = studyGroup.CreateDate;
            command.Parameters.Add(nameof(StudyGroup.AdminUserId), SpannerDbType.String).Value = studyGroup.AdminUserId;

            await command.ExecuteNonQueryAsync();
        }
    }
}
