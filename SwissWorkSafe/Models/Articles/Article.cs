using System.Collections.Generic;

namespace SwissWorkSafe.Models.Articles
{
    public class Article
    {
        public int ArticleNumber { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<string> Elements { get; set; }
    }
}
