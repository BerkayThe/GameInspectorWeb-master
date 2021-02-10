using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GameInspectorWeb.Areas.Admin.Models
{
    public class EditArticleViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string CharacterContent { get; set; }

        public DateTime? Time { get; set; }

        public string ExistingCoverPhotoPath { get; set; }

        public string ExistingContentPhotoPath { get; set; }

        [NotMapped]
        public IFormFile CoverPhoto { get; set; }

        [NotMapped]
        public IFormFileCollection ContentPhotos { get; set; }

        public string VideoPath { get; set; }
    }
}
