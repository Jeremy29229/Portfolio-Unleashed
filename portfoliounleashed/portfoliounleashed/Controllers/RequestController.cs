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
    public class RequestController : Controller
    {
        
        public static DatabaseDAL db = new DatabaseDAL();

        [Authorize(Roles = "Admin, User")]
        public ActionResult CollaboratorRequest(int userId=-1, int projectId=-1)
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
                string error1 = "The Project relevant to the request either no longer exists or could not be found.";
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
                string error1 = "The User who made the request either not longer exists or could not be found.";
                string error2 = "User Id: " + userId;
                TempData["ErrorMessages"] = new string[] { error1, error2 };
                return RedirectToAction("Http404", "Error");
            }
            if (project.Contributions.Any(c => c.UserId == userId))
            {
                string error1 = "The user involved in the request is already a contributor to this project.";
                string error2 = "It is possible that another Project Editor has already confirmed the request and the request's original notification had simply not yet been removed.";
                string error3 = "User: " + user.FirstName + " " + user.LastName;
                string error4 = "Project: " + project.Title;
                TempData["ErrorMessages"] = new string[] { error1, error2, error3, error4 };
                return RedirectToAction("General", "Error");
            }
            if (!project.ProjectPermissions.Any(perm => perm.UserId == WebSecurity.CurrentUserId && perm.IsProjectEditor))
            {
                string error1 = "You do not have permission to confirm or deny requests for this project.";
                string error2 = "It is possible that you had permission when the request was initially sent, but such permission has since been revoked.";
                string error3 = "If you believe that you have reached this page in error, please contact another Project Editor or submit a report via the Support page.";
                TempData["ErrorMessages"] = new string[] { error1, error2, error3 };
                return RedirectToAction("General", "Error");
            }
            #endregion

            VMCollaborationRequest request = new VMCollaborationRequest(project, userId, user.FirstName + " " + user.LastName);
            return View(model: request);
        }

        [Authorize(Roles = "Admin, User")]
        public ActionResult CollaboratorRequestConfirm(int userId=-1, int projectId=-1)
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
                string error1 = "The Project relevant to the request either no longer exists or could not be found.";
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
                string error1 = "The User who made the request either not longer exists or could not be found.";
                string error2 = "User Id: " + userId;
                TempData["ErrorMessages"] = new string[] { error1, error2 };
                return RedirectToAction("Http404", "Error");
            }
            if (project.Contributions.Any(c => c.UserId == userId))
            {
                string error1 = "The user involved in the request is already a contributor to this project.";
                string error2 = "It is possible that another Project Editor has already confirmed the request.";
                string error3 = "If this is the case, you should recieve a notification stating that the User has been confirmed.";
                TempData["ErrorMessages"] = new string[] { error1, error2, error3 };
                return RedirectToAction("General", "Error");
            }
            if (!project.ProjectPermissions.Any(perm => perm.UserId == WebSecurity.CurrentUserId && perm.IsProjectEditor))
            {
                string error1 = "You do not have permission to confirm or deny requests for this project.";
                string error2 = "It is possible that you had permission when you initially viewed the request, but such permission has since been revoked.";
                string error3 = "If you believe that you have reached this page in error, please contact another Project Editor or submit a report via the Support page.";
                TempData["ErrorMessages"] = new string[] { error1, error2, error3 };
                return RedirectToAction("General", "Error");
            }
            #endregion

            db.addContribution(new Contribution() { UserId = userId, ProjectId = projectId }, projectId);

            new NotificationController().RemoveRemainingCollaborationRequests(userId, projectId);
            new NotificationController().CollaborationAddition(WebSecurity.CurrentUserId, userId, projectId);
            new NotificationController().CollaborationConfirmation(userId, WebSecurity.CurrentUserId, projectId);
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Admin, User")]
        public ActionResult CollaboratorRequestDeny(int userId = -1, int projectId = -1)
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
                string error1 = "The Project relevant to the request either no longer exists or could not be found.";
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
                string error1 = "The User who made the request either not longer exists or could not be found.";
                string error2 = "User Id: " + userId;
                TempData["ErrorMessages"] = new string[] { error1, error2 };
                return RedirectToAction("Http404", "Error");
            }
            if (project.Contributions.Any(c => c.UserId == userId))
            {
                string error1 = "The user involved in the request is already a contributor to this project.";
                string error2 = "It is possible that another Project Editor has already confirmed the request.";
                string error3 = "If this is the case, you should recieve a notification stating that the User has been confirmed.";
                string error4 = "If you believe that this User should not have been added, contact the Project Editor who confirmed the request.";
                TempData["ErrorMessages"] = new string[] { error1, error2, error3, error4 };
                return RedirectToAction("General", "Error");
            }
            if (!project.ProjectPermissions.Any(perm => perm.UserId == WebSecurity.CurrentUserId && perm.IsProjectEditor))
            {
                string error1 = "You do not have permission to confirm or deny requests for this project.";
                string error2 = "It is possible that you had permission when you initially viewed the request, but such permission has since been revoked.";
                string error3 = "If you believe that you have reached this page in error, please contact another Project Editor or submit a report via the Support page.";
                TempData["ErrorMessages"] = new string[] { error1, error2, error3 };
                return RedirectToAction("General", "Error");
            }
            #endregion

            new NotificationController().RemoveRemainingCollaborationRequests(userId, projectId);
            return RedirectToAction("Index", "Home");
        }
    }
}
