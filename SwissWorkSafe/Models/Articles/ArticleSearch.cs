using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace SwissWorkSafe.Models.Articles
{
    public class ArticleSearch
    {
        public string TextInput { get; set; }
        public List<string> Keywords { get; private set; }
        public List<Article> RelevantArticles { get; private set; }

        public List<Article> FindRelevantArticles()
        {
            Keywords = AnalyzeTextForKeywords();
            return Keywords.Any() ? MatchArticlesByElements() : new List<Article>();
        }

        public List<string> AnalyzeTextForKeywords()
        {
            return TextInput?
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Distinct()
                .ToList() ?? new List<string>();
        }

        public List<Article> MatchArticlesByElements()
        {
            using var connection = new SQLiteConnection("Data Source=Database/Articles.db;Version=3;");
            connection.Open();

            return Keywords
                .SelectMany(keyword => FindArticlesByKeyword(keyword, connection))
                .DistinctBy(article => article.ArticleNumber)
                .ToList();
        }

        private IEnumerable<Article> FindArticlesByKeyword(string keyword, SQLiteConnection connection)
        {
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
                    Elements = new List<string>()
                });
        }
    }
}
