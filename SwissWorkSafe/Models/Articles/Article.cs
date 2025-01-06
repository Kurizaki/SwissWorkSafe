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
    /// Represents an article with relevant details such as name, description, and associated signal words.
    /// </summary>
    public class Article
    {
        /// <summary>
        /// Gets or sets the unique identifier for the article.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the article.
        /// Serialized as "article_name" in JSON.
        /// </summary>
        [JsonProperty("article_name")]
        public required string ArticleName { get; set; }

        /// <summary>
        /// Gets or sets the description of the article.
        /// </summary>
        public required string Description { get; set; }

        /// <summary>
        /// Gets or sets the list of signal words associated with the article.
        /// Serialized as "signalwords" in JSON.
        /// </summary>
        [JsonProperty("signalwords")]
        public required List<string> SignalWords { get; set; }
    }
}