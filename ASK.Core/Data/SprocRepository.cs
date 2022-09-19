
//https://medium.com/hackernoon/execute-a-stored-procedure-that-gets-data-from-multiple-tables-in-ef-core-1638a7f010c

using ASK.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Reflection;
using Wms.Domain.Net6;

namespace ASK.Core.Data
{
	public class SprocRepository : ISprocRepository
	{
		private readonly WmsDbContext _dbContext;

		public SprocRepository(WmsDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public DbCommand GetStoredProcedure(string name, params (string, object)[] nameValueParams)
		{
			return _dbContext
					.LoadStoredProcedure(name)
					.WithSqlParams(nameValueParams);
		}

		public DbCommand GetStoredProcedure(string name)
		{
			return _dbContext.LoadStoredProcedure(name);
		}
	}

	public class SprocRepository<TEntity> : SprocRepository, ISprocRepository<TEntity> where TEntity : class
	{
		public SprocRepository(WmsDbContext dbContext) : base(dbContext)
		{
		}

		public IList<TEntity> ExecuteStoredProcedure(DbCommand command)
		{
			return command.ExecuteStoredProcedure<TEntity>();
		}

		public Task<IList<TEntity>> ExecuteStoredProcedureAsync(DbCommand command)
		{
			return command.ExecuteStoredProcedureAsync<TEntity>();
		}
	}

	public static class SprocRepositoryExtensions
	{
		public static DbCommand LoadStoredProcedure(this DbContext context, string storedProcName)
		{
			var cmd = context.Database.GetDbConnection().CreateCommand();
			cmd.CommandText = storedProcName;
			cmd.CommandType = System.Data.CommandType.StoredProcedure;
			return cmd;
		}

		public static DbCommand WithSqlParams(this DbCommand cmd, params (string, object)[] nameValueParamPairs)
		{
			foreach (var pair in nameValueParamPairs)
			{
				var param = cmd.CreateParameter();
				param.ParameterName = pair.Item1;
				param.Value = pair.Item2 ?? DBNull.Value;
				cmd.Parameters.Add(param);
			}

			return cmd;
		}

		public static IList<T> ExecuteStoredProcedure<T>(this DbCommand command) where T : class
		{
			using (command)
			{
				if (command.Connection.State == System.Data.ConnectionState.Closed)
					command.Connection.Open();
				try
				{
					using (var reader = command.ExecuteReader())
					{
						return reader.MapToList<T>();
					}
				}
				finally
				{
					command.Connection.Close();
				}
			}
		}

		public static async Task<IList<T>> ExecuteStoredProcedureAsync<T>(this DbCommand command) where T : class
		{
			using (command)
			{
				if (command.Connection.State == System.Data.ConnectionState.Closed)
					await command.Connection.OpenAsync();
				try
				{
					using (var reader = command.ExecuteReader())
					{
						return reader.MapToList<T>();
					}
				}
				finally
				{
					command.Connection.Close();
				}
			}
		}

		private static IList<T> MapToList<T>(this DbDataReader dr)
		{
			var objList = new List<T>();
			var props = typeof(T).GetRuntimeProperties();

			var colMapping = dr.GetColumnSchema()
					.Where(x => props.Any(y => y.Name.ToLower() == x.ColumnName.ToLower()))
					.ToDictionary(key => key.ColumnName.ToLower());

			if (dr.HasRows)
			{
				while (dr.Read())
				{
					T obj = Activator.CreateInstance<T>();
					foreach (var prop in props)
					{
						var val = dr.GetValue(colMapping[prop.Name.ToLower()].ColumnOrdinal.Value);
						prop.SetValue(obj, val == DBNull.Value ? null : val);
					}
					objList.Add(obj);
				}
			}
			return objList;
		}
	}
}
