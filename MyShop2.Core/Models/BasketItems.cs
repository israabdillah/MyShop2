using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop2.Core.Models
{
	public class BasketItems : BaseEntity
	{
		public string BasketId { get; set; }
		public string ProductId { get; set; }
		public string Name { get; set; }
		public int Quanity { get; set; }
	}
}
