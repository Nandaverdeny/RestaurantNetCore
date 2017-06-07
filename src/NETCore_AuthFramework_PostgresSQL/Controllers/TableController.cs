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
    public class TableController : Controller
    {
        private readonly dbContext _context;

        public TableController(dbContext context)
        {
            _context = context;    
        }

        // GET: Table
        public  ActionResult Index()
        {
            return View( _context.Table.ToList().Where(a =>a.IsDeleted != true));
        }

        // GET: Table/Details/5
        public  ActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var table =  _context.Table.SingleOrDefault(m => m.TableID == id);
            if (table == null)
            {
                return NotFound();
            }

            return View(table);
        }

        // GET: Table/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Table/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  ActionResult Create([Bind("TableID,CreatedBy,CreatedDate,IsDeleted,TableName,TableStatus,UpdatedBy,UpdatedDate")] Table table)
        {
            if (ModelState.IsValid)
            {
                table.CreatedBy = "Admin";
                table.CreatedDate = DateTime.Now;
                _context.Add(table);
                 _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(table);
        }

        // GET: Table/Edit/5
        public  ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var table =  _context.Table.SingleOrDefault(m => m.TableID == id);
            if (table == null)
            {
                return NotFound();
            }
            return View(table);
        }

        // POST: Table/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  ActionResult Edit(int id, [Bind("TableID,CreatedBy,CreatedDate,IsDeleted,TableName,TableStatus,UpdatedBy,UpdatedDate")] Table table)
        {
            if (id != table.TableID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var data = _context.Table.SingleOrDefault(m => m.TableID == table.TableID);
                    if(table != null)
                    {
                        data.TableName = table.TableName;
                        data.TableStatus = table.TableStatus;
                        data.UpdatedBy = "Admin";
                        data.UpdatedDate = DateTime.Now;
                    }
                    _context.Update(data);
                     _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TableExists(table.TableID))
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
            return View(table);
        }

        // GET: Table/Delete/5
        public  ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var table =  _context.Table.SingleOrDefault(m => m.TableID == id);
            if (table == null)
            {
                return NotFound();
            }

            return View(table);
        }

        // POST: Table/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public  ActionResult DeleteConfirmed(int id)
        {
            var table =  _context.Table.SingleOrDefault(m => m.TableID == id);
            table.IsDeleted = true;
            _context.Update(table);
             _context.SaveChanges();
            return RedirectToAction("Index");
        }

        private bool TableExists(int id)
        {
            return _context.Table.Any(e => e.TableID == id);
        }
    }
}
