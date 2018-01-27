using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AevumRebuild.Models;

namespace Aevum.Controllers
{
    public class WorksController : Controller
    {
        private AevumEntities db = new AevumEntities();

        // GET: Works
        //public ActionResult Index()
        //{
        //    var works = db.Works.Include(w => w.Category1);
        //    return View(works.ToList());
        //}

        public ActionResult Index(string Search, string searchString)
        {
            var titles = from title in db.Works
                         select title;

            if (!String.IsNullOrEmpty(searchString))
            {
                if (Search == "Title")
                {
                    titles = titles.Where(title => title.WorkTitle.Contains(searchString));
                }
                else if (Search == "Name")
                {
                    titles = titles.Where(title => title.AuthorName.Contains(searchString));
                }
                else if (Search == "Date")
                {
                    titles = titles.Where(title => title.Date.ToString().Contains(searchString));
                }
                else if (Search == "Description")
                {
                    titles = titles.Where(title => title.WorkDescription.Contains(searchString));
                }
                else if (Search == "Notes")
                {
                    titles = titles.Where(title => title.NotesOnWork.Contains(searchString));
                }
            }

            return View(titles.ToList().OrderBy(o => o.Date));

        }

        public ActionResult Authors(string Search, string searchString)
        {
            var authors = from author in db.Works
                          select author;

            if (!String.IsNullOrEmpty(searchString))
            {
                if (Search == "Name")
                {
                    authors = authors.Where(author => author.AuthorName.Contains(searchString));
                }
            }
            return View(authors.ToList().OrderBy(o => o.AuthorName));
        }

        // GET: Works/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Work work = db.Works.Find(id);
            if (work == null)
            {
                return HttpNotFound();
            }
            return View(work);
        }

        // GET: Works/Create
        public ActionResult Create()
        {
            ViewBag.Category = new SelectList(db.Categories, "CategoryID", "CategoryName");
            return View();
        }

        // POST: Works/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "WorkID,Date,WorkTitle,AuthorName,AuthorSurName,WorkDescription,ManuscriptType,LocationWritten,LocationFound,CurrentLocation,Category,NotesOnWork")] Work work)
        {
            if (ModelState.IsValid)
            {
                db.Works.Add(work);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Category = new SelectList(db.Categories, "CategoryID", "CategoryName", work.Category);
            return View(work);
        }

        // GET: Works/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Work work = db.Works.Find(id);
            if (work == null)
            {
                return HttpNotFound();
            }
            ViewBag.Category = new SelectList(db.Categories, "CategoryID", "CategoryName", work.Category);
            return View(work);
        }

        // POST: Works/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "WorkID,Date,WorkTitle,AuthorName,AuthorSurName,WorkDescription,ManuscriptType,LocationWritten,LocationFound,CurrentLocation,Category,NotesOnWork")] Work work)
        {
            if (ModelState.IsValid)
            {
                db.Entry(work).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Category = new SelectList(db.Categories, "CategoryID", "CategoryName", work.Category);
            return View(work);
        }

        // GET: Works/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Work work = db.Works.Find(id);
            if (work == null)
            {
                return HttpNotFound();
            }
            return View(work);
        }

        // POST: Works/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Work work = db.Works.Find(id);
            db.Works.Remove(work);
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