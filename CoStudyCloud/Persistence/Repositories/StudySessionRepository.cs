using CoStudyCloud.Core.Models;
using CoStudyCloud.Core.Repositories;
using Google.Cloud.Spanner.Data;

namespace CoStudyCloud.Persistence.Repositories
{
    /// <summary>
    /// Represents the default implementation of StudySession repository
    /// </summary>
    public class StudySessionRepository : IStudySessionRepository
    {
        private readonly IConfiguration _configuration;

        public StudySessionRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<StudySessionWithGroup>> GetStudySessionsWithGroups(string userId)
        {
            using var connection = new SpannerConnection(_configuration.GetConnectionString("SpannerConnection"));
            await connection.OpenAsync();

            string query = @"
                SELECT
                    s.Summary,
                    s.StartDate,
                    s.EndDate,
                    sg.Title AS StudyGroupTitle
                FROM
                    StudySessions s
                INNER JOIN
                    StudyGroups sg ON s.StudyGroupId = sg.Id
                WHERE
                    s.Id IN (
                        SELECT uss.StudySessionId
                        FROM User_StudySession_Mapping uss
                        WHERE uss.UserId = @UserId
                    )";

            using var command = new SpannerCommand(query, connection);
            command.Parameters.Add(nameof(UserStudySession.UserId), SpannerDbType.String).Value = userId;

            var studySessionsWithGroups = new List<StudySessionWithGroup>();

            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var studySessionWithGroup = new StudySessionWithGroup
                {
                    Summary = reader["Summary"]?.ToString(),
                    StartDate = (DateTime)reader["StartDate"],
                    EndDate = (DateTime)reader["EndDate"],
                    StudyGroupTitle = reader["StudyGroupTitle"]?.ToString()
                };

                studySessionsWithGroups.Add(studySessionWithGroup);
            }

            return studySessionsWithGroups;
        }

        public async Task Add(StudySession studySession)
        {
            using var connection = new SpannerConnection(_configuration.GetConnectionString("SpannerConnection"));
            await connection.OpenAsync();

            using (var transaction = await connection.BeginTransactionAsync())
            {
                try
                {
                    //Insert StudySession
                    string studySessionQuery = @"
                        INSERT INTO StudySessions (
                            Id,
                            StudyGroupId,
                            Title,
                            CalendarSyncId,
                            Summary,
                            Description,
                            CreateDate,
                            StartDate,
                            EndDate
                        ) VALUES (
                            @Id,
                            @StudyGroupId,
                            @Title,
                            @CalendarSyncId,
                            @Summary,
                            @Description,
                            @CreateDate,
                            @StartDate,
                            @EndDate
                        )";

                    using var command = new SpannerCommand(studySessionQuery, connection, transaction);

                    string studySessionId = Guid.NewGuid().ToString();

                    command.Parameters.Add(nameof(studySession.Id), SpannerDbType.String).Value = studySessionId;
                    command.Parameters.Add(nameof(studySession.StudyGroupId), SpannerDbType.String).Value = studySession.StudyGroupId;
                    command.Parameters.Add(nameof(studySession.Title), SpannerDbType.String).Value = studySession.Title;
                    command.Parameters.Add(nameof(studySession.CalendarSyncId), SpannerDbType.String).Value = studySession.CalendarSyncId;
                    command.Parameters.Add(nameof(studySession.Summary), SpannerDbType.String).Value = studySession.Summary;
                    command.Parameters.Add(nameof(studySession.Description), SpannerDbType.String).Value = studySession.Description;
                    command.Parameters.Add(nameof(studySession.CreateDate), SpannerDbType.Timestamp).Value = studySession.CreateDate;
                    command.Parameters.Add(nameof(studySession.StartDate), SpannerDbType.Timestamp).Value = studySession.StartDate;
                    command.Parameters.Add(nameof(studySession.EndDate), SpannerDbType.Timestamp).Value = studySession.EndDate;

                    await command.ExecuteNonQueryAsync();

                    //Insert User_StudySession_Mapping for each user in the StudyGroup
                    string userMappingQuery = @"
                                INSERT INTO User_StudySession_Mapping (
                                    UserId,
                                    StudySessionId
                                ) VALUES (
                                    @UserId,
                                    @StudySessionId
                                )";

                    foreach (var userId in await GetUsersInStudyGroup(studySession.StudyGroupId!))
                    {
                        using var userMappingCommand = new SpannerCommand(userMappingQuery, connection, transaction);

                        userMappingCommand.Parameters.Add(nameof(UserStudySession.UserId), SpannerDbType.String).Value = userId;
                        userMappingCommand.Parameters.Add(nameof(UserStudySession.StudySessionId), SpannerDbType.String).Value = studySessionId;

                        await userMappingCommand.ExecuteNonQueryAsync();
                    }

                    // Commit the transaction
                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    // Handle exceptions and possibly roll back the transaction
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        private async Task<List<string>> GetUsersInStudyGroup(string studyGroupId)
        {
            using var connection = new SpannerConnection(_configuration.GetConnectionString("SpannerConnection"));
            connection.Open();

            string query = @"
                SELECT UserId
                FROM User_StudyGroup_Mapping
                WHERE StudyGroupId = @StudyGroupId";

            using var command = new SpannerCommand(query, connection);
            command.Parameters.Add(nameof(UserStudyGroup.StudyGroupId), SpannerDbType.String).Value = studyGroupId;

            using var reader = await command.ExecuteReaderAsync();

            List<string> userIds = new List<string>();

            while (reader.Read())
            {
                userIds.Add(reader.GetFieldValue<string>("UserId"));
            }

            return userIds;
        }
    }
}
