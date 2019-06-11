using RBMTEngAmh.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RBMTEngAmh.Controllers
{
    public class ResourcesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Resources
        public ActionResult Index()
        {
            return View();
        }

        // GET: Resources/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Resources/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Resources/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Resources/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Resources/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Resources/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Resources/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
