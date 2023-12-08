using Google.Cloud.Spanner.Data;

namespace CoStudyCloud.Persistence
{
    /// <summary>
    /// Represents Application Database Initializer
    /// </summary>
    public static class ApplicationDbInitializer
    {
        public static async Task Initialize(string connectionString)
        {
            await CreateTables(connectionString);
        }

        private static async Task CreateTables(string connectionString)
        {
            using var connection = new SpannerConnection(connectionString);
            {
                bool tablesAlreadyExist = false;

                string query = $"SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Users' LIMIT 1";

                using var command = connection.CreateSelectCommand(query);
                using var reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    tablesAlreadyExist = true;
                }

                if (!tablesAlreadyExist)
                {
                    // Create Users table
                    await ExecuteDdlAsync(connection, @"
                        CREATE TABLE Users (
                            Id STRING(36) DEFAULT (GENERATE_UUID()),
                            Username STRING(50),
                            Email STRING(100),
                            FirstName STRING(50),
                            LastName STRING(50),
                            GoogleId STRING(50),
                            ProfilePicURL STRING(255),
                            UserRole STRING(50),
                            CreateDate TIMESTAMP,
                            LastEditDate TIMESTAMP,
                        ) PRIMARY KEY (Id)");

                    // Create StudyGroups table
                    await ExecuteDdlAsync(connection, @"
                        CREATE TABLE StudyGroups (
                            Id STRING(36) DEFAULT (GENERATE_UUID()),
                            Title STRING(100),
                            Description STRING(MAX),
                            CreateDate TIMESTAMP,
                            AdminUserId STRING(36),
                            CONSTRAINT FK_StudyGroupAdmin FOREIGN KEY (AdminUserId) REFERENCES Users(Id) ON DELETE CASCADE,
                        ) PRIMARY KEY (Id)");

                    // Create User_StudyGroup_Mapping table
                    await ExecuteDdlAsync(connection, @"
                        CREATE TABLE User_StudyGroup_Mapping (
                            Id STRING(36) DEFAULT (GENERATE_UUID()),
                            UserId STRING(36),
                            StudyGroupId STRING(36),
                            ApprovalStatus INT64,
                            CONSTRAINT FK_UserStudyGroup_UserId FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE,
                            CONSTRAINT FK_UserStudyGroup_GroupId FOREIGN KEY (StudyGroupId) REFERENCES StudyGroups(Id) ON DELETE CASCADE
                        ) PRIMARY KEY (Id)");

                    // Create StudySessions table
                    await ExecuteDdlAsync(connection, @"
                        CREATE TABLE StudySessions (
                            Id STRING(36) DEFAULT (GENERATE_UUID()),
                            StudyGroupId STRING(36),
                            Title STRING(100),
                            CalendarSyncId STRING(50),
                            Summary STRING(255),
                            Details STRING(MAX),
                            CreateDate TIMESTAMP,
                            StartDateTime TIMESTAMP,
                            EndDateTime TIMESTAMP,
                            CONSTRAINT FK_StudySession_StudyGroup FOREIGN KEY (StudyGroupId) REFERENCES StudyGroups(Id) ON DELETE CASCADE,
                        ) PRIMARY KEY (Id)");

                    // Create User_StudySession_Mapping table
                    await ExecuteDdlAsync(connection, @"
                        CREATE TABLE User_StudySession_Mapping (
                            Id STRING(36) DEFAULT (GENERATE_UUID()),
                            UserId STRING(36),
                            StudySessionId STRING(36),
                            CONSTRAINT FK_UserStudySession_User FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE,
                            CONSTRAINT FK_UserStudySession_StudySession FOREIGN KEY (StudySessionId) REFERENCES StudySessions(Id) ON DELETE CASCADE,
                        ) PRIMARY KEY (Id)");

                    // Create Documents table   
                    await ExecuteDdlAsync(connection, @"
                        CREATE TABLE Documents (
                            Id STRING(36) DEFAULT (GENERATE_UUID()),
                            StudyGroupId STRING(36),
                            UploaderUserId STRING(36),
                            Title STRING(100),
                            FileName STRING(255),
                            FileURL STRING(255),
                            CreateDate TIMESTAMP,
                            CONSTRAINT FK_Document_StudyGroup FOREIGN KEY (StudyGroupId) REFERENCES StudyGroups(Id) ON DELETE CASCADE,
                            CONSTRAINT FK_Document_UploaderUser FOREIGN KEY (UploaderUserId) REFERENCES Users(Id) ON DELETE CASCADE,
                        ) PRIMARY KEY (Id)");
                }
            }
        }

        private static async Task ExecuteDdlAsync(SpannerConnection connection, string ddlStatement)
        {
            using var spannerConnection = connection;
            using var command = spannerConnection.CreateDdlCommand(ddlStatement);
            await command.ExecuteNonQueryAsync();
        }
    }
}
