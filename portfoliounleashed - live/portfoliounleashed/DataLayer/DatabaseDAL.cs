using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using PortfolioUnleashed.Models;
using System.Data.Entity.Validation;

namespace PortfolioUnleashed.Data_Layer
{
    public class DatabaseDAL : IDAL
    {
        #region UserDAL

        public void addUser(User u)
        {
            using (var db = new PortfolioUnleashedContext())
            {
                db.Users.Add(u);
                db.SaveChanges();
            }
        }

        public void deleteUser(User u)
        {
            using (var db = new PortfolioUnleashedContext())
            {
                deleteUserPortfolios(u.Id);
                foreach (var v in db.ContactInfoes.Where(c => c.UserId == u.Id)) db.ContactInfoes.Remove(v);
                foreach (var v in db.Educations.Where(c => c.UserId == u.Id)) db.Educations.Remove(v);
                foreach (var v in db.Links.Where(c => c.UserId == u.Id)) db.Links.Remove(v);
                foreach (var v in db.ContributionMediums.Where(c => c.UserId == u.Id)) db.ContributionMediums.Remove(v);
                foreach (var v in db.QuickReferences.Where(c => c.UserId == u.Id || c.QuickReferenceId == u.Id)) db.QuickReferences.Remove(v);
                foreach (var v in db.Contributions.Where(c => c.UserId == u.Id)) db.Contributions.Remove(v);
                foreach (var v in db.ProjectPermissions.Where(c => c.UserId == u.Id)) db.ProjectPermissions.Remove(v);
                foreach (var v in db.Notifications.Where(c => c.UserId == u.Id)) db.Notifications.Remove(v);
                db.Database.ExecuteSqlCommand("DELETE FROM webpages_Membership WHERE UserId = {0}", u.Id);
                db.Database.ExecuteSqlCommand("DELETE FROM webpages_UsersInRoles WHERE UserId = {0}", u.Id);
                db.Users.Remove(db.Users.FirstOrDefault(user => user.Id == u.Id));
                db.SaveChanges();
            }
        }

        public void updateUser(User u)
        {
            using (var db = new PortfolioUnleashedContext())
            {
                if (u.FirstName != null) db.Users.Include("ContactInfoes").Include("Educations").Include("Links").Include("Portfolios").Include("QuickReferences").Include("UserSetting").FirstOrDefault(user => user.Id == u.Id).FirstName = u.FirstName;
                if (u.LastName != null) db.Users.Include("ContactInfoes").Include("Educations").Include("Links").Include("Portfolios").Include("QuickReferences").Include("UserSetting").FirstOrDefault(user => user.Id == u.Id).LastName = u.LastName;
                if (u.Email != null) db.Users.Include("ContactInfoes").Include("Educations").Include("Links").Include("Portfolios").Include("QuickReferences").Include("UserSetting").FirstOrDefault(user => user.Id == u.Id).Email = u.Email;
                db.SaveChanges();
            }
        }

        public void updateUserViewsData(User newUserData)
        {
            using (var db = new PortfolioUnleashedContext())
            {
                if (newUserData != null && newUserData.Id > 0 && db.Users.FirstOrDefault(u => u.Id == newUserData.Id) != null)
                {
                    db.Users.FirstOrDefault(u => u.Id == newUserData.Id).LastDailyReportSendTime = newUserData.LastDailyReportSendTime;
                    db.Users.FirstOrDefault(u => u.Id == newUserData.Id).ViewsSinceLastReport = newUserData.ViewsSinceLastReport;
                    db.Users.FirstOrDefault(u => u.Id == newUserData.Id).TotalViews = newUserData.TotalViews;
                    db.SaveChanges();
                }
            }
        }

        public User retrieveUser(int userID)
        {
            User theUser;
            using (var db = new PortfolioUnleashedContext())
            {
                theUser = db.Users.Include("ContactInfoes").Include("Educations").Include("Links").Include("Portfolios").Include("UserSetting").Include(u => u.Portfolios.Select(p => p.Projects)).Include(u => u.Portfolios.Select(p => p.Projects.Select(pr => pr.ProjectMedia))).Include("QuickReferences").Include("Notifications").FirstOrDefault(user => user.Id == userID);
            }
            return theUser;
        }

        public User retrieveUserByEmail(string email)
        {
            User theUser;
            using (var db = new PortfolioUnleashedContext())
            {
                theUser = db.Users.Include("ContactInfoes").Include("Educations").Include("Links").Include("Portfolios").Include("QuickReferences").Include("UserSetting").FirstOrDefault(user => user.Email == email);
            }
            return theUser;
        }

        public List<User> retrieveAllUsers()
        {
            List<User> result = new List<User>();
            using (var db = new PortfolioUnleashedContext())
            {
                result = db.Users.Include("ContactInfoes").Include("Educations").Include("Links").Include("Portfolios").Include("QuickReferences").Include("UserSetting").ToList();
            }
            return result;
        }

        #endregion

        #region QuickReferenceDAL

