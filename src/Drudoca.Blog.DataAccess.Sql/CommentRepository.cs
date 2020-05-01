using System;
using System.Threading.Tasks;
using Drudoca.Blog.Data;
using Dapper;
using System.Linq;

namespace Drudoca.Blog.DataAccess.Sql
{
    internal class SqlCommentRepository : ICommentRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public SqlCommentRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task CreateAsync(CommentData comment)
        {
            if (comment.Id == default)
            {
                throw new ArgumentException("Comment must have an id set.");
            }

            const string sql = @"
                INSERT INTO Comments ( Id, PostFileName, ParentId, UserId, Author, Email, Markdown, PostedOnUtc, IsDeleted )
                VALUES ( :id, :postFileName, :parentId, :author, :email, :markdown, :postedOnUtc, :isDeleted )
            ";

            using var connection = await _connectionFactory.CreateConnectionAsync();

            await connection.ExecuteAsync(sql, comment);
        }

        public async Task<CommentData?> GetAsync(Guid id)
        {
            const string sql = @"
                SELECT Id, PostFileName, ParentId, Author, Email, Markdown, PostedOnUtc, IsDeleted
                FROM   Comments
                WHERE  Id = :id
            ";

            using var connection = await _connectionFactory.CreateConnectionAsync();

            var query = await connection.QueryAsync<CommentData>(
                sql,
                new { id });

            var result = query.FirstOrDefault();
            return result;
        }

        public async Task<CommentData[]> GetByPostAsync(string postFileName)
        {
            const string sql = @"
                SELECT Id, PostFileName, ParentId, Author, Email, Markdown, PostedOnUtc, IsDeleted
                FROM   Comments
                WHERE  PostFileName = :postFileName
            ";

            using var connection = await _connectionFactory.CreateConnectionAsync();

            var query = await connection.QueryAsync<CommentData>(
                sql,
                new { postFileName });

            var results = query.ToArray();
            return results;
        }

        public async Task MarkDeletedAsync(Guid id)
        {
            const string sql = @"
                UPDATE Comments
                SET    IsDeleted = 1
                WHERE  Id = :id
            ";

            using var connection = await _connectionFactory.CreateConnectionAsync();

            await connection.ExecuteAsync(
                sql,
                new { id });
        }
    }
}
