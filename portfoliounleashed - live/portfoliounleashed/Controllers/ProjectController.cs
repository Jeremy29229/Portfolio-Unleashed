using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;
using PortfolioUnleashed.Models;
using PortfolioUnleashed.Models.ViewModels;
using PortfolioUnleashed.Enums;
using PortfolioUnleashed.Data_Layer;

namespace PortfolioUnleashed.Controllers
{
    public class ProjectController : Controller
    {
        //
        // GET: /Project/

        public static DatabaseDAL db = new DatabaseDAL();

        [Authorize(Roles = "Admin, User")]
        public ActionResult FullProject(int id = -1)
        {
            if (RouteData.Values["id"] != null)
            {
                if (int.TryParse(RouteData.Values["id"].ToString(), out id)) { }
            }
            Project p = new Project();
            if (id != -1)
            {
                p = db.retrieveProject(id);
                if (p == null)
                {
                    string error1 = "The Portfolio you tried to view either does not exist or could not be found.";
                    string error2 = "Portfolio Id: " + id;
                    TempData["ErrorMessages"] = new string[] { error1, error2 };
                    return RedirectToAction("Http404", "Error");
                }
            }
            VMFullProject project = new VMFullProject(p);
            return View(model: project);
        }

        [Authorize(Roles = "Admin, User")]
        public ActionResult ProjectCatalog(int id=-1)
        {
            if (RouteData.Values["id"] != null)
            {
                if (int.TryParse(RouteData.Values["id"].ToString(), out id)) { }
            }
            if (id == -1)
            {
                id = WebSecurity.CurrentUserId;
            }
            PortfolioUnleashed.User user = db.retrieveUser(id);
            if (user == null)
            {
                string error1 = "The Project Catalog you tried to view either does not exist or could not be found.";
                TempData["ErrorMessages"] = new string[] { error1 };
                return RedirectToAction("Http404", "Error");
            }

            List<Project> projects = db.retrieveAllProjectsByUserID(id);
            List<VMProject> vmprojects = new List<VMProject>();
            if (projects != null)
            {
                foreach (Project p in projects)
                {
                    vmprojects.Add(new VMProject(p, id));
                }
            }
            return View(model: vmprojects);
        }

        [Authorize(Roles = "Admin, User")]
        public ActionResult RemoveFromProjectCatalog(int userId=-1, int projectId=-1)
        {
            #region route data validation
            if (RouteData.Values["projectId"] != null)
            {
                if (int.TryParse(RouteData.Values["projectId"].ToString(), out projectId)) { }
            }
            Project project;
            project = db.retrieveProject(projectId);
            if (project == null)
            {
                string error1 = "The Project you tried to remove from you Project Catalog does not exist or could not be found.";
                string error2 = "If the project Id is -1, then a project Id was not entered.";
                string error3 = "Project Id: " + projectId;
                TempData["ErrorMessages"] = new string[] { error1, error2, error3 };
                return RedirectToAction("Http404", "Error");
            }
            if (RouteData.Values["userId"] != null)
            {
                if (int.TryParse(RouteData.Values["userId"].ToString(), out userId)) { }
            }
            PortfolioUnleashed.User user;
            user = db.retrieveUser(userId);
            if (user == null)
            {
                string error1 = "The User who tried to remove themselves from the project either does not exist or could not be found.";
                string error2 = "User Id: " + userId;
                TempData["ErrorMessages"] = new string[] { error1, error2 };
                return RedirectToAction("Http404", "Error");
            }
            if (!project.Contributions.Any(c => c.UserId == userId))
            {
                string error1 = "You are not a contributor to this project, so it is not a part of your Project Catalog.";
                string error2 = "User: " + user.FirstName + " " + user.LastName;
                string error3 = "Project: " + project.Title;
                TempData["ErrorMessages"] = new string[] { error1, error2, error3 };
                return RedirectToAction("General", "Error");
            }
            #endregion

            db.deleteContribution(new Contribution() { UserId = userId, ProjectId = projectId }, projectId);
            return RedirectToAction("ProjectCatalog", new {id = userId });
        }

