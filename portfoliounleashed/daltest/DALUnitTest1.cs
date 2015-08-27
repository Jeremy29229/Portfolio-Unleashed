using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PortfolioUnleashed.DataLayer;
using PortfolioUnleashed.Data_Layer;
using PortfolioUnleashed;
using System.Collections.Generic;

namespace DALTest
{
    [TestClass]
    public class DALUnitTest1
    {
        DatabaseDAL db = new DatabaseDAL();

        [TestMethod]
        public void addUserTest()
        {
            User testAdd = new User() 
            { 
                FirstName = "TestUser", LastName = "McTesterson", Email = "email@gmail.com", 
                ContactInfoes = new List<ContactInfo>(), Educations = new List<Education>(), 
                Links = new List<Link>(), Media = new List<Medium>(), 
                Contributions = new List<Contribution>(), Portfolios = new List<Portfolio>(),
                Notifications = new List<Notification>(), ProjectPermissions = new List<ProjectPermission>(),
                QuickReferences = new List<QuickReference>()
            };
            int initialNumUsers = db.retrieveAllUsers().Count;
            db.addUser(testAdd);

            int finalNumUsers = db.retrieveAllUsers().Count;
            Assert.AreEqual(initialNumUsers+1, finalNumUsers, "User Add Failed"); 
            
            //ContactInfo testCI = new ContactInfo() { Information = "info", Title = "Sir" };
            //db.addContactInfo(testCI, 5);
            //Portfolio testM = new Portfolio() {Description = "description!",  Title = "Test Contribution", IsMainPortfolio = true, Visibility = 1, Projects = new List<Project>()  };
            //db.addPortfolio(testM, 5);
            //User toDelete = db.retrieveUser(5);
            //db.deleteUser(toDelete);
        }

        [TestMethod]
        public void addContactInfoTest()
        {
            ContactInfo testCI = new ContactInfo() 
            { 
                Information = "TestInformation", Title = "TestTitle" 
            };
            int initialNumContactInfos = db.retrieveAllContactInfos().Count;
            db.addContactInfo(testCI, 5);
            int finalNumContactInfos = db.retrieveAllContactInfos().Count;

            Assert.AreEqual(initialNumContactInfos + 1, finalNumContactInfos, "ContactInfo Add Failed");

        }


        [TestMethod]
        public void addPortfolioTest()
        {
            Portfolio testP = new Portfolio() 
            { 
                Title = "TestTitle", Description = "TestDescription", IsMainPortfolio = true, 
                Projects = new List<Project>(), Visibility = 1            
            };
            int initialNumPortfolios = db.retrieveAllPortfolios().Count;
            db.addPortfolio(testP, 5);
            int finalNumPortfolios = db.retrieveAllPortfolios().Count;

            Assert.AreEqual(initialNumPortfolios + 1, finalNumPortfolios, "Portfolio Add Failed");

        }

        [TestMethod]
        public void addContributionTest()
        {
            Contribution testC = new Contribution()
            {
                Title = "TestTitle",
                Description = "TestDescription", Media = new List<Medium>(),
                ///////////////////////////////////////////////////////////////userID? ProjectID?
            };
            int initialNumContributions = db.retrieveAllContributions().Count;
            db.addContribution(testC, 5);
            int finalNumContributions = db.retrieveAllContributions().Count;

            Assert.AreEqual(initialNumContributions + 1, finalNumContributions, "Contribution Add Failed");

        }

        [TestMethod]
        public void addEducationTest()
        {
            Education testE = new Education()
            {
               Degree = "TestDegree", School = "TestSchool", StartYear = 2012, EndYear = 2015
            };
            int initialNumEducations = db.retrieveAllEducations().Count;
            db.addEducation(testE, 5);
            int finalNumEducations = db.retrieveAllPortfolios().Count;

            Assert.AreEqual(initialNumEducations + 1, finalNumEducations, "Education Add Failed");

        }

        [TestMethod]
        public void addLinkTest()
        {
            Link testL = new Link()
            {
                DisplayText = "TestDisplayText", URL = "www.Google.com" 
            };
            int initialNumLinks = db.retrieveAllLinks().Count;
            db.addLink(testL, 5);
            int finalNumLinks = db.retrieveAllLinks().Count;

            Assert.AreEqual(initialNumLinks + 1, finalNumLinks, "Link Add Failed");

        }

        [TestMethod]
        public void addMediumTest()
        {
            Medium testM = new Medium()
            {
               Caption = "TestCaption", Contributions = new List<Contribution>(),
               Link = "www.google.com", MediaType = 1, Projects = new List<Project>(),
            };
            int initialNumMediums = db.retrieveAllMediums().Count;
            db.addMedium(testM);
            int finalNumMediums = db.retrieveAllMediums().Count;

            Assert.AreEqual(initialNumMediums + 1, finalNumMediums, "Medium Add Failed");

        }

        [TestMethod]
        public void addNotificationTest()
        {
            Notification testN = new Notification()
            {
                Title = "TestTitle",
                Description = "TestDescription", isSeen = false, TimeStamp = DateTime.UtcNow, 
                URL = "www.google.com"
            };
            int initialNumNotifications = db.retrieveAllNotifications().Count;
            db.addNotification(testN, 5);
            int finalNumNotifications = db.retrieveAllNotifications().Count;

            Assert.AreEqual(initialNumNotifications + 1, finalNumNotifications, "Notification Add Failed");

        }

        [TestMethod]
        public void addProjectTest()
        {
            Project testP = new Project()
            {
                Title = "TestTitle",
                Description = "TestDescription", Contributions = new List<Contribution>(), 
                Media = new List<Medium>(), Portfolios = new List<Portfolio>(), Template = 1,
                ProjectPermissions = new List<ProjectPermission>()
            };
            int initialNumProjects = db.retrieveAllProjects().Count;
            db.addProject(testP, 5);
            int finalNumProjects = db.retrieveAllProjects().Count;

            Assert.AreEqual(initialNumProjects + 1, finalNumProjects, "Project Add Failed");

        }








        //[TestMethod]
        //public void deleteUserTest()
        //{
        //    User testDelete = db.retrieveUser(5);
        //    int initialNumUsers = db.retrieveAllUsers().Count;
        //    db.deleteUser(testDelete);

        //    int finalNumUsers = db.retrieveAllUsers().Count;
        //    Assert.AreEqual(initialNumUsers-1, finalNumUsers, "User Delete Failed");

            
        //}


        //[TestMethod]
        //public void deleteContactInfoTest()
        //{
        //    ContactInfo testDelete = db.retrieveContactInfo(1);
        //    int initialNumContacts = db.retrieveAllUsers().Count;
        //    db.deleteContactInfo(testDelete, 5);

        //    int finalNumContacts = db.retrieveAllUsers().Count;
        //    Assert.AreEqual(initialNumContacts - 1, finalNumContacts, "ContactInfo Delete Failed");


        //}
    }



}
