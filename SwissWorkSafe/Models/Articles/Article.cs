namespace SwissWorkSafe.Models.Articles
{
    public class Article
    {
        public int ArticleNumber { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<string> Elements { get; set; }

        /*
        public string ElementsAsString
        {
            get => string.Join(", ", Elements);
            set => Elements = value.Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }
        */
    }


}
