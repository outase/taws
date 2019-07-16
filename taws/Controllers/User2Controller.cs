using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using tawsLibrary.db;
using tawsLibrary.db.table;

namespace taws.Controllers
{
    public class User2Controller : Controller
    {
        private edm_user_t db = new edm_user_t();

        // GET: User2
        public ActionResult Index()
        {
            return View(db.userEntities.ToList());
        }

        // GET: User2/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            db_user_t db_user_t = db.userEntities.Find(id);
            if (db_user_t == null)
            {
                return HttpNotFound();
            }
            return View(db_user_t);
        }

        // GET: User2/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User2/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,user_name,login_id,password,email")] db_user_t db_user_t)
        {
            if (ModelState.IsValid)
            {
                db.userEntities.Add(db_user_t);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(db_user_t);
        }

        // GET: User2/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            db_user_t db_user_t = db.userEntities.Find(id);
            if (db_user_t == null)
            {
                return HttpNotFound();
            }
            return View(db_user_t);
        }

        // POST: User2/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,user_name,login_id,password,email")] db_user_t db_user_t)
        {
            if (ModelState.IsValid)
            {
                db.Entry(db_user_t).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(db_user_t);
        }

        // GET: User2/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            db_user_t db_user_t = db.userEntities.Find(id);
            if (db_user_t == null)
            {
                return HttpNotFound();
            }
            return View(db_user_t);
        }

        // POST: User2/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            db_user_t db_user_t = db.userEntities.Find(id);
            db.userEntities.Remove(db_user_t);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
