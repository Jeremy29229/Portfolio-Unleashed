using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PortfolioUnleashed.Models.ViewModels
{
    public class VMContribution
    {
        [Display(Name = "Position Title")]
        public string Title { get; set; }

        [Display(Name = "Contribution Description")]
        public string Description { get; set; }
        public List<VMContributionMedium> Media { get; set; }

        public int UserId { get; private set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public string Name { get; private set; }

        public int ProjectId { get; private set; }

        public VMContribution(Contribution contribution)
        {
            Title = contribution.Title;
            Description = contribution.Description;
            Media = new List<VMContributionMedium>();
            if (contribution.ContributionMedia != null && contribution.ContributionMedia.Count > 0)
            {
                foreach (ContributionMedium m in contribution.ContributionMedia)
                {
                    Media.Add(new VMContributionMedium(m));
                }
            }
            UserId = contribution.UserId;
            ProjectId = contribution.ProjectId;
            if (contribution.User != null)
            {
                Email = contribution.User.Email;
                Name = contribution.User.FirstName + " " + contribution.User.LastName;
            }
        }
        public VMContribution()
        {
            Media = new List<VMContributionMedium>();
        }
    }
}