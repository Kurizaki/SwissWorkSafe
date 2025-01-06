using SwissWorkSafe.Models.Articles;
using System.Data.SQLite;

namespace SwissWorkSafe.Models.Core
{
    /*
         $$$$$$\                $$\                     $$\      $$\                     $$\        $$$$$$\             $$$$$$\
        $$  __$$\               \__|                    $$ | $\  $$ |                    $$ |      $$  __$$\           $$  __$$\
        $$ /  \__|$$\  $$\  $$\ $$\  $$$$$$$\  $$$$$$$\ $$ |$$$\ $$ | $$$$$$\   $$$$$$\  $$ |  $$\ $$ /  \__| $$$$$$\  $$ /  \__|$$$$$$\
        \$$$$$$\  $$ | $$ | $$ |$$ |$$  _____|$$  _____|$$ $$ $$\$$ |$$  __$$\ $$  __$$\ $$ | $$  |\$$$$$$\   \____$$\ $$$$\    $$  __$$\
         \____$$\ $$ | $$ | $$ |$$ |\$$$$$$\  \$$$$$$\  $$$$  _$$$$ |$$ /  $$ |$$ |  \__|$$$$$$  /  \____$$\  $$$$$$$ |$$  _|   $$$$$$$$ |
        $$\   $$ |$$ | $$ | $$ |$$ | \____$$\  \____$$\ $$$  / \$$$ |$$ |  $$ |$$ |      $$  _$$<  $$\   $$ |$$  __$$ |$$ |     $$   ____|
        \$$$$$$  |\$$$$$\$$$$  |$$ |$$$$$$$  |$$$$$$$  |$$  /   \$$ |\$$$$$$  |$$ |      $$ | \$$\ \$$$$$$  |\$$$$$$$ |$$ |     \$$$$$$$\
         \______/  \_____\____/ \__|\_______/ \_______/ \__/     \__| \______/ \__|      \__|  \__| \______/  \_______|\__|      \_______|
        Authors: Keanu Koelewijn, Rebecca Wili, Salma Tanner, Lorenzo Lai
    */

    /// <summary>
    /// Provides functionalities to search for relevant articles based on input text by analyzing keywords.
    /// </summary>
    public class ArticleSearch
    {
        /// <summary>
        /// The relative path to the SQLite database file containing articles.
        /// </summary>
        private const string DatabasePath = "../../../Database/Articles.db";

        /// <summary>
        /// Gets or sets the input text used for keyword analysis.
        /// </summary>
        public required string TextInput { get; set; }

        /// <summary>
        /// Gets the list of keywords extracted from the input text.
        /// </summary>
        public List<string> Keywords { get; private set; }

        /// <summary>
        /// Gets the list of articles relevant to the extracted keywords.
        /// </summary>
        public List<Article> RelevantArticles { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArticleSearch"/> class.
        /// </summary>
        public ArticleSearch()
        {
            Keywords = new List<string>();
            RelevantArticles = new List<Article>();
        }

        /// <summary>
        /// Finds and retrieves relevant articles based on the input text by analyzing keywords.
        /// </summary>
        /// <returns>A list of relevant <see cref="Article"/> objects.</returns>
        public List<Article> FindRelevantArticles()
        {
            Keywords = AnalyzeTextForKeywords();
            return Keywords.Any() ? MatchArticlesByKeywords() : new List<Article>();
        }

        /// <summary>
        /// Analyzes the input text to extract unique keywords.
        /// </summary>
        /// <returns>A list of unique keywords extracted from the input text.</returns>
        public List<string> AnalyzeTextForKeywords()
        {
            return TextInput.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(word => word.Trim().ToLowerInvariant())
                .Distinct()
                .ToList();
        }

        /// <summary>
        /// Compares the extracted keywords with the articles in the database to find relevant articles.
        /// </summary>
        /// <returns>A list of unique <see cref="Article"/> objects that match the keywords.</returns>
        public List<Article> MatchArticlesByKeywords()
        {
            var allMatches = new List<Article>();

            try
            {
                using var connection = new SQLiteConnection($"Data Source={DatabasePath};Version=3;");
                connection.Open();

                foreach (var keyword in Keywords)
                {
                    var matches = FindArticlesByKeyword(keyword, connection);
                    allMatches.AddRange(matches);
                }

                // Remove duplicate articles based on Article ID
                RelevantArticles = allMatches
                    .GroupBy(a => a.Id)
                    .Select(g => g.First())
                    .ToList();

                return RelevantArticles;
            }
            catch (SQLiteException ex)
            {
                // Log exception or handle accordingly
                Console.Error.WriteLine($"SQLite Error in MatchArticlesByKeywords: {ex.Message}");
                return new List<Article>();
            }
            catch (Exception ex)
            {
                // Log exception or handle accordingly
                Console.Error.WriteLine($"Unexpected Error in MatchArticlesByKeywords: {ex.Message}");
                return new List<Article>();
            }
        }

        /// <summary>
        /// Finds articles in the database associated with the specified keyword.
        /// </summary>
        /// <param name="keyword">The keyword to search for.</param>
        /// <param name="connection">An open <see cref="SQLiteConnection"/> to the database.</param>
        /// <returns>A list of <see cref="Article"/> objects that match the keyword.</returns>
        private List<Article> FindArticlesByKeyword(string keyword, SQLiteConnection connection)
        {
            const string query = @"
                SELECT a.id,
                       a.article_name,
                       a.description
                FROM Articles a
                INNER JOIN ArticleSignalWords asw ON a.id = asw.article_id
                INNER JOIN SignalWords s ON asw.signalword_id = s.id
                WHERE LOWER(s.signalword) = @keyword;
            ";

            using var command = new SQLiteCommand(query, connection);
            command.Parameters.AddWithValue("@keyword", keyword.ToLowerInvariant());

            var articles = new List<Article>();

            try
            {
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var article = new Article
                    {
                        Id = reader.GetInt32(0),
                        ArticleName = reader.GetString(1),
                        Description = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                        SignalWords = GetSignalWordsForArticle(reader.GetInt32(0), connection)
                    };
                    articles.Add(article);
                }
            }
            catch (SQLiteException ex)
            {
                // Log exception or handle accordingly
                Console.Error.WriteLine($"SQLite Error in FindArticlesByKeyword: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Log exception or handle accordingly
                Console.Error.WriteLine($"Unexpected Error in FindArticlesByKeyword: {ex.Message}");
            }

            return articles;
        }

        /// <summary>
        /// Retrieves all signal words associated with a specific article.
        /// </summary>
        /// <param name="articleId">The unique identifier of the article.</param>
        /// <param name="connection">An open <see cref="SQLiteConnection"/> to the database.</param>
        /// <returns>A list of signal words associated with the article.</returns>
        private List<string> GetSignalWordsForArticle(int articleId, SQLiteConnection connection)
        {
            const string query = @"
                SELECT s.signalword
                FROM ArticleSignalWords asw
                INNER JOIN SignalWords s ON asw.signalword_id = s.id
                WHERE asw.article_id = @article_id;
            ";

            using var command = new SQLiteCommand(query, connection);
            command.Parameters.AddWithValue("@article_id", articleId);

            var signalWords = new List<string>();

            try
            {
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    signalWords.Add(reader.GetString(0));
                }
            }
            catch (SQLiteException ex)
            {
                // Log exception or handle accordingly
                Console.Error.WriteLine($"SQLite Error in GetSignalWordsForArticle: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Log exception or handle accordingly
                Console.Error.WriteLine($"Unexpected Error in GetSignalWordsForArticle: {ex.Message}");
            }

            return signalWords;
        }
    }
}