        [Authorize(Roles = "Admin, User")]
        public ActionResult AddSelfAsCollaborator(int projectId=-1, int userId=-1)
        {
            #region route data validation
            if (RouteData.Values["projectId"] != null)
            {
                if (int.TryParse(RouteData.Values["projectId"].ToString(), out projectId)) { }
            }
            Project project;
            project = db.retrieveProject(projectId);
            if (project == null)
            {
                string error1 = "The Project you tried to add yourself to as a Collaborator either does not exist or could not be found.";
                string error2 = "If the project Id is -1, then a project Id was not entered.";
                string error3 = "Project Id: " + projectId;
                TempData["ErrorMessages"] = new string[] { error1, error2, error3 };
                return RedirectToAction("Http404", "Error");
            }
            if (RouteData.Values["userId"] != null)
            {
                if (int.TryParse(RouteData.Values["userId"].ToString(), out userId)) { }
            }
            PortfolioUnleashed.User user;
            user = db.retrieveUser(userId);
            if (user == null)
            {
                string error1 = "The User you tried to add as a collaborator either does not exist or could not be found.";
                string error2 = "User Id: " + userId;
                TempData["ErrorMessages"] = new string[] { error1, error2 };
                return RedirectToAction("Http404", "Error");
            }
            if(project.Contributions.Any(c=> c.UserId == userId))
            {
                string error1 = "You are already a contributor to this project.";
                string error2 = "User: " + user.FirstName + " " + user.LastName;
                string error3 = "Project: " + project.Title;
                TempData["ErrorMessages"] = new string[] { error1, error2, error3};
                return RedirectToAction("General", "Error");
            }
            if (project.CollaborationSetting != (int)ContributionSetting.Open)
            {
                string error1 = "This Project does not allow users to add themselves as a Collaborator.";
                string error2 = "Either request to be added, or contact a Project Editor.";
                string error3 = "Project Collaboration Setting: " + (ContributionSetting)project.CollaborationSetting;
                TempData["ErrorMessages"] = new string[] { error1, error2, error3 };
                return RedirectToAction("General", "Error");
            }
            #endregion

            db.addContribution(new Contribution() { UserId = userId, ProjectId = projectId }, projectId);
            new NotificationController().CollaborationAddition(userId, projectId);
            return RedirectToAction("ProjectCatalog", routeValues: new {id=userId });
        }

        [Authorize(Roles = "Admin, User")]
        public ActionResult RequestAddAsCollaborator(int projectId = -1, int userId = -1)
        {
            #region route data validation
            if (RouteData.Values["projectId"] != null)
            {
                if (int.TryParse(RouteData.Values["projectId"].ToString(), out projectId)) { }
            }
            Project project;
            project = db.retrieveProject(projectId);
            if (project == null)
            {
                string error1 = "The Project you to which you requested to be added as a Collaborator either does not exist or could not be found.";
                string error2 = "If the project Id is -1, then a project Id was not entered.";
                string error3 = "Project Id: " + projectId;
                TempData["ErrorMessages"] = new string[] { error1, error2, error3 };
                return RedirectToAction("Http404", "Error");
            }
            if (RouteData.Values["userId"] != null)
            {
                if (int.TryParse(RouteData.Values["userId"].ToString(), out userId)) { }
            }
            PortfolioUnleashed.User user;
            user = db.retrieveUser(userId);
            if (user == null)
            {
                string error1 = "The User who tried to make the request either does not exist or could not be found.";
                string error2 = "User Id: " + userId;
                TempData["ErrorMessages"] = new string[] { error1, error2 };
                return RedirectToAction("Http404", "Error");
            }
            if (project.Contributions.Any(c => c.UserId == userId))
            {
                string error1 = "You are already a contributor to this project.";
                string error2 = "User: " + user.FirstName + " " + user.LastName;
                string error3 = "Project: " + project.Title;
                TempData["ErrorMessages"] = new string[] { error1, error2, error3 };
                return RedirectToAction("General", "Error");
            }
            if (project.CollaborationSetting != (int)ContributionSetting.RequestOnly)
            {
                string error1 = "This Project does not allow users to request to be added as a collaborator.";
                string error2 = "If you would like to be added as a collaborator, contact a Project Editor.";
                string error3 = "Project Collaboration Setting: " + (ContributionSetting)project.CollaborationSetting;
                TempData["ErrorMessages"] = new string[] { error1, error2, error3 };
                return RedirectToAction("General", "Error");
            }
            #endregion

            new NotificationController().CollaborationRequest(userId, projectId);
            string message2 = "You will be notified if/when a Project Editor confirms your request.";
            VMGeneralMessage m = new VMGeneralMessage("Request Submitted", "Your Request to be added as a collaborator has been committed.", new string[] { message2 });
            return View("GeneralMessage", model: m);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet]
        public ActionResult EditContribution(int projectId, int userId)
        {
            #region route data validation
            if (RouteData.Values["projectId"] != null)
            {
                if (int.TryParse(RouteData.Values["projectId"].ToString(), out projectId)) { }
            }
            Project project;
            if (projectId != -1)
            {
                project = db.retrieveProject(projectId);
                if (project == null)
                {
                    string error1 = "The Project, the contribution to which you tried to edit, either does not exist or could not be found.";
                    string error2 = "Project Id: " + projectId;
                    TempData["ErrorMessages"] = new string[] { error1, error2 };
                    return RedirectToAction("Http404", "Error");
                }
            }
            else
            {
                string error1 = "A project Id is required to edit a project contribution.";
                TempData["ErrorMessages"] = new string[] { error1 };
                return RedirectToAction("Http404", "Error");
            }
            if (RouteData.Values["userId"] != null)
            {
                if (int.TryParse(RouteData.Values["userId"].ToString(), out userId)) { }
            }
            PortfolioUnleashed.User user;
            if (userId != -1)
            {
                user = db.retrieveUser(userId);
                if (user == null)
                {
                    string error1 = "The User whose project contribution you tried to edit either does not exist or could not be found.";
                    string error2 = "User Id: " + userId;
                    TempData["ErrorMessages"] = new string[] { error1, error2 };
                    return RedirectToAction("Http404", "Error");
                }
            }
            else
            {
                string error1 = "A User Id is required to edit a User's contribution.";
                TempData["ErrorMessages"] = new string[] { error1 };
                return RedirectToAction("Http404", "Error");
            }
            #endregion
            VMProject vmProj = new VMProject(project, userId);
            VMContribution vmCon = new VMContribution();
            if (vmProj.PersonalContribution != null)
            {
                vmCon = vmProj.PersonalContribution;
            }
            else
            {
                vmCon = new VMContribution(new Contribution() { UserId = userId, ProjectId = projectId });
            }
            ViewBag.ProjectTitle = vmProj.Title;
            return View(model: vmCon);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpPost]
        public ActionResult EditContribution(VMContribution model)
        {
            int projectId = int.Parse(Request.Form["ProjectId"]);
            int userId = int.Parse(Request.Form["UserId"]);

            Contribution contribution = new Contribution();
            if (PersonalContributionVerified(out contribution, userId))
            {
                contribution.UserId = userId;
                contribution.ProjectId = projectId;
                if (db.retrieveProject(projectId).Contributions.Any(c => c.UserId == userId && c.ProjectId == projectId))
                {
                    db.updateContribution(contribution, projectId);
                    if (contribution.ContributionMedia != null && contribution.ContributionMedia.Count > 0)
                    {
                        foreach (ContributionMedium cm in contribution.ContributionMedia)
                        {
                            if (db.retrieveContribution(userId, projectId).ContributionMedia.Any(m => m.Id == cm.Id))
                            {
                                if (string.IsNullOrEmpty(cm.Caption) && string.IsNullOrEmpty(cm.Link))
                                {
                                    db.removeContributionMedium(cm);
                                }
                                else
                                {
                                    db.updateContributionMedium(cm);
                                }
                            }
                            else
                            {
                                db.addContributionMedium(cm, userId, projectId);
                            }
                        }
                    }
                }
                else
                {
                    db.addContribution(contribution, projectId);
                }

                return RedirectToAction("ProjectCatalog", routeValues: new { id = userId });
            }
            contribution.UserId = userId;
            contribution.ProjectId = projectId;
            VMContribution contrib = new VMContribution(contribution);
            return View(contrib);
        }

