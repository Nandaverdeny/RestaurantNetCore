using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantNetCore.Context;
using RestaurantNetCore.Model;
using RestaurantNetCore.Service;

namespace RestaurantNetCore.Controllers
{
    [Produces("application/json")]
    [Route("api/WaiterAPI")]
    public class WaiterAPIController : Controller
    {
        private readonly dbContext _context;

        public WaiterAPIController(dbContext context)
        {
            _context = context;
            
        }

        [Route("GetOrder")]
        public List<OrderType> GetOrder()
        {
            List<OrderType> listdata = new List<OrderType>();

            listdata = (from a in _context.Type
                        where a.IsDeleted != true
                        select new OrderType
                        {
                            TypeID = a.TypeID,
                            TypeName = a.TypeName
                        }).ToList();

            foreach (var item in listdata)
            {
                List<OrderViewModel> listorder = new List<OrderViewModel>();

                var order = (from a in _context.Order
                             where a.IsDeleted != true && a.Finish != true && a.TypeID == item.TypeID
                             select a).ToList();

                foreach (var item2 in order)
                {
                    OrderViewModel data = new OrderViewModel();
                    var track = (from a in _context.Track
                                 where a.OrderID == item2.OrderID
                                 select a).FirstOrDefault();
                    data.OrderID = item2.OrderID;
                    data.Name = item2.Name;
                    if (track != null)
                    {
                        var table = _context.Table.SingleOrDefault(m => m.TableID == track.TableID);
                        data.TableID = table.TableID;
                        data.TableName = table.TableName;
                    }
                    data.TypeID = item2.TypeID;
                    data.OrderDate = item2.CreatedDate;


                    var orderitem = from a in _context.OrderItem
                                    where a.IsDeleted != true && a.OrderID == item2.OrderID && a.Status != "Cancel"
                                    select a;
                    var i = 0;
                    var ii = 0;
                    foreach (var item3 in orderitem)
                    {
                        if (item3.Status == "Served")
                        {
                            i++;
                        }
                        if (item3.Status == "FinishCook")
                        {
                            ii++;
                        }
                    }
                    data.Status = ii;
                    data.OrderServed = i + "/" + orderitem.Count();
                    listorder.Add(data);
                }

                item.Order = listorder;
            }

            return listdata;
        }

        [HttpGet]
        [Route("DetailOrder/{id}")]
        public OrderViewModel DetailOrder(int id)
        {
            OrderViewModel data = new OrderViewModel();
            var order = _context.Order.Find(id);

            data.OrderID = order.OrderID;
            data.OrderDate = order.CreatedDate;
            data.TypeID = order.TypeID;

            if (_context.Type.Find(order.TypeID).TypeName == "Order")
            {
                var track = (from a in _context.Track
                             where a.OrderID == id
                             select a).FirstOrDefault();
                var table = _context.Table.SingleOrDefault(m => m.TableID == track.TableID);

                data.TableID = table.TableID;
                data.TableName = table.TableName;
            }

            var orderitem = (from a in _context.OrderItem
                             where a.IsDeleted != true && a.OrderID == data.OrderID && a.Status != "Cancel"
                             select new OrderItemViewModel
                             {
                                 OrderItemID = a.OrderItemID,
                                 MenuID = a.MenuID,
                                 MenuName = a.Menu.MenuName,
                                 Status = a.Status,
                                 Price = a.Menu.MenuPrice,
                                 Qty = a.Qty,
                                 Notes = a.Notes
                             }).ToList();

            data.OrderItem = orderitem;
            return data;
        }

        [Route("ServedOrder/{id}")]
        public ResponseViewModel ServedOrder(int id)
        {
            var orderitem = _context.OrderItem.Find(id);

            if (orderitem.IsDeleted != true)
            {
                orderitem.Status = "Served";
                _context.Update(orderitem);
                _context.SaveChanges();
                return new ResponseViewModel { Status = true };
            }
            else
            {
                return new ResponseViewModel { Status = false }; ;
            }
        }
        [Route("CancelOrder/{id}")]
        public ResponseViewModel CancelOrder(int id)
        {
            var orderitem = _context.OrderItem.Find(id);

            if (orderitem.IsDeleted != true)
            {
                orderitem.Status = "Cancel";
                orderitem.IsDeleted = true;
                _context.Update(orderitem);
                _context.SaveChanges();
                return new ResponseViewModel { Status = true };
            }
            else
            {
                return new ResponseViewModel { Status = false }; ;
            }
        }

        [Route("PayOrder/{id}")]
        public ResponseViewModel PayOrder(int id)
        {
            Bill bill = new Bill();
            bill.OrderID = id;
            bill.BillDate = DateTime.Now;
            var orderitem = _context.OrderItem.Include(m => m.Menu).Include(m => m.Order).Where(a => a.OrderID == id && a.IsDeleted != true && a.Status != "Cancel").ToList();
            double total = 0;
            foreach (var item in orderitem)
            {
          
                total = total + (item.Qty * Convert.ToDouble(item.Menu.MenuPrice));
                item.Status = "Paid";
                _context.Update(item);

            }
            total = (total * 0.1) + total;
            bill.TotalPrice = total.ToString();
            bill.CreatedBy = "Admin";
            bill.CreatedDate = DateTime.Now;
            _context.Add(bill);



            var order = _context.Order.Include(m=>m.Type).SingleOrDefault(m => m.OrderID == id);
            order.Finish = true;
            order.UpdateDate = DateTime.Now;
            order.UpdatedBy = "Admin";
            _context.Update(order) ;

            if (order.Type.TypeName == "Order")
            {
                var tableid = (from a in _context.Track
                               where a.OrderID == order.OrderID
                               select a.TableID).FirstOrDefault();
                var table = _context.Table.Find(tableid);
                table.TableStatus = "NotOccupied";
                _context.Update(table);
            }
            if (_context.SaveChanges() > 0)
            {
                return new ResponseViewModel
                {
                    Status = true
                };
            }
            else
            {
                return new ResponseViewModel
                {
                    Status = false
                };
            }
        }

        [HttpPost]
        [Route("Table")]
        public List<Table> GetTable()
        {
            var table = (from a in _context.Table
                         where a.IsDeleted != true && a.TableStatus == "NotOccupied"
                         select new Table
                         {
                             TableID = a.TableID,
                             TableName = a.TableName
                         }).ToList();
            return table;
        }

        [HttpGet]
        [Route("GetMenu/{id}")]
        public AddOrder GetMenu(int? id)
        {
            AddOrder AddOrder = new AddOrder();
            AddOrder.TableID = id;

            var category = from a in _context.Category
                           where a.IsDeleted != true
                           select a;

            List<CategoryViewModel> ListCategoryViewModel = new List<CategoryViewModel>();
            foreach (var item in category)
            {
                CategoryViewModel CategoryViewModel = new CategoryViewModel();
                CategoryViewModel.CategoryID = item.CategoryID;
                CategoryViewModel.CategoryName = item.CategoryName;
                var listmenu = (from a in _context.Menu
                                where a.CategoryID == item.CategoryID && a.Status.StatusName == "Ready"
                                select new OrderItemViewModel
                                {
                                    MenuID = a.MenuID,
                                    MenuName = a.MenuName,
                                    Price = a.MenuPrice,
                                    Content = a.Content,
                                    ContentType = a.ContentType
                                }).ToList();
                CategoryViewModel.OrderItem = listmenu;
                ListCategoryViewModel.Add(CategoryViewModel);
            }

            AddOrder.Category = ListCategoryViewModel;
            return AddOrder;
        }
    }
}