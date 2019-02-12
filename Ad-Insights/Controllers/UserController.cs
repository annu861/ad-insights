using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ad_Insights_DataAccessLayer;
using System.Web.Script.Serialization;
using System.Net;
using System.Data;
using System.Data.Entity;

namespace Ad_Insights.Controllers
{
    //[Authorize(Roles = "User")]
    public class UserController : Controller
    {
        // GET: User
        Ad_insightContext db = new Ad_insightContext();
        
        public ActionResult AddSales()
        {
            
            if (Session["EmailID"] == null)
            {

                return Redirect("~/Login/Login");
            }
            else
            {
                var id = Session["EmailID"];
                if (db.Users.Where(a=>a.EmailID == id).FirstOrDefault().Role == "User")
                {
                    ViewBag.Category = db.Categories.ToList();
                    ViewBag.Product = db.ProductDetail.ToList();
                    return View("");
                }
                else
                {
                    Session["EmailID"] = null;
                    return View("Error");
                }
                
            }


        }

        public IList<ProductDetail> GetProductName(int id)
        {
            return db.ProductDetail.Where(m => m.CategoryID == id).ToList();
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetPNameByCategory(string categoryId)
        {
            var productName = this.GetProductName(Convert.ToInt32(categoryId));
            var product = productName.Select(m => new SelectListItem()
            {
                Text = m.ProductName,
                Value = m.ProductID.ToString()
            });
            return Json(product, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddSales(Transactions transactions)
        {
            Transactions trans = new Transactions
            {
                UserID = Convert.ToInt32(Session["UserID"]),
                ProductID = transactions.ProductID,
                Quantity = transactions.Quantity,
                ProductDetail = transactions.ProductDetail,
                TransactionDate = transactions.TransactionDate,
                //TransactionID = transactions.TransactionID
                
            };
            transactions = trans;
            if (Session["EmailID"] == null)
            {

                return Redirect("~/Login/Login");
            }
            else
	        {
                if (!ModelState.IsValid)
                {
                    db.Transactions.Add(transactions);
                    db.SaveChanges();

                    ModelState.Clear();

                    //ViewBag.Message = "Sale is successfully added.";
                    //return View();
                   
                }
                return RedirectToAction("AddSales");
            }
        }

        public ActionResult SalesDetails(int? trans)
        {
            if (Session["EmailID"] == null)
            {

                return Redirect("~/Login/Login");
            }
            else
	        {
                var id = Session["EmailID"];
                if (db.Users.Where(a => a.EmailID == id).FirstOrDefault().Role == "User")
                {
                    trans = Convert.ToInt32(Session["UserID"]);
                    ViewBag.List = (from t in db.Transactions
                                    join u in db.Users on t.UserID equals u.UserID
                                    join p in db.ProductDetail on t.ProductID equals p.ProductID
                                    where t.UserID == trans
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
                    Transactions transaction = new Transactions();
                    var transactionId = db.Transactions.Where(a => a.UserID == trans).FirstOrDefault();
                    if (transactionId == null)
                    {
                        ViewBag.Message = "No Transactions are found";
                        return View(transactionId);
                    }
                    else
                    { 
                        Session["transactionID"] =db.Transactions.Where(a => a.UserID == trans).FirstOrDefault().TransactionID;
                        Session["ProductID"] = db.Transactions.Where(a => a.UserID == trans).FirstOrDefault().ProductID;
                       
                    } 
                    return View();
                }
                else
                {
                    Session["EmailID"] = null;
                    return View("Error");
                }
                    
            }
        }


        public ActionResult EditSales(int transId = 0)
        
        {
            if (transId == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transactions transactions = db.Transactions.Where(a => a.TransactionID == transId).FirstOrDefault();
            if (transactions == null)
            {
                return HttpNotFound();
            }
            return View(transactions);
        }

        [HttpPost, ActionName("EditSales")]
        [ValidateAntiForgeryToken]
        public ActionResult EditSalesPost(int? id)
        {
            var salesToUpdate = db.Transactions.Find(id);
            if (Session["EmailID"] == null)
            {

                return Redirect("~/Login/Login");
            }
            else
            {

               if(TryUpdateModel(salesToUpdate, "", new string[] {"Quantity" , "Date"}))
                {
                    db.SaveChanges();
                    return RedirectToAction("SalesDetails");
                }
                return View(salesToUpdate);
            }
        }


        public ActionResult DeleteSales(int? id, bool? saveChangeError = false)
        {
            if (Session["EmailID"] == null)
            {

                return Redirect("~/Login/Login");
            }
            else
	        {
                //id = Convert.ToInt32(Session["transactionID"]);

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                if (saveChangeError.GetValueOrDefault())
                {
                    ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";
                }
                Transactions transactions = db.Transactions.Find(id);
                if (transactions == null)
                {
                    return HttpNotFound();
                }
                return View(transactions); 
            }
        }

        [HttpPost, ActionName("DeleteSales")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmedForSales(int? id)
        {
            try
            {
                //id = Convert.ToInt32(Session["transactionID"]);
                if (Session["EmailID"] == null)
                {

                    return Redirect("~/Login/Login");
                }
                else
                {
                    Transactions transactions = db.Transactions.Find(id);
                    db.Transactions.Remove(transactions);
                    db.SaveChanges();
                    //list.Remove(removeList);
                    return RedirectToAction("SalesDetails");
                }
            }
            catch (DataException/* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
        }

    }

}