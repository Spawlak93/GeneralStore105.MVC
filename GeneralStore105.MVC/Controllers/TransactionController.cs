using GeneralStore105.MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace GeneralStore105.MVC.Controllers
{
    public class TransactionController : Controller
    {
        ApplicationDbContext _db = new ApplicationDbContext();
        // GET: Transaction
        public ActionResult Index()
        {
            var transactions = _db.Transactions.ToList();
            return View(transactions);
        }

        //Get: Create
        public ActionResult Create()
        {
            PopulateDropDownLists();

            return View();
        }

        //Helper methods:
        //Likely move these to bottom of class
        private void PopulateDropDownLists()
        {
            PopulateCustomerDropDownList();
            PopulateProductDropDownList();
        }

        private void PopulateCustomerDropDownList()
        {
            ViewBag.Customers = _db.Customers.ToList().Select(cust => new SelectListItem { Value = cust.CustomerID.ToString(), Text = cust.CustomerID + " " + cust.FullName }).ToList();
        }
        private void PopulateProductDropDownList()
        {
            ViewBag.Products = _db.Products.Select(prod => new SelectListItem { Value = prod.ProductId.ToString(), Text = prod.ProductId + " " + prod.Name }).ToList();
        }








        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Transaction transaction)
        {
            transaction.DateOfTransaction = DateTime.Now;
            if (!ModelState.IsValid)
            {
                PopulateDropDownLists();
                return View(transaction);
            }

            var product = _db.Products.Find(transaction.ProductId);

            if (product.InventoryCount < transaction.ItemCount)
            {
                PopulateDropDownLists();
                return View(transaction);
            }

            product.InventoryCount -= transaction.ItemCount;
            _db.Transactions.Add(transaction);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }








        //Get: Transaction/Edit/{id}
        public ActionResult Edit(int? id)
        {
            if (id is null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var transaction = _db.Transactions.Find(id);
            if (transaction is null)
                return HttpNotFound();

            PopulateDropDownLists();

            return View(transaction);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //Post: Transaction/Edit/{id}
        public ActionResult Edit(Transaction updatedTransaction)
        {
            if (!ModelState.IsValid)
            {
                PopulateDropDownLists();
                return View(updatedTransaction);
            }

            var transaction = _db.Transactions.Find(updatedTransaction.TransactionId);

            var difference = transaction.ItemCount - updatedTransaction.ItemCount;
            if (transaction.Product.InventoryCount + difference < 0)
            {
                PopulateDropDownLists();
                return View(updatedTransaction);
            }

            transaction.Product.InventoryCount += difference;

            transaction.ItemCount = updatedTransaction.ItemCount;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        //Get: Transaction/Delete/{id}
        public ActionResult Delete(int? id)
        {
            if (id is null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var transaction = _db.Transactions.Find(id);
            if (transaction is null)
                return HttpNotFound();

            return View(transaction);
        }

        //Post Transaction/Delete/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var transaction = _db.Transactions.Find(id);
            transaction.Product.InventoryCount += transaction.ItemCount;
            _db.Transactions.Remove(transaction);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        //Get: Transaction/Detail/{id}
        public ActionResult Details(int? id)
        {
            if (id is null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var transaction = _db.Transactions.Find(id);
            if (transaction is null)
                return HttpNotFound();

            return View(transaction);
        }
    }
}