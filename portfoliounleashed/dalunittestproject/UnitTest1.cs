using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PortfolioUnleashed.DataLayer;
using PortfolioUnleashed.Data_Layer;
using PortfolioUnleashed;
using System.Collections.Generic;
using System.Linq;

namespace DALTest
{


    [TestClass]
    public class UnitTest1
    {
        #region TestObjects
        User testUser = new User()
        {
            FirstName = "TestUser",
            LastName = "McTesterson",
            Email = "email@gmail.com",
            ContactInfoes = new List<ContactInfo>(),
            Educations = new List<Education>(),
            Links = new List<Link>(),
            Contributions = new List<Contribution>(),
            Portfolios = new List<Portfolio>(),
            Notifications = new List<Notification>(),
            ProjectPermissions = new List<ProjectPermission>(),
            QuickReferences = new List<QuickReference>()
        };

        Portfolio testPortfolio = new Portfolio()
        {
            Title = "TestTitle",
            Description = "TestDescription",
            IsMainPortfolio = true,
            Projects = new List<Project>(),
            Visibility = 1
        };

        Project testProject = new Project()
        {
            Title = "TestTitle",
            Description = "TestDescription",
            Contributions = new List<Contribution>(),
            Portfolios = new List<Portfolio>(),
            Template = 1,
            ProjectPermissions = new List<ProjectPermission>(),
            ProjectMedia = new List<ProjectMedium>(),
            CollaborationSetting = 1
        };

        Education testEducation = new Education()
        {
            Degree = "TestDegree",
            School = "TestSchool",
            StartYear = 2012,
            EndYear = 2015
        };

        Link testLink = new Link()
        {
            DisplayText = "TestDisplayText",
            URL = "www.Google.com"
        };

        Notification testNotification = new Notification()
        {
            Title = "TestTitle",
            Description = "TestDescription",
            isSeen = false,
            TimeStamp = DateTime.UtcNow,
            URL = "www.google.com"
        };
        #endregion

        DatabaseDAL db = new DatabaseDAL();

        [TestMethod]
        public void addUserTest()
        {
            
            int preAddUsers = db.retrieveAllUsers().Count;
            db.addUser(testUser);

            int postAddUsers = db.retrieveAllUsers().Count;
            Assert.AreEqual(preAddUsers+1, postAddUsers, "User Add Failed");

            User toDelete = db.retrieveUser(postAddUsers);
            db.deleteUser(toDelete);

            int postDeleteUsers = db.retrieveAllUsers().Count;
            Assert.AreEqual(postAddUsers - 1, postDeleteUsers, "User Delete Failed"); 

        }

        [TestMethod]
        public void addContactInfoTest()
        {
           
            db.addUser(testUser);

            int numUsers = db.retrieveAllUsers().Count;
            
            ContactInfo testCI = new ContactInfo() 
            { 
                Information = "TestInformation", Title = "TestTitle", UserId = numUsers
            };
            int preAddNumContactInfos = db.retrieveAllContactInfos().Count;
            db.addContactInfo(testCI, numUsers);
            int postAddNumContactInfos = db.retrieveAllContactInfos().Count;

            Assert.AreEqual(preAddNumContactInfos + 1, postAddNumContactInfos, "ContactInfo Add Failed");


            User toDelete = db.retrieveUser(numUsers);
            db.deleteUser(toDelete);

            int postDeleteUsers = db.retrieveAllUsers().Count;
            Assert.AreEqual(numUsers - 1, postDeleteUsers, "User Delete Failed");
             

        }


        [TestMethod]
        public void addPortfolioTest()
        {
           
            db.addUser(testUser);

            int numUsers = db.retrieveAllUsers().Count;
            Portfolio testP = new Portfolio() 
            { 
                Title = "TestTitle", Description = "TestDescription", IsMainPortfolio = true, 
                Projects = new List<Project>(), Visibility = 1            
            };
            int initialNumPortfolios = db.retrieveAllPortfolios().Count;
            db.addPortfolio(testP, numUsers);
            int finalNumPortfolios = db.retrieveAllPortfolios().Count;

            Assert.AreEqual(initialNumPortfolios + 1, finalNumPortfolios, "Portfolio Add Failed");

            User toDelete = db.retrieveUser(numUsers);
            db.deleteUser(toDelete);

            int postDeleteUsers = db.retrieveAllUsers().Count;
            Assert.AreEqual(numUsers - 1, postDeleteUsers, "User Delete Failed");

        }

