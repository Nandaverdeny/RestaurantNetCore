using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantNetCore.Context;
using RestaurantNetCore.Model;

namespace RestaurantNetCore.Controllers
{
    public class StatusController : Controller
    {
        private readonly dbContext _context;

        public StatusController(dbContext context)
        {
            _context = context;    
        }

        // GET: Status
        public ActionResult Index()
        {
            var data = from a in _context.Status
                       where a.IsDeleted != true
                       select a;
            return View( data.ToList());
        }

        // GET: Status/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var status =  _context.Status.SingleOrDefault(m => m.StatusID == id);
            if (status == null)
            {
                return NotFound();
            }

            return View(status);
        }

        // GET: Status/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Status/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("StatusID,CreatedBy,CreatedDate,IsDeleted,StatusName,UpdatedBy,UpdatedDate")] Status status)
        {
            if (ModelState.IsValid)
            {
                status.CreatedBy = "Admin";
                status.CreatedDate = DateTime.Now;
                _context.Add(status);
                 _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(status);
        }

        // GET: Status/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var status =  _context.Status.SingleOrDefault(m => m.StatusID == id);
            if (status == null)
            {
                return NotFound();
            }
            return View(status);
        }

        // POST: Status/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind("StatusID,CreatedBy,CreatedDate,IsDeleted,StatusName,UpdatedBy,UpdatedDate")] Status status)
        {
            if (id != status.StatusID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var data = _context.Status.SingleOrDefault(m => m.StatusID == status.StatusID);
                   
                    if(data != null)
                    {
                        data.StatusName = status.StatusName;
                        data.UpdatedBy = "Admin";
                        data.UpdatedDate = DateTime.Now;
                    }
                    _context.Update(data);
                     _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StatusExists(status.StatusID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(status);
        }

        // GET: Status/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var status =  _context.Status.SingleOrDefault(m => m.StatusID == id);
            if (status == null)
            {
                return NotFound();
            }

            return View(status);
        }

        // POST: Status/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var status =  _context.Status.SingleOrDefault(m => m.StatusID == id);
            status.IsDeleted = true;
            _context.Update(status);
             _context.SaveChanges();
            return RedirectToAction("Index");
        }

        private bool StatusExists(int id)
        {
            return _context.Status.Any(e => e.StatusID == id);
        }
    }
}
