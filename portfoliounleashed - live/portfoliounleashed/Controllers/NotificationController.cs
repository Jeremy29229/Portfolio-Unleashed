using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;
using PortfolioUnleashed.Data_Layer;
using PortfolioUnleashed.Enums;
using PortfolioUnleashed.Models.ViewModels;
using PortfolioUnleashed.DataLayer;
using Postal;

namespace PortfolioUnleashed.Controllers
{
    public class NotificationController : Controller
    {
        DatabaseDAL DAL = new DatabaseDAL();

        public ActionResult Index()
        {
            return All();
        }

        public ActionResult All(string orderBy = "TimeStamp", bool isDescending = true)
        {
            if (WebSecurity.IsAuthenticated)
            {
                DAL.MarkAllUsersNotificationsAsSeen(WebSecurity.CurrentUserId);
                VMAll all = new VMAll { IsDescending = isDescending, OrderBy = orderBy };
                all.User = new VMUser(DAL.retrieveUser(WebSecurity.CurrentUserId));

                if (isDescending)
                {
                    if (orderBy == "TimeStamp")
                    {
                        all.User.Notifications = all.User.Notifications.OrderByDescending(n => n.TimeStamp).ToList();
                    }
                    else if (orderBy == "Title")
                    {
                        all.User.Notifications = all.User.Notifications.OrderByDescending(n => n.Title).ToList();
                    }
                    else if (orderBy == "Description")
                    {
                        all.User.Notifications = all.User.Notifications.OrderByDescending(n => n.Description).ToList();
                    }
                    else if (orderBy == "Sender")
                    {
                        all.User.Notifications = all.User.Notifications.OrderByDescending(n => n.Sender).ToList();
                    }
                }
                else
                {
                    if (orderBy == "TimeStamp")
                    {
                        all.User.Notifications = all.User.Notifications.OrderBy(n => n.TimeStamp).ToList();
                    }
                    else if (orderBy == "Title")
                    {
                        all.User.Notifications = all.User.Notifications.OrderBy(n => n.Title).ToList();
                    }
                    else if (orderBy == "Description")
                    {
                        all.User.Notifications = all.User.Notifications.OrderBy(n => n.Description).ToList();
                    }
                    else if (orderBy == "Sender")
                    {
                        all.User.Notifications = all.User.Notifications.OrderBy(n => n.Sender).ToList();
                    }
                }

                return View(viewName: "All", model: all);
            }
            else
            {
                return View("../User/Login");
            }
        }

        [HttpGet]
        public ActionResult LeaveFeedback(int? senderId, int? portfolioId)
        {
            if (senderId == null)
            {
                return View("../User/Login");
            }
            else if (portfolioId == null)
            {
                return View("../Home/Index");
            }
            else
            {
                VMLeaveFeedback v = new VMLeaveFeedback{
                IsSeen = false,
                Title = DAL.retrieveUser(WebSecurity.CurrentUserId).FirstName + " " + DAL.retrieveUser(WebSecurity.CurrentUserId).LastName + " has critiqued your " + DAL.retrievePortfolio((int)portfolioId).Title,
                Description = "",
                URL = "~/Portfolio/Portfolio/" + portfolioId,
                Sender = DAL.retrieveUser(WebSecurity.CurrentUserId).FirstName + " " +  DAL.retrieveUser(WebSecurity.CurrentUserId).LastName,
                TimeStamp = DateTime.UtcNow,
                UserId = DAL.retrievePortfolio((int)portfolioId).UserId,
                ProjectName = DAL.retrievePortfolio((int)portfolioId).Title,
                Receiver = DAL.retrieveUser(DAL.retrievePortfolio((int)portfolioId).UserId).FirstName + " " + DAL.retrieveUser(DAL.retrievePortfolio((int)portfolioId).UserId).LastName,
                PortfolioId = (int)portfolioId,
                SenderId = senderId
            };
                return View(model: v);
            }
        }