        [TestMethod]
        public void addContributionTest()
        {
            
            db.addUser(testUser);

            int numUsers = db.retrieveAllUsers().Count;
            
            db.addPortfolio(testPortfolio, numUsers);
            int numPortfolios = db.retrieveAllPortfolios().Count;

            
            db.addProjectToPortfolio(testProject, numPortfolios);
            int numProjects = db.retrieveAllProjects().Count;
            Contribution testC = new Contribution()
            {
                Title = "TestTitle",
                Description = "TestDescription",  UserId = numUsers, ProjectId = numProjects
            };
            int initialNumContributions = db.retrieveAllContributions().Count;
            db.addContribution(testC, numProjects);
            int finalNumContributions = db.retrieveAllContributions().Count;

            Assert.AreEqual(initialNumContributions + 1, finalNumContributions, "Contribution Add Failed");

            User toDelete = db.retrieveUser(numUsers);
            db.deleteUser(toDelete);

            int postDeleteUsers = db.retrieveAllUsers().Count;
            Assert.AreEqual(numUsers - 1, postDeleteUsers, "User Delete Failed");

        }

        [TestMethod]
        public void addEducationTest()
        {
            
            db.addUser(testUser);

            int numUsers = db.retrieveAllUsers().Count;
           
            int initialNumEducations = db.retrieveAllEducations().Count;
            db.addEducation(testEducation, numUsers);
            int finalNumEducations = db.retrieveAllEducations().Count;

            Assert.AreEqual(initialNumEducations + 1, finalNumEducations, "Education Add Failed");

            User toDelete = db.retrieveUser(numUsers);
            db.deleteUser(toDelete);

            int postDeleteUsers = db.retrieveAllUsers().Count;
            Assert.AreEqual(numUsers - 1, postDeleteUsers, "User Delete Failed");

        }

        [TestMethod]
        public void addLinkTest()
        {
           
            db.addUser(testUser);

            int numUsers = db.retrieveAllUsers().Count;
            Link testL = new Link()
            {
                DisplayText = "TestDisplayText", URL = "www.Google.com" 
            };
            int initialNumLinks = db.retrieveAllLinks().Count;
            db.addLink(testL, numUsers);
            int finalNumLinks = db.retrieveAllLinks().Count;

            Assert.AreEqual(initialNumLinks + 1, finalNumLinks, "Link Add Failed");

            User toDelete = db.retrieveUser(numUsers);
            db.deleteUser(toDelete);

            int postDeleteUsers = db.retrieveAllUsers().Count;
            Assert.AreEqual(numUsers - 1, postDeleteUsers, "User Delete Failed");

        }

        [TestMethod]
        public void addNotificationTest()
        {
           
            db.addUser(testUser);

            int numUsers = db.retrieveAllUsers().Count;
            Notification testN = new Notification()
            {
                Title = "TestTitle",
                Description = "TestDescription", isSeen = false, TimeStamp = DateTime.UtcNow, 
                URL = "www.google.com"
            };
            int initialNumNotifications = db.retrieveAllNotifications().Count;
            db.addNotification(testN, numUsers);
            int finalNumNotifications = db.retrieveAllNotifications().Count;

            Assert.AreEqual(initialNumNotifications + 1, finalNumNotifications, "Notification Add Failed");

            User toDelete = db.retrieveUser(numUsers);
            db.deleteUser(toDelete);

            int postDeleteUsers = db.retrieveAllUsers().Count;
            Assert.AreEqual(numUsers - 1, postDeleteUsers, "User Delete Failed");

        }

        [TestMethod]
        public void addProjectTest()
        {
            
            db.addUser(testUser);

            int numUsers = db.retrieveAllUsers().Count;
            
            db.addPortfolio(testPortfolio, numUsers);
            int numPortfolios = db.retrieveAllPortfolios().Count;

           
            int initialNumProjects = db.retrieveAllProjects().Count;
            db.addProjectToPortfolio(testProject, numPortfolios);
            int finalNumProjects = db.retrieveAllProjects().Count;

            Assert.AreEqual(initialNumProjects + 1, finalNumProjects, "Project Add Failed");

        }










        [TestMethod]
        public void updateUserTest()
        {
            User testAdd = new User()
            {
                FirstName = "TestUser",
                LastName = "McTesterson",
                Email = "email@gmail.com",
                ContactInfoes = new List<ContactInfo>(),
                Educations = new List<Education>(),
                Links = new List<Link>(),
                Contributions = new List<Contribution>(),
                Portfolios = new List<Portfolio>(),
                Notifications = new List<Notification>(),
                ProjectPermissions = new List<ProjectPermission>(),
                QuickReferences = new List<QuickReference>()
            };
            int preAddUsers = db.retrieveAllUsers().Count;
            db.addUser(testAdd);

            int postAddUsers = db.retrieveAllUsers().Count;
            Assert.AreEqual(preAddUsers + 1, postAddUsers, "User Add Failed");



            User testEdit = new User()
            {
                FirstName = "Edited",
                LastName = "McEdited",
                Email = "edited@gmail.com",
                Id = postAddUsers
            };

            db.updateUser(testEdit);

            User updatedUser = db.retrieveUser(postAddUsers);
            Assert.AreEqual(testEdit.FirstName, updatedUser.FirstName, "User Update Failed (FirstName)");
            Assert.AreEqual(testEdit.LastName, updatedUser.LastName, "User Update Failed (LastName)");
            Assert.AreEqual(testEdit.Email, updatedUser.Email, "User Update Failed (Email)");

        }

