using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PortfolioUnleashed.Models;
using PortfolioUnleashed.Models.ViewModels;
using PortfolioUnleashed.Enums;
using PortfolioUnleashed.Data_Layer;
using PortfolioUnleashed.DataLayer;
using WebMatrix.WebData;

namespace PortfolioUnleashed.Controllers
{
    public class PortfolioController : Controller
    {
        //
        // GET: /Portfolio/

        public static DatabaseDAL db = new DatabaseDAL();

        [Authorize(Roles = "Admin, User")]
        [HttpGet]
        public ActionResult PortfolioCreateEdit(int id = -1)
        {
            if (RouteData.Values["id"] != null)
            {
                if (int.TryParse(RouteData.Values["id"].ToString(), out id)) { }
            }
            Portfolio p = new Portfolio();
            if (id != -1)
            {
                p = db.retrievePortfolio(id);
                if (p == null)
                {
                    string error1 = "The Portfolio you tried to edit either does not exist or could not be found.";
                    string error2 = "Portfolio Id: " + id;
                    TempData["ErrorMessages"] = new string[] { error1, error2 };
                    return RedirectToAction("Http404", "Error");
                }
            }
            int userId = WebSecurity.CurrentUserId;
            List<Project> projects = db.retrieveAllProjectsByUserID(userId);
            VMEditingPortfolio vmEdit;
            if (projects != null)
            {
                vmEdit = new VMEditingPortfolio(p, userId, projects);
            }
            else
            {
                vmEdit = new VMEditingPortfolio(p, userId);
            }
            return View(vmEdit);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpPost]
        public ActionResult PortfolioCreateEdit(VMEditingPortfolio model)
        {
            //model.Id = 4;
            //int.TryParse(Request.Form["Id"], out model.Id))

            if (ModelState.IsValid)
            {
                List<string> keys = Request.Form.AllKeys.Where(k => k.Contains("AddBox")).ToList();

                bool isNewPortoflio = true;
                Portfolio existing = db.retrievePortfolio(model.Id);
                if (existing != null)//portfolio exists
                {
                    isNewPortoflio = false;
                    existing.Title = model.Title;
                    existing.Description = model.Description;
                    existing.IsMainPortfolio = model.IsMainPortfolio;
                    db.updatePortfolio(existing, model.UserId);

                    if (keys != null && keys.Count > 0)
                    {
                        foreach (string key in keys)
                        {
                            int projectId = int.Parse(key.Substring(0, key.IndexOf("AddBox")));
                            bool addAsProj = Request.Form.GetValues(key).FirstOrDefault().Equals("true");
                            if (addAsProj)//They want it in portfolio
                            {
                                if (!existing.Projects.Any(p => p.Id == projectId))//Isn't already in portfolio
                                {
                                    db.addProjectToPortfolio(db.retrieveProject(projectId), model.Id);
                                }
                            }
                            else//don't want in portfolio
                            {
                                if (existing.Projects.Any(p => p.Id == projectId))//Is in portfolio
                                {
                                    db.removeProjectFromPortfolio(db.retrieveProject(projectId), model.Id);
                                }
                            }
                        }
                    }
                }
                else
                {
                    existing = new Portfolio();
                    existing.Title = model.Title;
                    existing.Description = model.Description;
                    existing.IsMainPortfolio = model.IsMainPortfolio;
                    db.addPortfolio(existing, WebSecurity.CurrentUserId);

                    if (keys != null && keys.Count > 0)
                    {
                        foreach (string key in keys)
                        {
                            int projectId = int.Parse(key.Substring(0, key.IndexOf("AddBox")));
                            bool addAsProj = Request.Form.GetValues(key).FirstOrDefault().Equals("true");
                            if (addAsProj)
                            {
                                existing.Projects.Add(db.retrieveProject(projectId));
                                db.addProjectToPortfolio(db.retrieveProject(projectId), existing.Id);
                            }
                        }
                    }
                }

                if (model.IsMainPortfolio)
                {
                    int userId = (isNewPortoflio) ? WebSecurity.CurrentUserId : WebSecurity.CurrentUserId;
                    PortfolioUnleashed.User user = db.retrieveUser(userId);
                    if (user.Portfolios != null && user.Portfolios.Count > 0)
                    {
                        foreach (Portfolio p in user.Portfolios.Where(p => p.Id != existing.Id))
                        {
                            if (p.IsMainPortfolio)
                            {
                                p.IsMainPortfolio = false;
                                db.updatePortfolio(p, userId);
                            }
                        }
                    }
                }

                return RedirectToAction("Account", "User", new { id = WebSecurity.CurrentUserId });
            }
            Portfolio port = new Portfolio();
            port.Id = model.Id;
            port.Title = model.Title;
            port.Description = model.Description;
            port.IsMainPortfolio = model.IsMainPortfolio;
            port.UserId = model.UserId;
            //port.Projects = model.Projects;

            return View(model: new VMEditingPortfolio(port, model.UserId) {ProjectCatalog = model.ProjectCatalog});
        }

        [AllowAnonymous]
        public ActionResult Portfolio(int id=-1)
        {
            if (RouteData.Values["id"] != null)
            {
                if (int.TryParse(RouteData.Values["id"].ToString(), out id)) { }
            }
            Portfolio p = new Portfolio();
            if (id != -1)
            {
                p = db.retrievePortfolio(id);
                if (p == null)
                {
                    string error1 = "The Portfolio you tried to view either does not exist or could not be found.";
                    string error2 = "Portfolio Id: " + id;
                    TempData["ErrorMessages"] = new string[] { error1, error2 };
                    return RedirectToAction("Http404", "Error");
                }
            }
            VMPortfolio portfolio = new VMPortfolio(p);
            return View(model: portfolio);
        }
    }
}
