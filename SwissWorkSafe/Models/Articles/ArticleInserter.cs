using System.Data.SQLite;
using System.IO;
using Newtonsoft.Json;

namespace SwissWorkSafe.Models.Articles
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
    /// Handles the insertion of articles and their associated signal words into the SQLite database.
    /// </summary>
    public class ArticleInserter
    {
        /// <summary>
        /// Relative path to the SQLite database file.
        /// </summary>
        private const string DatabasePath = "../../../Database/Articles.db";

        /// <summary>
        /// Relative path to the JSON file containing article data.
        /// </summary>
        private const string JsonFilePath = "../../../Resources/articles.json";

        /// <summary>
        /// Maximum number of retry attempts in case of an error.
        /// </summary>
        private const int MaxRetryAttempts = 3;

        /// <summary>
        /// Ensures that all articles from the JSON file are inserted into the database.
        /// Creates the database and necessary tables if they do not exist.
        /// If an error occurs, deletes the existing database and retries the process.
        /// </summary>
        public static void EnsureArticlesAreInserted()
        {
            int currentAttempt = 0;
            bool success = false;

            while (currentAttempt < MaxRetryAttempts && !success)
            {
                try
                {
                    Console.WriteLine($"Resolved Database Path: {Path.GetFullPath(DatabasePath)}");
                    Console.WriteLine("Starting EnsureArticlesAreInserted...");

                    // Create the database file if it doesn't exist
                    if (!File.Exists(DatabasePath))
                    {
                        Console.WriteLine($"Database file not found at {DatabasePath}. Creating new database...");
                        SQLiteConnection.CreateFile(DatabasePath);
                        Console.WriteLine("Database file created.");
                    }

                    // Check if the JSON file exists
                    if (!File.Exists(JsonFilePath))
                    {
                        Console.WriteLine($"JSON file not found at {JsonFilePath}.");
                        return;
                    }

                    // Load articles from JSON
                    Console.WriteLine("Loading articles from JSON...");
                    var articles = LoadArticlesFromJson();

                    if (articles == null)
                    {
                        Console.Error.WriteLine("Failed to load articles from JSON.");
                        return;
                    }

                    Console.WriteLine($"Successfully loaded {articles.Count} articles from JSON.");

                    // Open SQLite connection
                    Console.WriteLine("Opening SQLite connection...");
                    using var connection = new SQLiteConnection($"Data Source={DatabasePath};Version=3;");
                    connection.Open();
                    Console.WriteLine("SQLite connection opened.");

                    // Ensure required tables exist
                    Console.WriteLine("Ensuring tables exist...");
                    CreateTablesIfNotExists(connection);
                    Console.WriteLine("Tables are ready.");

                    // Insert articles and their signal words
                    Console.WriteLine("Inserting articles and signal words...");
                    InsertArticlesAndSignalWords(articles, connection);
                    Console.WriteLine("Insertion process complete.");

                    // If all operations succeed
                    success = true;
                }
                catch (SQLiteException ex)
                {
                    Console.Error.WriteLine($"SQLite Error: {ex.Message}");
                    HandleError(ref currentAttempt);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Unexpected Error: {ex.Message}");
                    HandleError(ref currentAttempt);
                }
            }

            if (!success)
            {
                Console.Error.WriteLine("Failed to insert articles after multiple attempts.");
                // Optionally, throw an exception or handle it as needed
            }
        }

        /// <summary>
        /// Handles errors by deleting the database file and incrementing the attempt counter.
        /// </summary>
        /// <param name="currentAttempt">Reference to the current attempt counter.</param>
        private static void HandleError(ref int currentAttempt)
        {
            currentAttempt++;
            Console.WriteLine($"Attempt {currentAttempt} failed.");

            // Delete the existing database file if it exists
            if (File.Exists(DatabasePath))
            {
                try
                {
                    Console.WriteLine($"Deleting database file at {DatabasePath}...");
                    File.Delete(DatabasePath);
                    Console.WriteLine("Database file deleted.");
                }
                catch (IOException ioEx)
                {
                    Console.Error.WriteLine($"Error deleting database file: {ioEx.Message}");
                    // Depending on requirements, you might want to exit or continue
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Unexpected Error deleting database file: {ex.Message}");
                }
            }

            // Wait before retrying if more attempts are left
            if (currentAttempt < MaxRetryAttempts)
            {
                Console.WriteLine("Retrying to insert articles...");
            }
        }

        /// <summary>
        /// Loads articles from the specified JSON file.
        /// </summary>
        /// <returns>A list of <see cref="Article"/> objects if successful; otherwise, <c>null</c>.</returns>
        private static List<Article> LoadArticlesFromJson()
        {
            try
            {
                var jsonContent = File.ReadAllText(JsonFilePath);
                return JsonConvert.DeserializeObject<List<Article>>(jsonContent);
            }
            catch (JsonException ex)
            {
                Console.Error.WriteLine($"JSON Parsing Error: {ex.Message}");
                return null;
            }
            catch (IOException ex)
            {
                Console.Error.WriteLine($"File Read Error: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected Error while loading JSON: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Creates the necessary tables in the database if they do not already exist.
        /// </summary>
        /// <param name="connection">An open <see cref="SQLiteConnection"/>.</param>
        private static void CreateTablesIfNotExists(SQLiteConnection connection)
        {
            // SQL commands to create tables
            var createArticlesTable = @"
                CREATE TABLE IF NOT EXISTS Articles (
                    id INTEGER PRIMARY KEY,
                    article_name TEXT NOT NULL,
                    description TEXT
                );
            ";

            var createSignalWordsTable = @"
                CREATE TABLE IF NOT EXISTS SignalWords (
                    id INTEGER PRIMARY KEY,
                    signalword TEXT UNIQUE NOT NULL
                );
            ";

            var createArticleSignalWordsTable = @"
                CREATE TABLE IF NOT EXISTS ArticleSignalWords (
                    id INTEGER PRIMARY KEY,
                    article_id INTEGER NOT NULL,
                    signalword_id INTEGER NOT NULL,
                    FOREIGN KEY(article_id) REFERENCES Articles(id),
                    FOREIGN KEY(signalword_id) REFERENCES SignalWords(id),
                    UNIQUE(article_id, signalword_id)
                );
            ";

            try
            {
                // Execute table creation commands
                ExecuteNonQuery(createArticlesTable, connection);
                Console.WriteLine("'Articles' table checked/created.");

                ExecuteNonQuery(createSignalWordsTable, connection);
                Console.WriteLine("'SignalWords' table checked/created.");

                ExecuteNonQuery(createArticleSignalWordsTable, connection);
                Console.WriteLine("'ArticleSignalWords' table checked/created.");
            }
            catch (SQLiteException ex)
            {
                Console.Error.WriteLine($"SQLite Error during table creation: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected Error during table creation: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Inserts a list of articles and their associated signal words into the database.
        /// </summary>
        /// <param name="articles">The list of articles to insert.</param>
        /// <param name="connection">An open <see cref="SQLiteConnection"/>.</param>
        private static void InsertArticlesAndSignalWords(List<Article> articles, SQLiteConnection connection)
        {
            Console.WriteLine("Beginning insertion of articles and their signal words...");

            foreach (var article in articles)
            {
                Console.WriteLine($"Processing Article ID {article.Id}: '{article.ArticleName}'");

                try
                {
                    // Insert article if it doesn't exist
                    if (!ArticleExists(article.Id, connection))
                    {
                        Console.WriteLine($"Article ID {article.Id} does not exist. Inserting...");
                        InsertArticle(article, connection);
                    }
                    else
                    {
                        Console.WriteLine($"Article ID {article.Id} already exists. Skipping insert.");
                    }

                    // Insert associated signal words
                    InsertSignalWordsForArticle(article, connection);
                }
                catch (SQLiteException ex)
                {
                    Console.Error.WriteLine($"SQLite Error while processing Article ID {article.Id}: {ex.Message}");
                    throw; // Re-throw to trigger the retry mechanism
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Unexpected Error while processing Article ID {article.Id}: {ex.Message}");
                    throw; // Re-throw to trigger the retry mechanism
                }
            }
        }

        /// <summary>
        /// Checks if an article with the specified ID exists in the database.
        /// </summary>
        /// <param name="articleId">The ID of the article to check.</param>
        /// <param name="connection">An open <see cref="SQLiteConnection"/>.</param>
        /// <returns><c>true</c> if the article exists; otherwise, <c>false</c>.</returns>
        private static bool ArticleExists(int articleId, SQLiteConnection connection)
        {
            const string query = "SELECT COUNT(1) FROM Articles WHERE id = @id;";
            using var cmd = new SQLiteCommand(query, connection);
            cmd.Parameters.AddWithValue("@id", articleId);
            return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        }

        /// <summary>
        /// Inserts a new article into the Articles table.
        /// </summary>
        /// <param name="article">The <see cref="Article"/> to insert.</param>
        /// <param name="connection">An open <see cref="SQLiteConnection"/>.</param>
        private static void InsertArticle(Article article, SQLiteConnection connection)
        {
            const string insertQuery = @"
                INSERT INTO Articles (id, article_name, description)
                VALUES (@id, @name, @desc);
            ";
            using var cmd = new SQLiteCommand(insertQuery, connection);
            cmd.Parameters.AddWithValue("@id", article.Id);
            cmd.Parameters.AddWithValue("@name", article.ArticleName);
            cmd.Parameters.AddWithValue("@desc", article.Description);
            cmd.ExecuteNonQuery();
            Console.WriteLine($"Inserted Article ID {article.Id}: '{article.ArticleName}'");
        }

        /// <summary>
        /// Inserts the signal words associated with an article into the database.
        /// </summary>
        /// <param name="article">The <see cref="Article"/> whose signal words are to be inserted.</param>
        /// <param name="connection">An open <see cref="SQLiteConnection"/>.</param>
        private static void InsertSignalWordsForArticle(Article article, SQLiteConnection connection)
        {
            if (article.SignalWords == null || article.SignalWords.Count == 0)
            {
                Console.WriteLine($"Article ID {article.Id} has no signal words to insert.");
                return;
            }

            Console.WriteLine($"Inserting signal words for Article ID {article.Id}...");

            foreach (var word in article.SignalWords)
            {
                try
                {
                    // Get or create the signal word and retrieve its ID
                    int signalWordId = GetOrCreateSignalWord(word, connection);

                    // Link the article with the signal word if not already linked
                    if (!ArticleSignalWordExists(article.Id, signalWordId, connection))
                    {
                        InsertArticleSignalWord(article.Id, signalWordId, connection);
                    }
                    else
                    {
                        Console.WriteLine($"Signal word '{word}' already linked with Article ID {article.Id}.");
                    }
                }
                catch (SQLiteException ex)
                {
                    Console.Error.WriteLine($"SQLite Error while inserting signal word '{word}' for Article ID {article.Id}: {ex.Message}");
                    throw; // Re-throw to trigger the retry mechanism
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Unexpected Error while inserting signal word '{word}' for Article ID {article.Id}: {ex.Message}");
                    throw; // Re-throw to trigger the retry mechanism
                }
            }
        }

        /// <summary>
        /// Retrieves the ID of a signal word, inserting it into the database if it does not already exist.
        /// </summary>
        /// <param name="word">The signal word to retrieve or insert.</param>
        /// <param name="connection">An open <see cref="SQLiteConnection"/>.</param>
        /// <returns>The ID of the signal word.</returns>
        private static int GetOrCreateSignalWord(string word, SQLiteConnection connection)
        {
            const string selectQuery = "SELECT id FROM SignalWords WHERE signalword = @word;";
            using var selectCmd = new SQLiteCommand(selectQuery, connection);
            selectCmd.Parameters.AddWithValue("@word", word);
            var result = selectCmd.ExecuteScalar();

            if (result != null)
            {
                return Convert.ToInt32(result);
            }

            const string insertQuery = @"
                INSERT INTO SignalWords (signalword)
                VALUES (@word);
                SELECT last_insert_rowid();
            ";
            using var insertCmd = new SQLiteCommand(insertQuery, connection);
            insertCmd.Parameters.AddWithValue("@word", word);
            var insertedId = insertCmd.ExecuteScalar();
            Console.WriteLine($"Inserted new SignalWord '{word}' with ID {insertedId}.");
            return Convert.ToInt32(insertedId);
        }

        /// <summary>
        /// Checks if a link between an article and a signal word already exists in the database.
        /// </summary>
        /// <param name="articleId">The ID of the article.</param>
        /// <param name="signalWordId">The ID of the signal word.</param>
        /// <param name="connection">An open <see cref="SQLiteConnection"/>.</param>
        /// <returns><c>true</c> if the link exists; otherwise, <c>false</c>.</returns>
        private static bool ArticleSignalWordExists(int articleId, int signalWordId, SQLiteConnection connection)
        {
            const string query = @"
                SELECT COUNT(1)
                FROM ArticleSignalWords
                WHERE article_id = @articleId AND signalword_id = @signalwordId;
            ";
            using var cmd = new SQLiteCommand(query, connection);
            cmd.Parameters.AddWithValue("@articleId", articleId);
            cmd.Parameters.AddWithValue("@signalwordId", signalWordId);
            return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        }

        /// <summary>
        /// Inserts a link between an article and a signal word into the database.
        /// </summary>
        /// <param name="articleId">The ID of the article.</param>
        /// <param name="signalWordId">The ID of the signal word.</param>
        /// <param name="connection">An open <see cref="SQLiteConnection"/>.</param>
        private static void InsertArticleSignalWord(int articleId, int signalWordId, SQLiteConnection connection)
        {
            const string insertQuery = @"
                INSERT INTO ArticleSignalWords (article_id, signalword_id)
                VALUES (@articleId, @signalWordId);
            ";
            using var cmd = new SQLiteCommand(insertQuery, connection);
            cmd.Parameters.AddWithValue("@articleId", articleId);
            cmd.Parameters.AddWithValue("@signalWordId", signalWordId);
            cmd.ExecuteNonQuery();
            Console.WriteLine($"Linked Article ID {articleId} with SignalWord ID {signalWordId}.");
        }

        /// <summary>
        /// Executes a non-query SQL command.
        /// </summary>
        /// <param name="query">The SQL command to execute.</param>
        /// <param name="connection">An open <see cref="SQLiteConnection"/>.</param>
        private static void ExecuteNonQuery(string query, SQLiteConnection connection)
        {
            using var cmd = new SQLiteCommand(query, connection);
            cmd.ExecuteNonQuery();
        }
    }
}
