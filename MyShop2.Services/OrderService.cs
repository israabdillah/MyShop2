using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyShop2.Core.Contracts;
using MyShop2.Core.Models;
using MyShop2.Core.ViewModels;

namespace MyShop2.Services
{
	public class OrderService : IOrderService
	{
		IRepository<Order> orderContext;
		public OrderService(IRepository<Order> OrderContext)
		{
			this.orderContext = OrderContext;
		}

		public void CreateOrder(Order baseOrder, List<BasketItemViewModel> basketItems)
		{
			foreach (var item in basketItems)
			{
				baseOrder.OrderItems.Add(new OrderItems()
				{
					ProductId = item.Id,
					Image = item.Image,
					Price = item.Price,
					ProductName = item.ProductName,
					Quanity = item.Quanity

				});
			}

			orderContext.Insert(baseOrder);
			orderContext.Commit();
		}

		public List<Order> GetOrderList()
		{
			return orderContext.Collection().ToList();
		}

		public Order GetOrder(string Id)
		{
			return orderContext.Find(Id);
		}

		public void UpdateOrder(Order updateOrder)
		{
			orderContext.Update(updateOrder);
			orderContext.Commit();
		}
	}
}