        #region managing project collaborators
        [Authorize(Roles = "Admin, User")]
        public ActionResult CollaboratorManagement(int id = -1)
        {
            #region verifying project id & project editing permissions
            if (RouteData.Values["id"] != null)
            {
                if (int.TryParse(RouteData.Values["id"].ToString(), out id)) { }
            }
            Project project;
            if (id != -1)
            {
                project = db.retrieveProject(id);
                if (project == null)
                {
                    string error1 = "The Project whose collaborators you tried to manage either does not exist or could not be found.";
                    string error2 = "Project Id: " + id;
                    TempData["ErrorMessages"] = new string[] { error1, error2 };
                    return RedirectToAction("Http404", "Error");
                }
            }
            else
            {
                string error1 = "A project Id is required to manage a project's collaborators.";
                TempData["ErrorMessages"] = new string[] { error1 };
                return RedirectToAction("Http404", "Error");
            }
            VMFullProject vmProj = new VMFullProject(project);
            if (!vmProj.ProjectEditorIds.Any(editorId => editorId == WebSecurity.CurrentUserId) && !User.IsInRole("Admin"))//If user trying to edit project is not a project editor or admin
            {
                string error1 = "You do not have permission to edit this project.";
                string error2 = "To edit your contribution to the project, please do so from your project catalog via the edit button.";
                string error3 = "If you are not registered as a contributor to this project, either add/request to add yourself as one, or contact a project editor for that project.";
                string error4 = "If you are the original creator of this project or otherwise believe that you have reached this page in error, please see the Support page to submit a report.";
                string error5 = "For mor information on Projects, Project Permissions, Project Editors, and Project Collaborators, please see the FAQ or Glossary page.";
                TempData["ErrorMessages"] = new string[] { error1, error2, error3, error4, error5 };
                return RedirectToAction("General", "Error");
            }
            #endregion
            if (TempData["AddCollabError"] != null)
            {
                ViewBag.AddCollabError = TempData["AddCollabError"];
                TempData["AddCollabError"] = null;
            }
            return View(model: vmProj);
        }
        public ActionResult AddProjectMaster(int userId, int projectId)
        {
            #region verifying route data
            if (RouteData.Values["userId"] != null)
            {
                if (int.TryParse(RouteData.Values["userId"].ToString(), out userId)) { }
            }
            PortfolioUnleashed.User user = db.retrieveUser(userId);
            if (user == null)
            {
                string error1 = "The User you tried to add as a Project Master either does not exist or could not be found.";
                string error2 = "User Id: " + userId;
                TempData["ErrorMessages"] = new string[] { error1, error2 };
                return RedirectToAction("General", "Error");
            }
            if (RouteData.Values["projectId"] != null)
            {
                if (int.TryParse(RouteData.Values["projectId"].ToString(), out projectId)) { }
            }
            Project proj = db.retrieveProject(projectId);
            if (proj == null)
            {
                string error1 = "The Project you tried to add a Project Master to either does not exist or could not be found.";
                string error2 = "Project Id: " + projectId;
                TempData["ErrorMessages"] = new string[] { error1, error2 };
                return RedirectToAction("General", "Error");
            }
            #endregion
            new NotificationController().ProjectEditorAddition(WebSecurity.CurrentUserId, userId, projectId);
            if (proj.ProjectPermissions.Any(pp => pp.UserId == userId))
            {
                ProjectPermission perm = proj.ProjectPermissions.FirstOrDefault(pp => pp.UserId == userId);
                perm.IsProjectEditor = true;
                perm.IsProjectMaster = true;
                db.updatePermission(perm);
            }
            else
            {
                db.addPermissionToProject(new ProjectPermission() { ProjectId = projectId, IsProjectEditor = true, IsProjectMaster = true, UserId = userId }, projectId);
            }
            return RedirectToAction("CollaboratorManagement", routeValues: new { id = projectId });
        }
        public ActionResult AddProjectEditor(int userId, int projectId)
        {
            #region verifying route data
            if (RouteData.Values["userId"] != null)
            {
                if (int.TryParse(RouteData.Values["userId"].ToString(), out userId)) { }
            }
            PortfolioUnleashed.User user = db.retrieveUser(userId);
            if (user == null)
            {
                string error1 = "The User you tried to add as a Project Editor either does not exist or could not be found.";
                string error2 = "User Id: " + userId;
                TempData["ErrorMessages"] = new string[] { error1, error2 };
                return RedirectToAction("General", "Error");
            }
            if (RouteData.Values["projectId"] != null)
            {
                if (int.TryParse(RouteData.Values["projectId"].ToString(), out projectId)) { }
            }
            Project proj = db.retrieveProject(projectId);
            if (proj == null)
            {
                string error1 = "The Project you tried to add a Project Editor to either does not exist or could not be found.";
                string error2 = "Project Id: " + projectId;
                TempData["ErrorMessages"] = new string[] { error1, error2 };
                return RedirectToAction("General", "Error");
            }
            #endregion
            new NotificationController().ProjectEditorAddition(WebSecurity.CurrentUserId, userId, projectId);
            if (proj.ProjectPermissions.Any(pp => pp.UserId == userId))
            {
                ProjectPermission perm = proj.ProjectPermissions.FirstOrDefault(pp => pp.UserId == userId);
                perm.IsProjectEditor = true;
                db.updatePermission(perm);
            }
            else
            {
                db.addPermissionToProject(new ProjectPermission() { ProjectId = projectId, IsProjectEditor = true, IsProjectMaster = false, UserId = userId }, projectId);
            }
            return RedirectToAction("CollaboratorManagement", routeValues: new {id=projectId});
        }
        public ActionResult RemoveProjectEditor(int userId, int projectId)
        {
            #region verifying route data
            if (RouteData.Values["userId"] != null)
            {
                if (int.TryParse(RouteData.Values["userId"].ToString(), out userId)) { }
            }
            PortfolioUnleashed.User user = db.retrieveUser(userId);
            if (user == null)
            {
                string error1 = "The User you tried to remove as a Project Editor either does not exist or could not be found.";
                string error2 = "User Id: " + userId;
                TempData["ErrorMessages"] = new string[] { error1, error2 };
                return RedirectToAction("General", "Error");
            }
            if (RouteData.Values["projectId"] != null)
            {
                if (int.TryParse(RouteData.Values["projectId"].ToString(), out projectId)) { }
            }
            Project proj = db.retrieveProject(projectId);
            if (proj == null)
            {
                string error1 = "The Project you tried to remove a Project Editor from either does not exist or could not be found.";
                string error2 = "Project Id: " + projectId;
                TempData["ErrorMessages"] = new string[] { error1, error2 };
                return RedirectToAction("General", "Error");
            }
            #endregion

            ProjectPermission perm = proj.ProjectPermissions.FirstOrDefault(pp => pp.UserId == userId);
            perm.IsProjectEditor = false;
            db.updatePermission(perm);
            new NotificationController().ProjectEditorRemoval(WebSecurity.CurrentUserId, userId, projectId);
            return RedirectToAction("CollaboratorManagement", routeValues: new { id = projectId });
        }
        public ActionResult RemoveCollaborator(int userId, int projectId)
        {
            #region verifying route data
            if (RouteData.Values["userId"] != null)
            {
                if (int.TryParse(RouteData.Values["userId"].ToString(), out userId)) { }
            }
            PortfolioUnleashed.User user = db.retrieveUser(userId);
            if (user == null)
            {
                string error1 = "The User you tried to remove as a Collaborator either does not exist or could not be found.";
                string error2 = "User Id: " + userId;
                TempData["ErrorMessages"] = new string[] { error1, error2 };
                return RedirectToAction("General", "Error");
            }
            if (RouteData.Values["projectId"] != null)
            {
                if (int.TryParse(RouteData.Values["projectId"].ToString(), out projectId)) { }
            }
            Project proj = db.retrieveProject(projectId);
            if (proj == null)
            {
                string error1 = "The Project you tried to remove a Collaborator from either does not exist or could not be found.";
                string error2 = "Project Id: " + projectId;
                TempData["ErrorMessages"] = new string[] { error1, error2 };
                return RedirectToAction("General", "Error");
            }
            #endregion
            db.deleteContribution(proj.Contributions.Where(c => c.UserId == userId).FirstOrDefault(), projectId);
            new NotificationController().CollaborationRemoval(WebSecurity.CurrentUserId, userId, projectId);
            return RedirectToAction("CollaboratorManagement", routeValues: new { id = projectId });
        }
        public ActionResult AddCollaborator(int projectId)
        {
            string email = Request.Form["NewCollaboratorEmail"];
            if (string.IsNullOrEmpty(email))
            {
                TempData["AddCollabError"] =  "You must enter an email to add a user as a Collaborator";
            }
            else
            {
                PortfolioUnleashed.User user = db.retrieveUserByEmail(email);
                if (user == null)
                {
                    TempData["AddCollabError"] = "The email you entered was not a valid email, or no existing User is registered with that email.";
                }
                else
                {
                    if (RouteData.Values["projectId"] != null)
                    {
                        if (int.TryParse(RouteData.Values["projectId"].ToString(), out projectId)) { }
                    }
                    Project proj = db.retrieveProject(projectId);
                    if (proj.Contributions.Any(c => c.UserId == user.Id))
                    {
                        TempData["AddCollabError"] = "You must enter an email to add a user as a Collaborator";
                    }
                    else
                    {
                        db.addContribution(new Contribution() { UserId = user.Id, ProjectId = projectId }, projectId);
                        new NotificationController().CollaborationAddition(WebSecurity.CurrentUserId, user.Id, projectId);
                        return RedirectToAction("CollaboratorManagement", routeValues: new { id = projectId });
                    }
                }
            }
            return RedirectToAction("CollaboratorManagement", routeValues: new { id = projectId });
        }
        #endregion 

