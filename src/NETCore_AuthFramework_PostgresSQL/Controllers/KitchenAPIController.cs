using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantNetCore.Context;
using RestaurantNetCore.Model;

namespace RestaurantNetCore.Controllers
{
    [Produces("application/json")]
    [Route("api/KitchenAPI")]
    public class KitchenAPIController : Controller
    {
        private readonly dbContext _context;

        public KitchenAPIController(dbContext context)
        {
            _context = context;

        }

        [Route("CancelOrderItem/{id}")]
        public ResponseViewModel CancelOrderItem(int id)
        {
            var data = _context.OrderItem.Find(id);

            if (data.Status == "Order")
            {
                data.Status = "Cancel";
                _context.Update(data);
                _context.SaveChanges();
                return new ResponseViewModel { Status = true };
            }
            else
            {
                return new ResponseViewModel { Status = false };
            }

        }

        [Route("CookOrderItem/{id}")]
        public ResponseViewModel CookOrderItem(int id)
        {
            var data = _context.OrderItem.Find(id);

            if (data.Status == "Order")
            {
                data.Status = "Cook";
                _context.Update(data);
                _context.SaveChanges();
                return new ResponseViewModel { Status = true };
            }
            else
            {
                return new ResponseViewModel { Status = false };
            }

        }

        [Route("FinishOrderItem/{id}")]
        public ResponseViewModel FinishOrderItem(int id)
        {
            var data = _context.OrderItem.SingleOrDefault(m => m.OrderItemID == id && m.IsDeleted != true);

            if (data.Status == "Cook")
            {
                data.Status = "FinishCook";
                _context.Update(data);
                _context.SaveChanges();
                return new ResponseViewModel { Status = true };
            }
            else
            {
                return new ResponseViewModel { Status = false };
            }
        }

