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
    public class TypeController : Controller
    {
        private readonly dbContext _context;

        public TypeController(dbContext context)
        {
            _context = context;    
        }

        // GET: Type
        public  ActionResult Index()
        {
            return View( _context.Type.ToList().Where(m => m.IsDeleted != true));
        }

        // GET: Type/Details/5
        public  ActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @type =  _context.Type.SingleOrDefault(m => m.TypeID == id);
            if (@type == null)
            {
                return NotFound();
            }

            return View(@type);
        }

        // GET: Type/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Type/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  ActionResult Create([Bind("TypeID,CreatedBy,CreatedDate,IsDeleted,TypeName,UpdatedBy,UpdatedDate")] Model.Type @type)
        {
            if (ModelState.IsValid)
            {
                @type.CreatedBy = "Admin";
                @type.CreatedDate = DateTime.Now;   
                _context.Add(@type);
                 _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(@type);
        }

        // GET: Type/Edit/5
        public  ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @type =  _context.Type.SingleOrDefault(m => m.TypeID == id);
            if (@type == null)
            {
                return NotFound();
            }
            return View(@type);
        }

        // POST: Type/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  ActionResult Edit(int id, [Bind("TypeID,CreatedBy,CreatedDate,IsDeleted,TypeName,UpdatedBy,UpdatedDate")] Model.Type @type)
        {
            if (id != @type.TypeID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var data = _context.Type.SingleOrDefault(m => m.TypeID == @type.TypeID);
                    if(@type != null)
                    {
                        data.TypeName = @type.TypeName;
                        data.UpdatedBy = "Admin";
                        data.UpdatedDate = DateTime.Now;
                    }
                    _context.Update(data);
                     _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TypeExists(@type.TypeID))
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
            return View(@type);
        }

        // GET: Type/Delete/5
        public  ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @type =  _context.Type.SingleOrDefault(m => m.TypeID == id);
            if (@type == null)
            {
                return NotFound();
            }

            return View(@type);
        }

        // POST: Type/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public  ActionResult DeleteConfirmed(int id)
        {
            var @type =  _context.Type.SingleOrDefault(m => m.TypeID == id);
            @type.IsDeleted = true;
            _context.Update(@type);
             _context.SaveChanges();
            return RedirectToAction("Index");
        }

        private bool TypeExists(int id)
        {
            return _context.Type.Any(e => e.TypeID == id);
        }
    }
}