        #region Project Creation and Editing Things
        [Authorize(Roles = "Admin, User")]
        [HttpGet]
        public ActionResult ProjectCreateEdit(int id = -1)
        {
            if (RouteData.Values["id"] != null)
            {
                if (int.TryParse(RouteData.Values["id"].ToString(), out id)) { }
            }
            Project project;
            if (id != -1)
            {
                project = db.retrieveProject(id);
                if (project == null)
                {
                    string error1 = "The Project you tried to edit either does not exist or could not be found.";
                    string error2 = "Project Id: " + id;
                    TempData["ErrorMessages"] = new string[] { error1, error2 };
                    return RedirectToAction("Http404", "Error");
                }
            }
            else
            {
                project = new Project();
            }
            int currentUserId = WebSecurity.CurrentUserId;
            List<PortfolioUnleashed.User> quickContacts = new List<PortfolioUnleashed.User>();
            IEnumerable<int> quickContactIds = db.retrieveUser(currentUserId).QuickReferences.Select(c => c.QuickReferenceId);
            if (quickContactIds != null && quickContactIds.Count() > 0)
            {
                foreach (int Id in quickContactIds)
                {
                    quickContacts.Add(db.retrieveUser(Id));
                }
            }
            if (quickContacts == null)
            {
                quickContacts = new List<User>();
            }
            VMEditingProject vmEdit = new VMEditingProject(project, quickContacts, currentUserId);
            if (vmEdit.ProjectEditorIds != null && vmEdit.ProjectEditorIds.Count > 0)
            {
                if (!vmEdit.ProjectEditorIds.Any(editorId => editorId == WebSecurity.CurrentUserId) && !User.IsInRole("Admin"))//If user trying to edit project is not a project editor or admin
                {
                    string error1 = "You do not have permission to edit this project.";
                    string error2 = "To edit your contribution to the project, please do so from your project catalog via the edit button.";
                    string error3 = "If you are not registered as a contributor to this project, either add/request to add yourself as one, or contact a project editor for that project.";
                    string error4 = "If you are the original creator of this project or otherwise believe that you have reached this page in error, please see the Support page to submit a report.";
                    string error5 = "For mor information on Projects, Project Permissions, Project Editors, and Project Collaborators, please see the FAQ or Glossary page.";
                    TempData["ErrorMessages"] = new string[] { error1, error2, error3, error4, error5 };
                    return RedirectToAction("General", "Error");
                }
            }
            return View(vmEdit);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpPost]
        public ActionResult ProjectCreateEdit(VMEditingProject model)
        {
            bool isValidInput = true;

            List<ProjectMedium> projectMedia = new List<ProjectMedium>();
            if (!AllProjectMediaVerified(out projectMedia))
            {
                isValidInput = false;
            }

            List<Contribution> contributions = new List<Contribution>();
            if (!AllContributionsVerified(out contributions))
            {
                isValidInput = false;
            }

            Contribution personalContribution = new Contribution();
            if(!PersonalContributionVerified(out personalContribution, model._currentUserId))
            {
                isValidInput = false;
            }

            if (isValidInput && ModelState.IsValid)
            {
                int currentUserId = WebSecurity.CurrentUserId;
                string selectedTemplate = Request.Form.GetValues("Template").FirstOrDefault();
                int template = (int)((ProjectTemplate)Enum.Parse(typeof(ProjectTemplate), selectedTemplate));

                string selectedContributionSetting = Request.Form.GetValues("ContributionSetting").FirstOrDefault();
                int setting = (int)((ContributionSetting)Enum.Parse(typeof(ContributionSetting), selectedContributionSetting));

                int projectId = int.Parse(Request.Form.GetValues("Id").FirstOrDefault());
                Project p = db.retrieveProject(projectId);
                bool newProject = true;
                #region Creating new project properties
                if (p == null)//Creating
                {
                    p = new Project();
                    personalContribution.UserId = currentUserId;
                    p.Title = model.Title;
                    p.Description = model.Description;
                    p.Template = template;
                    p.CollaborationSetting = setting;
                    foreach (Contribution c in contributions)
                    {
                        c.UserId = db.retrieveUserByEmail(c.User.Email).Id;
                        c.User = null;
                    }
                    p.Contributions = contributions;
                    personalContribution.User = null;
                    if (!string.IsNullOrEmpty(personalContribution.Title))
                    {
                        p.Contributions.Add(personalContribution);
                    }
                    else
                    {
                        p.Contributions.Add(new Contribution(){ UserId = currentUserId});
                    }
                    p.ProjectMedia = projectMedia;
                    p.ProjectPermissions.Add(new ProjectPermission() { UserId = currentUserId, IsProjectEditor = true, IsProjectMaster = true });
                }
                #endregion
                #region Editing existing project properties
                else //editing
                {
                    newProject = false;
                    p.Title = model.Title;
                    p.Description = model.Description;
                    p.Template = template;
                    p.CollaborationSetting = setting;
                    db.updateProject(p);
                    foreach (Contribution c in contributions)
                    {
                        if (c.ProjectId != projectId)//new contribution entry
                        {
                            c.UserId = db.retrieveUserByEmail(c.User.Email).Id;
                            c.User = null;
                            c.ProjectId = projectId;
                            new NotificationController().CollaborationAddition(WebSecurity.CurrentUserId, c.UserId, projectId);
                            db.addContribution(c, projectId);
                        }
                    }
                    foreach (ProjectMedium m in projectMedia)
                    {
                        ProjectMedium dm = db.retrieveProjectMedium(m.Id);
                        if (dm == null)//medium doesn't exist in project
                        {
                            db.addProjectMedium(m, projectId);
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(m.Caption) && string.IsNullOrEmpty(m.Link))//existed, wants to remove
                            {
                                db.removeProjectMedium(dm);
                            }
                            else
                            {
                                dm.Caption = m.Caption;
                                dm.Link = m.Link;
                                dm.MediumType = m.MediumType;
                                db.updateProjectMedium(dm);
                            }
                        }
                    }
                    #region personal Contribution
                    personalContribution.ProjectId = projectId;
                    personalContribution.User = null;
                    if (!p.Contributions.Any(c=> c.UserId == currentUserId))//didn't have personal contribution
                    {
                        if (string.IsNullOrEmpty(personalContribution.Title))
                        {
                            db.addContribution(new Contribution() { UserId = currentUserId }, projectId);
                        }
                        else
                        {
                            db.addContribution(personalContribution, projectId);
                        }
                    }
                    else //had personal contribution
                    {
                        Contribution existing = db.retrieveContribution(currentUserId, projectId);
                        existing.Title = personalContribution.Title;
                        existing.Description = personalContribution.Description;
                        foreach (ContributionMedium m in personalContribution.ContributionMedia)
                        {
                            if (existing.ContributionMedia.Any(me => me.Id == m.Id))//Contribution has that medium
                            {
                                if (string.IsNullOrEmpty(m.Link))//blanked out medium
                                {
                                    existing.ContributionMedia.Remove(existing.ContributionMedia.Where(me => me.Id == m.Id).First());
                                    db.removeContributionMedium(m);
                                }
                                else
                                {
                                    ContributionMedium med = existing.ContributionMedia.Where(me => me.Id == m.Id).First();
                                    med.Caption = m.Caption;
                                    med.Link = m.Link;
                                    med.MediumType = m.MediumType;
                                    db.updateContributionMedium(med);
                                }
                            }
                            else
                            {
                                db.addContributionMedium(m, projectId, currentUserId);
                            }
                        }
                        db.updateContribution(existing, projectId);
                    }
                    #endregion
                }
                #endregion

                #region Adding quick contact collaborators
                List<string> keys = Request.Form.AllKeys.Where(k => k.Contains("AddBox")).ToList();
                if (keys != null && keys.Count > 0)
                {
                    foreach (string key in keys)
                    {
                        int quickContactId = int.Parse(key.Substring(0, key.IndexOf("AddBox")));
                        bool addAsCollab = Request.Form.GetValues(key).FirstOrDefault().Equals("true");
                        if (addAsCollab)//Box was checked
                        {
                            if (!p.Contributions.Any(c => c.UserId == quickContactId))//That contact isn't already a collaborator
                            {
                                p.Contributions.Add(new Contribution() { UserId = quickContactId });
                                if (!newProject)
                                {
                                    db.addContribution(new Contribution() { UserId = quickContactId }, projectId);
                                    new NotificationController().CollaborationAddition(WebSecurity.CurrentUserId, quickContactId, projectId);
                                }
                            }
                        }
                    }
                }
                #endregion

                if (newProject)
                {
                    db.addProject(p);
                    foreach (Contribution c in p.Contributions.Where(c=>c.UserId != WebSecurity.CurrentUserId))
                    {
                        new NotificationController().CollaborationAddition(WebSecurity.CurrentUserId, c.UserId, p.Id);
                    }
                }
                return RedirectToAction("ProjectCatalog", new { id = currentUserId });
            }
            int projId = int.Parse(Request.Form.GetValues("Id").FirstOrDefault());
            Project proj = new Project();
            proj.Id = projId;
            proj.Title = model.Title;
            proj.ProjectMedia = projectMedia;
            proj.Contributions = contributions;

