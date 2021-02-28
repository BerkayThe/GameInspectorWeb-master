using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GameInspectorWeb.Data
{
    public class Comment
    {
        public int Id { get; set; }

        
        [Display(Name = "Yorum İçeriği")]
        public string CommentContent { get; set; }

        [ForeignKey("Author")]
        public string ApplicationUserId { get; set; }

        public int ArticleId { get; set; }

        public virtual ApplicationUser Author { get; set; }

        public virtual Article Article { get; set; }


    }
}
