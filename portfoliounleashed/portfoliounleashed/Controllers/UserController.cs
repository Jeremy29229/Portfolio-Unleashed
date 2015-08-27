using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PortfolioUnleashed.Models;
using PortfolioUnleashed.Models.ViewModels;
using PortfolioUnleashed.Enums;
using PortfolioUnleashed.Data_Layer;
using System.Web.Security;
using WebMatrix.WebData;
using PortfolioUnleashed.DataLayer;
using Postal;

namespace PortfolioUnleashed.Controllers
{
    public class UserController : Controller
    {

        public static DatabaseDAL db = new DatabaseDAL();


        [AllowAnonymous]
        [HttpGet]
        public ActionResult AccountCreation()
        {
            return View(model:new VMCreatingUser());
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult AccountCreation(VMCreatingUser model)
        {
            bool isValidInput = true;

            List<ContactInfo> contacts = new List<ContactInfo>();
            if (!AllContactInfosVerified(out contacts))
            {
                isValidInput = false;
            }

            List<Education> educations = new List<Education>();
            if (!AllEducationsVerified(out educations))
            {
                isValidInput = false;
            }

            List<Link> links = new List<Link>();
            if (!AllLinksVerified(out links))
            {
                isValidInput = false;
            }

            if (ModelState.IsValid)
            {
                if (!model.Email.Equals(model.ConfirmationEmail))
                {
                    isValidInput = false;
                    ModelState.AddModelError("ConfirmationEmail", "Email and Confirmation Email do not match.");
                }
                if (model.Password.Count() < 6)
                {
                    isValidInput = false;
                    ModelState.AddModelError("Password", "Your Password must be at least six characters long.");
                }
                else if (!model.Password.Equals(model.ConfirmationPassword))
                {
                    isValidInput = false;
                    ModelState.AddModelError("ConfirmationPassword", "Password and Confirmation Password do not match.");
                }
            }

            if (isValidInput && ModelState.IsValid)
            {
                try
                {
                    string confirmationToken = WebSecurity.CreateUserAndAccount(model.Email, model.Password, new { model.FirstName, model.LastName },true);
                    VMEditAccountSettings settings = new VMEditAccountSettings()
                    {
                        EmailAdminMessage = false,
                        EmailCollaborationAddition = false,
                        EmailCollaborationRemoval = false,
                        EmailCollaborationRequest = false,
                        EmailDailyViewReport = false,
                        ProfileVisibility = false,
                        UserID = db.retrieveUserByEmail(model.Email).Id
                    };

                    db.addUserSettings(Translator.UserSettingFromVMEditAccountSettings(settings));
                    dynamic email = new Email("RegEmail");
                    email.To = model.Email;
                    email.UserName = model.FirstName;
                    email.ConfirmationToken = confirmationToken;
                    try
                    {
                        email.Send();
                    }
                    catch (Exception e)
                    {
                        
                    }

                    int NewUserId = db.retrieveUserByEmail(model.Email).Id;

                    foreach (ContactInfo c in contacts)
                    {
                        db.addContactInfo(c, NewUserId);
                    }
                    foreach (Education e in educations)
                    {
                        db.addEducation(e, NewUserId);
                    }
                    foreach (Link l in links)
                    {
                        db.addLink(l, NewUserId);
                    }

                    System.Web.Security.Roles.AddUserToRole(model.Email, "User");                    
                    return RedirectToAction("RegistratoinStepTwo", "User");
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }

            User u = new User();
            u.FirstName = model.FirstName;
            u.LastName = model.LastName;
            u.Email = model.Email;
            u.ContactInfoes = contacts;
            u.Links = links;
            u.Educations = educations;

            return View(new VMCreatingUser(u));
        }

        [AllowAnonymous]
        public ActionResult RegistratoinStepTwo()
        {
            string confirmation = "Please check your email for a confirmation link to activate your account.";
            VMGeneralMessage message = new VMGeneralMessage("RegistrationStepTwo", "Registration Step Two", new string[] { confirmation });
            return View("GeneralMessage", message);
        }

        [AllowAnonymous]
        public ActionResult RegisterConfirmation(string Id)
        {
            if (WebSecurity.ConfirmAccount(Id))
            {
                return RedirectToAction("ConfirmationSuccess");
            }
            return RedirectToAction("ConfirmationFailure");
        }

        [AllowAnonymous]
        public ActionResult ConfirmationSuccess()
        {
            string success = "You have confirmed your account";
            VMGeneralMessage message = new VMGeneralMessage("Confirmed", "confirmed Account", new string[] { success });
            return View("GeneralMessage", message);
        }

        [AllowAnonymous]
        public ActionResult ConfirmationFailure()
        {
            string failure = "You have fails to confirm your account";
            VMGeneralMessage message = new VMGeneralMessage("NotConfirmed", "Faild to confirm Account", new string[] { failure });
            return View("GeneralMessage", message);
        }

        [Authorize(Roles="Admin, User")]
        [HttpGet]
        public ActionResult AccountEdit(int id)
        {
            if (RouteData.Values["id"] != null)
            {
                if (int.TryParse(RouteData.Values["id"].ToString(), out id)) { }
            }
            PortfolioUnleashed.User user = db.retrieveUser(id);
            if (user == null)
            {
                string error1 = "The User Account you tried to edit either does not exist or could not be found.";
                string error2 = "User Id: " + id;
                ViewBag.ErrorMessages = new string[] { error1, error2 };
                return View("PageNotFound");
            }

            VMEditingUser userToEdit = new VMEditingUser(user);
            return View(model: userToEdit);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpPost]
        public ActionResult AccountEdit(VMEditingUser model)
        {
            bool isValidInput = true;
            int userId = int.Parse(Request.Form["UserId"]);
            List<ContactInfo> contacts = new List<ContactInfo>();
            if (!AllContactInfosVerified(out contacts))
            {
                isValidInput = false;
            }

            List<Education> educations = new List<Education>();
            if (!AllEducationsVerified(out educations))
            {
                isValidInput = false;
            }

            List<Link> links = new List<Link>();
            if (!AllLinksVerified(out links))
            {
                isValidInput = false;
            }

            if ((WebSecurity.CurrentUserId == userId) && !string.IsNullOrEmpty(model.NewPassword))
            {
                if (model.NewPassword.Length < 6)
                {
                    isValidInput = false;
                    ModelState.AddModelError("NewPassword", "New Password must contain at least six characters.");
                }
                else if (string.IsNullOrEmpty(model.ConfirmNewPassword) || !model.NewPassword.Equals(model.ConfirmNewPassword))
                {
                    isValidInput = false;
                    ModelState.AddModelError("ConfirmNewPassword", "New Password and Confirm New Password do not match.");
                }
            }

            if ((WebSecurity.CurrentUserId == userId) && isValidInput && ModelState.IsValid)
            {
                if(string.IsNullOrEmpty(model.CurrentPassword))
                {
                    isValidInput = false;
                    ModelState.AddModelError("CurrentPassword", "You must enter your current password.");
                }
                else if (!WebSecurity.Login(model.Email, model.CurrentPassword))
                {
                    isValidInput = false;
                    ModelState.AddModelError("CurrentPassword", "The Password you entered is incorrect.");
                }
                else
                {
                    if (!string.IsNullOrEmpty(model.NewPassword))
                    {
                        if (!WebSecurity.ChangePassword(model.Email, model.CurrentPassword, model.NewPassword))
                        {
                            isValidInput = false;
                            ModelState.AddModelError("NewPassword", "Could not change password to the new one provided. Please try again with a different password.");
                        }
                    }
                }
            }

            #region Update User's Properties
            if (isValidInput && ModelState.IsValid)
            {
                PortfolioUnleashed.User updatedUser = db.retrieveUser(userId);
                updatedUser.FirstName = model.FirstName;
                updatedUser.LastName = model.LastName;
                db.updateUser(updatedUser);
                foreach (ContactInfo c in contacts)
                {
                    if (c.UserId == userId)//It's an existing entry
                    {
                        if (string.IsNullOrEmpty(c.Title) && string.IsNullOrEmpty(c.Information))
                        {
                            db.deleteContactInfo(c, userId);
                        }
                        else
                        {
                            db.updateContactInfo(c);
                        }
                    }
                    else//new entry
                    {
                        db.addContactInfo(c, userId);
                    }
                }
                foreach (Education e in educations)
                {
                    if (e.UserId == userId)//It's an existing entry
                    {
                        if (string.IsNullOrEmpty(e.School) && string.IsNullOrEmpty(e.Degree))
                        {
                            //They blanked out the fields, want to remove the entry
                            db.deleteEducation(e, userId);
                        }
                        else
                        {
                            //edit the user's education info via e.Id and UserId
                            db.updateEducation(e, userId);
                        }
                    }
                    else//new entry
                    {
                        db.addEducation(e, userId);
                    }
                }
                foreach (Link l in links)
                {
                    if (l.UserId == userId)//It's an existing entry
                    {
                        if (string.IsNullOrEmpty(l.DisplayText) && string.IsNullOrEmpty(l.URL))
                        {
                            //They blanked out the fields, want to remove the entry
                            db.deleteLink(l, userId);
                        }
                        else
                        {
                            //edit the user's education info via e.Id and UserId
                            db.updateLink(l, userId);
                        }
                    }
                    else//new entry
                    {
                        db.addLink(l, userId);
                    }
                }

                return RedirectToAction("Account", "User", new { id = userId });
            }
            #endregion

            User u = new User();
            u.Id = userId;
            u.FirstName = model.FirstName;
            u.LastName = model.LastName;
            u.Email = model.Email;
            u.ContactInfoes = contacts;
            u.Links = links;
            u.Educations = educations;

            return View(new VMEditingUser(u));
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(VMLoginUser user)
        {
            if (ModelState.IsValid)
            {
                if (System.Web.Security.Roles.GetRolesForUser(user.Email).Contains("Banned"))
                {
                    string failure = "Your account is currently banned";
                    VMGeneralMessage message = new VMGeneralMessage("Banned", "Account Banned", new string[] { failure });
                    return View("GeneralMessage", message);
                }
                if (WebSecurity.Login(user.Email, user.Password))
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return View(user);
                }
            }
            return View(user);
        }

        [AllowAnonymous]
        public ActionResult LogOut()
        {
            WebSecurity.Logout();
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Admin, User")]
        public ActionResult Account(int id=-1)
        {
            if (RouteData.Values["id"] != null)
            {
                if (int.TryParse(RouteData.Values["id"].ToString(), out id)) { }
            }
            if (id == -1)
            {
                //string error1 = "To view a User's account, you must pass in a User Id.";
                //ViewBag.ErrorMessages = new string[] { error1 };
                //return View("PageNotFound");
                id = WebSecurity.CurrentUserId;
            }
            PortfolioUnleashed.User user = db.retrieveUser(id);
            if (user == null)
            {
                string error1 = "The User whose account you tried to view either does not exist or could not be found.";
                //string error2 = "User Id: "+ id;
                TempData["ErrorMessages"] = new string[] { error1 };
                return RedirectToAction("Http404", "Error");
            }
            VMUser vmUser = new VMUser(user);

            if (Request.IsAuthenticated)
            {
                if (id != WebSecurity.CurrentUserId)
                {
                    ViewBag.IsQuickReference = db.retrieveQuickReferences(WebSecurity.CurrentUserId).Any(q => q.QuickReferenceId == id);
                }
            }

            return View(vmUser);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult PasswordRecovery()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PasswordRecovery(VMPasswordRecovery recover)
        {
            if (ModelState.IsValid)
            {
                MembershipUser user;
                if (db.retrieveUserByEmail(recover.Email) != null)
                {
                    user = Membership.GetUser(recover.Email);
                }
                else
                {
                    user = null;
                }
                if (user != null)
                {
                    var token = WebSecurity.GeneratePasswordResetToken(user.UserName);
                    dynamic email = new Email("PassRecover");
                    email.To = user.UserName;
                    email.ConfirmationToken = token;
                    try
                    {
                        email.Send();
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("", "Issue sending email: " + e.Message);
                    }
                }

            }
            return View(recover);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult PasswordChange(string id)
        {
            VMPasswordReset model = new VMPasswordReset();
            model.ReturnToken = id;
            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult PasswordChange(VMPasswordReset model)
        {
            if (ModelState.IsValid)
            {
                bool resetResponse = WebSecurity.ResetPassword(model.ReturnToken, model.Password);
                if (resetResponse)
                {
                    ViewBag.Message = "Password Changed";
                }
                else
                {
                    ViewBag.Message = "Password Not Changed!";
                }
            }

            return View(model);
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }

        //Compiles & verifies Educations - False=errors
        private bool AllEducationsVerified(out List<Education> educations)
        {
            bool isValidInput = true;
            educations = new List<Education>();

            /*Get values by name - name = Object property*/
            string[] educationSchools = Request.Form.GetValues("Education.School");
            string[] educationDegrees = Request.Form.GetValues("Education.Degree");
            string[] educationStartYears = Request.Form.GetValues("Education.StartYear");
            string[] educationEndYears = Request.Form.GetValues("Education.EndYear");
            string[] educationUserIds = Request.Form.GetValues("Education.UserId");
            string[] educationIds = Request.Form.GetValues("Education.Id");

            /*Each array should have same count*/
            for (int i = 0; i < educationSchools.Count(); i++)
            {
                string[] entryInputs = new string[] { educationSchools[i], educationDegrees[i], educationStartYears[i], educationEndYears[i], educationUserIds[i] };
                /*All fields empty - skip entry*/
                if (!allAreNullOrEmpty(entryInputs))
                {
                    entryInputs = new string[] { educationSchools[i], educationDegrees[i], educationStartYears[i], educationEndYears[i]};
                    /*Only some fields empty*/
                    if (someAreNullOrEmpty(entryInputs))
                    {
                        isValidInput = false;
                        ModelState.AddModelError("", "If you add an Education information entry, you must either complete all fields for that entry or leave all fields blank.");
                    }
                    /*Populate Education - persist data entered*/
                    Education education = new Education();
                    education.UserId = int.Parse(educationUserIds[i]);
                    education.School = educationSchools[i];
                    education.Degree = educationDegrees[i];
                    education.Id = int.Parse(educationIds[i]);
                    short temp;
                    /*StartYear must be number*/
                    if (short.TryParse(educationStartYears[i], out temp))
                    {
                        education.StartYear = temp;
                    }
                    else
                    {
                        education.StartYear = 0;
                        isValidInput = false;
                        ModelState.AddModelError("", "Start and End years for Education entries must be valid years consisting only of numbers.");
                    }
                    /*EndYear must be number*/
                    if (short.TryParse(educationEndYears[i], out temp))
                    {
                        education.EndYear = temp;
                    }
                    else
                    {
                        education.EndYear = 0;
                        isValidInput = false;
                        ModelState.AddModelError("", "Start and End years for Education entries must be valid years consisting only of numbers.");
                    }
                    /*End year can't come before Start year*/
                    if ((education.StartYear != 0 && education.EndYear != 0) && education.EndYear < education.StartYear)
                    {
                        isValidInput = false;
                        ModelState.AddModelError("", "End years must come after Start years in Education entries.");
                    }
                    educations.Add(education);
                }
            }
            return isValidInput;
        }

        //Compiles & verifies Links - False=errors
        private bool AllLinksVerified(out List<Link> links)
        {
            bool isValidInput = true;
            links = new List<Link>();
            /*Getting the contents of every input field with the passed in name (each name matches a property of the view model) */
            string[] linkTitles = Request.Form.GetValues("Link.Title");
            string[] linkInformations = Request.Form.GetValues("Link.Information");
            string[] linkUserIds = Request.Form.GetValues("Link.UserId");
            string[] linkIds = Request.Form.GetValues("Link.Id");
            /*The index of each array correlates to which entry the fields belong to, so each array should have the same Count()*/
            for (int i = 0; i < linkTitles.Count(); i++)
            {
                string[] entryInputs = new string[] { linkTitles[i], linkInformations[i], linkUserIds[i] };
                /*Links are not required so if all fields are empty for this entry, we skip it */
                if (!allAreNullOrEmpty(entryInputs))
                {
                    entryInputs = new string[] { linkTitles[i], linkInformations[i] };
                    /*If only some fields are complete and others in the entry are left empty, throw an error*/
                    if (someAreNullOrEmpty(entryInputs))
                    {
                        isValidInput = false;
                        ModelState.AddModelError("", "If you add a Link entry, you must either complete all fields for that entry or leave all fields blank.");
                    }
                    /*Still populate the Link so that they don't have to start over if they left fields blank*/
                    Link link = new Link();
                    link.DisplayText = linkTitles[i];
                    link.URL = linkInformations[i];
                    link.UserId = int.Parse(linkUserIds[i]);
                    link.Id = int.Parse(linkIds[i]);
                    links.Add(link);
                }
            }
            return isValidInput;
        }

        //Compiles & verifies ContactInfos - False=errors
        private bool AllContactInfosVerified(out List<ContactInfo> contacts)
        {
            bool isValid = true;
            contacts = new List<ContactInfo>();
            /*Getting the contents of every input field with the passed in name (each name matches a property of the view model) */
            string[] contactInfoTitles = Request.Form.GetValues("ContactInfo.Title");
            string[] contactInfoInformations = Request.Form.GetValues("ContactInfo.Information");
            string[] contactInfoUserIds = Request.Form.GetValues("ContactInfo.UserId");
            string[] contactInfoIds = Request.Form.GetValues("ContactInfo.Id");
            /*The index of each array correlates to which entry the fields belong to, so each array should have the same Count()*/
            for (int i = 0; i < contactInfoTitles.Count(); i++)
            {
                string[] entryInputs = new string[] { contactInfoTitles[i], contactInfoInformations[i], contactInfoUserIds[i] };
                /*ContactInfo is not required so if all fields are empty, that's fine */
                if (!allAreNullOrEmpty(entryInputs))
                {
                    entryInputs = new string[] { contactInfoTitles[i], contactInfoInformations[i] };
                    /*If only some fields are complete and others in the entry are left empty, throw an error*/
                    if (someAreNullOrEmpty(entryInputs))
                    {
                        isValid = false;
                        ModelState.AddModelError("", "If you add a Contact information entry, you must either complete all fields for that entry or leave all fields blank.");
                    }
                    /*Still populate the ContactInfo so that they don't have to start over if they left fields blank*/
                    ContactInfo contact = new ContactInfo();
                    contact.Title = contactInfoTitles[i];
                    contact.Information = contactInfoInformations[i];
                    contact.UserId = int.Parse(contactInfoUserIds[i]);
                    contact.Id = int.Parse(contactInfoIds[i]);
                    contacts.Add(contact);
                }
            }
            return isValid;
        }

        //Returns Link entry partial
        public ActionResult NewLinkEntry()
        {
            var link = new VMLink();
            return View("Partial/FormEntry/_NewLinkEntry", link);
        }

        //Returns ContactInfo entry partial
        public ActionResult NewContactInfoEntry()
        {
            var contact = new VMContactInfo();
            return View("Partial/FormEntry/_NewContactInfoEntry", contact);
        }

        //Returns Education entry partial
        public ActionResult NewEducationEntry()
        {
            var education = new VMEducation();
            return View("Partial/FormEntry/_NewEducationEntry", education);
        }

        //Returns true if all strings are either null, empty, or 0
        private bool allAreNullOrEmpty(string[] strings)
        {
            bool allEmpty = true;
            foreach (string s in strings)
            {
                if (!string.IsNullOrEmpty(s) && s!="0")
                {
                    allEmpty = false;
                }
            }
            return allEmpty;
        }

        //Returns true if some strings are either null or empty
        //Returns false if either all or none are null or empty
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
            return (numEmpty != 0 || (numEmpty != 0 && numEmpty < strings.Count()));
        }

        public ActionResult AddQuickContact(int id)
        {
            if (RouteData.Values["id"] != null)
            {
                if (int.TryParse(RouteData.Values["id"].ToString(), out id)) { }
            }
            PortfolioUnleashed.User user = db.retrieveUser(id);
            if (user == null)
            {
                string error1 = "The User you tried to add as a QuickContact either does not exist or could not be found.";
                string error2 = "User Id: " + id;
                ViewBag.ErrorMessages = new string[] { error1, error2 };
                return View("PageNotFound");
            }

            db.addQuickReference(WebSecurity.CurrentUserId, id);

            return RedirectToAction("Account", new { id = id });
        }

        public ActionResult RemoveQuickContact(int id)
        {
            if (RouteData.Values["id"] != null)
            {
                if (int.TryParse(RouteData.Values["id"].ToString(), out id)) { }
            }
            PortfolioUnleashed.User user = db.retrieveUser(id);
            if (user == null)
            {
                string error1 = "The User you tried to remove as a QuickContact either does not exist or could not be found.";
                string error2 = "User Id: " + id;
                ViewBag.ErrorMessages = new string[] { error1, error2 };
                return View("PageNotFound");
            }

            db.removeQuickReference(WebSecurity.CurrentUserId, id);

            return RedirectToAction("Account", new { id = id });
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet]
        public ActionResult EditAccountSettings()
        {
            int idCheck = db.retrieveUserByEmail(User.Identity.Name).Id;
            bool boolCheck = db.retrieveUserSettings(db.retrieveUserByEmail(User.Identity.Name).Id).IsEmailedForAdminMessage;
            VMEditAccountSettings model = new VMEditAccountSettings()
            {
                EmailAdminMessage = db.retrieveUserSettings(db.retrieveUserByEmail(User.Identity.Name).Id).IsEmailedForAdminMessage,
                EmailCollaborationAddition = db.retrieveUserSettings(db.retrieveUserByEmail(User.Identity.Name).Id).IsEmailedForCollaborationAddition,
                EmailCollaborationRemoval = db.retrieveUserSettings(db.retrieveUserByEmail(User.Identity.Name).Id).IsEmailedForCollaborationRemoval,
                EmailCollaborationRequest = db.retrieveUserSettings(db.retrieveUserByEmail(User.Identity.Name).Id).IsEmailedForCollaborationRequest,
                EmailDailyViewReport = db.retrieveUserSettings(db.retrieveUserByEmail(User.Identity.Name).Id).IsEmailedForViewReport,
                ProfileVisibility = db.retrieveUserSettings(db.retrieveUserByEmail(User.Identity.Name).Id).IsPublic,
                UserID = db.retrieveUserSettings(db.retrieveUserByEmail(User.Identity.Name).Id).UserId,
                ID = db.retrieveUserSettings(db.retrieveUserByEmail(User.Identity.Name).Id).Id
            };
            return View(model);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpPost]
        public ActionResult EditAccountSettings(VMEditAccountSettings model)
        {
            if (ModelState.IsValid)
            {
                UserSetting test = Translator.UserSettingFromVMEditAccountSettings(model);
                db.updateUserSettings(test, model.UserID);
                string completedUpdate = "You have updated your settings";
                VMGeneralMessage message = new VMGeneralMessage("AccountUpdated","Updated Account", new string[] {completedUpdate});
                return View("GeneralMessage", message);
            }
            return View(model);
        }
    }

}
