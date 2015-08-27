using PortfolioUnleashed.Enums;
using PortfolioUnleashed.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortfolioUnleashed.DataLayer
{
    public static class Translator
    {
        public static User userFromVMCreatingUser(VMCreatingUser vmUser)
        {
            User user = new User()
            {
                FirstName = vmUser.FirstName,
                LastName = vmUser.LastName,
                Email = vmUser.Email,
                ContactInfoes = contactInfoListFromVMContactInfoList(vmUser.ContactInfos),
                Educations = educationListFromVMEducationList(vmUser.Education),
                Links = linkListFromVMLinkList(vmUser.Links),
                Id = vmUser.UserId,
            };

            return user;
        }
        public static User userFromVMEditingUser(VMEditingUser vmUser)
        {
            User user = new User()
            {
                FirstName = vmUser.FirstName,
                LastName = vmUser.LastName,
                Email = vmUser.Email,
                ContactInfoes = contactInfoListFromVMContactInfoList(vmUser.ContactInfos),
                Educations = educationListFromVMEducationList(vmUser.Education),
                Links = linkListFromVMLinkList(vmUser.Links),
                Id = vmUser.UserId,
            };

            return user;
        }

        public static User userFromVMUser(VMUser vmUser)
        {
            User user = new User()
            {
                FirstName = vmUser.FirstName,
                LastName = vmUser.LastName,
                Email = vmUser.Email,
                ContactInfoes = contactInfoListFromVMContactInfoList(vmUser.ContactInfos),
                Educations = educationListFromVMEducationList(vmUser.Education),
                Portfolios = portfolioListFromVMportfolioList(vmUser.Portfolios),
                Links = linkListFromVMLinkList(vmUser.Links),
                TotalViews = vmUser.TotalViews,
                ViewsSinceLastReport = vmUser.ViewsSinceLastReport,
                LastDailyReportSendTime = vmUser.LastDailyReportSendTime,
                Id = vmUser.UserId
            };

            return user;
        }

        public static List<ContactInfo> contactInfoListFromVMContactInfoList(List<VMContactInfo> vmContactInfos)
        {
            List<ContactInfo> contactInfos = new List<ContactInfo>();
            foreach(VMContactInfo vmContact in vmContactInfos)
            {
                ContactInfo contact = new ContactInfo()
                {
                    Title = vmContact.Title,
                    Information = vmContact.Information,
                    Id = vmContact.Id
                
                };
                contactInfos.Add(contact);
            }
            return contactInfos;
        }

        public static List<Education> educationListFromVMEducationList(List<VMEducation> vmEducations)
        {
            List<Education> educations = new List<Education>();
            foreach(VMEducation vmEducation in vmEducations)
            {
                Education education = new Education()
                {
                    School = vmEducation.School,
                    Degree = vmEducation.Degree,
                    StartYear = (short)(vmEducation.StartYear),
                    EndYear = (short)(vmEducation.EndYear),
                    Id = vmEducation.Id
                };
                educations.Add(education);
            }
            return educations;
        }

        public static List<Link> linkListFromVMLinkList(List<VMLink> vmLinks)
        {
            List<Link> links = new List<Link>();
            foreach (VMLink vmLink in vmLinks)
            {
                Link link = new Link()
                {
                    DisplayText = vmLink.Title,
                    URL = vmLink.Url,
                    Id = vmLink.Id
                };
                links.Add(link);
            }
            return links;
        }

        public static List<Project> projectListFromVMProjectList(List<VMProject> vmProjects)
        {
            List<Project> projects = new List<Project>();
            foreach (VMProject vmProject in vmProjects)
            {
                Project project = new Project()
                {
                    Title = vmProject.Title,
                    Description = vmProject.Description,
                    Contributions = contributionListFromVMContributionList(vmProject.Collaborators),
                    Template = (int)vmProject.Template,
                    Id = vmProject.Id,
                    ProjectMedia = projectMediumListFromVMProjectMediumList(vmProject.Media),
                    CollaborationSetting = (int)vmProject.ContributionSetting
                };
                projects.Add(project);
            }
            return projects;
        }

        public static List<ProjectMedium> projectMediumListFromVMProjectMediumList(List<VMProjectMedium> vmMedias)
        {
            List<ProjectMedium> mediums = new List<ProjectMedium>();
            foreach (VMProjectMedium vmMedium in vmMedias)
            {
                ProjectMedium medium = new ProjectMedium()
                {
                    Link = vmMedium.Link,
                    MediumType = (int)vmMedium.MediaType,
                    Caption = vmMedium.Caption,
                    ProjectId = vmMedium.ProjectId,
                    Id = vmMedium.Id,
                };
                mediums.Add(medium);
            }
            return mediums;
        }

        public static List<ContributionMedium> contributionMediumListFromVMContributionMediumList(List<VMContributionMedium> vmMedias)
        {
            List<ContributionMedium> mediums = new List<ContributionMedium>();
            foreach (VMContributionMedium vmMedium in vmMedias)
            {
                ContributionMedium medium = new ContributionMedium()
                {
                    Link = vmMedium.Link,
                    MediumType = (int)vmMedium.MediaType,
                    Caption = vmMedium.Caption,
                    UserId = vmMedium.UserId,
                    ProjectId = vmMedium.ProjectId,
                    Id = vmMedium.Id,

                };
                mediums.Add(medium);
            }
            return mediums;
        }

         public static List<Contribution> contributionListFromVMContributionList(List<VMContribution> vmContributions)
        {
            List<Contribution> contributions = new List<Contribution>();
            foreach (VMContribution vmContribution in vmContributions)
            {
                Contribution contribution = new Contribution()
                {
                    UserId = vmContribution.UserId,
                    ProjectId = vmContribution.ProjectId,
                    Title = vmContribution.Title,
                    Description = vmContribution.Description,
                    ContributionMedia = contributionMediumListFromVMContributionMediumList(vmContribution.Media)
                };
                contributions.Add(contribution);
            }
            return contributions;
        }

        public static List<Portfolio> portfolioListFromVMportfolioList(List<VMPortfolio> vmPortfolios)
        {
            List<Portfolio> portfolios = new List<Portfolio>();
            foreach (VMPortfolio vmPortfolio in vmPortfolios)
            {
                Portfolio portfolio = new Portfolio()
                {
                    Title = vmPortfolio.Title,
                    Id = vmPortfolio.Id,
                    Description = vmPortfolio.Description,
                    Projects = projectListFromVMProjectList(vmPortfolio.Projects),
                    Visibility = (int)(vmPortfolio.Visibility)
                };
                portfolios.Add(portfolio);
            }
            return portfolios;
        }

        public static Portfolio portfolioFromVMEditingPortfolio(VMEditingPortfolio vmFolio)
        {
            Portfolio folio = new Portfolio()
            {
                Id = vmFolio.Id,
                UserId = vmFolio.UserId,
                Description = vmFolio.Description,
                Title = vmFolio.Title,
                IsMainPortfolio = vmFolio.IsMainPortfolio,
                Projects = projectListFromVMProjectList(vmFolio.Projects),
                Visibility = (int)vmFolio.Visibility,
            };

            return folio;
        }

        public static Notification NotificationFromVMNotification(VMNotification n)
        {
            Notification no = new Notification()
            {
                Id = n.Id,
                Description = n.Description,
                isSeen = n.IsSeen,
                NotificationType = (int)n.NotificationType,
                Sender = n.Sender,
                TimeStamp = n.TimeStamp,
                Title = n.Title,
                URL = n.URL,
                UserId = n.UserId,
                SenderId = n.SenderId
            };

            return no;
        }

        public static UserSetting UserSettingFromVMEditAccountSettings(VMEditAccountSettings n)
        {
            UserSetting set = new UserSetting()
            {
                IsPublic = n.ProfileVisibility,
                IsEmailedForViewReport = n.EmailDailyViewReport,
                IsEmailedForAdminMessage = n.EmailAdminMessage,
                IsEmailedForCollaborationAddition = n.EmailCollaborationAddition,
                IsEmailedForCollaborationRequest = n.EmailCollaborationRequest,
                IsEmailedForCollaborationRemoval = n.EmailCollaborationRemoval,
                UserId = n.UserID,
                Id = n.ID
            };

            return set;
        }

        public static VMNotifications VMNotificationsFromNotifications(List<Notification> notifications)
        {
            VMNotifications vmNotifications = new VMNotifications();
            vmNotifications.Notifications = new List<VMNotification>();

            foreach( var n in notifications)
            {
                vmNotifications.Notifications.Add(new VMNotification(n));
            }

            return vmNotifications;
        }
    }
}