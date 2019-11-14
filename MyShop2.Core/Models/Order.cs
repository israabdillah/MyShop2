﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop2.Core.Models
{
	public class Order : BaseEntity
	{
		public Order() {
			this.OrderItems = new List<OrderItems>();
		}
		public string FirstName { get; set; }
		public string Surname { get; set; }
		public string Email { get; set; }
		public string Street { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string ZipCode { get; set; }
		public string OrderStatus { get; set; }
		public virtual ICollection<OrderItems> OrderItems { get; set; }
	}
}