        public void addQuickReference(int UserId, int QuickReferenceUserId)
        {
            using (var db = new PortfolioUnleashedContext())
            {
                db.Users.Include("QuickReferences").FirstOrDefault(u => u.Id == UserId).QuickReferences.Add(new QuickReference() { UserId = UserId, QuickReferenceId = QuickReferenceUserId });
                db.SaveChanges();
            }
        }

        public void removeQuickReference(int UserId, int QuickReferenceUserId)
        {
            using (var db = new PortfolioUnleashedContext())
            {
                db.Users.Include("QuickReferences").FirstOrDefault(u => u.Id == UserId).QuickReferences.Remove(
                    db.Users.Include("QuickReferences").FirstOrDefault(u => u.Id == UserId).QuickReferences.FirstOrDefault(q => q.QuickReferenceId == QuickReferenceUserId));
                db.SaveChanges();
            }
        }

        public List<QuickReference> retrieveQuickReferences(int userId)
        {
            List<QuickReference> result;
            using (var db = new PortfolioUnleashedContext())
            {
                result = db.Users.Include("QuickReferences").FirstOrDefault(u => u.Id == userId).QuickReferences.ToList();
            }
            return result;
        }

        #endregion

        #region PortfolioDAL
        public void addPortfolio(Portfolio p, int userID)
        {
            using (var db = new PortfolioUnleashedContext())
            {
                p.Projects.Clear();
                p.UserId = userID;
                p.User = null;
                p.Id = 0;
                db.Portfolios.Add(p);
                db.SaveChanges();
            }
        }

        public void deletePortfolio(Portfolio p, int userID)
        {
            using (var db = new PortfolioUnleashedContext())
            {
                removePortfolioProjects(p.Id);
                db.Users.Include("Portfolios").FirstOrDefault(user => user.Id == userID).Portfolios.Remove(db.Users.Include("Portfolios").FirstOrDefault(user => user.Id == userID).Portfolios.FirstOrDefault(port => port.Id == p.Id));
                db.Portfolios.Remove(db.Portfolios.FirstOrDefault(port => port.Id == p.Id));
                db.SaveChanges();
            }
        }

        public void deleteUserPortfolios(int userID)
        {
            using (var db = new PortfolioUnleashedContext())
            {
                foreach (Portfolio p in db.Users.Include("Portfolios").FirstOrDefault(user => user.Id == userID).Portfolios)
                    removePortfolioProjects(p.Id);
                db.Users.Include("Portfolios").FirstOrDefault(user => user.Id == userID).Portfolios.Clear();
                db.Portfolios.RemoveRange(db.Portfolios.Where(port => port.UserId == userID));
                db.SaveChanges();
            }
        }

        public void updatePortfolio(Portfolio p, int userID)
        {
            using (var db = new PortfolioUnleashedContext())
            {
                if (p.Title != null) db.Users.Include("Portfolios").FirstOrDefault(user => user.Id == userID).Portfolios.FirstOrDefault(port => port.Id == p.Id).Title = p.Title;
                if (p.Description != null) db.Users.Include("Portfolios").FirstOrDefault(user => user.Id == userID).Portfolios.FirstOrDefault(port => port.Id == p.Id).Description = p.Description;
                if (p.Visibility != db.Users.Include("Portfolios").FirstOrDefault(user => user.Id == userID).Portfolios.FirstOrDefault(port => port.Id == p.Id).Visibility) db.Users.Include("Portfolios").FirstOrDefault(user => user.Id == userID).Portfolios.FirstOrDefault(port => port.Id == p.Id).Visibility = p.Visibility;
                if (p.IsMainPortfolio != db.Users.Include("Portfolios").FirstOrDefault(user => user.Id == userID).Portfolios.FirstOrDefault(port => port.Id == p.Id).IsMainPortfolio) db.Users.Include("Portfolios").FirstOrDefault(user => user.Id == userID).Portfolios.FirstOrDefault(port => port.Id == p.Id).IsMainPortfolio = p.IsMainPortfolio;

                db.SaveChanges();
            }
        }

        public Portfolio retrievePortfolio(int portfolioID)
        {
            using (var db = new PortfolioUnleashedContext())
            {
                return db.Portfolios.Include("User").Include("Projects")
                    .Include(port => port.Projects.Select(proj => proj.ProjectMedia))
                    .Include(port => port.Projects.Select(proj => proj.Contributions))
                    .Include(port => port.Projects.Select(proj => proj.Contributions.Select(c => c.ContributionMedia)))
                    .FirstOrDefault(port => port.Id == portfolioID);
            }
        }

        public List<Portfolio> retrieveAllPortfolios()
        {
            List<Portfolio> result = new List<Portfolio>();
            using (var db = new PortfolioUnleashedContext())
            {
                result = db.Portfolios.Include("User").Include("Projects").ToList();
            }
            return result;
        }

        public void addFeaturedPortfolio(int folioId)
        {
            using (var db = new PortfolioUnleashedContext())
            {
                db.FeaturedPortfolios.Add(new FeaturedPortfolio { PortfolioId = folioId });
                db.SaveChanges();
            }
        }

