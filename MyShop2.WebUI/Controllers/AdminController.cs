using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShop2.WebUI.Controllers
{
	public class AdminController : Controller
	{
		[Authorize(Roles = "Admin")]
		public ActionResult Index()
		{
			return View();
		}
	}
}