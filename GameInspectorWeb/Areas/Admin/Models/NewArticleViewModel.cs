﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GameInspectorWeb.Areas.Admin.Models
{
    public class NewArticleViewModel
    {

        public string Title { get; set; }

        public string Content { get; set; }

        public string CharacterContent { get; set; }

        public DateTime? Time { get; set; } = DateTime.Now;
        [NotMapped]
        public IFormFile CoverPhoto { get; set; }

        //public IFormFileCollection ContentPhotos { get; set; }
        [NotMapped]
        public IFormFileCollection ContentPhotos { get; set; }

        public string VideoPath { get; set; }
    }
}
