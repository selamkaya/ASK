using ASK.Shared.Interfaces;
using System.Collections;
using Wms.Domain.Net6;

namespace ASK.Core.Data
{
	public class UnitOfWork : IUnitOfWork
	{
		//private readonly ILoginService _currentUserService;
		private readonly WmsDbContext _dbContext;
		private bool disposed;
		private Hashtable _repositories;
		//private readonly IAppCache _cache;

		public UnitOfWork(WmsDbContext dbContext) //, ICurrentUserService currentUserService, IAppCache cache )
		{
			_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
			//_currentUserService = currentUserService;
			//_cache = cache;
		}

		public IRepository<TEntity> Repository<TEntity>() where TEntity : class
		{
			if (_repositories == null)
				_repositories = new Hashtable();

			var type = typeof(TEntity).Name;

			if (!_repositories.ContainsKey(type))
			{
				var repositoryType = typeof(Repository<>);

				var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _dbContext);

				_repositories.Add(type, repositoryInstance);
			}

			return (IRepository<TEntity>)_repositories[type];
		}

		public async Task<int> Commit(CancellationToken cancellationToken)
		{
			return await _dbContext.SaveChangesAsync(cancellationToken);
		}

		public async Task<int> CommitAndRemoveCache(CancellationToken cancellationToken, params string[] cacheKeys)
		{
			var result = await _dbContext.SaveChangesAsync(cancellationToken);
			foreach (var cacheKey in cacheKeys)
			{
				//_cache.Remove( cacheKey );
			}
			return result;
		}

		public Task Rollback()
		{
			_dbContext.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
			return Task.CompletedTask;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposed)
			{
				if (disposing)
				{
					//dispose managed resources
					_dbContext.Dispose();
				}
			}
			//dispose unmanaged resources
			disposed = true;
		}
	}
}