        public List<Portfolio> retrieveFeaturedPortfolios()
        {
            List<Portfolio> result = new List<Portfolio>();
            using (var db = new PortfolioUnleashedContext())
            {
                result = db.Portfolios.Include("User").Include("Projects").Where(p => db.FeaturedPortfolios.Contains(db.FeaturedPortfolios.FirstOrDefault(f => f.PortfolioId == p.Id))).ToList();
            }
            return result;
        }

        #endregion

        #region ProjectDAL

        public void addProject(Project p)
        {
            using (var db = new PortfolioUnleashedContext())
            {
                List<ProjectPermission> permissionsTemp = new List<ProjectPermission>();
                if (p.ProjectPermissions != null && p.ProjectPermissions.Count > 0)
                {
                    permissionsTemp = p.ProjectPermissions.ToList();
                    p.ProjectPermissions.Clear();
                }
                db.Projects.Add(p);
                db.SaveChanges();
                foreach (ProjectPermission pp in permissionsTemp)
                {
                    addPermissionToProject(pp, p.Id);
                }
            }
        }

        public void addProjectToPortfolio(Project p, int portfolioID)
        {
            using (var db = new PortfolioUnleashedContext())
            {

                if (!db.Projects.Any(proj => proj.Id == p.Id))
                {
                    addProject(p);
                }
                db.Portfolios.Include("User").Include("Projects").FirstOrDefault(port => port.Id == portfolioID).Projects.Add(db.Projects.Where(pr=> pr.Id == p.Id).FirstOrDefault());
                db.SaveChanges();
            }
        }

        public void updatePermission(ProjectPermission p)
        {
            using (var db = new PortfolioUnleashedContext())
            {
                db.ProjectPermissions.FirstOrDefault(pp => pp.UserId == p.UserId && pp.ProjectId == pp.ProjectId).IsProjectEditor = p.IsProjectEditor;
                db.SaveChanges();
            }
        }

        public void addPermissionToProject(ProjectPermission p, int projectId)
        {
            using (var db = new PortfolioUnleashedContext())
            {
                p.ProjectId = projectId;
                db.ProjectPermissions.Add(p);
                db.SaveChanges();
            }
        }

        public void removePermissionFromProject(ProjectPermission p, int projectId)
        {
            using (var db = new PortfolioUnleashedContext())
            {
                db.ProjectPermissions.Remove(db.ProjectPermissions.FirstOrDefault(pp => pp.UserId == p.UserId && pp.ProjectId == pp.ProjectId));
                db.SaveChanges();
            }
        }

        public void removeProjectFromPortfolio(Project p, int portfolioID)
        {
            using (var db = new PortfolioUnleashedContext())
            {
                db.Portfolios.Include("User").Include("Projects").FirstOrDefault(port => port.Id == portfolioID).Projects.Remove(db.Projects.Where(pr => pr.Id == p.Id).FirstOrDefault());
                db.SaveChanges();
            }
        }

        public void removePortfolioProjects(int portfolioID)
        {
            using (var db = new PortfolioUnleashedContext())
            {
                db.Portfolios.Include("User").Include("Projects").FirstOrDefault(port => port.Id == portfolioID).Projects.Clear();
                db.SaveChanges();
            }
        }

        //public void removeProjectfromUserProjects(Project p, int userID)
        //{
        //    using (var db = new PortfolioUnleashedContext())
        //    {
                
        //    }
        //}

        public void deleteProject(Project p, int portfolioID)
        {
            using (var db = new PortfolioUnleashedContext())
            {
                db.Portfolios.Include("User").Include("Projects").FirstOrDefault(port => port.Id == portfolioID).Projects.Remove(db.Projects.Where(pr => pr.Id == p.Id).FirstOrDefault());
                db.SaveChanges();
            }
        }

        public void deleteProjectCompletely(Project p)
        {
            using (var db = new PortfolioUnleashedContext())
            {
                foreach (var v in p.Portfolios.Where(port => port.Projects.Contains(p)).ToList()) v.Projects.Remove(db.Projects.FirstOrDefault(proj => proj.Id==p.Id));
                foreach (var v in db.ProjectPermissions.Where(c => c.ProjectId == p.Id)) db.ProjectPermissions.Remove(v);
                foreach (var v in db.ProjectMediums.Where(c => c.ProjectId == p.Id)) db.ProjectMediums.Remove(v);
                foreach (var v in db.Contributions.Where(c => c.ProjectId == p.Id)) db.Contributions.Remove(v);
                db.Projects.Remove(db.Projects.FirstOrDefault(pro => pro.Id == p.Id));
                db.SaveChanges();
            }
        }

        public void updateProject(Project p, int portfolioID)
        {
            using (var db = new PortfolioUnleashedContext())
            {
                if (p.Title != null) db.Projects.FirstOrDefault(proj => proj.Id == p.Id).Title = p.Title;
                if (p.Description != null) db.Projects.FirstOrDefault(proj => proj.Id == p.Id).Description = p.Description;
                if (p.Template != db.Projects.FirstOrDefault(proj => proj.Id == p.Id).Template) db.Projects.FirstOrDefault(proj => proj.Id == p.Id).Template = p.Template;
                if (p.CollaborationSetting != db.Projects.FirstOrDefault(proj => proj.Id == p.Id).CollaborationSetting) db.Projects.FirstOrDefault(proj => proj.Id == p.Id).CollaborationSetting = p.CollaborationSetting;
                db.SaveChanges();
            }
        }

