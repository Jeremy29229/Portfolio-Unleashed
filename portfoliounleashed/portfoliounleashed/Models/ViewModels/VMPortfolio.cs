using PortfolioUnleashed.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PortfolioUnleashed.Models.ViewModels
{
    public class VMPortfolio
    {
        public VMPortfolio(Portfolio portfolio)
        {
            Title = portfolio.Title;
            URL = portfolio.URL;
            Description = portfolio.Description;
            Projects = new List<VMProject>();
            if (portfolio.Projects != null && portfolio.Projects.Count > 0)
            {
                foreach (Project u in portfolio.Projects)
                {
                    Projects.Add(new VMProject(u, portfolio.UserId));
                }
            }
            Visibility = (VisibilityType)portfolio.Visibility;
            Id = portfolio.Id;
            UserId = portfolio.UserId;
        }
        public VMPortfolio()
        {
            Projects = new List<VMProject>();
            Visibility = VisibilityType.Public;
        }

        [Display(Name = "Portfolio Title")]
        public string Title { get; set; }
        public string URL { get; set; }
        [Display(Name = "Portfolio Description")]
        public string Description { get; set; }
        public List<VMProject> Projects { get; set; }
        public VisibilityType Visibility { get; set; }
        public bool IsMainPortfolio { get; set; }

        public int Id { get; private set; }
        public int UserId { get; private set; }
    }
}