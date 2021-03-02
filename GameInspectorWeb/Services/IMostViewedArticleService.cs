using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameInspectorWeb.Services
{
    public interface IMostViewedArticleService
    {
        IEnumerable<MostViewedArticleView> MostViewedArticles { get; set; }
    }
}