        public void updateProject(Project p)
        {
            using (var db = new PortfolioUnleashedContext())
            {
                if (p.Title != null) db.Projects.FirstOrDefault(proj => proj.Id == p.Id).Title = p.Title;
                if (p.Description != null) db.Projects.FirstOrDefault(proj => proj.Id == p.Id).Description = p.Description;
                if (p.Template != db.Projects.FirstOrDefault(proj => proj.Id == p.Id).Template) db.Projects.FirstOrDefault(proj => proj.Id == p.Id).Template = p.Template;
                if (p.CollaborationSetting != db.Projects.FirstOrDefault(proj => proj.Id == p.Id).CollaborationSetting) db.Projects.FirstOrDefault(proj => proj.Id == p.Id).CollaborationSetting = p.CollaborationSetting;
                db.SaveChanges();
            }
        }

        public Project retrieveProject(int projectID)
        {
            using (var db = new PortfolioUnleashedContext())
            {
                return db.Projects.Include("Contributions").Include(p => p.Contributions.Select(c => c.ContributionMedia)).Include(p => p.Contributions.Select(c => c.User)).Include("ProjectMedia").Include("Portfolios").Include("ProjectPermissions").FirstOrDefault(proj => proj.Id == projectID);
            }
        }

        public List<Project> retrieveAllProjectsByUserID(int userID)
        {
            using (var db = new PortfolioUnleashedContext())
            {
                return db.Projects.Include("Contributions").Include(p => p.Contributions.Select(c => c.ContributionMedia)).Include("ProjectMedia").Include("Portfolios").Include("ProjectPermissions").Where(p => p.Contributions.Any(c => c.UserId == userID)).ToList();
            }
        }

        public List<Project> retrieveAllProjects()
        {
            List<Project> result = new List<Project>();
            using (var db = new PortfolioUnleashedContext())
            {
                result = db.Projects.Include("Contributions").Include("ProjectMedia").Include("Portfolios").Include("ProjectPermissions").ToList();
            }
            return result;
        }

        #endregion

        #region ContactDAL

        public void addContactInfo(ContactInfo c, int userID)
        {
            using (var db = new PortfolioUnleashedContext())
            {
                db.Users.Include("ContactInfoes").Include("Educations").Include("Links").Include("Portfolios").Include("QuickReferences").FirstOrDefault(user => user.Id == userID).ContactInfoes.Add(c);
                db.SaveChanges();
            }
        }

        public void deleteContactInfo(ContactInfo c, int userID)
        {
            using (var db = new PortfolioUnleashedContext())
            {
                db.Users.Include("ContactInfoes").Include("Educations").Include("Links").Include("Portfolios").Include("QuickReferences").FirstOrDefault(user => user.Id == userID).ContactInfoes.Remove(c);

                db.SaveChanges();
            }
        }

        public void updateContactInfo(ContactInfo c)
        {
            using (var db = new PortfolioUnleashedContext())
            {
                //if (c.UserId != retrieveUser(userID).ContactInfoes.FirstOrDefault(cont => cont.Id == c.Id).UserId) retrieveUser(userID).ContactInfoes.FirstOrDefault(cont => cont.Id == c.Id).UserId = c.UserId;
                if (c.Title != null) db.ContactInfoes.FirstOrDefault(cont => cont.Id == c.Id).Title = c.Title;
                if (c.Information != null) db.ContactInfoes.FirstOrDefault(cont => cont.Id == c.Id).Information = c.Information;
                db.SaveChanges();
            }
        }

        public ContactInfo retrieveContactInfo(int contactID)
        {
            using (var db = new PortfolioUnleashedContext())
            {
                return db.ContactInfoes.Include("User").FirstOrDefault(cont => cont.Id == contactID);
            }
        }

        public List<ContactInfo> retrieveAllContactInfos()
        {
            List<ContactInfo> result = new List<ContactInfo>();
            using (var db = new PortfolioUnleashedContext())
            {
                result = db.ContactInfoes.Include("User").ToList();
            }
            return result;
        }

        #endregion

        #region ContributionDAL

        public void addContribution(Contribution c, int projectID)
        {
            using (var db = new PortfolioUnleashedContext())
            {
                db.Projects.Include("Contributions").Include(p => p.Contributions.Select(co => co.ContributionMedia)).FirstOrDefault(u => u.Id == projectID).Contributions.Add(c);
                db.SaveChanges();
            }
        }

