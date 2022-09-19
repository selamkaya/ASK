namespace ASK.Shared.Interfaces;

public interface IUnitOfWork : IDisposable
{
	IRepository<T> Repository<T>() where T : class;

	Task<int> Commit( CancellationToken cancellationToken );

	Task<int> CommitAndRemoveCache( CancellationToken cancellationToken, params string[] cacheKeys );

	Task Rollback();
}
