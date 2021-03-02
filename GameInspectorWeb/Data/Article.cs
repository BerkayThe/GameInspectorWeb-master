using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GameInspectorWeb.Data
{
    public class Article
    {
        public Article()
        {
            ArticleCategories = new HashSet<ArticleCategory>();
            Comments = new HashSet<Comment>();
        }
        public int Id { get; set; }

        [Required, MaxLength(ErrorMessage = "Makale için lütfen bir başlık giriniz.")]
        [Display(Name = "Başlık")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Makale içeriği boş olamaz.")]
        [Display(Name = "Makale İçeriği")]
        public string Content { get; set; }

        [Display(Name = "Karakter Tanıtım")]
        public string CharacterContent { get; set; }

        public DateTime? Time { get; set; }

        public DateTimeOffset Created { get; set; }

        public DateTimeOffset? LastUpdate { get; set; }

        public DateTimeOffset? Deleted { get; set; }

        [NotMapped]
        public IFormFile CoverPhoto { get; set; }

        public string CoverPhotoPath { get; set; }


        //public IFormFileCollection ContentPhotos { get; set; }
        [NotMapped]
        public IFormFileCollection ContentPhotos { get; set; }

        public string ContentPhotosPaths { get; set; }

        public string VideoPath { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public ICollection<ArticleCategory> ArticleCategories { get; set; }
    }
}