        public void deleteContribution(Contribution c, int projectID)
        {
            using (var db = new PortfolioUnleashedContext())
            {
                db.Projects.Include("Contributions").Include(p => p.Contributions.Select(co => co.ContributionMedia)).FirstOrDefault(u => u.Id == projectID)
                    .Contributions.Remove(db.Contributions.Where(con => con.ProjectId == projectID && con.UserId == c.UserId).FirstOrDefault());
                if (db.ContributionMediums.Any(co => co.UserId == c.UserId && co.ProjectId == projectID)) //Remove all media for contribution
                {
                    foreach (ContributionMedium cm in db.ContributionMediums.Where(co => co.UserId == c.UserId && co.ProjectId == projectID))
                    {
                        db.ContributionMediums.Remove(cm);
                    }
                }
                db.Contributions.Remove(db.Contributions.Where(cb => cb.UserId == c.UserId && cb.ProjectId == c.ProjectId).FirstOrDefault()); //Remove the contribution
                db.SaveChanges();
                if (db.Projects.Include("Contributions").FirstOrDefault(proj => proj.Id == projectID).Contributions.Count <= 0)//If no contributions remaining
                {

                    deleteProjectCompletely(db.Projects.Include("Contributions").FirstOrDefault(prod => prod.Id == projectID));
                }
                else if(db.Users.Include("ProjectPermissions").FirstOrDefault(user => user.Id == c.UserId).ProjectPermissions.Any(proj => proj.IsProjectEditor == true)) //Contributing user was a project editor
                {
                    if(!db.Users.Include("ProjectPermissions").Any(user => (user.ProjectPermissions.Any(proj => (proj.ProjectId == projectID && proj.IsProjectEditor == true))))) //If no remaining contributing users are project editors
                    {
                        int selectedUserToPromote = db.Projects.Include("Contributions").FirstOrDefault(proj => proj.Id == projectID).Contributions.FirstOrDefault(cont => cont.UserId != null).UserId;
                        db.Users.Include("ProjectPermissions").FirstOrDefault(user => user.Id == selectedUserToPromote).ProjectPermissions.FirstOrDefault(perm => perm.ProjectId == projectID).IsProjectEditor = true;
                    }
                }
                if(db.ProjectPermissions.Any(perm => perm.UserId == c.UserId && perm.ProjectId == projectID))
                {
                    db.Projects.Include("ProjectPermissions").Where(pr => pr.Id == projectID).FirstOrDefault().ProjectPermissions
                        .Remove(db.ProjectPermissions.Include("Project").Include("User").Where(perm => perm.UserId == c.UserId && perm.ProjectId == projectID).FirstOrDefault());
                }
                db.SaveChanges();
            }
        }

        public void updateContribution(Contribution c, int projectID)
        {
            using (var db = new PortfolioUnleashedContext())
            {
                if (c.Title != null) db.Projects.Include("Contributions").FirstOrDefault(u => u.Id == projectID).Contributions.FirstOrDefault(cont => cont.UserId == c.UserId && cont.ProjectId == c.ProjectId).Title = c.Title;
                if (c.Description != null) db.Projects.Include("Contributions").FirstOrDefault(u => u.Id == projectID).Contributions.FirstOrDefault(cont => cont.UserId == c.UserId && cont.ProjectId == c.ProjectId).Description = c.Description;
                db.SaveChanges();
            }
        }

        public Contribution retrieveContribution(int userID, int projectID)
        {
            using (var db = new PortfolioUnleashedContext())
            {
                return db.Contributions.Include("Project").Include("User").Include("ContributionMedia").FirstOrDefault(cont => (cont.UserId == userID && cont.ProjectId == projectID));
            }
        }

        public List<Contribution> retrieveAllContributions()
        {
            List<Contribution> result = new List<Contribution>();
            using (var db = new PortfolioUnleashedContext())
            {
                result = db.Contributions.Include("Project").Include("User").Include("ContributionMedia").ToList();
            }
            return result;
        }

        #endregion

        #region EducationDAL

        public void addEducation(Education e, int userID)
        {
            using (var db = new PortfolioUnleashedContext())
            {
                db.Users.Include("ContactInfoes").Include("Educations").Include("Links").Include("Portfolios").Include("QuickReferences").FirstOrDefault(user => user.Id == userID).Educations.Add(e);
                db.SaveChanges();
            }
        }

        public void deleteEducation(Education e, int userID)
        {
            using (var db = new PortfolioUnleashedContext())
            {
                db.Users.Include("ContactInfoes").Include("Educations").Include("Links").Include("Portfolios").Include("QuickReferences").FirstOrDefault(user => user.Id == userID).Educations.Remove(e);
                db.SaveChanges();
            }
        }

        public void updateEducation(Education e, int userID)
        {
            using (var db = new PortfolioUnleashedContext())
            {
                if (e.School != null) db.Users.Include("Educations").FirstOrDefault(u => u.Id == userID).Educations.FirstOrDefault(edu => edu.Id == e.Id).School = e.School;
                if (e.Degree != null) db.Users.Include("Educations").FirstOrDefault(u => u.Id == userID).Educations.FirstOrDefault(edu => edu.Id == e.Id).Degree = e.Degree;
                if (e.StartYear != null) db.Users.Include("Educations").FirstOrDefault(u => u.Id == userID).Educations.FirstOrDefault(edu => edu.Id == e.Id).StartYear = e.StartYear;
                if (e.EndYear != null) db.Users.Include("Educations").FirstOrDefault(u => u.Id == userID).Educations.FirstOrDefault(edu => edu.Id == e.Id).EndYear = e.EndYear;
                db.SaveChanges();
            }
        }

