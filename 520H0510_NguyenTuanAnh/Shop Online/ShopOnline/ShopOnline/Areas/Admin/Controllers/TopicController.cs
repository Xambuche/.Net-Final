using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShopOnline.Models;

namespace ShopOnline.Areas.Admin.Controllers
{
    public class TopicController : Controller
    {
        private ShopThoiTrangDBContext db = new ShopThoiTrangDBContext();

        // GET: Admin/Topic
        public ActionResult Index()
        {
            var list = db.Topics.Where(m => m.Status != 0)
                .OrderByDescending(m => m.Status)
                .ToList();
            return View("Index", list);
        }

        public ActionResult Trash()
        {
            var list = db.Topics.Where(m => m.Status == 0)
                .OrderByDescending(m => m.Status)
                .ToList();
            return View("Trash", list);
        }

        // GET: Admin/Topic/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topic topic = db.Topics.Find(id);
            if (topic == null)
            {
                return HttpNotFound();
            }
            return View(topic);
        }

        // GET: Admin/Topic/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Topic/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Slug,ParentID,Orders,Metaky,Metadesc,Created_By,Created_At,Updated_By,Updated_At,Status")] Topic topic)
        {
            if (ModelState.IsValid)
            {
                db.Topics.Add(topic);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(topic);
        }

        // GET: Admin/Topic/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topic topic = db.Topics.Find(id);
            if (topic == null)
            {
                return HttpNotFound();
            }
            return View(topic);
        }

        // POST: Admin/Topic/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Slug,ParentID,Orders,Metaky,Metadesc,Created_By,Created_At,Updated_By,Updated_At,Status")] Topic topic)
        {
            if (ModelState.IsValid)
            {
                db.Entry(topic).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(topic);
        }

        // GET: Admin/Topic/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topic topic = db.Topics.Find(id);
            if (topic == null)
            {
                return HttpNotFound();
            }
            return View(topic);
        }

        // POST: Admin/Topic/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Topic topic = db.Topics.Find(id);
            db.Topics.Remove(topic);
            db.SaveChanges();
            return RedirectToAction("Trash","Topic");
        }

        public ActionResult Status(int id)
        {
            Topic topic = db.Topics.Find(id);
            int status = (topic.Status == 1) ? 2 : 1;
            topic.Status = status;
            topic.Updated_By = 1;
            topic.Updated_At = DateTime.Now;
            db.Entry(topic).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index","Topic");
        }


        //Xoa 
        public ActionResult DelTrash(int id)
        {
            Topic topic = db.Topics.Find(id);
            topic.Status = 0;
            topic.Updated_By = 1;
            topic.Updated_At = DateTime.Now;
            db.Entry(topic).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Trash", "Topic");
        }

        //Huy xoa
        public ActionResult ReTrash(int id)
        {
            Topic topic = db.Topics.Find(id);
            topic.Status = 2;
            topic.Updated_By = 1;
            topic.Updated_At = DateTime.Now;
            db.Entry(topic).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Trash", "Topic");
        }
    }
}
