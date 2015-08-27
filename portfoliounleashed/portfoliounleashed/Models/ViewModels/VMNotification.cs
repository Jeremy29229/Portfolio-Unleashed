using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PortfolioUnleashed.Enums;

namespace PortfolioUnleashed.Models.ViewModels
{
    public class VMNotification
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime TimeStamp { get; set; }
        public bool IsSeen { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string URL { get; set; }
        public string Sender { get; set; }
        public NotificationType NotificationType { get; set; }
        public int? SenderId { get; set; }

        //derived
        public string Email { get; set; }

        public VMNotification(Notification n)
        {
            Id = n.Id;
            UserId = n.UserId;
            TimeStamp = n.TimeStamp;
            IsSeen = n.isSeen;
            Title = n.Title;
            Description = n.Description;
            URL = n.URL;
            Sender = n.Sender;
            NotificationType = (NotificationType)n.NotificationType;
            SenderId = n.SenderId;
        }

        public VMNotification(VMLeaveFeedback f)
        {
            UserId = f.UserId;
            TimeStamp = f.TimeStamp;
            IsSeen = f.IsSeen;
            Title = f.Title;
            Description = f.Description;
            URL = f.URL;
            Sender = f.Sender;
            NotificationType = NotificationType.PortfolioFeedback;
            SenderId = f.SenderId;
        }

        public VMNotification(VMLeaveCard f)
        {
            UserId = f.UserId;
            TimeStamp = f.TimeStamp;
            IsSeen = f.IsSeen;
            Title = f.Title;
            Description = f.Description;
            URL = f.URL;
            Sender = f.Sender;
            NotificationType = NotificationType.LeaveCard;
            SenderId = f.SenderId;
        }

        public VMNotification(VMAnnouncementNotification f)
        {
            UserId = f.UserId;
            TimeStamp = f.TimeStamp;
            IsSeen = f.IsSeen;
            Title = f.Title;
            Description = f.Description;
            URL = f.URL;
            Sender = f.Sender;
            NotificationType = NotificationType.LeaveCard;
            SenderId = f.SenderId;
        }
    }
}