        public Education retrieveEducation(int educationID)
        {
            using (var db = new PortfolioUnleashedContext())
            {
                return db.Educations.Include("User").FirstOrDefault(cont => cont.Id == educationID);
            }
        }

        public List<Education> retrieveAllEducations()
        {
            List<Education> result = new List<Education>();
            using (var db = new PortfolioUnleashedContext())
            {
                result = db.Educations.Include("User").ToList();
            }
            return result;
        }

        #endregion

        #region LinkDAL

        public void addLink(Link l, int userID)
        {
            using (var db = new PortfolioUnleashedContext())
            {
                db.Users.Include("ContactInfoes").Include("Educations").Include("Links").Include("Portfolios").Include("QuickReferences").FirstOrDefault(user => user.Id == userID).Links.Add(l);
                db.SaveChanges();
            }
        }

        public void deleteLink(Link l, int userID)
        {
            using (var db = new PortfolioUnleashedContext())
            {
                db.Users.Include("ContactInfoes").Include("Educations").Include("Links").Include("Portfolios").Include("QuickReferences").FirstOrDefault(user => user.Id == userID).Links.Remove(l);
                db.SaveChanges();
            }
        }

        public void updateLink(Link l, int userID)
        {
            using (var db = new PortfolioUnleashedContext())
            {
                if (l.DisplayText != null) db.Users.Include("Links").FirstOrDefault(u => u.Id == userID).Links.FirstOrDefault(link => link.Id == l.Id).DisplayText = l.DisplayText;
                if (l.URL != null) db.Users.Include("Links").FirstOrDefault(u => u.Id == userID).Links.FirstOrDefault(link => link.Id == l.Id).URL = l.URL;
                db.SaveChanges();
            }
        }

        public Link retrieveLink(int linkID)
        {
            using (var db = new PortfolioUnleashedContext())
            {
                return db.Links.Include("User").FirstOrDefault(link => link.Id == linkID);
            }
        }

        public List<Link> retrieveAllLinks()
        {
            List<Link> result = new List<Link>();
            using (var db = new PortfolioUnleashedContext())
            {
                result = db.Links.Include("User").ToList();
            }
            return result;
        }

        #endregion

        #region ProjectMediumDAL

        public void addProjectMedium(ProjectMedium m, int projectId)
        {
            using (var db = new PortfolioUnleashedContext())
            {
                m.ProjectId = projectId;//in case they didn't set this (not auto set by db)
                db.Projects.Include("ProjectMedia").Where(p => p.Id == projectId).FirstOrDefault().ProjectMedia.Add(m);
                db.SaveChanges();
            }
        }

        public void removeProjectMedium(ProjectMedium m)
        {
            using (var db = new PortfolioUnleashedContext())
            {
                if (db.Projects.Include("ProjectMedia").Any(p => p.ProjectMedia.Any(me => me.Id == m.Id)))//if any project has this project medium
                {
                    foreach (Project p in db.Projects.Include("ProjectMedia").Where(p => p.ProjectMedia.Any(me => me.Id == m.Id)))//for each project that has this project medium
                    {
                        db.Projects.Include("ProjectMedia").FirstOrDefault(pr => pr.Id == p.Id).ProjectMedia.Remove(p.ProjectMedia.Where(me => me.Id == m.Id).FirstOrDefault());//remove that medium
                    }
                }
                db.ProjectMediums.Remove(db.ProjectMediums.Where(me => me.Id == m.Id).FirstOrDefault());//remove the project medium from table
                db.SaveChanges();
            }
        }

        public void updateProjectMedium(ProjectMedium m)
        {
            using (var db = new PortfolioUnleashedContext())
            {
                if (m.Link != null) db.ProjectMediums.FirstOrDefault(med => med.Id == m.Id).Link = m.Link;
                if (m.MediumType != db.ProjectMediums.FirstOrDefault(med => med.Id == m.Id).MediumType) db.ProjectMediums.FirstOrDefault(med => med.Id == m.Id).MediumType = m.MediumType;
                if (m.Caption != null) db.ProjectMediums.FirstOrDefault(med => med.Id == m.Id).Caption = m.Caption;
                db.SaveChanges();
            }
        }

        public ProjectMedium retrieveProjectMedium(int mediumID)
        {
            using (var db = new PortfolioUnleashedContext())
            {
                return db.ProjectMediums.FirstOrDefault(med => med.Id == mediumID);
            }
        }

        public List<ProjectMedium> retrieveAllProjectMediums()
        {
            List<ProjectMedium> result = new List<ProjectMedium>();
            using (var db = new PortfolioUnleashedContext())
            {
                result = db.ProjectMediums.ToList();
            }
            return result;
        }

        #endregion

        #region ContributionMediumDAL

