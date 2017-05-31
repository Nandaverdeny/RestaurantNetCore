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
    public class BillController : Controller
    {
        private readonly dbContext _context;

        public BillController(dbContext context)
        {
            _context = context;    
        }

        // GET: Bill
        public  ActionResult Index()
        {
            var dbContext = _context.Bill.Include(b => b.Order);
            return View( dbContext.ToList());
        }

        // GET: Bill/Details/5
        public  ActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bill =  _context.Bill.SingleOrDefault(m => m.BillID == id);
            if (bill == null)
            {
                return NotFound();
            }

            return View(bill);
        }

        // GET: Bill/Create
        public IActionResult Create()
        {
            ViewData["OrderID"] = new SelectList(_context.Order, "OrderID", "OrderID");
            return View();
        }

        // POST: Bill/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  ActionResult Create([Bind("BillID,BillDate,CreatedBy,CreatedDate,IsDeleted,OrderID,TotalPrice,UpdatedBy,UpdatedDate")] Bill bill)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bill);
                 _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewData["OrderID"] = new SelectList(_context.Order, "OrderID", "OrderID", bill.OrderID);
            return View(bill);
        }

        // GET: Bill/Edit/5
        public  ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bill =  _context.Bill.SingleOrDefault(m => m.BillID == id);
            if (bill == null)
            {
                return NotFound();
            }
            ViewData["OrderID"] = new SelectList(_context.Order, "OrderID", "OrderID", bill.OrderID);
            return View(bill);
        }

        // POST: Bill/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  ActionResult Edit(int id, [Bind("BillID,BillDate,CreatedBy,CreatedDate,IsDeleted,OrderID,TotalPrice,UpdatedBy,UpdatedDate")] Bill bill)
        {
            if (id != bill.BillID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var data = _context.Bill.SingleOrDefault(m => m.BillID == bill.BillID);
                    if(bill != null)
                    {
                        data.TotalPrice = bill.TotalPrice;
                        data.UpdatedBy = "Admin";
                        data.UpdatedDate = DateTime.Now;
                    }
                    _context.Update(data);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BillExists(bill.BillID))
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
            ViewData["OrderID"] = new SelectList(_context.Order, "OrderID", "OrderID", bill.OrderID);
            return View(bill);
        }

        // GET: Bill/Delete/5
        public  ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bill =  _context.Bill.SingleOrDefault(m => m.BillID == id);
            if (bill == null)
            {
                return NotFound();
            }

            return View(bill);
        }

        // POST: Bill/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public  ActionResult DeleteConfirmed(int id)
        {
            var bill =  _context.Bill.SingleOrDefault(m => m.BillID == id);
            _context.Bill.Remove(bill);
             _context.SaveChanges();
            return RedirectToAction("Index");
        }

        private bool BillExists(int id)
        {
            return _context.Bill.Any(e => e.BillID == id);
        }
    }
}
