using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DotNetLab12.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        [MinLength(2)]
        [MaxLength(40)]
        [DisplayName("Category")]
        public string Name { get; set; }
        public ICollection<Article> Articles { get; set; }


        public Category() { }

        public Category(int categoryId, string name, ICollection<Article> articles)
        {
            CategoryId = categoryId;
            Name = name;
            Articles = articles;
        }
    }
}
