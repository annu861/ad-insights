using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ad_Insights_DataAccessLayer;

namespace Ad_Insights.Controllers
{
    public class LoginController : Controller
    {
       [AllowAnonymous]
        public ActionResult Login()
        {
            Ad_insightContext db = new Ad_insightContext();
            if (Session["EmailID"] != null)
            {
                //    User user = new User();
                //    if(db.Users.Where(a => a.EmailID == user.EmailID).FirstOrDefault().Role == 1 )
                //    {
                //        return Redirect("~/Admin/RegisteredUser");
                //    }
                return Redirect("~/Admin/RegisteredUser");

            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(User user)
        {
            Ad_insightContext db = new Ad_insightContext();
            user = db.Users.Where(a => a.EmailID == user.EmailID).SingleOrDefault();
            if (db.Users.Where(a => a.EmailID == user.EmailID).Count() > 0)

            {
                if (db.Users.Where(a => a.EmailID == user.EmailID).FirstOrDefault().Password == user.Password)
                {
                    Session["EmailID"] = user.EmailID;
                    Session["UserID"] = user.UserID;
                    //code here for Authorization
                    if(db.Users.Where(a => a.EmailID == user.EmailID).FirstOrDefault().Role == "Admin")
                    {
                        return Redirect("~/Admin/RegisteredUser");
                    }
                    else if (db.Users.Where(a => a.EmailID == user.EmailID).FirstOrDefault().Role == "User")
                    {
                        if (db.Users.Where(a => a.EmailID == user.EmailID).FirstOrDefault().Status == "Active")
                        {
                            return Redirect("~/User/AddSales");
                        }
                        else
                        {
                            string errorMessage = "You did not get Approve, Wait for Admin Approval";
                            ViewBag.Message = errorMessage;
                        }

                    }
                    else if(db.Users.Where(a => a.EmailID == user.EmailID).FirstOrDefault().Role == null)
                    {
                            string errorMessage = "Account is not yet registered, Please wait for the approval";
                            ViewBag.Message = errorMessage;
                    }
                    
                }
                else
                {
                    return Redirect("~/User/Register");
                }

            }
            else
            {
                string errorMessage = "Email ID Doesn't exists, Please register to login";
                ViewBag.Message = errorMessage;
            }
            return View();                  
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Ad_insightContext db = new Ad_insightContext();
                    var userRecord = db.Users.Where(a => a.EmailID == user.EmailID).FirstOrDefault();
                    if (userRecord != null )
                    {
                        if (userRecord.EmailID == user.EmailID)
                        {
                            ViewBag.Error = "User already exist";
                            ModelState.Clear();
                            return View();
                        }
                    }
                    else
                    {
                        db.Users.Add(user);
                        db.SaveChanges();
                        ModelState.Clear();
                        ViewBag.Message = "Successfully Registered";
                        return View();
                    }
                    
                }
                return View();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult Logout()
        {
            Session.Remove("EmailID");
            return Redirect("~/Login/Login");
        }
    }
}