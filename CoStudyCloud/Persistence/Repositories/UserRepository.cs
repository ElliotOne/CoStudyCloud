using CoStudyCloud.Core.Models;
using CoStudyCloud.Core.Repositories;
using Google.Cloud.Spanner.Data;

namespace CoStudyCloud.Persistence.Repositories
{
    /// <summary>
    /// Represents the default implementation of User repository
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration _configuration;

        public UserRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            using var connection = new SpannerConnection(_configuration.GetConnectionString("SpannerConnection"));
            await connection.OpenAsync();

            string query = @"
                SELECT
                    Id,
                    Email,
                    FirstName,
                    LastName,
                    GoogleId,
                    ProfileImageUrl,
                    UserRole,
                    CreateDate,
                    LastEditDate
                FROM
                    Users";

            using var command = new SpannerCommand(query, connection);

            var users = new List<User>();

            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var user = new User
                {
                    Id = reader[nameof(User.Id)].ToString(),
                    Email = reader[nameof(User.Email)].ToString(),
                    FirstName = reader[nameof(User.FirstName)].ToString(),
                    LastName = reader[nameof(User.LastName)].ToString(),
                    GoogleId = reader[nameof(User.GoogleId)].ToString(),
                    ProfileImageUrl = reader[nameof(User.ProfileImageUrl)].ToString(),
                    UserRole = reader[nameof(User.UserRole)].ToString(),
                    CreateDate = (DateTime)reader[nameof(User.CreateDate)],
                    LastEditDate = (DateTime)reader[nameof(User.LastEditDate)]
                };

                users.Add(user);
            }

            return users;
        }

        public async Task<User?> GetByEmail(string email)
        {
            using var connection = new SpannerConnection(_configuration.GetConnectionString("SpannerConnection"));
            connection.Open();

            string query = @"
                SELECT *
                FROM USERS
                WHERE Email = @Email";

            using var command = new SpannerCommand(query, connection);
            command.Parameters.Add(nameof(User.Email), SpannerDbType.String).Value = email;

            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new User
                {
                    Id = reader[nameof(User.Id)].ToString(),
                    Email = reader[nameof(User.Email)].ToString(),
                    FirstName = reader[nameof(User.FirstName)].ToString(),
                    LastName = reader[nameof(User.LastName)].ToString(),
                    GoogleId = reader[nameof(User.GoogleId)].ToString(),
                    ProfileImageUrl = reader[nameof(User.ProfileImageUrl)].ToString(),
                    UserRole = reader[nameof(User.UserRole)].ToString(),
                    CreateDate = (DateTime)reader[nameof(User.CreateDate)],
                    LastEditDate = (DateTime)reader[nameof(User.LastEditDate)]
                };
            }

            return null;
        }

        public async Task Add(User user)
        {
            using var connection = new SpannerConnection(_configuration.GetConnectionString("SpannerConnection"));
            connection.Open();
            string query = @"
                INSERT INTO USERS (
                    Email,
                    FirstName,
                    LastName,
                    GoogleId,
                    ProfileImageUrl,
                    UserRole,
                    CreateDate,
                    LastEditDate
                ) VALUES (
                    @Email,
                    @FirstName,
                    @LastName,
                    @GoogleId,
                    @ProfileImageUrl,
                    @UserRole,
                    @CreateDate,
                    @LastEditDate
                )";
            using var command = new SpannerCommand(query, connection);

            command.Parameters.Add(nameof(User.Email), SpannerDbType.String).Value = user.Email;
            command.Parameters.Add(nameof(User.FirstName), SpannerDbType.String).Value = user.FirstName;
            command.Parameters.Add(nameof(User.LastName), SpannerDbType.String).Value = user.LastName;
            command.Parameters.Add(nameof(User.GoogleId), SpannerDbType.String).Value = user.GoogleId;
            command.Parameters.Add(nameof(User.ProfileImageUrl), SpannerDbType.String).Value = user.ProfileImageUrl;
            command.Parameters.Add(nameof(User.UserRole), SpannerDbType.String).Value = user.UserRole;
            command.Parameters.Add(nameof(User.CreateDate), SpannerDbType.Timestamp).Value = user.CreateDate;
            command.Parameters.Add(nameof(User.LastEditDate), SpannerDbType.Timestamp).Value = user.LastEditDate;

            await command.ExecuteNonQueryAsync();
        }

        public async Task Update(User user)
        {
            using var connection = new SpannerConnection(_configuration.GetConnectionString("SpannerConnection"));
            connection.Open();

            string query = @"
                UPDATE USERS
                SET FirstName = @FirstName,
                    LastName = @LastName,
                    ProfileImageUrl = @ProfileImageUrl,

                    LastEditDate = @LastEditDate
                WHERE Id = @Id";

            using var command = new SpannerCommand(query, connection);
            command.Parameters.Add(nameof(User.Id), SpannerDbType.String).Value = user.Id;
            command.Parameters.Add(nameof(User.FirstName), SpannerDbType.String).Value = user.FirstName;
            command.Parameters.Add(nameof(User.LastName), SpannerDbType.String).Value = user.LastName;
            command.Parameters.Add(nameof(User.ProfileImageUrl), SpannerDbType.String).Value = user.ProfileImageUrl;
            command.Parameters.Add(nameof(User.LastEditDate), SpannerDbType.Timestamp).Value = user.LastEditDate;

            await command.ExecuteNonQueryAsync();
        }


        public async Task<bool> Exists(string email)
        {
            using var connection = new SpannerConnection(_configuration.GetConnectionString("SpannerConnection"));
            connection.Open();

            string query = @"
                SELECT COUNT(*) 
                FROM USERS 
                WHERE Email = @Email";

            using var command = new SpannerCommand(query, connection);
            command.Parameters.Add(nameof(User.Email), SpannerDbType.String).Value = email;

            var result = await command.ExecuteScalarAsync();
            return Convert.ToInt64(result) > 0;
        }
    }
}
