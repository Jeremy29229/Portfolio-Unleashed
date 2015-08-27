using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortfolioUnleashed.Models.ViewModels
{
    public class VMPortfolioSearchResult
    {
        public string PortfolioTitle { get; set; }
        public string PortfolioURL { get; set; }
        public string PortfolioDescription { get; set; }
        public int PortfolioId { get; set; }
        public string OwnersFirstName { get; set; }
        public string OwnersLastName { get; set; }
        public int OwnerId { get; set; }
    }
}