        public void removeContributionMedium(ContributionMedium m)
        {
            using (var db = new PortfolioUnleashedContext())
            {
                if (db.Contributions.Include("ContributionMedia").Any(c => c.ContributionMedia.Any(me => me.Id == m.Id)))//if any contribution has this contribution medium
                {
                    foreach (Contribution c in db.Contributions.Include("ContributionMedia").Where(c => c.ContributionMedia.Any(me => me.Id == m.Id)))//for each contribution that has this contribution medium
                    {
                        db.Contributions.Include("ContributionMedia").FirstOrDefault(co => co.UserId == c.UserId).ContributionMedia.Remove(c.ContributionMedia.Where(me => me.Id == m.Id).FirstOrDefault());//remove that medium
                    }
                }
                db.ContributionMediums.Remove(db.ContributionMediums.Where(me => me.Id == m.Id).FirstOrDefault());//remove the contribution medium from table
                db.SaveChanges();
            }
        }

        public void addContributionMedium(ContributionMedium m, int userId, int projectId)
        {
            using (var db = new PortfolioUnleashedContext())
            {
                m.UserId = userId;//in case they didn't set this (not auto set by db)
                m.ProjectId = projectId;//in case they didn't set this (not auto set by db)
                db.Contributions.Include("ContributionMedia").Include("Project").Include("User").FirstOrDefault(c => c.UserId == userId && c.ProjectId == projectId).ContributionMedia.Add(m);
                db.SaveChanges();
            }
        }

        public void updateContributionMedium(ContributionMedium m)
        {
            using (var db = new PortfolioUnleashedContext())
            {
                if (m.Link != null) db.ContributionMediums.FirstOrDefault(med => med.Id == m.Id).Link = m.Link;
                if (m.MediumType != db.ContributionMediums.FirstOrDefault(med => med.Id == m.Id).MediumType) db.ProjectMediums.FirstOrDefault(med => med.Id == m.Id).MediumType = m.MediumType;
                if (m.Caption != null) db.ContributionMediums.FirstOrDefault(med => med.Id == m.Id).Caption = m.Caption;
                db.SaveChanges();
            }
        }

        public ContributionMedium retrieveContributionMedium(int mediumID)
        {
            using (var db = new PortfolioUnleashedContext())
            {
                return db.ContributionMediums.Include("User").Include("Projects").FirstOrDefault(med => med.Id == mediumID);
            }
        }

        public List<ContributionMedium> retrieveAllContributionMediums()
        {
            List<ContributionMedium> result = new List<ContributionMedium>();
            using (var db = new PortfolioUnleashedContext())
            {
                result = db.ContributionMediums.Include("User").Include("Contributions").Include("Projects").ToList();
            }
            return result;
        }

        #endregion

        #region NotificationDAL

        public void addNotification(Notification n, int userID)
        {
            using (var db = new PortfolioUnleashedContext())
            {
                db.Users.Include("Notifications").FirstOrDefault(user => user.Id == userID).Notifications.Add(n);

                db.SaveChanges();
            }
        }

        public List<int> retrieveAllUserIds()
        {
            List<int> allUserIds;
            using (var db = new PortfolioUnleashedContext())
            {
                allUserIds = db.Users.Select(u => u.Id).ToList();
            }
            return allUserIds;
        }

        public void deleteNotification(int notificationId)
        {
            using (var db = new PortfolioUnleashedContext())
            {
                var deletedNotifications = db.Notifications.Where(n => n.Id == notificationId);
                db.Notifications.RemoveRange(deletedNotifications);
                db.SaveChanges();
            }
        }

        public void deleteNotification(int requestOwnerId, int requestSenderId)
        {
            using (var db = new PortfolioUnleashedContext())
            {
                var deletedNotifications = db.Notifications.Where(n => n.UserId == requestOwnerId && n.SenderId == requestSenderId);
                db.Notifications.RemoveRange(deletedNotifications);
                db.SaveChanges();
            }
        }

        public void DeleteAllUserNotifications(int userId)
        {
            using (var db = new PortfolioUnleashedContext())
            {
                var deletedNotifications = db.Notifications.Where(n => n.UserId == userId);
                db.Notifications.RemoveRange(deletedNotifications);
                db.SaveChanges();
            }
        }

        public void updateNotification(Notification n, int userID)
        {
            using (var db = new PortfolioUnleashedContext())
            {

                if (n.Description != null) db.Notifications.FirstOrDefault(notif => notif.Id == n.Id).Description = n.Description;
                if (n.TimeStamp != null) db.Notifications.FirstOrDefault(notif => notif.Id == n.Id).TimeStamp = n.TimeStamp;
                if (n.Title != null) db.Notifications.FirstOrDefault(notif => notif.Id == n.Id).Title = n.Title;

                db.SaveChanges();
            }
        }

        public void MarkAllUsersNotificationsAsSeen(int userId)
        {
            using (var db = new PortfolioUnleashedContext())
            {
                var userNotifications = db.Notifications.Where(n => n.UserId == userId);
                foreach (Notification n in userNotifications)
                {
                    db.Notifications.FirstOrDefault(notif => notif.Id == n.Id).isSeen = true;
                }
                db.SaveChanges();

            }
        }