            VMEditingProject vmproj = new VMEditingProject(proj, null, model._currentUserId) { EditingCollaborators = model.EditingCollaborators,PersonalContribution = new VMContribution(personalContribution)};

            return View(new VMEditingProject());
        }

        public ActionResult NewProjectMediumEntry()
        {
            ViewBag.ContributionMedia = false;
            var media = new VMProjectMedium();
            return View("Partial/FormEntry/_NewProjectMediumEntry", media);
        }
        public ActionResult NewContributionMediumEntry()
        {
            ViewBag.ContributionMedia = true;
            var media = new VMContributionMedium();
            return View("Partial/FormEntry/_NewContributionMediumEntry", media);
        }
        public ActionResult NewCollaboratorEntry()
        {
            var contribution = new VMContribution();
            return View("Partial/FormEntry/_NewCollaboratorEntry", contribution);
        }

        #region Input Verification Methods
        private bool PersonalContributionVerified(out Contribution personalContribution, int currentUserId)
        {
            bool isValidInput = true;
            personalContribution = new Contribution();

            List<ContributionMedium> contributionMedia = new List<ContributionMedium>();
            if (!AllContributionMediaVerified(out contributionMedia))
            {
                isValidInput = false;
            }
            personalContribution.ContributionMedia = contributionMedia;

            string description = Request.Form.GetValues("PersonalContribution.Description").FirstOrDefault();
            string title = Request.Form.GetValues("PersonalContribution.Title").FirstOrDefault();
            string[] entry = new string[] { description, title };
            if (!allAreNullOrEmpty(entry))
            {
                if (someAreNullOrEmpty(entry))
                {
                    isValidInput = false;
                    ModelState.AddModelError("", "If you add personal contribution details, you must enter both your Position Title and a Contribution Description.");
                }
                personalContribution.Title = title;
                personalContribution.Description = description;
                personalContribution.UserId = currentUserId;
                personalContribution.User = db.retrieveUser(currentUserId);
            }
            else //if description and title empty but they added media
            {
                if (contributionMedia.Count > 0)
                {
                    isValidInput = false;
                    ModelState.AddModelError("", "If you add media to your personal contribution for the project, you must also enter a Title and Description for that contribution.");
                }
            }

            return isValidInput;
        }

