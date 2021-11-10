using GeneralStore105.MVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeneralStore105.MVC.Controllers
{
    public class CustomerController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();
        // GET: Customer
        public ActionResult Index()
        {
            var listOfCustomers = _db.Customers.ToList().OrderBy(cust => cust.LastName);
            return View(listOfCustomers);
        }

        //Get: Create
        public ActionResult Create()
        {
            return View();
        }
        //Post: Create
        [HttpPost]
        public ActionResult Create(Customer customer)
        {
            if(ModelState.IsValid)
            {
                _db.Customers.Add(customer);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        //Get: Delete
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            var customer = _db.Customers.Find(id);

            if (customer == null)
                return HttpNotFound();

            return View(customer);
        }
        //Post: Delete
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var customer = _db.Customers.Find(id);
            _db.Customers.Remove(customer);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        //Get: Edit
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            var customer = _db.Customers.Find(id);

            if (customer == null)
                return HttpNotFound();

            return View(customer);
        }

        //Post: Edit
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Customer customer)
        {
            if(ModelState.IsValid)
            {
                _db.Entry(customer).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        //Get: Details
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            var customer = _db.Customers.Find(id);

            if (customer == null)
                return HttpNotFound();

            return View(customer);
        }
    }
}