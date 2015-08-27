using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortfolioUnleashed.Models.ViewModels
{
    public class VMNotifications
    {
        public List<VMNotification> Notifications { get; set; }
        public bool IsDescending { get; set; }
        public string OrderBy { get; set; }
    }
}