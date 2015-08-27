using PortfolioUnleashed.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PortfolioUnleashed.Models.ViewModels
{
    public class VMProject
    {
        public VMProject(Project p, int userId)
        {
            Title = p.Title;
            Description = p.Description;
            Collaborators = new List<VMContribution>();
            if (p.Contributions != null && p.Contributions.Count > 0)
            {
                foreach (Contribution c in p.Contributions)
                {
                    Collaborators.Add(new VMContribution(c));
                }
            }
            Media = new List<VMProjectMedium>();
            if (p.ProjectMedia != null && p.ProjectMedia.Count > 0)
            {
                foreach (ProjectMedium u in p.ProjectMedia)
                {
                    Media.Add(new VMProjectMedium(u));
                }
            }
            Template = (ProjectTemplate)p.Template;
            Id = p.Id;
            currentUserId = userId;
            if (p.Contributions != null && p.Contributions.Count > 0)
            {
                if(p.Contributions.Any(c => c.UserId == userId))
                {
                    PersonalContribution = new VMContribution(p.Contributions.FirstOrDefault(c => c.UserId == userId));
                }
            }
            ProjectEditorIds = new List<int>();
            if(p.ProjectPermissions !=null && p.ProjectPermissions.Count>0)
            {
                foreach (ProjectPermission pm in p.ProjectPermissions)
                {
                    if (pm.IsProjectEditor)
                    {
                        ProjectEditorIds.Add(pm.UserId);
                    }
                }
            }

            ContributionSetting = (ContributionSetting)p.CollaborationSetting;
        }
        public VMProject()
        {
            Collaborators = new List<VMContribution>();
            Media = new List<VMProjectMedium>();
            Template = ProjectTemplate.Template_1;
            ContributionSetting = ContributionSetting.Open;
        }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public List<VMProjectMedium> Media { get; set; }
        public List<VMContribution> Collaborators { get; set; }
        public List<int> ProjectEditorIds { get; set; }
        public ProjectTemplate Template { get; set; }
        public VMContribution PersonalContribution { get; set; }

        public int Id { get; private set; }
        public int currentUserId { get; private set; }

        [Display(Name = "Contribution Settings")]
        public ContributionSetting ContributionSetting { get; set; }
    }
}