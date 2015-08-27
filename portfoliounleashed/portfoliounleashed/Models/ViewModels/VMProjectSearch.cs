using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortfolioUnleashed.Models.ViewModels
{
    public class VMProjectSearch
    {
        public List<VMProjectSearchResult> Results { get; set; }
        public string SearchText { get; set; }
        public string SearchType { get; set; }
        public string OrderBy { get; set; }
        public bool IsDescending { get; set; }
    }
}