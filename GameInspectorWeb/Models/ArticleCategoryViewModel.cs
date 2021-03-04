using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameInspectorWeb.Models
{
    public class ArticleCategoryViewModel
    {
        public int ArticleId { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public string ArticleTitle { get; set; }

        public string ArticleContent { get; set; }

        public string CoverPhotoPath { get; set; }

        public DateTime? Time { get; set; }
    }
}
