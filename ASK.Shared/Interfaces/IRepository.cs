namespace ASK.Shared.Interfaces;

public interface IRepository<T> where T : class
{
	IQueryable<T> Entities { get; }

	Task<T> GetByIdAsync( int id );

	Task<List<T>> GetAllAsync();

	Task<List<T>> GetPagedResponseAsync( int pageNumber, int pageSize );

	Task<T> AddAsync( T entity );

	Task UpdateAsync( T entity );

	Task DeleteAsync( T entity );

	Task DeleteAsync( params T[] entities );
}

