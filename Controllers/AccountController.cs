using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using CRF_Final_Project.Models;

namespace CRF_Final_Project.Controllers
{
    public class AccountController : Controller
    {
        private readonly CRFDBEntities db = new CRFDBEntities();
        // GET: Account
        [AllowAnonymous]
        public ActionResult Login()
        {
           
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        
        public ActionResult Login(AccountViewModel account)
        {
            if (ModelState.IsValid)
            {
                string returnUrl = "";

                if (!string.IsNullOrWhiteSpace(Request.UrlReferrer.Query))
                {
                     returnUrl = Request.UrlReferrer.Query.Split('=')[1].Replace("%2f", "/");
                }

                var user = db.Users.Where(x => x.UserName == account.UserName).FirstOrDefault();
                if (user !=null)
                {
                    string path = "LDAP://axdc.speed.com.eg";

                    DirectoryEntry directoryEntry = new DirectoryEntry(path, account.UserName, account.Password);
                    DirectorySearcher directorySearcher = new DirectorySearcher(directoryEntry);

                    try
                    {
                        if (directorySearcher.FindOne() != null)
                        {
                            FormsAuthentication.SetAuthCookie(account.UserName, true);
                            if (Url.IsLocalUrl(returnUrl))
                            {
                                return Redirect(returnUrl);
                            }
                            else
                            {
                                return RedirectToAction("Index", "Upload");
                            }
                        }
                    }
                    catch (Exception)
                    {
                        ViewBag.error = "invalid  account name or password";

                    }
                }
               else
                {
                    ViewBag.error = "You Are Not Authorised To Login";
                    return View(account);
                }
                
            }

            ModelState.AddModelError("", "Error Happend");
            return View(account);
            
        }
    }
}