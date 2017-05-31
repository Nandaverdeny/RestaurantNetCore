using Microsoft.EntityFrameworkCore;
using RestaurantNetCore.Context;
using RestaurantNetCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantNetCore.Service
{
    public class OrderService
    {
        private readonly dbContext _context;

        public OrderService(dbContext context)
        {
            _context = context;
        }


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
                        var table =  _context.Table.SingleOrDefault(m => m.TableID == track.TableID);
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
    }
}