        [TestMethod]
        public void updateContactInfoTest()
        {

            db.addUser(testUser);

            int numUsers = db.retrieveAllUsers().Count;

            ContactInfo testCI = new ContactInfo()
            {
                Information = "TestInformation",
                Title = "TestTitle",
                UserId = numUsers
            };
            int preAddNumContactInfos = db.retrieveAllContactInfos().Count;
            db.addContactInfo(testCI, numUsers);
            int postAddNumContactInfos = db.retrieveAllContactInfos().Count;

            Assert.AreEqual(preAddNumContactInfos + 1, postAddNumContactInfos, "ContactInfo Add Failed");


            ContactInfo testEdit = new ContactInfo()
            {
                Information = "EditedInformation",
                Title = "EditedTitle",
                UserId = numUsers,
                Id = postAddNumContactInfos
            };

            db.updateContactInfo(testEdit);

            ContactInfo updatedCI = db.retrieveContactInfo(postAddNumContactInfos);
            Assert.AreEqual(testEdit.Information, updatedCI.Information, "ContactInfo Update Failed (Information)");
            Assert.AreEqual(testEdit.Title, updatedCI.Title, "ContactInfo Update Failed (Title)");

        }

        [TestMethod]
        public void updatePortfolioTest()
        {

            db.addUser(testUser);

            int numUsers = db.retrieveAllUsers().Count;
           
            int initialNumPortfolios = db.retrieveAllPortfolios().Count;
            db.addPortfolio(testPortfolio, numUsers);
            int finalNumPortfolios = db.retrieveAllPortfolios().Count;

            Assert.AreEqual(initialNumPortfolios + 1, finalNumPortfolios, "Portfolio Add Failed");

           

            Portfolio testEdit = new Portfolio()
            {
                Title = "EditedTitle",
                Description = "EditedDescription",
                IsMainPortfolio = false,
                Visibility = 2,
                Id = finalNumPortfolios
            };

            db.updatePortfolio(testEdit, numUsers);

            Portfolio updatedPortfolio = db.retrievePortfolio(finalNumPortfolios);
            Assert.AreEqual(testEdit.Title, updatedPortfolio.Title, "Portfolio Update Failed (Title)");
            Assert.AreEqual(testEdit.Description, updatedPortfolio.Description, "Portfolio Update Failed (Description)");
            Assert.AreEqual(testEdit.IsMainPortfolio, updatedPortfolio.IsMainPortfolio, "Portfolio Update Failed (IsMainPortfolio)");
            Assert.AreEqual(testEdit.Visibility, updatedPortfolio.Visibility, "Portfolio Update Failed (Visibility)");

        }

        [TestMethod]
        public void updateContributionTest()
        {

            db.addUser(testUser);

            int numUsers = db.retrieveAllUsers().Count;
            
            db.addPortfolio(testPortfolio, numUsers);
            int numPortfolios = db.retrieveAllPortfolios().Count;

            
            db.addProjectToPortfolio(testProject, numPortfolios);
            int numProjects = db.retrieveAllProjects().Count;
            Contribution testC = new Contribution()
            {
                Title = "TestTitle",
                Description = "TestDescription",
                UserId = numUsers,
                ProjectId = numProjects
            };
            int initialNumContributions = db.retrieveAllContributions().Count;
            db.addContribution(testC, numProjects);
            int finalNumContributions = db.retrieveAllContributions().Count;

            Assert.AreEqual(initialNumContributions + 1, finalNumContributions, "Contribution Add Failed");

            
            Contribution testEdit = new Contribution()
            {
                Title = "EditedTitle",
                Description = "EditedDescription",
                UserId = numUsers,
                ProjectId = numProjects
            };

            db.updateContribution(testEdit, numProjects);

            Contribution updatedContribution = db.retrieveContribution(numUsers, numProjects);
            Assert.AreEqual(testEdit.Title, updatedContribution.Title, "Contribution Update Failed (Title)");
            Assert.AreEqual(testEdit.Description, updatedContribution.Description, "Contribution Update Failed (Description)");
           
        }

