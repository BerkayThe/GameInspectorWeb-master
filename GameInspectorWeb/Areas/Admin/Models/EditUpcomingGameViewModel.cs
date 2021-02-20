using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameInspectorWeb.Areas.Admin.Models
{
    public class EditUpcomingGameViewModel
    {

        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime Time { get; set; }

        public string Platform { get; set; }


        public string PhotoPath { get; set; }

        public IFormFile Photo { get; set; }
    }
}
