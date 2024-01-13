using DotNetLab12.Models;
using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Http;

namespace DotNetLab12.ViewModels
{
    public class ArticleViewModel
    {
        public int ArticleId { get; set; }

        [MinLength(2, ErrorMessage = "Too short name")]
        [MaxLength(25, ErrorMessage = " Too long name, do not exceed {0}")]
        public string Name { get; set; }


        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        [Range(0.0, Double.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
        public double Price { get; set; }
        
        public IFormFile? Picture { get; set; }
        
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public ArticleViewModel(Article article)
        {
            ArticleId = article.ArticleId;
            Name = article.Name;
            Price = article.Price;
            CategoryId = article.CategoryId;
            Category = article.Category;
        }

        public ArticleViewModel() { }   

        public ArticleViewModel(int articleId, string name, double price, IFormFile picture, int categoryId, Category category)
        {
            ArticleId = articleId;
            Name = name;
            Price = price;
            Picture = picture;
            CategoryId = categoryId;
            Category = category;
        }
    }
}
