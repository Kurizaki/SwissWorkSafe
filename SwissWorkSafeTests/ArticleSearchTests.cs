using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using SwissWorkSafe.Models.Articles;
using SwissWorkSafe.Models.Core;

namespace SwissWorkSafeTests
{
    [TestClass]
    public class ArticleSearchTests
    {
        // Fixed relative path to the production database
        private const string DatabaseRelativePath = "../../../Database/Articles.db";
        private static string TestDatabasePath;

        /// <summary>
        /// Runs once before all tests in this class.
        /// Resets the test database and initializes it with test data.
        /// </summary>
        /// <param name="context">TestContext object.</param>
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            // Determine the absolute path to the database
            TestDatabasePath = Path.GetFullPath(DatabaseRelativePath);

            // Ensure the database directory exists
            string dbDirectory = Path.GetDirectoryName(TestDatabasePath);
            if (!Directory.Exists(dbDirectory))
            {
                Directory.CreateDirectory(dbDirectory);
            }

            // Delete the existing database file if it exists
            if (File.Exists(TestDatabasePath))
            {
                try
                {
                    File.Delete(TestDatabasePath);
                }
                catch (IOException ex)
                {
                    Assert.Fail($"Error deleting existing database: {ex.Message}");
                }
            }

            // Create a new database file
            SQLiteConnection.CreateFile(TestDatabasePath);

            // Set up tables and insert test data
            using (var connection = new SQLiteConnection($"Data Source={TestDatabasePath};Version=3;"))
            {
                connection.Open();

                CreateTables(connection);
                InsertTestData(connection);
            }
        }

        /// <summary>
        /// Runs once after all tests in this class.
        /// Deletes the test database file.
        /// </summary>
        [ClassCleanup]
        public static void ClassCleanup()
        {
            // Delete the test database file after all tests
            if (File.Exists(TestDatabasePath))
            {
                try
                {
                    File.Delete(TestDatabasePath);
                }
                catch (IOException ex)
                {
                    Assert.Fail($"Error deleting test database: {ex.Message}");
                }
            }
        }

        #region Test Methods

        [TestMethod]
        public void AnalyzeTextForKeywords_SplitsAndExtractsUniqueKeywords()
        {
            // Arrange
            var articleSearch = new ArticleSearch
            {
                TextInput = "Safety Health Environment safety"
            };

            // Act
            var keywords = articleSearch.AnalyzeTextForKeywords();

            // Assert
            var expected = new List<string> { "safety", "health", "environment" };
            CollectionAssert.AreEquivalent(expected, keywords, "Keywords were not correctly extracted.");
        }

        [TestMethod]
        public void AnalyzeTextForKeywords_HandlesEmptyInput()
        {
            // Arrange
            var articleSearch = new ArticleSearch
            {
                TextInput = "   "
            };

            // Act
            var keywords = articleSearch.AnalyzeTextForKeywords();

            // Assert
            Assert.AreEqual(0, keywords.Count, "Empty input should not extract any keywords.");
        }

        [TestMethod]
        public void FindRelevantArticles_ReturnsArticlesMatchingKeywords()
        {
            // Arrange
            var articleSearch = new ArticleSearch
            {
                TextInput = "Safety Health"
            };

            // Act
            var relevantArticles = articleSearch.FindRelevantArticles();

            // Assert
            Assert.AreEqual(2, relevantArticles.Count, "Two relevant articles should be returned.");

            var articleIds = relevantArticles.Select(a => a.Id).ToList();
            CollectionAssert.Contains(articleIds, 1, "Article with ID 1 should be returned.");
            CollectionAssert.Contains(articleIds, 2, "Article with ID 2 should be returned.");
        }

        [TestMethod]
        public void FindRelevantArticles_ReturnsEmptyListWhenNoKeywords()
        {
            // Arrange
            var articleSearch = new ArticleSearch
            {
                TextInput = ""
            };

            // Act
            var relevantArticles = articleSearch.FindRelevantArticles();

            // Assert
            Assert.AreEqual(0, relevantArticles.Count, "No keywords should return no relevant articles.");
        }

        [TestMethod]
        public void FindRelevantArticles_ReturnsEmptyListWhenNoMatchingArticles()
        {
            // Arrange
            var articleSearch = new ArticleSearch
            {
                TextInput = "NonExistentKeyword"
            };

            // Act
            var relevantArticles = articleSearch.FindRelevantArticles();

            // Assert
            Assert.AreEqual(0, relevantArticles.Count, "No matching articles should be returned.");
        }

        [TestMethod]
        public void FindRelevantArticles_HandlesDuplicateKeywords()
        {
            // Arrange
            var articleSearch = new ArticleSearch
            {
                TextInput = "Safety Safety Health"
            };

            // Act
            var relevantArticles = articleSearch.FindRelevantArticles();

            // Assert
            Assert.AreEqual(2, relevantArticles.Count, "Two relevant articles should be returned, even with duplicate keywords.");
        }

        [TestMethod]
        public void FindRelevantArticles_IgnoresCaseInKeywords()
        {
            // Arrange
            var articleSearch = new ArticleSearch
            {
                TextInput = "safety HEALTH"
            };

            // Act
            var relevantArticles = articleSearch.FindRelevantArticles();

            // Assert
            Assert.AreEqual(2, relevantArticles.Count, "Case of keywords should be ignored.");
        }

        #endregion

        #region Helper Methods for Database Setup

