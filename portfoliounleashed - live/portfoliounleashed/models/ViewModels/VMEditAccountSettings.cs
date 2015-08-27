using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortfolioUnleashed.Models.ViewModels
{
    public class VMEditAccountSettings
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public bool ProfileVisibility { get; set; }
        public bool EmailDailyViewReport { get; set; }
        public bool EmailAdminMessage { get; set; }
        public bool EmailCollaborationAddition { get; set; }
        public bool EmailCollaborationRemoval { get; set; }
        public bool EmailCollaborationRequest { get; set; }
    }
}