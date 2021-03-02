using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameInspectorWeb.Data
{
    public class ArticleHit
    {
        public int Id { get; set; }
        public int ArticleId { get; set; }

        public DateTimeOffset Created { get; set; }

        public DateTimeOffset? LastUpdate { get; set; }

        public DateTimeOffset? Deleted { get; set; }

        public Article Article { get; set; }

        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ArticleHit>()
                .HasOne(prop => prop.Article)
                .WithMany()
                .HasPrincipalKey(article => article.Id)
                .HasForeignKey(articleHit => articleHit.ArticleId);
        }
    }
}
