using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PortfolioUnleashed.Models.ViewModels;

namespace PortfolioUnleashed.Models.ViewModels
{
    public class VMUserSearch
    {
        public List<VMUserSearchResult> Results { get; set; }
        public string SearchText { get; set; }
        public string SearchType { get; set; }
        public string OrderBy { get; set; }
        public bool IsDescending { get; set; }
    }
}