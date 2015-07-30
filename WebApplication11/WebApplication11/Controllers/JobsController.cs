using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication11.Models;

namespace WebApplication11.Controllers
{
    public class JobsController : Controller
    {

        ApplicationDbContext db = new ApplicationDbContext();
        private List<Jobs> list = new List<Jobs>();

        // GET: Jobs
        public ActionResult Index(String SearchString)
        {
            var json = new WebClient().DownloadString("https://gist.githubusercontent.com/WillemLabu/34cfb50187ec334c48ee/raw/cb46400505afd82d9e354b591ad71d97f07613be/jobs.json");
            Trace.TraceError(json.ToString());
            var objects = JObject.Parse(json.ToString()); // parse as array
            var subject = JArray.Parse(objects.GetValue("jobs").ToString());
            List<Jobs> jobs = new List<Jobs>();
            

            foreach (var root in subject)
            {

                var data = JObject.Parse(root.ToString());
                Jobs job = new Jobs();
                job.client = data.GetValue("client").ToString();
                job.jobNumber = data.GetValue("job-number").ToString();
                job.jobName = data.GetValue("job-name").ToString();
                job.due = data.GetValue("due").ToString();
                job.status = data.GetValue("status").ToString();
                
                jobs.Add(job);

            }


            var joblist = from m in jobs
                          select m;
            if (!String.IsNullOrEmpty(SearchString))
            {
                jobs = jobs.Where(b => b.jobName.ToUpper().Contains(SearchString.ToUpper())).ToList();
              
                return View(jobs);
            
            }
            ViewBag.Last = 0;
            list = jobs;
            return View(jobs.GetRange(0,10));
        }

        public ActionResult next(string n) {


            
                var json = new WebClient().DownloadString("https://gist.githubusercontent.com/WillemLabu/34cfb50187ec334c48ee/raw/cb46400505afd82d9e354b591ad71d97f07613be/jobs.json");
                Trace.TraceError(json.ToString());
                var objects = JObject.Parse(json.ToString()); // parse as array
                var subject = JArray.Parse(objects.GetValue("jobs").ToString());
                List<Jobs> jobs = new List<Jobs>();

                foreach (var root in subject)
                {

                    var data = JObject.Parse(root.ToString());
                    Jobs job = new Jobs();
                    job.client = data.GetValue("client").ToString();
                    job.jobNumber = data.GetValue("job-number").ToString();
                    job.jobName = data.GetValue("job-name").ToString();
                    job.due = data.GetValue("due").ToString();
                    job.status = data.GetValue("status").ToString();

                    jobs.Add(job);

                }


                if (Int32.Parse(n) >= jobs.Count() || Int32.Parse(n) < 0)
                {
                ViewBag.Last = 0;
                return View("Index",jobs.GetRange(0, 10));
            }
            
                ViewBag.Last = Int32.Parse(n);
            return View("Index",jobs.GetRange(Int32.Parse(n), 10));
        }

        // search
        [HttpGet]
        public ActionResult Search(string SearchString)
        {
             List<Jobs> jobs = new List<Jobs>();

             var joblist = from m in jobs
                          select m;

             if (!String.IsNullOrEmpty(SearchString))
             {
                 joblist = joblist.Where(b => b.client.ToUpper().Contains(SearchString.ToUpper())).ToList();
             }

             return View("Index", jobs.ToList());

        }

        // GET: Jobs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Jobs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "client,jobNumber,jobName,due,status")] Jobs jobs)
        {
            if (ModelState.IsValid)
            {
                db.Jobs.Add(jobs);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(jobs);
        }

        // GET: Jobs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Jobs jobs = db.Jobs.Find(id);
            if (jobs == null)
            {
                return HttpNotFound();
            }
            return View(jobs);
        }

        // POST: Jobs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "client,jobNumber,jobName,due,status")] Jobs jobs)
        {
            if (ModelState.IsValid)
            {
                db.Entry(jobs).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(jobs);
        }

        // GET: Jobs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Jobs jobs = db.Jobs.Find(id);
            if (jobs == null)
            {
                return HttpNotFound();
            }
            return View(jobs);
        }

        // POST: Jobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Jobs jobs = db.Jobs.Find(id);
            db.Jobs.Remove(jobs);
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