        private bool AllContributionsVerified(out List<Contribution> contributions)
        {
            bool isValidInput = true;
            contributions = new List<Contribution>();
            string[] contributionProjectIds = Request.Form.GetValues("Contribution.ProjectId");
            string[] contributionEmails = Request.Form.GetValues("Contribution.Email");
            for (int i = 0; i < contributionProjectIds.Count(); i++)
            {
                string[] entryInputs = new string[] { contributionProjectIds[i], contributionEmails[i] };
                if (!allAreNullOrEmpty(entryInputs) && !string.IsNullOrEmpty(contributionEmails[i]))
                {
                    PortfolioUnleashed.User userToAdd = db.retrieveUserByEmail(contributionEmails[i]);
                    if (userToAdd == null)
                    {
                        isValidInput = false;
                        ModelState.AddModelError("", "There is no registered User with the email address " + contributionEmails[i] + ". They must be registered with the site to be added as a collaborator.");
                    }
                    Contribution c = new Contribution();
                    c.User = new User() { Email = contributionEmails[i] };
                    c.ProjectId = int.Parse(contributionProjectIds[i]);
                    contributions.Add(c);
                }
            }
            return isValidInput;
        }

        private bool AllContributionMediaVerified(out List<ContributionMedium> media)
        {
            bool isValidInput = true;
            media = new List<ContributionMedium>();
            string[] mediaIds = Request.Form.GetValues("ContributionMedia.Id");
            string[] mediaLinks = Request.Form.GetValues("ContributionMedia.Link");
            string[] mediaMediaTypes = Request.Form.GetValues("ContributionMedia.MediaType");
            string[] mediaCaptions = Request.Form.GetValues("ContributionMedia.Caption");
            for (int i = 0; i < mediaIds.Count(); i++)
            {
                string[] entryInputs = new string[] { mediaIds[i], mediaLinks[i], mediaCaptions[i] };
                if (!allAreNullOrEmpty(entryInputs))
                {
                    entryInputs = new string[] { mediaLinks[i], mediaCaptions[i] };
                    if (someAreNullOrEmpty(entryInputs))
                    {
                        isValidInput = false;
                        ModelState.AddModelError("", "If you add a Media entry to your contribution, you must either complete all fields for that entry or leave all fields blank.");
                    }
                    else
                    {
                        int specifiedMediaType = (int)((MediaType)Enum.Parse(typeof(MediaType), mediaMediaTypes[i]));
                        isValidInput = verifyMediaTypeMatchesMedia(specifiedMediaType, mediaLinks[i]);
                    }
                    ContributionMedium medium = new ContributionMedium();
                    medium.Caption = mediaCaptions[i];
                    medium.Link = mediaLinks[i];
                    medium.Id = int.Parse(mediaIds[i]);
                    medium.MediumType = (int)((MediaType)Enum.Parse(typeof(MediaType), mediaMediaTypes[i]));
                    medium.UserId = WebSecurity.CurrentUserId;
                    media.Add(medium);
                }
            }
            return isValidInput;
        }

