﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MyShop2.Core.Contracts;
using MyShop2.Core.Models;
using MyShop2.DataAccess.InMemory;

namespace MyShop2.WebUI.Controllers
{
	public class ProductCategoryManagerController : Controller
	{
		IRepository<ProductCategory> context;

		public ProductCategoryManagerController(IRepository<ProductCategory> context)
		{
			this.context = context;
		}
		public ActionResult Index()
		{
			List<ProductCategory> productCategories = context.Collection().ToList();
			return View(productCategories);
		}

		public ActionResult Create()
		{
			ProductCategory productCategory = new ProductCategory();
			return View(productCategory);
		}
		[HttpPost]
		public ActionResult Create(ProductCategory productCategory)
		{
			if (!ModelState.IsValid)
			{
				return View(productCategory);
			}
			else
			{
				context.Insert(productCategory);
				context.Commit();

				return RedirectToAction("Index");
			}
		}

		public ActionResult Edit(string Id)
		{
			ProductCategory productCategory = context.Find(Id);
			if (productCategory == null)
			{
				return HttpNotFound();
			}
			else
			{
				return View(productCategory);
			}
		}
		[HttpPost]
		public ActionResult Edit(ProductCategory productCategory, string Id)
		{
			ProductCategory productCategoryToEdit = context.Find(Id);
			if (productCategoryToEdit == null)
			{
				return HttpNotFound();
			}
			else
			{
				if (!ModelState.IsValid)
				{
					return View(productCategory);
				}
				else
				{
					productCategoryToEdit.Category = productCategory.Category;

					context.Commit();

					return RedirectToAction("Index");
				}
			}
		}
		public ActionResult Delete(string Id)
		{
			ProductCategory productCategoryToDelete = context.Find(Id);
			if (productCategoryToDelete == null)
			{
				return HttpNotFound();

			}
			else
			{
				return View(productCategoryToDelete);
			}
		}
		[HttpPost]
		[ActionName("Delete")]
		public ActionResult ConfirmDelete(string Id)
		{
			ProductCategory productCategoryToDelete = context.Find(Id);
			if (productCategoryToDelete == null)
			{
				return HttpNotFound();

			}
			else
			{
				context.Delete(Id);
				context.Commit();
				return RedirectToAction("Index");
			}
		}
	}
}