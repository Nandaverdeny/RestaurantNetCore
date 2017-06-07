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
    public class CategoryController : Controller
    {
        private readonly dbContext _context;

        public CategoryController(dbContext context)
        {
            _context = context;    
        }

        // GET: Category
        public  ActionResult Index()
        {
            return View( _context.Category.ToList().Where(a=>a.IsDeleted != true));
        }

        // GET: Category/Details/5
        public  ActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category =  _context.Category.SingleOrDefault(m => m.CategoryID == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Category/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  ActionResult Create([Bind("CategoryID,CategoryName,CreatedBy,CreatedDate,IsDeleted,UpdatedBy,UpdatedDate")] Category category)
        {
            if (ModelState.IsValid)
            {
                category.CreatedBy = "Admin";
                category.CreatedDate = DateTime.Now;
                _context.Add(category);
                 _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: Category/Edit/5
        public  ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category =  _context.Category.SingleOrDefault(m => m.CategoryID == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Category/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  ActionResult Edit(int id, [Bind("CategoryID,CategoryName,CreatedBy,CreatedDate,IsDeleted,UpdatedBy,UpdatedDate")] Category category)
        {
            if (id != category.CategoryID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var data = _context.Category.SingleOrDefault(m => m.CategoryID == category.CategoryID);

                    if(category != null)
                    {
                        data.CategoryName = category.CategoryName;
                        data.UpdatedBy = "Admin";
                        data.UpdatedDate = DateTime.Now;
                    }
                    _context.Update(data);
                     _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.CategoryID))
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
            return View(category);
        }

        // GET: Category/Delete/5
        public  ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category =  _context.Category.SingleOrDefault(m => m.CategoryID == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public  ActionResult DeleteConfirmed(int id)
        {
            var category =  _context.Category.SingleOrDefault(m => m.CategoryID == id);
            category.IsDeleted = true;
            _context.Update(category);
             _context.SaveChanges();
            return RedirectToAction("Index");
        }

        private bool CategoryExists(int id)
        {
            return _context.Category.Any(e => e.CategoryID == id);
        }
    }
}