        private bool verifyMediaTypeMatchesMedia(int specifiedMediaType, string link)
        {
            bool mediaMatchesMediaType = true;
            if (specifiedMediaType == 0)//Check if Image
            {
                if (!(link.Contains(".jpg") ||
                    link.Contains(".gif") ||
                    link.Contains(".png") ||
                    link.Contains(".apng") ||
                    link.Contains(".svg") ||
                    link.Contains(".bmp")))
                {
                    mediaMatchesMediaType = false;
                }
            }
            else if (specifiedMediaType == 1)//Check if Video
            {
                if (!(link.Contains(".mp4") ||
                  link.Contains(".webm") ||
                  link.Contains(".ogg")))
                {
                    mediaMatchesMediaType = false;
                }
            }
            else if (specifiedMediaType == 2)//Check if YoutubeVideo
            {
                if (!(link.Contains("youtu")))
                {
                    mediaMatchesMediaType = false;
                }
            }

            else if (specifiedMediaType == 4)//Check if Sound
            {
                if (!(link.Contains(".mp3") ||
                    link.Contains(".wav") ||
                    link.Contains(".ogg") ||
                    link.Contains("soundcloud")))
                {
                    mediaMatchesMediaType = false;
                }
            }
            if (!mediaMatchesMediaType)
            {
                ModelState.AddModelError("", "Specified media type does not match the media link.");

            }
            return mediaMatchesMediaType;
        }

