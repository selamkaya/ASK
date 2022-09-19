using ASK.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using Wms.Domain.Entities;
using Wms.Domain.Net6;

namespace ASK.Core.Data;

public class Repository<T> : IRepository<T> where T : EntityBase
{
	private readonly WmsDbContext _dbContext;

	public Repository(WmsDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public IQueryable<T> Entities => _dbContext.Set<T>();

	public async Task<T> AddAsync(T entity)
	{
		await _dbContext.Set<T>().AddAsync(entity);
		return entity;
	}

	public async Task<List<T>> GetAllAsync()
	{
		return await _dbContext
				.Set<T>()
				.ToListAsync();
	}

	public async Task<T> GetByIdAsync(int id)
	{
		return await _dbContext.Set<T>().FindAsync(id);
	}

	public async Task<List<T>> GetPagedResponseAsync(int pageNumber, int pageSize)
	{
		return await _dbContext
				.Set<T>()
				.Skip((pageNumber - 1) * pageSize)
				.Take(pageSize)
				.AsNoTracking()
				.ToListAsync();
	}

	public Task UpdateAsync(T entity)
	{
		if (entity == null)
			throw new ArgumentNullException("entity");

		T exist = _dbContext.Set<T>().Find(entity.Id);
		_dbContext.Entry(exist).CurrentValues.SetValues(entity);
		return Task.CompletedTask;
	}

	public Task DeleteAsync(T entity)
	{
		if (entity == null)
			throw new ArgumentNullException("entity");

		_dbContext.Set<T>().Remove(entity);
		return Task.CompletedTask;
	}

	public Task DeleteAsync(params T[] entities)
	{
		_dbContext.Set<T>().RemoveRange(entities);

		return Task.CompletedTask;
	}
}