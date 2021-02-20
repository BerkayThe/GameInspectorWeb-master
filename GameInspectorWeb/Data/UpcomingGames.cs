using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GameInspectorWeb.Data
{
    public class UpcomingGames
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        [Display(Name ="Çıkış Tarihi")]
        public DateTime Time { get; set; }

        public string PhotoPath { get; set; }

        [NotMapped]
        public IFormFile Photo { get; set; }

        public string Platform { get; set; } 
    }
}
