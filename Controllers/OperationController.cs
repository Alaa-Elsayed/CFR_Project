using CRF_Final_Project.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRF_Final_Project.Controllers
{
    [Authorize]
    public class OperationController : Controller
    {
        // GET: Operation
        public ActionResult Show()
        {
            return View(); 
        }

        public ActionResult GetData()
        {
            using (CRFDBEntities db = new CRFDBEntities())
            {

                List<CRF_Table> tablelist = db.CRF_Table.ToList<CRF_Table>();
                return Json(new { data = tablelist }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                return View(new CRF_Table());
            }
            else
            {
                using (CRFDBEntities db = new CRFDBEntities())
                {
                    return View(db.CRF_Table.Where(x => x.ID == id).FirstOrDefault());
                }
            }
        }

        [HttpPost]
        public ActionResult AddOrEdit(CRF_Table entity)
        {
            if (ModelState.IsValid)
            {
                using (CRFDBEntities db = new CRFDBEntities())
                {
                    if (entity.ID == 0)
                    {
                        db.CRF_Table.Add(entity);
                        db.SaveChanges();
                        return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        db.Entry(entity).State = EntityState.Modified;
                        db.SaveChanges();
                        return Json(new { success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return View(entity);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (ModelState.IsValid)
            {
                using (CRFDBEntities db = new CRFDBEntities())
                {
                    var entity = db.CRF_Table.Where(x => x.ID == id).FirstOrDefault();
                    db.CRF_Table.Remove(entity);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelState.AddModelError("", "Error");
                return View();
            }

        }

        public ActionResult DeletAll()
        {
            using (CRFDBEntities db = new CRFDBEntities())
            {
                db.Database.ExecuteSqlCommand("truncate table CRF_Table");
                db.SaveChanges();
                return RedirectToAction("Show");
            }

        }
    }
}