using PortfolioUnleashed.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PortfolioUnleashed.Models.ViewModels
{
    public class VMEditingProject
    {
        [Display(Name = "Project Title")]
        [Required]
        public string Title { get; set; }
        [Display(Name = "Project Description")]
        [Required]
        public string Description { get; set; }
        public List<VMProjectMedium> Media { get; set; }

        public VMEditingCollaborators EditingCollaborators { get; set; }

        public List<int> ProjectEditorIds { get; set; }
        public ProjectTemplate Template { get; set; }
        [Display(Name= "Contribution Settings")]
        public ContributionSetting ContributionSetting { get; set; }
        public VMContribution PersonalContribution { get; set; }

        public int Id { get; private set; }
        public int _currentUserId { get; private set; }

        public VMEditingProject(Project p, IEnumerable<User> quickContacts, int currentUserId)
        {
            Title = p.Title;
            Description = p.Description;
            Media = new List<VMProjectMedium>();
            if (p.ProjectMedia != null && p.ProjectMedia.Count > 0)
            {
                foreach (ProjectMedium u in p.ProjectMedia)
                {
                    Media.Add(new VMProjectMedium(u));
                }
            }
            EditingCollaborators = new VMEditingCollaborators(p.Contributions, quickContacts);
            Template = (ProjectTemplate)p.Template;
            ContributionSetting = (ContributionSetting)p.CollaborationSetting;

            Id = p.Id;
            _currentUserId = currentUserId;
            if (p.Contributions != null && p.Contributions.FirstOrDefault(c => c.UserId == _currentUserId) != null)
            {
                PersonalContribution = new VMContribution(p.Contributions.FirstOrDefault(c => c.UserId == _currentUserId));
            }
            else
            {
                PersonalContribution = new VMContribution();
            }
            ProjectEditorIds = new List<int>();
            if (p.ProjectPermissions != null && p.ProjectPermissions.Count > 0)
            {
                foreach (ProjectPermission pm in p.ProjectPermissions)
                {
                    if (pm.IsProjectEditor)
                    {
                        ProjectEditorIds.Add(pm.UserId);
                    }
                }
            }
        }
        public VMEditingProject()
        {
            Media = new List<VMProjectMedium>();
            EditingCollaborators = new VMEditingCollaborators();
            Template = ProjectTemplate.Template_1;
            ContributionSetting = ContributionSetting.Open;
        }
    }
}