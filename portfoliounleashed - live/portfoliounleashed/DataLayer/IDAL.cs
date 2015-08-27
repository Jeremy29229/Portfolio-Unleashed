using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortfolioUnleashed.Data_Layer
{
    public interface IDAL
    {
        void addUser(User u);
        void deleteUser(User u);
        void updateUser(User u);
        User retrieveUser(int userID);
        List<User> retrieveAllUsers();


        void addPortfolio(Portfolio p, int userID);
        void deletePortfolio(Portfolio p, int userID);
        void deleteUserPortfolios(int userID);
        void updatePortfolio(Portfolio p, int userID);
        Portfolio retrievePortfolio(int portfolioID);
        List<Portfolio> retrieveAllPortfolios();

        void addProjectToPortfolio(Project p, int portfolioID);
        void deleteProject(Project p, int portfolioID);
        void deleteProjectCompletely(Project p);
        void removePortfolioProjects(int portfolioID);
        void updateProject(Project p, int portfolioID);
        Project retrieveProject(int projectID);
        List<Project> retrieveAllProjects();


        void addContactInfo(ContactInfo c, int userID);
        void deleteContactInfo(ContactInfo c, int userID);
        void updateContactInfo(ContactInfo c);
        ContactInfo retrieveContactInfo(int contactID);
        List<ContactInfo> retrieveAllContactInfos();

        void addContribution(Contribution c, int projectID);
        void deleteContribution(Contribution c, int projectID);
        void updateContribution(Contribution c, int projectID);
        Contribution retrieveContribution(int userID, int projectID);
        List<Contribution> retrieveAllContributions();


        void addEducation(Education e, int userID);
        void deleteEducation(Education e, int userID);
        void updateEducation(Education e, int userID);
        Education retrieveEducation(int educationID);
        List<Education> retrieveAllEducations();


        void addLink(Link l, int userID);
        void deleteLink(Link l, int userID);
        void updateLink(Link l, int userID);
        Link retrieveLink(int linkID);
        List<Link> retrieveAllLinks();


        void addProjectMedium(ProjectMedium m, int projectId);
        void addContributionMedium(ContributionMedium m, int userId, int projectId);
        void removeProjectMedium(ProjectMedium m);
        void removeContributionMedium(ContributionMedium m);
        void updateProjectMedium(ProjectMedium m);
        void updateContributionMedium(ContributionMedium m);
        ProjectMedium retrieveProjectMedium(int mediumID);
        ContributionMedium retrieveContributionMedium(int mediumID);
        List<ProjectMedium> retrieveAllProjectMediums();
        List<ContributionMedium> retrieveAllContributionMediums();


        void addNotification(Notification n, int userID);
        void deleteNotification(int notificationId);
        void updateNotification(Notification n, int userID);
        Notification retrieveNotification(int notificationID);
        List<Notification> retrieveAllNotifications();


        void addUserSettings(UserSetting n);
        void deleteUserSettings(UserSetting n, int userID);
        void updateUserSettings(UserSetting n, int userID);
        UserSetting retrieveUserSettings(int userId);
    }
}