        public Notification retrieveNotification(int notificationID)
        {
            using (var db = new PortfolioUnleashedContext())
            {
                return db.Notifications.Include("User").FirstOrDefault(noti => noti.Id == notificationID);
            }
        }

        public List<Notification> retrieveAllNotifications()
        {
            List<Notification> result = new List<Notification>();
            using (var db = new PortfolioUnleashedContext())
            {
                result = db.Notifications.Include("User").ToList();
            }
            return result;
        }

        #endregion

        #region DangerousDAL
        public void deleteAllUsers()
        {
            using (var db = new PortfolioUnleashedContext())
            {
                db.Database.ExecuteSqlCommand("delete from webpages_UsersInRoles");
                db.Database.ExecuteSqlCommand("delete from webpages_Membership");
                db.Database.ExecuteSqlCommand("delete from QuickReference");
                db.Database.ExecuteSqlCommand("delete from ProjectPermission");
                db.Database.ExecuteSqlCommand("delete from Portfolio");
                db.Database.ExecuteSqlCommand("delete from Notification");
                db.Database.ExecuteSqlCommand("delete from ContributionMedium");
                db.Database.ExecuteSqlCommand("delete from Link");
                db.Database.ExecuteSqlCommand("delete from Education");
                db.Database.ExecuteSqlCommand("delete from Contribution");
                db.Database.ExecuteSqlCommand("delete from ContactInfo");
                db.Database.ExecuteSqlCommand("delete from [User]");
            }
        }

        public void deleteAllData()
        {
            using (var db = new PortfolioUnleashedContext())
            {
                db.Database.ExecuteSqlCommand("delete from webpages_UsersInRoles");
                db.Database.ExecuteSqlCommand("delete from webpages_Membership");
                db.Database.ExecuteSqlCommand("delete from QuickReference");
                db.Database.ExecuteSqlCommand("delete from ProjectPermission");
                db.Database.ExecuteSqlCommand("delete from Portfolio");
                db.Database.ExecuteSqlCommand("delete from Notification");
                db.Database.ExecuteSqlCommand("delete from ContributionMedium");
                db.Database.ExecuteSqlCommand("delete from Link");
                db.Database.ExecuteSqlCommand("delete from Education");
                db.Database.ExecuteSqlCommand("delete from Contribution");
                db.Database.ExecuteSqlCommand("delete from ContactInfo");
                db.Database.ExecuteSqlCommand("delete from [User]");


                db.Database.ExecuteSqlCommand("delete from Project_Portfolio");
                db.Database.ExecuteSqlCommand("delete from ProjectMedium");
                db.Database.ExecuteSqlCommand("delete from Project");
                db.Database.ExecuteSqlCommand("delete from webpages_OAuthMembership");
                db.Database.ExecuteSqlCommand("delete from webpages_Roles");
            }
        }
        #endregion

        #region UserSettingsDAL
        public void addUserSettings(UserSetting n)
        {
            using (var db = new PortfolioUnleashedContext())
            {
                db.Users.FirstOrDefault(user => user.Id == n.UserId).UserSetting = n;
                db.SaveChanges();
            }
        }

        public void deleteUserSettings(UserSetting n, int userID)
        {
            using (var db = new PortfolioUnleashedContext())
            {
                db.Users.Include("ContactInfoes").Include("Educations").Include("Links").Include("Portfolios").Include("QuickReferences").FirstOrDefault(user => user.Id == userID).UserSettingId = null;
                db.UserSettings.Remove(n);
                db.SaveChanges();
            }
        }

        public void updateUserSettings(UserSetting n, int userID)
        {
            using (var db = new PortfolioUnleashedContext())
            {
                db.UserSettings.FirstOrDefault(set => set.UserId == userID).IsEmailedForAdminMessage = n.IsEmailedForAdminMessage;
                db.UserSettings.FirstOrDefault(set => set.UserId == userID).IsEmailedForCollaborationAddition = n.IsEmailedForCollaborationAddition;
                db.UserSettings.FirstOrDefault(set => set.UserId == userID).IsEmailedForCollaborationRemoval = n.IsEmailedForCollaborationRemoval;
                db.UserSettings.FirstOrDefault(set => set.UserId == userID).IsEmailedForCollaborationRequest = n.IsEmailedForCollaborationRequest;
                db.UserSettings.FirstOrDefault(set => set.UserId == userID).IsEmailedForViewReport = n.IsEmailedForViewReport;
                db.UserSettings.FirstOrDefault(set => set.UserId == userID).IsPublic = n.IsPublic;
                db.SaveChanges();
            }
        }

        public UserSetting retrieveUserSettings(int userId)
        {
            using (var db = new PortfolioUnleashedContext())
            {
                //var testy = db.UserSettings.Include("User").FirstOrDefault(set => set.UserId == userId);
                var test2 = db.UserSettings.FirstOrDefault(set => set.UserId == userId);
                return test2;
            }
        }
        #endregion
    }
}