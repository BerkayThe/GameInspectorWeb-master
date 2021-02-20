using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GameInspectorWeb.Areas.Admin.Models
{
    public class UpcomingGameViewModel
    {

        public string Title { get; set; }

        public DateTime Time { get; set; }

        public string Platform { get; set; }


        public string PhotoPath { get; set; }

        [NotMapped]
        public IFormFile Photo { get; set; }

    }
}