        private bool AllProjectMediaVerified(out List<ProjectMedium> media)
        {
            bool isValidInput = true;
            media = new List<ProjectMedium>();
            string[] mediaIds = Request.Form.GetValues("ProjectMedia.Id");
            string[] mediaLinks = Request.Form.GetValues("ProjectMedia.Link");
            string[] mediaMediaTypes = Request.Form.GetValues("ProjectMedia.MediaType");
            string[] mediaCaptions = Request.Form.GetValues("ProjectMedia.Caption");
            for (int i = 0; i < mediaIds.Count(); i++)
            {
                string[] entryInputs = new string[] { mediaIds[i], mediaLinks[i], mediaCaptions[i] };
                if (!allAreNullOrEmpty(entryInputs))
                {
                    entryInputs = new string[] { mediaLinks[i], mediaCaptions[i] };
                    if (someAreNullOrEmpty(entryInputs))
                    {
                        isValidInput = false;
                        ModelState.AddModelError("", "If you add a Media entry, you must either complete all fields for that entry or leave all fields blank.");
                    }
                    else
                    {
                        int specifiedMediaType = (int)((MediaType)Enum.Parse(typeof(MediaType), mediaMediaTypes[i]));
                        isValidInput = verifyMediaTypeMatchesMedia(specifiedMediaType, mediaLinks[i]);
                    }
                    ProjectMedium medium = new ProjectMedium();
                    medium.Caption = mediaCaptions[i];
                    medium.Link = mediaLinks[i];
                    medium.Id = int.Parse(mediaIds[i]);
                    medium.MediumType = (int)((MediaType)Enum.Parse(typeof(MediaType), mediaMediaTypes[i]));
                    media.Add(medium);
                }
            }
            return isValidInput;
        }

        private bool allAreNullOrEmpty(string[] strings)
        {
            bool allEmpty = true;
            foreach (string s in strings)
            {
                if (!string.IsNullOrEmpty(s) && s != "0")
                {
                    allEmpty = false;
                }
            }
            return allEmpty;
        }

        private bool someAreNullOrEmpty(string[] strings)
        {
            int numEmpty = 0;
            foreach (string s in strings)
            {
                if (string.IsNullOrEmpty(s))
                {
                    numEmpty++;
                }
            }
            bool noneEmpty = (numEmpty == 0);
            bool allEmpty = (numEmpty == strings.Count());
            return (!noneEmpty && !allEmpty);
        }
        #endregion
    }
        #endregion
}
