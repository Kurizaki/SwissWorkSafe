namespace SwissWorkSafe.Models.Articles
{    
    public class ArticleSearch
    {
        public string TextInput { get; set; }
        public List<string> Keywords { get; set; }
        public List<Article> RelevantArticles { get; set; }

        public List<Article> FindRelevantArticles() { return null; }
        public List<string> AnalyzeTextForKeywords() { return null; }
        public List<Article> MatchArticlesByElements() { return null; }
    }
}
