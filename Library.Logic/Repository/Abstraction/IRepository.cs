using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Library.Logic.Repository.Abstraction
{
	public interface IRepository<T> where T : class
	{
		Task<T> GetSingleByConditionAsync(Expression<Func<T, bool>> expression);
		Task<IQueryable<T>> GetAllAsync();
		Task<IQueryable<T>> GetByConditionAsync(Expression<Func<T, bool>> expression);
		Task<T> GetByIdAsync(int id);
		Task InsertAsync(T entity);
		Task<int> Insert(T entity);
		Task UpdateAsync(T entity);
		Task<int> Update(T entity);
		Task DeleteAsync(T entity);
		Task<IEnumerable<T>> SearchAsync(string searchTerm);
		Task<bool> Any(Expression<Func<T, bool>> expression);
		Task DeleteAsync(Expression<Func<T, bool>> expression);
	}
}