        [Route("CookAllOrderItem/{id}")]
        public ResponseViewModel CookAllOrderItem(int id)
        {
            var orderitem = (from a in _context.OrderItem
                             where a.IsDeleted != true && a.Status == "Order" && a.OrderID == id
                             select a).ToList();
            foreach (var item in orderitem)
            {
                item.Status = "Cook";
                _context.Update(item);
            }
            if (_context.SaveChanges() == orderitem.Count)
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

        [Route("GetAllOrderItemCateByOrder")]
        public List<KitchenViewModel> GetAllOrderItemCateByOrder()
        {
            List<KitchenViewModel> listdata = new List<KitchenViewModel>();

            listdata.Add(new KitchenViewModel() { Status = "Order" });
            listdata.Add(new KitchenViewModel() { Status = "Cook" });


           
            var listorder = _context.Order.Include(m => m.Type).Where(m => m.IsDeleted != true && m.Finish != true).ToList();
            
            foreach (var item in listdata)
            {
                List<OrderViewModel> listorderdata = new List<OrderViewModel>();
                foreach (var item2 in listorder)
                {
                    OrderViewModel order = new OrderViewModel();
                    order.OrderID = item2.OrderID;
                    order.TableName = (from a in _context.Track
                                       where a.OrderID == item2.OrderID
                                       select a.Table.TableName).SingleOrDefault();
                    order.Name = item2.Name;
                    order.TypeName = item2.Type.TypeName;
                    var orderitem = (from a in _context.OrderItem
                                     where a.IsDeleted != false && a.Status == item.Status && a.OrderID == item2.OrderID
                                     select new OrderItemViewModel
                                     {
                                         OrderItemID = a.OrderItemID,
                                         MenuName = a.Menu.MenuName,
                                         Notes = a.Notes,
                                         Time = a.CreatedDate,
                                         Qty = a.Qty
                                     }).ToList();

                    order.OrderItem = orderitem;
                    listorderdata.Add(order);
                }
                item.Order = listorderdata.OrderBy(m => m.OrderID).ToList();
            }
            return listdata;
        }


        [Route("GetAllOrderItemPrint/{id}")]
        public OrderViewModel GetAllOrderItemPrint(int id)
        {
            OrderViewModel data = new OrderViewModel();
            var order = _context.Order.Find(id);
            data.Name = order.Name;
            data.OrderID = order.OrderID;
            data.TableName = (from a in _context.Track
                              where a.OrderID == order.OrderID
                              select a.Table.TableName).FirstOrDefault();
            var orderitem = (from a in _context.OrderItem
                             where a.IsDeleted != true && a.Status == "Cook" && a.OrderID == order.OrderID
                             select new OrderItemViewModel
                             {
                                 MenuName = a.Menu.MenuName,
                                 Qty = a.Qty,
                                 Notes = a.Notes
                             }).ToList();
            data.OrderItem = orderitem;
            return data;
        }
        [Route("GetOrderItemPrint/{id}")]
        public OrderViewModel GetOrderItemPrint(int id)
        {
            var item = _context.OrderItem.Find(id);
            OrderViewModel data = new OrderViewModel();
            var order = _context.Order.Find(item.OrderID);
            data.Name = order.Name;
            data.OrderID = order.OrderID;
            data.TableName = (from a in _context.Track
                              where a.OrderID == order.OrderID
                              select a.Table.TableName).FirstOrDefault();
            var orderitem = (from a in _context.OrderItem
                             where a.IsDeleted != true && a.Status == "Cook" && a.OrderID == order.OrderID && a.OrderItemID == id
                             select new OrderItemViewModel
                             {
                                 MenuName = a.Menu.MenuName,
                                 Qty = a.Qty,
                                 Notes = a.Notes
                             }).ToList();
            data.OrderItem = orderitem;
            return data;
        }

        [Route("GetAllOrderItem")]
        public List<KitchenViewModel> GetAllOrderItem()
        {
            List<KitchenViewModel> listdata = new List<KitchenViewModel>();

            listdata.Add(new KitchenViewModel() { Status = "Order" });
            listdata.Add(new KitchenViewModel() { Status = "Cook" });


            foreach (var item in listdata)
            {

                var orderitem = (from a in _context.OrderItem
                                 where a.IsDeleted != true && a.Status != "Cancel" && a.Status != "FinishCook" && a.Status != "Served" && a.Status != "Paid" && a.Status == item.Status
                                 select new OrderItemViewModel
                                 {
                                     OrderItemID = a.OrderItemID,
                                     MenuName = a.Menu.MenuName,
                                     Notes = a.Notes,
                                     Qty = a.Qty,
                                     OrderID = a.OrderID,
                                     Time = a.CreatedDate
                                 }).ToList();
                foreach (var item2 in orderitem)
                {
                    var typename = (from a in _context.Order
                                    where a.OrderID == item2.OrderID
                                    select a.Type.TypeName).FirstOrDefault();
                    item2.TypeName = typename;

                }

                item.OrderItem = orderitem;
            }
            return listdata;
        }
        [Route("GetAllOrder")]
        public List<OrderViewModel> GetAllOrder()
        {
            var order = (from a in _context.Order
                         where a.IsDeleted != true && a.Finish != true
                         select new OrderViewModel
                         {
                             OrderID = a.OrderID,
                             Name = a.Name,
                             OrderDate = a.CreatedDate,

                         }).ToList();
            foreach (var item in order)
            {
                item.TableName = (from a in _context.Track
                                  where a.OrderID == item.OrderID
                                  select a.Table.TableName).FirstOrDefault();
            }

            return order;
        }

        [HttpPost]
        [Route("FinishAllOrderItem/{id}")]
        public ResponseViewModel FinishAllOrderItem(int id)
        {
            var orderitem = (from a in _context.OrderItem
                            where a.IsDeleted != true && a.Status == "Cook" && a.OrderID == id
                            select a).ToList();
            foreach(var item in orderitem)
            {
                item.Status = "FinishCook";
                _context.Update(item);
            }
            if(_context.SaveChanges() == orderitem.Count)
            {
                return new ResponseViewModel
                {
                    Status = true
                };
            }else
            {
                return new ResponseViewModel
                {
                    Status = false
                };
            }
            
        }
    }
}