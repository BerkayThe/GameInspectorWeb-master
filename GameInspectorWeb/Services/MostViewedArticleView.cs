using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameInspectorWeb.Services
{
    public class MostViewedArticleView
    {
        public int ArticleId { get; set; }

        public int Hits { get; set; }

        public string Title { get; set; }
    }
}
