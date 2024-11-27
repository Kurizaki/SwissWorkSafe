namespace SwissWorkSafe.Models.Articles
{
  public class ArticleDatabase
    {
        public List<Article> ArticleList { get; set; }

        public List<Article> LoadArticles() { return null; }
        public Article SearchArticle(int number) { return null; }
        public List<Article> FindArticlesWithElement(string keyword) { return null; }
    }
}
