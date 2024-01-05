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

        public async Task<IEnumerable<StudyGroupWithJoinStatus>> GetStudyGroupsWithJoinStatus(string userId)
        {
            using var connection = new SpannerConnection(_configuration.GetConnectionString("SpannerConnection"));
            connection.Open();

            string query = @"
                SELECT
                    sg.Id AS StudyGroupId,
                    usm.Id AS MappingId,
                    sg.Title,
                    sg.Description,
                    sg.CreateDate,
                    usm.UserId IS NOT NULL AS IsJoined
                FROM StudyGroups sg
                LEFT JOIN User_StudyGroup_Mapping usm ON sg.Id = usm.StudyGroupId AND usm.UserId = @UserId";

            using var command = new SpannerCommand(query, connection);
            command.Parameters.Add(nameof(userId), SpannerDbType.String).Value = userId;

            using var reader = await command.ExecuteReaderAsync();

            var studyGroups = new List<StudyGroupWithJoinStatus>();

            while (await reader.ReadAsync())
            {
                var studyGroup = new StudyGroupWithJoinStatus
                {
                    StudyGroupId = reader["StudyGroupId"]?.ToString(),
                    MappingId = reader["MappingId"]?.ToString(),
                    Title = reader["Title"]?.ToString(),
                    Description = reader["Description"]?.ToString(),
                    CreateDate = (DateTime)reader["CreateDate"],
                    IsJoined = !DBNull.Value.Equals(reader["MappingId"])
                };

                studyGroups.Add(studyGroup);
            }

            return studyGroups;
        }

        public async Task Add(StudyGroup studyGroup)
        {
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

        public async Task AddUserToStudyGroup(UserStudyGroup userStudyGroup)
        {
            using var connection = new SpannerConnection(_configuration.GetConnectionString("SpannerConnection"));
            connection.Open();

            string query = @"
                INSERT INTO User_StudyGroup_Mapping (
                    UserId,
                    StudyGroupId
                ) VALUES (
                    @UserId,
                    @StudyGroupId
                )";

            using var command = new SpannerCommand(query, connection);

            command.Parameters.Add(nameof(UserStudyGroup.UserId), SpannerDbType.String).Value = userStudyGroup.UserId;
            command.Parameters.Add(nameof(UserStudyGroup.StudyGroupId), SpannerDbType.String).Value = userStudyGroup.StudyGroupId;

            await command.ExecuteNonQueryAsync();
        }

        public async Task RemoveUserFromStudyGroup(string userStudyGroupMappingId)
        {
            using var connection = new SpannerConnection(_configuration.GetConnectionString("SpannerConnection"));
            connection.Open();

            string query = @"
                DELETE FROM User_StudyGroup_Mapping
                WHERE Id = @UserStudyGroupMappingId";

            using var command = new SpannerCommand(query, connection);

            command.Parameters.Add(nameof(userStudyGroupMappingId), SpannerDbType.String).Value = userStudyGroupMappingId;

            await command.ExecuteNonQueryAsync();
        }
    }
}
