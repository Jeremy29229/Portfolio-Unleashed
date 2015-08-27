using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortfolioUnleashed.Models.ViewModels
{
    public class VMLeaveCard
    {
        public int UserId { get; set; }
        public DateTime TimeStamp { get; set; }
        public bool IsSeen { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string URL { get; set; }
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public int? SenderId { get; set; }
    }
}