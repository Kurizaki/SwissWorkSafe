using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace SwissWorkSafe.Models.Articles
{
    public class ArticleDatabase
    {
        public List<Article> ArticleList { get; private set; } = new List<Article>();

        public List<Article> LoadArticles()
        {
            using var connection = new SQLiteConnection("Data Source=Database/Articles.db;Version=3;");
            connection.Open();
            var command = new SQLiteCommand("SELECT id, article_name, description FROM Articles", connection);

            using var reader = command.ExecuteReader();
            return reader.Cast<System.Data.Common.DbDataRecord>()
                .Select(record => new Article
                {
                    ArticleNumber = record.GetInt32(0),
                    Title = record.GetString(1),
                    Description = record.GetString(2),
                    Elements = GetSignalWordsForArticle(record.GetInt32(0), connection)
                })
                .ToList();
        }

        public Article SearchArticle(int number)
        {
            using var connection = new SQLiteConnection("Data Source=Database/Articles.db;Version=3;");
            connection.Open();
            var command = new SQLiteCommand("SELECT id, article_name, description FROM Articles WHERE id = @id", connection);
            command.Parameters.AddWithValue("@id", number);

            using var reader = command.ExecuteReader();
            return reader.Cast<System.Data.Common.DbDataRecord>()
                .Select(record => new Article
                {
                    ArticleNumber = record.GetInt32(0),
                    Title = record.GetString(1),
                    Description = record.GetString(2),
                    Elements = GetSignalWordsForArticle(record.GetInt32(0), connection) ?? new List<string>()
                })
                .FirstOrDefault();
        }

        public List<Article> FindArticlesWithElement(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword)) return new List<Article>();

            using var connection = new SQLiteConnection("Data Source=Database/Articles.db;Version=3;");
            connection.Open();
            var command = new SQLiteCommand(
                "SELECT a.id, a.article_name, a.description FROM Articles a JOIN SignalWords s ON a.id = s.article_id WHERE s.signalword = @keyword",
                connection
            );
            command.Parameters.AddWithValue("@keyword", keyword);

            using var reader = command.ExecuteReader();
            return reader.Cast<System.Data.Common.DbDataRecord>()
                .Select(record => new Article
                {
                    ArticleNumber = record.GetInt32(0),
                    Title = record.GetString(1),
                    Description = record.GetString(2),
                    Elements = GetSignalWordsForArticle(record.GetInt32(0), connection)
                })
                .DistinctBy(article => article.ArticleNumber)
                .ToList();
        }

        private List<string> GetSignalWordsForArticle(int articleId, SQLiteConnection connection)
        {
            var command = new SQLiteCommand("SELECT signalword FROM SignalWords WHERE article_id = @article_id", connection);
            command.Parameters.AddWithValue("@article_id", articleId);

            using var reader = command.ExecuteReader();
            return reader.Cast<System.Data.Common.DbDataRecord>()
                .Select(record => record.GetString(0))
                .ToList();
        }
    }
}

public static class EnumerableExtensions
{
    public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector)
    {
        var seenKeys = new HashSet<TKey>();
        foreach (var element in source)
        {
            if (seenKeys.Add(keySelector(element)))
            {
                yield return element;
            }
        }
    }
}
