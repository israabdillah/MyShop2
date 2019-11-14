using System.Collections.Generic;
using MyShop2.Core.Models;
using MyShop2.Core.ViewModels;

namespace MyShop2.Core.Contracts
{
	public interface IOrderService
	{
		void CreateOrder(Order baseOrder, List<BasketItemViewModel> basketItems);
		Order GetOrder(string Id);
		List<Order> GetOrderList();
		void UpdateOrder(Order updateOrder);
	}
}