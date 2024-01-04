﻿using CoStudyCloud.Core.Models;
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