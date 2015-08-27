//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PortfolioUnleashed
{
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        public User()
        {
            this.ContactInfoes = new HashSet<ContactInfo>();
            this.Contributions = new HashSet<Contribution>();
            this.Educations = new HashSet<Education>();
            this.Links = new HashSet<Link>();
            this.Notifications = new HashSet<Notification>();
            this.Portfolios = new HashSet<Portfolio>();
            this.ProjectPermissions = new HashSet<ProjectPermission>();
            this.QuickReferences = new HashSet<QuickReference>();
        }
    
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Nullable<System.DateTime> LastDailyReportSendTime { get; set; }
        public int TotalViews { get; set; }
        public int ViewsSinceLastReport { get; set; }
        public Nullable<int> UserSettingId { get; set; }
    
        public virtual ICollection<ContactInfo> ContactInfoes { get; set; }
        public virtual ICollection<Contribution> Contributions { get; set; }
        public virtual ICollection<Education> Educations { get; set; }
        public virtual ICollection<Link> Links { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<Portfolio> Portfolios { get; set; }
        public virtual ICollection<ProjectPermission> ProjectPermissions { get; set; }
        public virtual ICollection<QuickReference> QuickReferences { get; set; }
        public virtual UserSetting UserSetting { get; set; }
    }
}