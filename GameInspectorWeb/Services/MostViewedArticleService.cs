using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameInspectorWeb.Services
{
    public class MostViewedArticleService : IMostViewedArticleService
    {
        public IEnumerable<MostViewedArticleView> MostViewedArticles { get; set; }
    }
}
