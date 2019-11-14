using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyShop2.Core.Contracts;
using MyShop2.Core.Models;
using MyShop2.Services;

namespace MyShop2.WebUI.Controllers
{

	public class BasketController : Controller
	{
		IBasketService basketService;
		IOrderService orderService;
		IRepository<Customer> customers;

		public BasketController(IBasketService BasketService, IOrderService OrderService, IRepository<Customer> Customers)
		{
			basketService = BasketService;
			orderService = OrderService;
			customers = Customers;
		}
		public ActionResult Index()
		{
			var model = basketService.GetBasketItems(HttpContext);
			return View(model);
		}

		public ActionResult AddToBasket(string Id)
		{
			basketService.AddToBasket(HttpContext, Id);
			return RedirectToAction("Index");
		}

		public ActionResult RemoveFromBasket(string Id)
		{
			basketService.RemoveFromBasket(this.HttpContext, Id);

			return RedirectToAction("Index");
		}

		public PartialViewResult BasketSummary()
		{
			var basketSummary = basketService.GetBasketSummary(this.HttpContext);

			return PartialView(basketSummary);
		}
		[Authorize]
		public ActionResult Checkout()
		{
			return View();
		}

		[HttpPost]
		[Authorize]
		public ActionResult Checkout(Order order)
		{
			var basketItems = basketService.GetBasketItems(this.HttpContext);
			order.OrderStatus = "Order Created";
			order.Email = User.Identity.Name;

			//Process Payment

			order.OrderStatus = "Payment Proccesed";
			orderService.CreateOrder(order, basketItems);
			basketService.ClearBasket(this.HttpContext);

			return RedirectToAction("Thankyou", new { OrderId = order.Id });
		}

		public ActionResult Thankyou(string OrderId)
		{
			ViewBag.OrderId = OrderId;
			return View();
		}
	}
}