        [HttpPost]
        public ActionResult LeaveFeedback(VMLeaveFeedback v)
        {
            if (ModelState.IsValid)
            {
                Notification test = Translator.NotificationFromVMNotification(new VMNotification(v));
                DAL.addNotification(Translator.NotificationFromVMNotification(new VMNotification(v)), v.UserId);

                return RedirectToAction("Portfolio", "Portfolio", new { id = v.PortfolioId }); 
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult LeaveCard(int? senderId, int? receiverId)
        {
            if (senderId == null)
            {
                return View("../User/Login");
            }
            else if (receiverId == null)
            {
                return View("../Home/Index");
            }
            else
            {

                return View(model: new VMLeaveCard {
                    IsSeen = false,
                    Title = DAL.retrieveUser(WebSecurity.CurrentUserId).FirstName + " " + DAL.retrieveUser(WebSecurity.CurrentUserId).LastName +  " has left their business card ",
                    Description = "",
                    URL = "~/User/Account/" + senderId,
                    Sender = DAL.retrieveUser(WebSecurity.CurrentUserId).FirstName + " " +  DAL.retrieveUser(WebSecurity.CurrentUserId).LastName,
                    TimeStamp = DateTime.UtcNow,
                    UserId = (int)receiverId,
                    Receiver = DAL.retrieveUser((int)receiverId).FirstName + " " + DAL.retrieveUser((int)receiverId).LastName,
                    SenderId = senderId
                } );
            }
        }

        [HttpPost]
        public ActionResult LeaveCard(VMLeaveCard c)
        {
            if (ModelState.IsValid)
            {
                Notification test = Translator.NotificationFromVMNotification(new VMNotification(c));
                DAL.addNotification(Translator.NotificationFromVMNotification(new VMNotification(c)), c.UserId);
                return RedirectToAction("Account", "User", new { id = c.UserId}); 
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult TargetedNotification()
        {
            int targetId = int.Parse(RouteData.Values["id"].ToString());
            User user = DAL.retrieveUser(WebSecurity.CurrentUserId);

            VMLeaveFeedback v = new VMLeaveFeedback
            {
                IsSeen = false,
                Title = "Admin " + user.FirstName + " " + user.LastName + " sent you a message",
                Description = "",
                URL = null,
                Sender = "Admin " + user.FirstName + " " + user.LastName,
                TimeStamp = DateTime.UtcNow,
                UserId = targetId,
                ProjectName = null,
                Receiver = DAL.retrieveUser(targetId).FirstName + " " + DAL.retrieveUser(targetId).LastName,
                PortfolioId = 0,
                SenderId = user.Id,
            };
            return View(model: v);
        }

        [HttpPost]
        public ActionResult TargetedNotification(VMLeaveFeedback v)
        {
            if (ModelState.IsValid)
            {
                DAL.addNotification(Translator.NotificationFromVMNotification(new VMNotification(v)), v.UserId);
                return RedirectToAction("AllUsers", "Admin");
            }
            else
            {
                return View();
            }
        }

        [Authorize(Roles = "User")]
        public ActionResult Report(int portId, string portfolioName)
        {
            List<int> allAdminIds = DAL.retrieveAllUserIds();
            User sendingUser = DAL.retrieveUser(WebSecurity.CurrentUserId);
            foreach (int id in allAdminIds)
            {
                if (Roles.IsUserInRole(DAL.retrieveUser(id).Email, "Admin"))
                {
                    Notification report = new Notification
                    {
                        Description = "The portfolio, " + portfolioName + ", has been flagged as inappropriate.",
                        isSeen = false,
                        NotificationType = (int)NotificationType.FlaggedPortfolio,
                        Sender = sendingUser.FirstName + " " + sendingUser.LastName,
                        SenderId = WebSecurity.CurrentUserId,
                        TimeStamp = DateTime.UtcNow,
                        Title = portfolioName + " Flagged",
                        URL = "~/Portfolio/Portfolio/" + portId,
                        UserId = id
                    };
                    DAL.addNotification(report, id);
                }
            }

            return Redirect("~/Portfolio/Portfolio/"+portId); 
        }


        public ActionResult DeleteNotification(int id)
        {
            DAL.deleteNotification(id);
            return View("All", model: new VMUser(DAL.retrieveUser(WebSecurity.CurrentUserId)));
        }

        public ActionResult DeleteAllUserNotifications(int userId)
        {
            DAL.DeleteAllUserNotifications(userId);
            return View("All", model: new VMUser(DAL.retrieveUser(WebSecurity.CurrentUserId)));
        }

        public void AddProfileView(int userId)
        {
            User viewedUser = DAL.retrieveUser(userId);
            viewedUser.ViewsSinceLastReport++;
            viewedUser.TotalViews++;
            DAL.updateUserViewsData(viewedUser);
        }

        public void AddPortfolioView(int portfolioId)
        {
            User viewedUser = DAL.retrievePortfolio(portfolioId).User;
            viewedUser.ViewsSinceLastReport++;
            viewedUser.TotalViews++;
            DAL.updateUserViewsData(viewedUser);
        }

        public void DailyNotification()
        {
            if (WebSecurity.IsAuthenticated)
            {
                User currentUser = DAL.retrieveUser(WebSecurity.CurrentUserId);
                if (currentUser.LastDailyReportSendTime == null || (currentUser.LastDailyReportSendTime.Value.Date != DateTime.UtcNow.Date && currentUser.ViewsSinceLastReport > 0))
                {
                    SendDailyNotification();
                    currentUser.LastDailyReportSendTime = DateTime.UtcNow;
                    currentUser.ViewsSinceLastReport = 0;
                    DAL.updateUserViewsData(currentUser);
                }
            }
        }

        public void SendDailyNotification()
        {
            User currentUser = DAL.retrieveUser(WebSecurity.CurrentUserId);

            Notification viewReport = new Notification
            {
                Description = "",
                isSeen = false,
                NotificationType = (int)NotificationType.DailyViewReport,
                Sender = "Automated",
                SenderId = null,
                TimeStamp = DateTime.UtcNow,
                Title = "View Report " + DateTime.UtcNow.ToShortDateString(),
                URL = null,
                UserId = currentUser.Id
            };

            if (currentUser.LastDailyReportSendTime == null)
            {
                viewReport.Description = "Greetings! "
                + "The View Report is a tool provided on Portfolio Unleashed to help you keep track of how much traffic your content is getting on the site. "
                + "This report will be sent to you from time to time, but no more than once per day by default, informing you about the number of views your content has received recently and since you joined Portfolio Unleashed. "
                + "You can disable or configure this feature at any time by going to your account settings page.";
            }
            else
            {
                viewReport.Description = "Greetings! You have received " + currentUser.ViewsSinceLastReport + " views since " + currentUser.LastDailyReportSendTime.Value.ToLocalTime().ToShortDateString()
                + " and " + currentUser.TotalViews + " total views on your profile and portfolios.";
            }


            if (DAL.retrieveUserSettings(viewReport.UserId).IsEmailedForViewReport)
            {
                dynamic email = new Email("NotifcationEmail");
                email.To = DAL.retrieveUser(viewReport.UserId).Email;
                email.UserName = DAL.retrieveUser(viewReport.UserId).FirstName;
                email.notificationType = "View Report";
                email.Send();
            }
            DAL.addNotification(viewReport, currentUser.Id);
        }

        /// <summary>
        /// Sends a notification to the user who was added as a project editor by one of the project's existing project editors.
        /// </summary>
        /// <param name="projectEditersUserId">Project editor's user id who added the new collaborator</param>
        /// <param name="newCollaboratorsUserId">Used id of the user who was just added as a project editor</param>
        /// <param name="projectId">Id of the project being modified</param>
        public void ProjectEditorAddition(int projectEditersUserId, int newCollaboratorsUserId, int projectId)
        {
            Project p = DAL.retrieveProject(projectId);
            User projectEditor = DAL.retrieveUser(projectEditersUserId);

            Notification collaborationAddition = new Notification
            {
                Title = "You have been promoted to Project Editor of the project " + p.Title,
                Description = projectEditor.FirstName + " " + projectEditor.LastName + " has added you as a Project Editor to " + p.Title + ".",
                isSeen = false,
                NotificationType = (int)NotificationType.CollaborationAddition,
                Sender = "Automated",
                SenderId = projectEditersUserId,
                TimeStamp = DateTime.UtcNow,
                URL = "~/Project/ProjectCatalog/" + newCollaboratorsUserId,
                UserId = newCollaboratorsUserId
            };

            DAL.addNotification(collaborationAddition, newCollaboratorsUserId);
        }

        /// <summary>
        /// Sends a notification to the user who was removed as a project editor by one of the project's existing project editors.
        /// </summary>
        /// <param name="projectEditersUserId">Project editor's user id who removed the Project Editor</param>
        /// <param name="newCollaboratorsUserId">Used id of the user who was just removed as a project editor</param>
        /// <param name="projectId">Id of the project being modified</param>
        public void ProjectEditorRemoval(int projectEditersUserId, int newCollaboratorsUserId, int projectId)
        {
            Project p = DAL.retrieveProject(projectId);
            User projectEditor = DAL.retrieveUser(projectEditersUserId);

            Notification collaborationAddition = new Notification
            {
                Title = "You are no longer a Project Editor of the project " + p.Title,
                Description = projectEditor.FirstName + " " + projectEditor.LastName + " has removed you as a Project Editor to " + p.Title + ".",
                isSeen = false,
                NotificationType = (int)NotificationType.CollaborationAddition,
                Sender = "Automated",
                SenderId = projectEditersUserId,
                TimeStamp = DateTime.UtcNow,
                URL = "~/Project/ProjectCatalog/" + newCollaboratorsUserId,
                UserId = newCollaboratorsUserId
            };

            DAL.addNotification(collaborationAddition, newCollaboratorsUserId);
        }

        /// <summary>
        /// Sends a notification to the user who was added to a project by one of its project editors.
        /// </summary>
        /// <param name="projectEditersUserId">Project editor's user id who added the new collaborator</param>
        /// <param name="newCollaboratorsUserId">Used id of the user who was just added to the project</param>
        /// <param name="projectId">Id of the project being modified</param>
        public void CollaborationAddition(int projectEditersUserId, int newCollaboratorsUserId, int projectId)
        {
            Project p = DAL.retrieveProject(projectId);
            User projectEditor = DAL.retrieveUser(projectEditersUserId);

            Notification collaborationAddition = new Notification
            {
                Title = "You have been added to " + p.Title,
                Description = projectEditor.FirstName + " " + projectEditor.LastName + " has added you as a collaborator to " + p.Title + ".",
                isSeen = false,
                NotificationType = (int)NotificationType.CollaborationAddition,
                Sender = "Automated",
                SenderId = projectEditersUserId,
                TimeStamp = DateTime.UtcNow,
                URL = "~/Project/ProjectCatalog/" + newCollaboratorsUserId,
                UserId = newCollaboratorsUserId
            };

            if (DAL.retrieveUserSettings(collaborationAddition.UserId).IsEmailedForCollaborationAddition)
            {
                dynamic email = new Email("NotifcationEmail");
                email.To = DAL.retrieveUser(collaborationAddition.UserId).Email;
                email.UserName = DAL.retrieveUser(collaborationAddition.UserId).FirstName;
                email.notificationType = "Added as collabrator";
                email.Send();
            }

            DAL.addNotification(collaborationAddition, newCollaboratorsUserId);
        }

        /// <summary>
        /// Sends a notification to each project editor in the project that a the random user just added themselve to.
        /// </summary>
        /// <param name="newSelfAddingCollaboratorsUserId">User id of the user who just added themselve to the project</param>
        /// <param name="projectId">Id of the project being modified</param>
        public void CollaborationAddition(int newSelfAddingCollaboratorsUserId, int projectId)
        {
            Project p = DAL.retrieveProject(projectId);
            User newCollaborator = DAL.retrieveUser(newSelfAddingCollaboratorsUserId);

            foreach (var permissions in p.ProjectPermissions)
            {
                if (permissions.IsProjectEditor)
                {
                    Notification collaborationAddition = new Notification
                    {
                        Title = newCollaborator.FirstName + " " + newCollaborator.LastName + " has joined " + p.Title,
                        Description = "If this person was not a part of this project you may remove them manually on the Project's page by clicking edit.",
                        isSeen = false,
                        NotificationType = (int)NotificationType.CollaborationAddition,
                        Sender = "Automated",
                        SenderId = newSelfAddingCollaboratorsUserId,
                        TimeStamp = DateTime.UtcNow,
                        URL = "~/Project/ProjectCatalog/" + permissions.UserId,
                        UserId = permissions.UserId
                    };

                    DAL.addNotification(collaborationAddition, permissions.UserId);
                }
            }
        }

        /// <summary>
        /// Sends a notification to each project editor (except the editor that confirmed the user) in the project that the collaborater was accepted into
        /// </summary>
        /// <param name="newCollaboratorsUserId">Id of the user who was accepted into the project</param>
        /// <param name="acceptingProjectEditorsUserId">Id of the project editor that accepted this request</param>
        /// <param name="projectId">Id of the project being modified</param>
        public void CollaborationConfirmation(int newCollaboratorsUserId, int acceptingProjectEditorsUserId, int projectId)
        {
            Project p = DAL.retrieveProject(projectId);
            User newCollaborator = DAL.retrieveUser(newCollaboratorsUserId);
            User acceptingProjectEditor = DAL.retrieveUser(acceptingProjectEditorsUserId);

            foreach (var permissions in p.ProjectPermissions)
            {
                if (permissions.IsProjectEditor)
                {
                    Notification collaborationAddition = new Notification
                    {
                        Title = newCollaborator.FirstName + " " + newCollaborator.LastName + " was accepted into " + p.Title,
                        Description = acceptingProjectEditor.FirstName + " " + acceptingProjectEditor.LastName + " has accepted " + newCollaborator.FirstName 
                        + " " + newCollaborator.LastName + "'s request to join " + p.Title + ".",
                        isSeen = false,
                        NotificationType = (int)NotificationType.CollaborationAddition,
                        Sender = "Automated",
                        SenderId = acceptingProjectEditorsUserId,
                        TimeStamp = DateTime.UtcNow,
                        URL = "~/Project/ProjectCatalog/" + permissions.UserId,
                        UserId = permissions.UserId
                    };

                    DAL.addNotification(collaborationAddition, permissions.UserId);
                }
            }
        }

        /// <summary>
        /// Sends a notification to the user who was just removed from a project
        /// </summary>
        /// <param name="removingProjectEditorsUserId">Id of the project editor who removed the user</param>
        /// <param name="removedCollaboratorsUserId">Id of the user being removed from the project</param>
        /// <param name="projectId">Id of the project being modified</param>
        public void CollaborationRemoval(int removingProjectEditorsUserId, int removedCollaboratorsUserId, int projectId)
        {
            Project p = DAL.retrieveProject(projectId);
            User formerCollaborator = DAL.retrieveUser(removedCollaboratorsUserId);
            User removingProjectEditor = DAL.retrieveUser(removingProjectEditorsUserId);

            Notification collaborationRemoval = new Notification
            {
                Title = "You have been removed from " + p.Title + " (Reference Id: " + p.Id + " )",
                Description = "If you feel this was done in error contact the project editors of the project. "
                + "If you feel this was just abusively contact Portfolio Unleashed support.",
                isSeen = false,
                NotificationType = (int)NotificationType.CollaborationRemoval,
                Sender = "Automated",
                SenderId = removingProjectEditorsUserId,
                TimeStamp = DateTime.UtcNow,
                UserId = removedCollaboratorsUserId
            };


            if (DAL.retrieveUserSettings(collaborationRemoval.UserId).IsEmailedForCollaborationRemoval)
            {
                dynamic email = new Email("NotifcationEmail");
                email.To = DAL.retrieveUser(collaborationRemoval.UserId).Email;
                email.UserName = DAL.retrieveUser(collaborationRemoval.UserId).FirstName;
                email.notificationType = "removec as collabrator";
                email.Send();
            }

            DAL.addNotification(collaborationRemoval, removedCollaboratorsUserId);
        }

        /// <summary>
        /// Sends a notification to all the project's project editors with a link to the request page
        /// </summary>
        /// <param name="requestingUserId">Id of the user requesting to become a collaborator</param>
        /// <param name="projectId">Id of the project being modified</param>
        public void CollaborationRequest(int requestingUserId, int projectId)
        {
            Project p = DAL.retrieveProject(projectId);
            User requestingUser = DAL.retrieveUser(requestingUserId);

            foreach (var permissions in p.ProjectPermissions)
            {
                if (permissions.IsProjectEditor)
                {
                    Notification collaborationRequest = new Notification
                    {
  
                    };

                        collaborationRequest.Title = requestingUser.FirstName + " " + requestingUser.LastName + " has requested to join " + p.Title;
                        collaborationRequest.Description = "Click the relevant link to either accept or reject this request.";
                        collaborationRequest.isSeen = false;
                        collaborationRequest.NotificationType = (int)NotificationType.CollaborationRequest;
                        collaborationRequest.Sender = "Automated";
                        collaborationRequest.SenderId = requestingUserId;
                        collaborationRequest.TimeStamp = DateTime.UtcNow;
                        collaborationRequest.URL = "~/Request/CollaboratorRequest/?userId="+requestingUserId+"&projectId="+ projectId;
                        collaborationRequest.UserId = permissions.UserId;

                        if (DAL.retrieveUserSettings(collaborationRequest.UserId).IsEmailedForCollaborationRequest)
                        {
                            dynamic email = new Email("NotifcationEmail");
                            email.To = DAL.retrieveUser(collaborationRequest.UserId).Email;
                            email.UserName = DAL.retrieveUser(collaborationRequest.UserId).FirstName;
                            email.notificationType = "Request to be collabrator";
                            email.Send();
                        }

                    DAL.addNotification(collaborationRequest, permissions.UserId);
                }
            }
        }

        /// <summary>
        /// Removes all notifications relevent to the handled request
        /// </summary>
        /// <param name="requestingUserId">Id of the user requesting to become a collaborato</param>
        /// <param name="projectId">Id of the project being modified</param>
        public void RemoveRemainingCollaborationRequests(int requestingUserId, int projectId)
        {
            Project p = DAL.retrieveProject(projectId);
            User requestingUser = DAL.retrieveUser(requestingUserId);

            foreach (var permissions in p.ProjectPermissions)
            {
                if (permissions.IsProjectEditor)
                {
                    DAL.deleteNotification(permissions.UserId, requestingUserId);
                }
            }
        }
    }
}