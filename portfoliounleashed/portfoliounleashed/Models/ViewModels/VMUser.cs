using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PortfolioUnleashed.Data_Layer;

namespace PortfolioUnleashed.Models.ViewModels
{
    public class VMUser
    {
        public VMUser(User user, List<Project> projectsUserHasContributedTo=null)
        {
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            ContactInfos = new List<VMContactInfo>();
            LastDailyReportSendTime = user.LastDailyReportSendTime;
            ViewsSinceLastReport = user.ViewsSinceLastReport;
            TotalViews = user.TotalViews;

            if (user.ContactInfoes != null && user.ContactInfoes.Count > 0)
            {
                foreach (ContactInfo c in user.ContactInfoes)
                {
                    ContactInfos.Add(new VMContactInfo(c));
                }
            }

            Education = new List<VMEducation>();
            if (user.Educations != null && user.Educations.Count > 0)
            {
                foreach (Education e in user.Educations)
                {
                    Education.Add(new VMEducation(e));
                }
            }

            Links = new List<VMLink>();
            if (user.Links != null && user.Links.Count > 0)
            {
                foreach (Link l in user.Links)
                {
                    Links.Add(new VMLink(l));
                }
            }

            ProjectCatalog = new List<VMProject>();
            if (projectsUserHasContributedTo != null && projectsUserHasContributedTo.Count>0)
            {
                foreach (Project p in projectsUserHasContributedTo)
                {
                    ProjectCatalog.Add(new VMProject(p, user.Id));
                }
            }

            Portfolios = new List<VMPortfolio>();
            if (user.Portfolios != null && user.Portfolios.Count > 0)
            {
                foreach (Portfolio p in user.Portfolios)
                {
                    Portfolios.Add(new VMPortfolio(p));
                }
                if(user.Portfolios.Any(p=> p.IsMainPortfolio))
                {
                    MainPortfolio = new VMPortfolio(user.Portfolios.Where(p => p.IsMainPortfolio).FirstOrDefault());
                }
            }

            DatabaseDAL data = new DatabaseDAL();

            QuickContacts = new List<VMQuickContact>();
            if (user.QuickReferences != null && user.QuickReferences.Count > 0)
            {
                foreach(QuickReference q in user.QuickReferences)
                {
                    QuickContacts.Add(new VMQuickContact(data.retrieveUser(q.QuickReferenceId)));
                }
            }
            UserId = user.Id;

            Notifications = new List<VMNotification>();
            if (user.Notifications != null && user.Notifications.Count > 0)
            {
                foreach (var n in user.Notifications)
                {
                    Notifications.Add(new VMNotification(n));
                }
            }
        }
        public VMUser()
        {
            ContactInfos = new List<VMContactInfo>();
            Education = new List<VMEducation>();
            Links = new List<VMLink>();

            ProjectCatalog = new List<VMProject>();
            //get each project from project catalog that has this user as collaborator
            Portfolios = new List<VMPortfolio>();
        }

        public int UserId { get; private set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public DateTime? LastDailyReportSendTime { get; set; }
        public int TotalViews { get; set; }
        public int ViewsSinceLastReport { get; set; }

        public List<VMEducation> Education { get; set; }
        public List<VMContactInfo> ContactInfos { get; set; }
        public List<VMLink> Links { get; set; }

        public List<VMQuickContact> QuickContacts { get; set; }

        public List<VMProject> ProjectCatalog { get; set; }
        public List<VMPortfolio> Portfolios { get; set; }
        public VMPortfolio MainPortfolio { get; set; }

        public List<VMNotification> Notifications { get; set; }
        
    }
}