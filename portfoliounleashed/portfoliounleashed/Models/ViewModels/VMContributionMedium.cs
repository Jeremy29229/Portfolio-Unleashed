using PortfolioUnleashed.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PortfolioUnleashed.Models.ViewModels
{
    public class VMContributionMedium
    {
        public VMContributionMedium(ContributionMedium medium)//ContributionMedium medium)
        {
            Link = medium.Link;
            MediaType = (MediaType)medium.MediumType;
            Caption = medium.Caption;
            Id = medium.Id;
            UserId = medium.UserId;
            ProjectId = medium.ProjectId;
        }
        public VMContributionMedium()
        {

        }

        public int UserId { get; private set; }
        public int ProjectId { get; private set; }
        [Display(Name = "Link to the media")]
        public string Link { get; set; }
        [Display(Name = "Media Type")]
        public MediaType MediaType { get; set; }
        [Display(Name = "Caption")]
        public string Caption { get; set; }
        public int Id { get; protected set; } 
    }
}