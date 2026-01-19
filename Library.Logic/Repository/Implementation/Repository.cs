using Library.Domain.DataBase;
using Library.Domain.Models;
using Library.Logic.Repository.Abstraction;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Library.Logic.Repository.Implementation
{
	public class Repository<T> : IRepository<T> where T : class
	{
		private readonly BookDbContext _dbContext;
		private readonly DbSet<T> _dbSet;

		public Repository(BookDbContext dbContext)
		{
			_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
			_dbSet = dbContext.Set<T>();
		}

		public async Task<IQueryable<T>> GetAllAsync()
		{
			return await Task.FromResult(_dbSet.AsQueryable());
		}

		public async Task<IQueryable<T>> GetByConditionAsync(Expression<Func<T, bool>> expression)
		{
			return await Task.FromResult(_dbSet.Where(expression).AsQueryable());
		}
		public async Task<T> GetSingleByConditionAsync(Expression<Func<T, bool>> expression)
		{
			return await _dbSet.FirstOrDefaultAsync(expression);
		}
		public async Task<T> GetByIdAsync(int id)
		{
			return await _dbSet.FindAsync(id);
		}

		public async Task InsertAsync(T entity)
		{
			await _dbSet.AddAsync(entity);
			await _dbContext.SaveChangesAsync();
		}

		public async Task UpdateAsync(T entity)
		{
			_dbContext.Entry(entity).State = EntityState.Modified;
			var count = await _dbContext.SaveChangesAsync();
		}

		public async Task DeleteAsync(Expression<Func<T, bool>> expression)
		{
			var entity = await _dbSet.FirstOrDefaultAsync(expression);
			if (entity != null)
			{
				_dbSet.Remove(entity);
				await _dbContext.SaveChangesAsync();
			}
			// Optional: throw exception if entity not found
		}

		public async Task DeleteAsync(T entity)
		{
			_dbSet.Remove(entity);
			await _dbContext.SaveChangesAsync();
		}


		private string connectionString = "Server=192.168.41.50;Database=CpcAppDb;User Id=AOP_USER;Password=Password%125P;"; // Replace with your actual connection string


		public async Task<bool> Any(Expression<Func<T, bool>> expression)
		{
			return await Task.FromResult(_dbSet.Any(expression));
		}


		public async Task<int> Insert(T entity)
		{
			await _dbSet.AddAsync(entity);
			int affectedRows = await _dbContext.SaveChangesAsync();

			return affectedRows;
		}

		public async Task<int> Update(T entity)
		{
			_dbContext.Entry(entity).State = EntityState.Modified;
			int affectedrows = await _dbContext.SaveChangesAsync();

			return affectedrows;
		}

		public async Task<IEnumerable<T>> SearchAsync(string searchTerm)
		{
			if (string.IsNullOrWhiteSpace(searchTerm))
				return await GetAllAsync();

			// For Books entity specifically
			if (typeof(T) == typeof(Books))
			{
				return await _dbSet.Cast<Books>()
					.Where(b => b.Title.Contains(searchTerm) || b.Author.Contains(searchTerm))
					.Cast<T>()
					.ToListAsync();
			}

			// Generic fallback - override in specific repositories
			return await GetAllAsync();
		}
	}
}