        /// <summary>
        /// Creates the necessary tables in the database.
        /// </summary>
        /// <param name="connection">Open SQLite connection.</param>
        private static void CreateTables(SQLiteConnection connection)
        {
            var createArticlesTable = @"
                CREATE TABLE Articles (
                    id INTEGER PRIMARY KEY,
                    article_name TEXT NOT NULL,
                    description TEXT
                );
            ";

            var createSignalWordsTable = @"
                CREATE TABLE SignalWords (
                    id INTEGER PRIMARY KEY,
                    signalword TEXT UNIQUE NOT NULL
                );
            ";

            var createArticleSignalWordsTable = @"
                CREATE TABLE ArticleSignalWords (
                    id INTEGER PRIMARY KEY,
                    article_id INTEGER NOT NULL,
                    signalword_id INTEGER NOT NULL,
                    FOREIGN KEY(article_id) REFERENCES Articles(id),
                    FOREIGN KEY(signalword_id) REFERENCES SignalWords(id),
                    UNIQUE(article_id, signalword_id)
                );
            ";

            using var cmd = new SQLiteCommand(createArticlesTable, connection);
            cmd.ExecuteNonQuery();

            cmd.CommandText = createSignalWordsTable;
            cmd.ExecuteNonQuery();

            cmd.CommandText = createArticleSignalWordsTable;
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Inserts test data into the database.
        /// </summary>
        /// <param name="connection">Open SQLite connection.</param>
        private static void InsertTestData(SQLiteConnection connection)
        {
            // Insert articles
            var insertArticles = @"
                INSERT INTO Articles (id, article_name, description) VALUES
                (1, 'Safety Guidelines', 'Guidelines for workplace safety.'),
                (2, 'Health Standards', 'Standards to maintain health at work.');
            ";

            using var cmd = new SQLiteCommand(insertArticles, connection);
            cmd.ExecuteNonQuery();

            // Insert signal words
            var insertSignalWords = @"
                INSERT INTO SignalWords (signalword) VALUES
                ('safety'),
                ('health'),
                ('environment');
            ";

            cmd.CommandText = insertSignalWords;
            cmd.ExecuteNonQuery();

            // Insert relationships between articles and signal words
            var insertArticleSignalWords = @"
                INSERT INTO ArticleSignalWords (article_id, signalword_id) VALUES
                (1, 1), -- safety -> Safety Guidelines
                (2, 2), -- health -> Health Standards
                (1, 3); -- environment -> Safety Guidelines
            ";

            cmd.CommandText = insertArticleSignalWords;
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Drops the tables if they exist.
        /// </summary>
        /// <param name="connection">Open SQLite connection.</param>
        private static void DropTablesIfExists(SQLiteConnection connection)
        {
            var dropArticleSignalWords = "DROP TABLE IF EXISTS ArticleSignalWords;";
            var dropSignalWords = "DROP TABLE IF EXISTS SignalWords;";
            var dropArticles = "DROP TABLE IF EXISTS Articles;";

            using var cmd = new SQLiteCommand(dropArticleSignalWords, connection);
            cmd.ExecuteNonQuery();

            cmd.CommandText = dropSignalWords;
            cmd.ExecuteNonQuery();

            cmd.CommandText = dropArticles;
            cmd.ExecuteNonQuery();
        }

        #endregion
    }

    // Example implementation of the ArticleSearch class
    // Ensure this class matches your actual implementation.
    namespace SwissWorkSafe.Models.Core
    {
        public class ArticleSearch
        {
            /// <summary>
            /// The relative path to the SQLite database file containing articles.
            /// </summary>
            public const string DatabasePath = "../../../Database/Articles.db";

            public string TextInput { get; set; }

            /// <summary>
            /// Analyzes the input text and extracts unique keywords.
            /// </summary>
            /// <returns>List of unique keywords.</returns>
            public List<string> AnalyzeTextForKeywords()
            {
                if (string.IsNullOrWhiteSpace(TextInput))
                    return new List<string>();

                // Split by spaces, commas, periods, etc.
                var keywords = TextInput
                    .Split(new char[] { ' ', ',', '.', ';', ':' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(k => k.ToLower())
                    .Distinct()
                    .ToList();

                return keywords;
            }

            /// <summary>
            /// Finds relevant articles based on the extracted keywords.
            /// </summary>
            /// <returns>List of relevant articles.</returns>
            public List<Article> FindRelevantArticles()
            {
                var keywords = AnalyzeTextForKeywords();
                if (keywords.Count == 0)
                    return new List<Article>();

                var relevantArticles = new List<Article>();

                using var connection = new SQLiteConnection($"Data Source={DatabasePath};Version=3;");
                connection.Open();

                foreach (var keyword in keywords)
                {
                    var query = @"
                        SELECT a.id, a.article_name, a.description, sw.signalword
                        FROM Articles a
                        JOIN ArticleSignalWords asw ON a.id = asw.article_id
                        JOIN SignalWords sw ON asw.signalword_id = sw.id
                        WHERE sw.signalword = @keyword;
                    ";

                    using var cmd = new SQLiteCommand(query, connection);
                    cmd.Parameters.AddWithValue("@keyword", keyword);

                    using var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var articleId = Convert.ToInt32(reader["id"]);
                        var existingArticle = relevantArticles.FirstOrDefault(a => a.Id == articleId);

                        if (existingArticle == null)
                        {
                            existingArticle = new Article
                            {
                                Id = articleId,
                                ArticleName = reader["article_name"].ToString(),
                                Description = reader["description"].ToString(),
                                SignalWords = new List<string>()
                            };
                            relevantArticles.Add(existingArticle);
                        }

                        var signalWord = reader["signalword"].ToString();
                        if (!existingArticle.SignalWords.Contains(signalWord))
                        {
                            existingArticle.SignalWords.Add(signalWord);
                        }
                    }
                }

                return relevantArticles;
            }
        }
    }
}
