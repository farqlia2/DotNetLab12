namespace DotNetLab12.Models
{
    public class ArticleWrapper
    {
        public string ArticleId { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
        public string? PictureName { get; set; }

        public string Category { get; set; }

        public ArticleWrapper(Article atr)
        {
            ArticleId = atr.ArticleId.ToString();
            Name = atr.Name;
            Price = atr.Price.ToString();
            PictureName = "";
            Category = atr.Category.Name;
        }

    }
}
