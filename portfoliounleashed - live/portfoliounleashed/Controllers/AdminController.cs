using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PortfolioUnleashed.Data_Layer;
using PortfolioUnleashed.Models;
using PortfolioUnleashed.Models.ViewModels;
using PortfolioUnleashed.Enums;
using WebMatrix.WebData;
using PortfolioUnleashed.DataLayer;

namespace PortfolioUnleashed.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        DatabaseDAL data = new DatabaseDAL();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AllUsers()
        {
            List<User> users = data.retrieveAllUsers();
            List<VMUser> vmUsers = new List<VMUser>();
            foreach (PortfolioUnleashed.User u in users)
            {
                List<Project> projects = data.retrieveAllProjectsByUserID(u.Id);
                if (projects != null)
                {
                    vmUsers.Add(new VMUser(u, projects));
                }
                else
                {
                    vmUsers.Add(new VMUser(u));
                }
            }
            return View(model:vmUsers);
        }

        public ActionResult AllPortfolios(string orderBy = "PortfolioId", bool isDescending = false)
        {
            VMAllPortfolios allPortfolios = new VMAllPortfolios{ IsDescending = isDescending, OrderBy = orderBy, Portfolios = new List<VMPortfolio>()};
            List<Portfolio> portfolios = data.retrieveAllPortfolios();

            foreach (Portfolio p in portfolios)
            {
                allPortfolios.Portfolios.Add(new VMPortfolio(p));
            }

            if (isDescending)
            {
                if(orderBy == "PortfolioId")
                {
                    allPortfolios.Portfolios = allPortfolios.Portfolios.OrderByDescending(p => p.Id).ToList();
                }
                else if(orderBy == "Title")
                {
                    allPortfolios.Portfolios = allPortfolios.Portfolios.OrderByDescending(p => p.Title).ToList();
                }
                else if(orderBy == "UserId")
                {
                    allPortfolios.Portfolios = allPortfolios.Portfolios.OrderByDescending(p => p.UserId).ToList();
                }
                else if (orderBy == "Visibility")
                {
                    allPortfolios.Portfolios = allPortfolios.Portfolios.OrderByDescending(p => p.Visibility).ToList();
                }
            }
            else
            {
                if (orderBy == "PortfolioId")
                {
                    allPortfolios.Portfolios = allPortfolios.Portfolios.OrderBy(p => p.Id).ToList();
                }
                else if (orderBy == "Title")
                {
                    allPortfolios.Portfolios = allPortfolios.Portfolios.OrderBy(p => p.Title).ToList();
                }
                else if (orderBy == "UserId")
                {
                    allPortfolios.Portfolios = allPortfolios.Portfolios.OrderBy(p => p.UserId).ToList();
                }
                else if (orderBy == "Visibility")
                {
                    allPortfolios.Portfolios = allPortfolios.Portfolios.OrderBy(p => p.Visibility).ToList();
                }
            }

            return View(model: allPortfolios);
        }

        public ActionResult AllProjects()
        {
            List<Project> Projects = data.retrieveAllProjects();
            List<VMFullProject> vmProject = new List<VMFullProject>();
            foreach (Project p in Projects)
            {
                vmProject.Add(new VMFullProject(p));
            }
            return View(model: vmProject);
        }

        public ActionResult DeleteUser()
        {
            int ID = int.Parse(RouteData.Values["id"].ToString());
            data.deleteUser(data.retrieveUser(ID));
            return RedirectToAction("AllUsers", "Admin");
        }

        public ActionResult FeaturePortfolio()
        {
            int ID = int.Parse(RouteData.Values["id"].ToString());
            data.addFeaturedPortfolio(ID);
            return RedirectToAction("AllPortfolios", "Admin");
        }

        //[HttpGet]
        //public ActionResult EditPortfolio()
        //{
        //    int ID = int.Parse(RouteData.Values["id"].ToString());
        //    data.addFeaturedPortfolio(ID);
        //    return RedirectToAction("AllPortfolios", "Admin");
        //}
        //
        //[HttpPost]
        //public ActionResult EditPortfolio()
        //{
        //    int ID = int.Parse(RouteData.Values["id"].ToString());
        //    data.addFeaturedPortfolio(ID);
        //    return RedirectToAction("AllPortfolios", "Admin");
        //}

        public ActionResult DeletePortfolio()
        {
            int ID = int.Parse(RouteData.Values["id"].ToString());
            data.deletePortfolio(data.retrievePortfolio(ID), data.retrievePortfolio(ID).UserId);
            return RedirectToAction("AllPortfolios", "Admin");
        }

        public ActionResult DeleteProject()
        {
            int ID = int.Parse(RouteData.Values["id"].ToString());
            data.deleteProjectCompletely(data.retrieveProject(ID));
            return RedirectToAction("AllProjects", "Admin");
        }

        public ActionResult AddDummyPortfolio()
        {
            int ID = int.Parse(RouteData.Values["id"].ToString());
            Portfolio p = new Portfolio
            {
                Title = RandomTitle(),
                Visibility = (int)VisibilityType.Public,
                Description = RandomDescription(),
            };
            data.addPortfolio(p, ID);
            return RedirectToAction("AllUsers", "Admin");
        }

        public ActionResult AddDummyProject()
        {
            int ID = int.Parse(RouteData.Values["id"].ToString());
            Project p = new Project
            {
                Title = RandomTitle(),
                Template = 0,
                Description = RandomDescription(),
                Contributions = new List<Contribution>{ new Contribution
                {
                    Title = "What I done did",
                    Description = RandomDescription(),
                    UserId = data.retrievePortfolio(ID).UserId,
                },},
                ProjectPermissions = new List<ProjectPermission>{ new ProjectPermission
                {
                     UserId = data.retrievePortfolio(ID).UserId,
                     IsProjectEditor = true,
                },},
            };
            data.addProjectToPortfolio(p,ID);
            return RedirectToAction("AllPortfolios", "Admin");
        }

        [HttpGet]
        public ActionResult AnnouncementNotification()
        {
            User currentUser = data.retrieveUser(WebSecurity.CurrentUserId);
            return View(model: new VMAnnouncementNotification { Sender = "Admin " + currentUser.FirstName + " " + currentUser.LastName, Title = "Announcement " + DateTime.UtcNow.ToShortDateString()});
        }

        [HttpPost]
        public ActionResult AnnouncementNotification(VMAnnouncementNotification a)
        {
            if (ModelState.IsValid)
            {
                List<int> allUserIds = data.retrieveAllUserIds();
                foreach (int id in allUserIds)
                {
                    Notification announcement = new Notification
                    {
                        Description = a.Description,
                        isSeen = false,
                        NotificationType = (int)NotificationType.GlobalAccouncement,
                        Sender = a.Sender,
                        SenderId = WebSecurity.CurrentUserId,
                        TimeStamp = DateTime.UtcNow,
                        Title = a.Title,
                        URL = null,
                        UserId = id
                    };
                    data.addNotification(announcement, id);
                }

                return View(viewName: "index");
            }

            return View();
        }

        [HttpGet]
        public ActionResult ModifyRoles()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ModifyRoles(VMEditingRoles model)
        {
            if (ModelState.IsValid
                && WebSecurity.UserExists(model.UserName))
            {
                System.Web.Security.Roles.AddUserToRole(model.UserName, model.RoleName);
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        private string RandomTitle()
        {
            List<string> titles = new List<string>
            {
                "The Expended 3",
                "Fish Quest Online Unleashed",
                "All in the Family part 2",
                "The Reckoning Redemption: Revelations",
                "Black Beard's Booty",
                "Je Suis Pretentious",
                "The Avengers: Season Pass Map Pack",
                "This is a Random Title",
            };
            Random randy = new Random();
            return titles[randy.Next(0, titles.Count)];
        }

        private string RandomDescription()
        {
            List<string> titles = new List<string>
            {
                "An Oculus Rift first person biography of Ray Charles.",
                "A gurl was walkin2 skewel wit her bf n they were crossin da rode. she se \"bbz will u luv me 4evr\" he said \"NO..\"\" da gurl cryed N ran across da rode b4 da green man came on the sine. boy was cryin and wnet 2 pic up her body. she was ded. he whispered 2 her corpse \"I ment 2 sey i will luv u FIVE-ever...\" (dat mean he luv her moar den 4evr)\nxxx~*...LIKE DIS IF U CRY EVERY TIM...*~xxx",
                "Im a great guy who just needs to find a bro to give my couch to. It's in great condition, like me. It has burgundy uphosltry, and short, teakwood legs. Still nice and firm after years of use. Seats 3 but I've managed to fit 5 guys on it. Call if interested.",
                "A first person, sandbox puzzle shooter with moral choices. Better with Kinect!",
                "I make all the best decisions, here are some of the dicisions that I made. I am a leader.",
                "Hi! My name is (name), and I (occupation). *Place examples of your work below*",
                "These are a bunch of pictures of me finding things. Things have stories and I find things to find sotries. Sometimes they're interesting.",
                "This is a Random Description",
            };
            Random randy = new Random();
            return titles[randy.Next(0, titles.Count)];
        }

        public ActionResult AllNotifications(string orderBy = "TimeStamp", bool isDescending = false)
        {
            VMNotifications notifications = Translator.VMNotificationsFromNotifications(data.retrieveAllNotifications());
            foreach (var n in notifications.Notifications)
            {
                n.Email = data.retrieveUser(n.UserId).Email;
            }
            notifications.IsDescending = isDescending;
            notifications.OrderBy = orderBy;

            if (isDescending)
            {
                if (orderBy == "Title")
                {
                    notifications.Notifications = notifications.Notifications.OrderByDescending(n => n.Title).ToList();
                }
                else if (orderBy == "Description")
                {
                    notifications.Notifications = notifications.Notifications.OrderByDescending(n => n.Description).ToList();
                }
                else if (orderBy == "TimeStamp")
                {
                    notifications.Notifications = notifications.Notifications.OrderByDescending(n => n.TimeStamp).ToList();
                }
                else if (orderBy == "URL")
                {
                    notifications.Notifications = notifications.Notifications.OrderByDescending(n => n.URL).ToList();
                }
                else if (orderBy == "NotificationType")
                {
                    notifications.Notifications = notifications.Notifications.OrderByDescending(n => n.NotificationType).ToList();
                }
                else if (orderBy == "Seen")
                {
                    notifications.Notifications = notifications.Notifications.OrderByDescending(n => n.IsSeen).ToList();
                }
                else if (orderBy == "Email")
                {
                    notifications.Notifications = notifications.Notifications.OrderByDescending(n => n.Email).ToList();
                }
                else if (orderBy == "OwnerId")
                {
                    notifications.Notifications = notifications.Notifications.OrderByDescending(n => n.UserId).ToList();
                }
                else if (orderBy == "Sender")
                {
                    notifications.Notifications = notifications.Notifications.OrderByDescending(n => n.Sender).ToList();
                }
                else if (orderBy == "SenderId")
                {
                    notifications.Notifications = notifications.Notifications.OrderByDescending(n => n.SenderId).ToList();
                }
            }
            else
            {
                if (orderBy == "Title")
                {
                    notifications.Notifications = notifications.Notifications.OrderBy(n => n.Title).ToList();
                }
                else if (orderBy == "Description")
                {
                    notifications.Notifications = notifications.Notifications.OrderBy(n => n.Description).ToList();
                }
                else if (orderBy == "TimeStamp")
                {
                    notifications.Notifications = notifications.Notifications.OrderBy(n => n.TimeStamp).ToList();
                }
                else if (orderBy == "URL")
                {
                    notifications.Notifications = notifications.Notifications.OrderBy(n => n.URL).ToList();
                }
                else if (orderBy == "NotificationType")
                {
                    notifications.Notifications = notifications.Notifications.OrderBy(n => n.NotificationType).ToList();
                }
                else if (orderBy == "Seen")
                {
                    notifications.Notifications = notifications.Notifications.OrderBy(n => n.IsSeen).ToList();
                }
                else if (orderBy == "Email")
                {
                    notifications.Notifications = notifications.Notifications.OrderBy(n => n.Email).ToList();
                }
                else if (orderBy == "OwnerId")
                {
                    notifications.Notifications = notifications.Notifications.OrderBy(n => n.UserId).ToList();
                }
                else if (orderBy == "Sender")
                {
                    notifications.Notifications = notifications.Notifications.OrderBy(n => n.Sender).ToList();
                }
                else if (orderBy == "SenderId")
                {
                    notifications.Notifications = notifications.Notifications.OrderBy(n => n.SenderId).ToList();
                }
            }

            return View(model: notifications);
        }
    }
}
