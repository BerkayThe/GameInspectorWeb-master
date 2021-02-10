using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GameInspectorWeb.Data
{
    public class Category
    {
        public int Id { get; set; }
        [Required, MaxLength(25, ErrorMessage = "Kategori Adı Boş Bırakılamaz.")]
        [Display(Name = "Kategori Adı")]
        public string CategoryName { get; set; }

        public ICollection<ArticleCategory> ArticleCategories { get; set; }
    }
}