        [TestMethod]
        public void updateEducationTest()
        {

            db.addUser(testUser);

            int numUsers = db.retrieveAllUsers().Count;
           
            int initialNumEducations = db.retrieveAllEducations().Count;
            db.addEducation(testEducation, numUsers);
            int finalNumEducations = db.retrieveAllEducations().Count;

            Assert.AreEqual(initialNumEducations + 1, finalNumEducations, "Education Add Failed");

            Education testEdit = new Education()
            {
                Degree = "EditedDegree",
                School = "EditedSchool",
                StartYear = 2033,
                EndYear = 2034,
                Id = finalNumEducations
            };

            db.updateEducation(testEdit, numUsers);

            Education updatedEducation = db.retrieveEducation(finalNumEducations);
            Assert.AreEqual(testEdit.Degree, updatedEducation.Degree, "Education Update Failed (Degree)");
            Assert.AreEqual(testEdit.School, updatedEducation.School, "Education Update Failed (School)");
            Assert.AreEqual(testEdit.StartYear, updatedEducation.StartYear, "Education Update Failed (StartYear)");
            Assert.AreEqual(testEdit.EndYear, updatedEducation.EndYear, "Education Update Failed (EndYear)");
           

        }

        [TestMethod]
        public void updateLinkTest()
        {

            db.addUser(testUser);

            int numUsers = db.retrieveAllUsers().Count;
           
            int initialNumLinks = db.retrieveAllLinks().Count;
            db.addLink(testLink, numUsers);
            int finalNumLinks = db.retrieveAllLinks().Count;

            Assert.AreEqual(initialNumLinks + 1, finalNumLinks, "Link Add Failed");

            Link testEdit = new Link()
            {
               DisplayText = "EditedDisplayText",
               URL = "www.edited.com",
               Id = finalNumLinks
            };

            db.updateLink(testEdit, numUsers);

            Link updatedLink = db.retrieveLink(finalNumLinks);
            Assert.AreEqual(testEdit.DisplayText, updatedLink.DisplayText, "Link Update Failed (DisplayText)");
            Assert.AreEqual(testEdit.URL, updatedLink.URL, "Link Update Failed (URL)");
           

        }

        [TestMethod]
        public void updateNotificationTest()
        {

            db.addUser(testUser);

            int numUsers = db.retrieveAllUsers().Count;
            
            int initialNumNotifications = db.retrieveAllNotifications().Count;
            db.addNotification(testNotification, numUsers);
            int finalNumNotifications = db.retrieveAllNotifications().Count;

            Assert.AreEqual(initialNumNotifications + 1, finalNumNotifications, "Notification Add Failed");

            var k = db.retrieveAllNotifications().Last().Id;
            int lastUserID = db.retrieveAllUserIds().Last();

            Notification testEdit = new Notification()
            {
                Title = "EditedTitle",
                Description = "EditedDescription",
                isSeen = true,
                TimeStamp = DateTime.UtcNow,
                URL = "www.edited.com",
                Id = k,
                NotificationType = 2,
                 UserId = lastUserID
            };
            
            db.updateNotification(testEdit, lastUserID);

            Notification updatedNotification = db.retrieveNotification(k);
            Assert.AreEqual(testEdit.Title, updatedNotification.Title, "Notification Update Failed (Title)");
            Assert.AreEqual(testEdit.Description, updatedNotification.Description, "Notification Update Failed (Description)");
            Assert.IsTrue(testEdit.TimeStamp == updatedNotification.TimeStamp, "Notification Update Failed (TimeStamp)");
           
        }

        [TestMethod]
        public void updateProjectTest()
        {

            db.addUser(testUser);

            int numUsers = db.retrieveAllUsers().Count;
            int lastUserID = db.retrieveAllUserIds().Last();

            db.addPortfolio(testPortfolio, lastUserID);
            int numPortfolios = db.retrieveAllPortfolios().Count;

            int lastPortID = db.retrieveAllPortfolios().Last().Id;

            int initialNumProjects = db.retrieveAllProjects().Count;
            db.addProjectToPortfolio(testProject, lastPortID);
            int finalNumProjects = db.retrieveAllProjects().Count;

            Assert.AreEqual(initialNumProjects + 1, finalNumProjects, "Project Add Failed");

            var lastID = db.retrieveAllNotifications().Last().Id;

            Project testEdit = new Project()
            {
                Title = "EditedTitle",
                Description = "EditedDescription",
                Template = 1,
                CollaborationSetting = 1,
                Id = lastID 

            };

            db.updateProject(testEdit, lastPortID);

            Project updatedProject = db.retrieveProject(lastID);
            Assert.AreEqual(testEdit.Title, updatedProject.Title, "Project Update Failed (Title)");
            Assert.AreEqual(testEdit.Description, updatedProject.Description, "Project Update Failed (Description)");
            Assert.AreEqual(testEdit.Template, updatedProject.Template, "Project Update Failed (Template)");
            Assert.AreEqual(testEdit.CollaborationSetting, updatedProject.CollaborationSetting, "Project Update Failed (CollaborationSetting)");

        }
    }
}
