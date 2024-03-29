﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using MyShop2.Core.Contracts;
using MyShop2.Core.Models;
using MyShop2.Core.ViewModels;
using MyShop2.DataAccess.InMemory;

namespace MyShop2.Services
{
	public class BasketService : IBasketService
	{
		IRepository<Product> productContext;
		IRepository<Basket> basketContext;

		public const string BasketSessionName = "eCommerceBasket";

		public BasketService(IRepository<Product> ProductContext, IRepository<Basket> BasketContext)
		{
			productContext = ProductContext;
			basketContext = BasketContext;
		}

		private Basket GetBasket(HttpContextBase httpContext, bool CreateIfNull)
		{
			HttpCookie cookie = httpContext.Request.Cookies.Get(BasketSessionName);
			Basket basket = new Basket();

			if (cookie != null)
			{
				string basketId = cookie.Value;
				if (!string.IsNullOrWhiteSpace(basketId))
				{
					basket = basketContext.Find(basketId);
				}
				else
				{
					if (CreateIfNull)
					{
						basket = CreateNewBasket(httpContext);
					}
				}

			}
			else
			{
				if (CreateIfNull)
				{
					basket = CreateNewBasket(httpContext);
				}
			}

			return basket;
		}

		private Basket CreateNewBasket(HttpContextBase httpContext)
		{
			Basket basket = new Basket();
			basketContext.Insert(basket);
			basketContext.Commit();

			HttpCookie cookie = new HttpCookie(BasketSessionName);
			cookie.Value = basket.Id;
			cookie.Expires = DateTime.Now.AddDays(1);
			httpContext.Response.Cookies.Add(cookie);

			return basket;
		}

		public void AddToBasket(HttpContextBase httpContext, string ProductId)
		{
			Basket basket = GetBasket(httpContext, true);
			BasketItems item = basket.BasketItems.FirstOrDefault(i => i.ProductId == ProductId);

			if (item == null)
			{
				item = new BasketItems()
				{
					BasketId = basket.Id,
					ProductId = ProductId,
					Quanity = 1,
				};

				basket.BasketItems.Add(item);
			}
			else
			{
				item.Quanity = item.Quanity + 1;
			}

			basketContext.Commit();
		}
		public void RemoveFromBasket(HttpContextBase httpContext, string itemId)
		{
			Basket basket = GetBasket(httpContext, true);
			BasketItems item = basket.BasketItems.FirstOrDefault(items => items.Id == itemId);

			if (item != null)
			{
				basket.BasketItems.Remove(item);
				basketContext.Commit();
			}
		}

		public List<BasketItemViewModel> GetBasketItems(HttpContextBase httpContext)
		{
			Basket basket = GetBasket(httpContext, false);

			if (basket != null)
			{
				var results = (from b in basket.BasketItems
							   join p in productContext.Collection() on b.ProductId equals p.Id
							   select new BasketItemViewModel()
							   {
								   Id = b.Id,
								   Quanity = b.Quanity,
								   ProductName = p.Name,
								   Image = p.Image,
								   Price = p.Price
							   }).ToList();
				return results;
			}
			else
			{
				return new List<BasketItemViewModel>();
			}


		}
		public BasketSummaryViewModel GetBasketSummary(HttpContextBase httpContext)
		{
			Basket basket = GetBasket(httpContext, false);
			BasketSummaryViewModel model = new BasketSummaryViewModel(0, 0);
			if (basket != null)
			{
				int? basketCount = (from item in basket.BasketItems
									select item.Quanity
									).Sum();

				decimal? basketTotal = (from item in basket.BasketItems
										join p in productContext.Collection() on item.ProductId equals p.Id
										select item.Quanity * p.Price).Sum();

				model.BasketCount = basketCount ?? 0;
				model.BasketTotal = basketTotal ?? decimal.Zero;

				return model;
			}
			else
			{
				return model;
			}
		}

		public void ClearBasket(HttpContextBase httpContext)
		{
			Basket basket = GetBasket(httpContext, false);
			basket.BasketItems.Clear();
			basketContext.Commit();
		}
	}
}
