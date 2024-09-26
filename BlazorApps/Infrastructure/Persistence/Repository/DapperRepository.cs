using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Persistence;
using Dapper;
using Domain.Common.Contracts;
using Finbuckle.MultiTenant.EntityFrameworkCore;
using Infrastructure.Persistence.Context;
using System.Data;

namespace Infrastructure.Persistence.Repository
{
    public class DapperRepository : IDapperRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ICurrentUser _currentUser;

        public DapperRepository(ApplicationDbContext dbContext, ICurrentUser currentUser) => (_dbContext, _currentUser) = (dbContext, currentUser);

        public async Task<IReadOnlyList<T>> QueryAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
        where T : class, IEntity =>
            (await _dbContext.Connection.QueryAsync<T>(sql, param, transaction))
                .AsList();
        public async Task<IReadOnlyList<T>> QueryAsync<T>(string sql, bool isStoreProcedure, DynamicParameters? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
      where T : class, IEntity
        {
            if (param == null)
            {
                param = new DynamicParameters();
            }
            param.Add("@tenant", _currentUser.GetTenant());
            return (await _dbContext.Connection.QueryAsync<T>(sql, param, transaction, commandType: CommandType.StoredProcedure))
                .AsList();
        }
        public async Task<IReadOnlyList<T>> QueryWithOutEntityAsync<T>(string sql, bool isStoreProcedure, DynamicParameters? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
     where T : class
        {
            if (param == null)
            {
                param = new DynamicParameters();
            }
            param.Add("@tenant", _currentUser.GetTenant());
            return (await _dbContext.Connection.QueryAsync<T>(sql, param, transaction, commandType: CommandType.StoredProcedure))
                .AsList();
        }
        public async Task<IReadOnlyList<T>> QueryWithOutEntityAsync<T>(string sql, bool isStoreProcedure, DynamicParameters? param = null, int pageNumber = 0, int pageSize = 0, string keyword = null, string[]? OrderBy = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
           where T : class
        {
            if (param == null)
            {
                param = new DynamicParameters();
            }

            var SortColumn = OrderBy.Length == 0 ? "" : OrderBy[0].ToString().Split(" ").ToList()[0];
            var SortOrder = OrderBy.Length == 0 ? "" : OrderBy.Length == 1 ? OrderBy[0].ToString().Split(" ").ToList().Count > 1 ? string.IsNullOrWhiteSpace(OrderBy[0].ToString().Split(" ").ToList()[1]) ? "ASC" : OrderBy[0].ToString().Split(" ").ToList()[1] == "Ascending" ? "ASC" : "DESC" : "" : "";

            param.Add("@PageNumber", pageNumber);
            param.Add("@PageSize", pageSize);
            param.Add("@Keyword", keyword);
            param.Add("@SortColumn", SortColumn);
            param.Add("@SortOrder", SortOrder);

            param.Add("@tenant", _currentUser.GetTenant());
            return (await _dbContext.Connection.QueryAsync<T>(sql, param, transaction, commandType: CommandType.StoredProcedure))
                .AsList();
        }

        public async Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
        where T : class, IEntity
        {
            if (!_dbContext.Model.GetMultiTenantEntityTypes().Any(t => t.ClrType == typeof(T)))
            {
                sql = sql.Replace("@tenant", _dbContext.TenantInfo.Id);
            }

            var entity = await _dbContext.Connection.QueryFirstOrDefaultAsync<T>(sql, param, transaction);

            return entity ?? throw new NotFoundException(string.Empty);
        }
        public async Task<T?> QueryFirstOrDefaultAsync<T>(string sql, bool isStoreProcedure, DynamicParameters? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
   where T : class, IEntity
        {
            if (param == null)
            {
                param = new DynamicParameters();
            }
            param.Add("@tenant", _currentUser.GetTenant());
            var entity = await _dbContext.Connection.QueryFirstOrDefaultAsync<T>(sql, param, transaction, commandType: CommandType.StoredProcedure);

            return entity ?? throw new NotFoundException(string.Empty);
        }
        public async Task<T?> QueryFirstOrDefaultWithOutEntityAsync<T>(string sql, bool isStoreProcedure, DynamicParameters? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
 where T : class
        {
            if (param == null)
            {
                param = new DynamicParameters();
            }
            param.Add("@tenant", _currentUser.GetTenant());
            var entity = await _dbContext.Connection.QueryFirstOrDefaultAsync<T>(sql, param, transaction, commandType: CommandType.StoredProcedure);

            return entity ?? throw new NotFoundException(string.Empty);
        }

        public Task<T> QuerySingleAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
        where T : class, IEntity
        {
            if (!_dbContext.Model.GetMultiTenantEntityTypes().Any(t => t.ClrType == typeof(T)))
            {
                sql = sql.Replace("@tenant", _dbContext.TenantInfo.Id);
            }

            return _dbContext.Connection.QuerySingleAsync<T>(sql, param, transaction);
        }
        public Task<T> QuerySingleAsync<T>(string sql, bool isStoreProcedure, DynamicParameters? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
        where T : class, IEntity
        {
            if (param == null)
            {
                param = new DynamicParameters();
            }
            param.Add("@tenant", _currentUser.GetTenant());
            return _dbContext.Connection.QuerySingleAsync<T>(sql, param, transaction, commandType: CommandType.StoredProcedure);
        }
        public Task<T> QuerySingleWithOutEntityAsync<T>(string sql, bool isStoreProcedure, DynamicParameters? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
        where T : class
        {
            if (param == null)
            {
                param = new DynamicParameters();
            }
            param.Add("@tenant", _currentUser.GetTenant());
            return _dbContext.Connection.QuerySingleAsync<T>(sql, param, transaction, commandType: CommandType.StoredProcedure);
        }
    }
}