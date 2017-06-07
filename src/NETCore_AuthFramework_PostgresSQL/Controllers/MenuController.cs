using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantNetCore.Context;
using RestaurantNetCore.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace RestaurantNetCore.Controllers
{
    public class MenuController : Controller
    {
        private readonly dbContext _context;
        //private IHostingEnvironment _environment;


        //public ActionResult test()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public ActionResult test(ICollection<IFormFile> files)
        //{
        //    var uploads = Path.Combine(_environment.WebRootPath, "uploads");
        //    foreach (var file in files)
        //    {
        //        if (file.Length > 0)
        //        {
        //            using (var fileStream = new FileStream(Path.Combine(uploads, file.FileName), FileMode.Create))
        //            {
        //                 file.CopyTo(fileStream);
        //            }
        //        }
        //    }
        //    return View();
        //}

        public MenuController(dbContext context/*, IHostingEnvironment enviroment*/)
        {
            _context = context;
            //_environment = enviroment;  
        }

        // GET: Menu
        public  ActionResult Index()
        {
            var dbContext = _context.Menu.Include(m => m.Category).Include(m => m.Status);
            return View( dbContext.ToList().Where(m => m.IsDeleted != true));
        }

        // GET: Menu/Details/5
        public  ActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menu =  _context.Menu.SingleOrDefault(m => m.MenuID == id);
            if (menu == null)
            {
                return NotFound();
            }

            return View(menu);
        }

        // GET: Menu/Create
        public IActionResult Create()
        {
            ViewData["CategoryID"] = new SelectList(_context.Category.Where(m=> m.IsDeleted != true), "CategoryID", "CategoryName");
            ViewData["StatusID"] = new SelectList(_context.Status.Where(m => m.IsDeleted != true), "StatusID", "StatusName");
            return View();
        }

        // POST: Menu/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  ActionResult Create([Bind("MenuID,CategoryID,Content,ContentType,CreatedBy,CreatedDate,IsDeleted,MenuDescription,MenuName,MenuPrice,StatusID,UpdatedBy,UpdatedDate")] Menu menu,ICollection<IFormFile> files)
        {
            if (ModelState.IsValid)
            {
                //var uploads = Path.Combine(_environment.WebRootPath, "uploads");
                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        using (var fileStream = file.OpenReadStream())
                        using (var ms = new MemoryStream())
                        {
                            fileStream.CopyTo(ms);
                            var fileBytes = ms.ToArray();
                            string s = Convert.ToBase64String(fileBytes);
                            menu.Content = fileBytes;
                            menu.ContentType = file.ContentType;
                            
                            // act on the Base64 data
                        }

                    }
                }
                menu.CreatedBy = "Admin";
                menu.CreatedDate = DateTime.Now;
                _context.Add(menu);
                 _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewData["CategoryID"] = new SelectList(_context.Category, "CategoryID", "CategoryName", menu.CategoryID);
            ViewData["StatusID"] = new SelectList(_context.Status, "StatusID", "StatusName", menu.StatusID);
            return View(menu);
        }

        // GET: Menu/Edit/5
        public  ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menu =  _context.Menu.SingleOrDefault(m => m.MenuID == id);
            if (menu == null)
            {
                return NotFound();
            }
            ViewData["CategoryID"] = new SelectList(_context.Category, "CategoryID", "CategoryName", menu.CategoryID);
            ViewData["StatusID"] = new SelectList(_context.Status, "StatusID", "StatusName", menu.StatusID);
            return View(menu);
        }

        // POST: Menu/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  ActionResult Edit(int id, [Bind("MenuID,CategoryID,Content,ContentType,CreatedBy,CreatedDate,IsDeleted,MenuDescription,MenuName,MenuPrice,StatusID,UpdatedBy,UpdatedDate")] Menu menu, ICollection<IFormFile> files)
        {
            if (id != menu.MenuID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var data = _context.Menu.SingleOrDefault(m => m.MenuID == menu.MenuID);
                    if(data!= null)
                    {
                        foreach (var file in files)
                        {
                            if (file.Length > 0)
                            {
                                using (var fileStream = file.OpenReadStream())
                                using (var ms = new MemoryStream())
                                {
                                    fileStream.CopyTo(ms);
                                    var fileBytes = ms.ToArray();
                                    string s = Convert.ToBase64String(fileBytes);
                                    data.Content = fileBytes;
                                    data.ContentType = file.ContentType;

                                    // act on the Base64 data
                                }

                            }
                        }
                        data.MenuName = menu.MenuName;
                        data.MenuPrice = menu.MenuPrice;
                        data.MenuDescription = menu.MenuDescription;
                        data.CategoryID = menu.CategoryID;
                        data.StatusID = menu.StatusID;
                        data.UpdatedBy = "Admin";
                        data.UpdatedDate = DateTime.Now;
                    }
                    _context.Update(data);
                     _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MenuExists(menu.MenuID))
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
            ViewData["CategoryID"] = new SelectList(_context.Category, "CategoryID", "CategoryName", menu.CategoryID);
            ViewData["StatusID"] = new SelectList(_context.Status, "StatusID", "StatusName", menu.StatusID);
            return View(menu);
        }

        // GET: Menu/Delete/5
        public  ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menu =  _context.Menu.SingleOrDefault(m => m.MenuID == id);
            if (menu == null)
            {
                return NotFound();
            }

            return View(menu);
        }

        // POST: Menu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public  ActionResult DeleteConfirmed(int id)
        {
            var menu =  _context.Menu.SingleOrDefault(m => m.MenuID == id);
            menu.IsDeleted = true;
            _context.Update(menu);
             _context.SaveChanges();
            return RedirectToAction("Index");
        }

        private bool MenuExists(int id)
        {
            return _context.Menu.Any(e => e.MenuID == id);
        }

        public ActionResult GetFile(int id)
        {
            var fileToRetrieve = _context.Menu.SingleOrDefault(m => m.MenuID == id);
            return File(fileToRetrieve.Content, fileToRetrieve.ContentType);
        }
    }
}
