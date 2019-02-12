using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Ad_Insights.ViewModel;
using Ad_Insights_DataAccessLayer;
using PagedList;

namespace Ad_Insights.Controllers
{
     //[Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        Ad_insightContext db = new Ad_insightContext();
        public ActionResult Category()
        {
            if (Session["EmailID"] == null)
            {
                return Redirect("~/Login/Login");
            }
            else
            {
                var id = Session["EmailID"];
                if (db.Users.Where(a => a.EmailID == id).FirstOrDefault().Role == "Admin")
                {
                    return View();
                }
                else
                {
                    Session["EmailID"] = null;
                    return View("Error");
                }
                    
            }

        }

        [HttpPost]
        public ActionResult Category(Category productCategory)
        {
            try
            {
                if (Session["EmailID"] == null)
                {
                    return Redirect("~/Login/Login");
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        db.Categories.Add(productCategory);
                        db.SaveChanges();

                        ModelState.Clear();
                        ViewBag.Message = "Category is successfully created.";
                        return View();
                    }
                    return View();
                }

            }
            catch (Exception)
            {

                throw;
            }
        }



        public ActionResult ProductDetail()
        {
            if (Session["EmailID"] == null)
            {
                return Redirect("~/Login/Login");
            }
            else
            {
                var emailId = Session["EmailID"];
                if (db.Users.Where(a => a.EmailID == emailId).FirstOrDefault().Role == "Admin")
                {
                    ViewBag.Category = db.Categories.ToList();
                    return View();
                }
                else
                {
                    Session["EmailID"] = null;
                    return View("Error");
                }
            }

        }


        [HttpPost]
        public ActionResult ProductDetail(ProductDetail productDetails)
        {
            if (Session["EmailID"] == null)
            {

                return Redirect("~/Login/Login");
            }

            else
            {
                if (ModelState.IsValid)
                {
                    db.ProductDetail.Add(productDetails);
                    db.SaveChanges();

                    ModelState.Clear();
                    //ViewBag.Message = "Product is successfully added.";
                    //return RedirectToAction("ProductDetail");
                }

                    return RedirectToAction("ProductDetail");
            }

        }

        public ActionResult AllProduct()
        {
            if (Session["EmailID"] == null)
            {

                return Redirect("~/Login/Login");
            }
            else
            {
                var emailId = Session["EmailID"];
                if (db.Users.Where(a => a.EmailID == emailId).FirstOrDefault().Role == "Admin")
                {
                    ViewBag.Category = db.Categories.ToList();
                    ProductDetail list = new ProductDetail();
                    var allList = from p in db.ProductDetail
                                  join c in db.Categories on p.CategoryID equals c.CategoryID
                                  select p;
                    list.GetProductDetails = allList.ToList();
                    return View("AllProduct", list);
                }
                else
                {
                    Session["EmailID"] = null;
                    return View("Error");
                }

            }
        }
        
        [HttpPost]
        public ActionResult AllProduct(int id)
        {
            if (Session["EmailID"] == null)
            {

                return Redirect("~/Login/Login");
            }
            else
            {
                var emailId = Session["EmailID"];
                if (db.Users.Where(a => a.EmailID == emailId).FirstOrDefault().Role == "Admin")
                {
                    if(ModelState.IsValid)
                    {
                        ViewBag.ProductName = (from c in db.Categories
                                               join p in db.ProductDetail on c.CategoryID equals p.CategoryID
                                               where c.CategoryID == id
                                               select new ProductDetailViewModel
                                               {
                                                   CategoryName = c.CategoryName,
                                                   ProductName = p.ProductName,
                                                   ProductPrice = p.ProductPrice,
                                                   ProductID = p.ProductID
                                               }).ToList();
                        return PartialView("_AllProducts");
                    }
                    return RedirectToAction("AllProduct");
                }
                else
                {
                    Session["EmailID"] = null;
                    return View("Error");
                }
            }
        }
        public ActionResult EditProduct(int? transId)

        {
            if (transId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductDetail productDetail = db.ProductDetail.Where(a => a.ProductID == transId).FirstOrDefault();
            if (productDetail == null)
            {
                return HttpNotFound();
            }
            return View(productDetail);
        }

        [HttpPost, ActionName("EditProduct")]
        [ValidateAntiForgeryToken]
        public ActionResult EditProductPost(int? id)
        {
            var productToUpdate = db.ProductDetail.Find(id);
            if (Session["EmailID"] == null)
            {

                return Redirect("~/Login/Login");
            }
            else
            {

                if (TryUpdateModel(productToUpdate, "", new string[] { "ProductName", "ProductPrice" }))
                {
                    db.SaveChanges();
                    return RedirectToAction("AllProduct");
                }
                return View(productToUpdate);
            }
        }

        public ActionResult DeleteProduct(int? id, bool? saveChangeError = false)
        {
            if (Session["EmailID"] == null)
            {

                return Redirect("~/Login/Login");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                if (saveChangeError.GetValueOrDefault())
                {
                    ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";
                }
                ProductDetail productDetail = db.ProductDetail.Find(id);
                if (productDetail == null)
                {
                    return HttpNotFound();
                }
                return View(productDetail);
            }
        }

        [HttpPost, ActionName("DeleteProduct")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmedForProduct(int? id)
        {
            try
            {
                if (Session["EmailID"] == null)
                {

                    return Redirect("~/Login/Login");
                }
                else
                {
                    ProductDetail productName = db.ProductDetail.Find(id);
                    db.ProductDetail.Remove(productName);
                    db.SaveChanges();
                    //list.Remove(removeList);
                    return RedirectToAction("AllProduct");
                }
            }
            catch (DataException/* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
        }

        public ActionResult RegisteredUser()
        {
            if (Session["EmailID"] == null)
            {

                return Redirect("~/Login/Login");
            }
            else
            {
                var id = Session["EmailID"];
                if (db.Users.Where(a => a.EmailID == id).FirstOrDefault().Role == "Admin")
                {
                    return View(db.Users.Where(a => a.Status == null).ToList());
                }
                else
                {
                    Session["EmailID"] = null;
                    return View("Error");
                }
                   
            }

        }

        public ActionResult DeleteRegisteredUser(int? id, bool? saveChangeError = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (saveChangeError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost, ActionName("DeleteRegisteredUser")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmedFromRegisteredUser(int id)
        {
            try
            {
                if (Session["EmailID"] == null)
                {

                    return Redirect("~/Login/Login");
                }
                else
                {
                    var list = db.Users.Where(a => a.Status == null).ToList();
                    var removeList = list.Find(a => a.UserID == id);
                    removeList.Status = "Not Registered";
                    db.SaveChanges();
                    list.Remove(removeList);
                    return View("RegisteredUser", list);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public ActionResult ExistingUser(int? id)
        {
            try
            {
                if (Session["EmailID"] == null)
                {

                    return Redirect("~/Login/Login");
                }
                else
                {
                    var emailId = Session["EmailID"];
                    if (db.Users.Where(a => a.EmailID == emailId).FirstOrDefault().Role == "Admin")
                    {

                        if (id != null)
                        {
                            var exist = db.Users.SingleOrDefault(a => a.UserID == id);
                            if (exist != null)
                            {
                                exist.Status = "Active";
                                exist.Role = "User";
                                db.SaveChanges();
                                return View(db.Users.Where(a => a.Status != null && a.Status != "Not Registered").ToList());
                            }
                            return View(db.Users.Where(a => a.Status != null && a.Status != "Not Registered").ToList());
                        }
                    }
                    else
                    {
                        Session["EmailID"] = null;
                        return View("Error");
                    }
                    return View(db.Users.Where(a => a.Status != null && a.Status != "Not Registered").ToList());
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }


        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int id)
        {
            try
            {
                if (Session["EmailID"] == null)
                {

                    return Redirect("~/Login/Login");
                }
                else
                {
                    var userToUpdate = db.Users.Find(id);
                    if (TryUpdateModel(userToUpdate, "",
                       new string[] { "EmailID", "FirstName", "LastName", "PhoneNumber", "Status", "Role" }))
                    {
                        try
                        {
                            db.SaveChanges();

                            return RedirectToAction("ExistingUser");
                        }
                        catch (DataException /* dex */)
                        {
                            //Log the error (uncomment dex variable name and add a line here to write a log.
                            ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                        }
                    }
                    return View(userToUpdate);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public ActionResult Disable(int? id, bool? saveChangeError = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (saveChangeError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Disable failed. Try again, and if the problem persists see your system administrator.";
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }



        [HttpPost, ActionName("Disable")]
        [ValidateAntiForgeryToken]
        public ActionResult DisableExistingUser(int id)
        {
            try
            {
                if (Session["EmailID"] == null)
                {

                    return Redirect("~/Login/Login");
                }
                else
                {
                    var list = db.Users.Where(a => a.Status != null && a.Status != "Not Registered").ToList();
                    var exist = db.Users.SingleOrDefault(a => a.UserID == id);
                    exist.Status = "In-Active";
                    db.SaveChanges();
                    return View("ExistingUser", list);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ActionResult DeleteExistingUser(int? id, bool? saveChangeError = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (saveChangeError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost, ActionName("DeleteExistingUser")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmedForExistingUser(int id)
        {
            try
            {
                if (Session["EmailID"] == null)
                {

                    return Redirect("~/Login/Login");
                }
                else
                {
                    var list = db.Users.Where(a => a.Status != null && a.Status != "Not Registered").ToList();
                    var removeList = list.Find(a => a.UserID == id);
                    removeList.Status = "Delete";
                    db.SaveChanges();
                    //list.Remove(removeList);
                    return View("ExistingUser", list);
                }
            }
            catch (DataException/* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
        }


        public ActionResult TotalSaleByUserId()
        {
            if (Session["EmailID"] == null)
            {

                return Redirect("~/Login/Login");
            }
            else
	        {
                var emailId = Session["EmailID"];
                if (db.Users.Where(a => a.EmailID == emailId).FirstOrDefault().Role == "Admin")
                {
                    ViewBag.UserName = db.Users.ToList();
                    Transactions list = new Transactions();
                    var allList = from a in db.Transactions
                                  join p in db.ProductDetail on a.ProductID equals p.ProductID
                                  select a;
                    list.GetTransaction = allList.ToList();
                    return View("TotalSaleByUserId", list);
                }
                else
                {
                    Session["EmailID"] = null;
                    return View("Error");
                }
                    
            }
        }

        [HttpPost]
        public ActionResult TotalSaleByUserId(int transactions)
        {
            if (Session["EmailID"] == null)
            {

                return Redirect("~/Login/Login");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var id = from p in db.ProductDetail
                             join c in db.Categories on p.CategoryID equals c.CategoryID
                             select p;
                    ViewBag.List = (from t in db.Transactions
                                join u in db.Users on t.UserID equals u.UserID
                                join p in id on t.ProductID equals p.ProductID
                                where t.UserID == transactions 
                                select new TransactionModel
                                {
                                    ProductName = p.ProductName,
                                    FirstName = u.FirstName,
                                    LastName = u.LastName,
                                    Quantity = t.Quantity,
                                    Transaction = t.TransactionDate,
                                    TotalPrice = p.ProductPrice * t.Quantity,
                                    Price = p.ProductPrice,
                                    TransactionID = t.TransactionID
                                }).ToList();
                    if(db.Transactions.Where(a=>a.UserID == transactions).FirstOrDefault() == null)
                    {
                        ViewBag.Error = "No Transactions are found";
                    }
                    
                    return PartialView("_TransactionList");
                }
                return RedirectToAction("TotalSaleByUserId"); 
            }
        }

        public string Pagination()
        {
            string sortOrder = null;
            string currentFilter = null;
            string searchString = null;
            int? Page = null;
            ViewBag.CurrentOrder = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                Page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            var user = from s in db.Users
                       select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                user = user.Where(s => s.LastName.Contains(searchString)
                 || s.FirstName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    user = user.OrderByDescending(s => s.LastName);
                    break;
                case "Date":
                    user = user.OrderBy(s => s.FirstName);
                    break;
                case "date_desc":
                    user = user.OrderByDescending(s => s.LastName);
                    break;
                default:
                    user = user.OrderBy(s => s.LastName);
                    break;
            }
            int pageSize = 5;
            int pageNumber = (Page ?? 1);
            return (user.ToPagedList(pageNumber, pageSize).ToString());
        }

    }
}