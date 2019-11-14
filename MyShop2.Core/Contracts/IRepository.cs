using System.Linq;
using MyShop2.Core.Models;

namespace MyShop2.Core.Contracts
{
	public interface IRepository<T> where T : BaseEntity
	{
		IQueryable<T> Collection();
		void Commit();
		void Delete(string Id);
		T Find(string Id);
		void Insert(T t);
		void Update(T t);
	}
}