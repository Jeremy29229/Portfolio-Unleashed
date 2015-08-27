using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortfolioUnleashed.Models.ViewModels
{
    public class VMAllPortfolios
    {
        public List<VMPortfolio> Portfolios { get; set; }
        public string OrderBy { get; set; }
        public bool IsDescending { get; set; }
    }
}