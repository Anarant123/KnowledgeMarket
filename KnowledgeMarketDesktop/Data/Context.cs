using KnowledgeMarketWebAPI.ApiClient;
using KnowledgeMarketWebAPI.ApiClient.Contracts.Responses;
using KnowledgeMarketWebAPI.Data.Models.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeMarketDesktop.Data
{
    public partial class Context
    {
        public static KnowledgeMarketApiClient Api = new KnowledgeMarketApiClient("http://localhost:5250/api/");
        public static AuthorizedModel? UserNow { get; set; }
        public static Course? CourseNow { get; set; }
        public static CourseListViewModel? AdList { get; set; } = null;
    }
}
