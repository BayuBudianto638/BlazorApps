using Dapper;
using System.Data;

namespace Application.Common.Persistence
{
    public interface IDapperRepository : ITransientService
    {
        /// <summary>
        /// Get an <see cref="IReadOnlyList{T}"/> using raw sql string with parameters.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="sql">The sql string.</param>
        /// <param name="param">The paramters in the sql string.</param>
        /// <param name="transaction">The transaction to be performed.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Returns <see cref="Task"/> of <see cref="IReadOnlyCollection{T}"/>.</returns>
        Task<IReadOnlyList<T>> QueryAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
        where T : class, IEntity;
        Task<IReadOnlyList<T>> QueryAsync<T>(string sql, bool isStoreProcedure, DynamicParameters? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
       where T : class, IEntity;
        Task<IReadOnlyList<T>> QueryWithOutEntityAsync<T>(string sql, bool isStoreProcedure, DynamicParameters? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
     where T : class;

        /// <summary>
        /// Get a <typeparamref name="T"/> using raw sql string with parameters.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="sql">The sql string.</param>
        /// <param name="param">The paramters in the sql string.</param>
        /// <param name="transaction">The transaction to be performed.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Returns <see cref="Task"/> of <typeparamref name="T"/>.</returns>
        Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
        where T : class, IEntity;
        Task<T?> QueryFirstOrDefaultAsync<T>(string sql, bool isStoreProcedure, DynamicParameters? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
        where T : class, IEntity;
        Task<T?> QueryFirstOrDefaultWithOutEntityAsync<T>(string sql, bool isStoreProcedure, DynamicParameters? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
               where T : class;
        Task<IReadOnlyList<T>> QueryWithOutEntityAsync<T>(string sql, bool isStoreProcedure, DynamicParameters? param = null, int pageNumber = 0, int pageSize = 0, string keyword = null, string[]? OrderBy = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)

                 //ALTER PROCEDURE Catalog.GetAllSubscription
                 // @tenant NVARCHAR(20),
                 //@PageNumber INT,
                 //@PageSize INT,
                 //@Keyword NVARCHAR(100),
                 //@SortColumn NVARCHAR(20) = 'Name',
                 //@SortOrder NVARCHAR(20) = 'ASC',
                 // @AssignUserId NVARCHAR(MAX)
                 //AS

                 //BEGIN
                 //  SET NOCOUNT ON;

                 //  IF(@PageNumber <= 0)
                 //  BEGIN
                 //    SET @PageNumber = 1;
                 //  END
                 //  IF(@PageSize <= 0)
                 //  BEGIN
                 //    SET @PageSize = 2147483647;
                 //  END
                 //  DECLARE @SkipRows INT = (@PageNumber - 1) * @PageSize;

                 //  ;
                 //  WITH _data
                 //  AS
                 //  (SELECT
                 //      Subscriptions.Id
                 //     , Subscriptions.Name
                 //     , Subscriptions.SubscriptionTypeId
                 //     , SubscriptionTypes.Name AS SubscriptionTypeName
                 //     , Subscriptions.LanguageId
                 //     , Languages.Name AS LanguageName
                 //     , Subscriptions.StartDate
                 //     , Subscriptions.EndDate
                 //     , Subscriptions.NoOfDays
                 //     , Subscriptions.PaymentTypeId
                 //     , PaymentTypes.Name AS PaymentTypeName
                 //     , Subscriptions.Remark
                 //     , Subscriptions.Amount
                 //     , Subscriptions.ReferenceNumber
                 //     , Subscriptions.ImagePath
                 //     , Subscriptions.CreatedOn

                 //    FROM Catalog.Subscriptions
                 //    INNER JOIN Master.SubscriptionTypes
                 //      ON Subscriptions.SubscriptionTypeId = SubscriptionTypes.Id
                 //    INNER JOIN Master.Languages
                 //      ON Subscriptions.LanguageId = Languages.Id
                 //    INNER JOIN Master.PaymentTypes
                 //      ON Subscriptions.PaymentTypeId = PaymentTypes.Id

                 //    WHERE Subscriptions.[DeletedBy] IS NULL
                 //    AND Subscriptions.TenantId = @tenant
                 //    AND AssignUserId = @AssignUserId
                 //    AND (@Keyword IS NULL
                 //    OR Subscriptions.Name LIKE '%' + @Keyword + '%'
                 //    OR SubscriptionTypes.Name LIKE '%' + @Keyword + '%'
                 //    OR Languages.Name LIKE '%' + @Keyword + '%'
                 //    OR PaymentTypes.Name LIKE '%' + @Keyword + '%'
                 //    OR Subscriptions.Remark LIKE '%' + @Keyword + '%'
                 //    OR Subscriptions.Amount LIKE '%' + @Keyword + '%')),

                 //  _count
                 //  AS
                 //  (SELECT
                 //      COUNT(1) AS TotalCount
                 //    FROM _data)

                 //  SELECT
                 //    *
                 //  FROM _data
                 //  CROSS APPLY _count
                 //  -- ORDER BY _data.CreatedOn DESC

                 //  ORDER BY CASE
                 //    WHEN(@SortColumn = 'Name' AND
                 //      @SortOrder = 'ASC') THEN _data.Name
                 //  END ASC,
                 //  CASE
                 //    WHEN (@SortColumn = 'Name' AND
                 //      @SortOrder = 'DESC') THEN _data.Name
                 //  END DESC,
                 //  CASE
                 //    WHEN (@SortColumn = 'SubscriptionTypeName' AND
                 //      @SortOrder = 'ASC') THEN _data.SubscriptionTypeName
                 //  END ASC,
                 //  CASE
                 //    WHEN (@SortColumn = 'SubscriptionTypeName' AND
                 //      @SortOrder = 'DESC') THEN _data.SubscriptionTypeName
                 //  END DESC,
                 //  CASE
                 //    WHEN (@SortColumn = 'LanguageName' AND
                 //      @SortOrder = 'ASC') THEN _data.LanguageName
                 //  END ASC,
                 //  CASE
                 //    WHEN (@SortColumn = 'LanguageName' AND
                 //      @SortOrder = 'DESC') THEN _data.LanguageName
                 //  END DESC,
                 //  CASE
                 //    WHEN (@SortColumn = 'PaymentTypeName' AND
                 //      @SortOrder = 'ASC') THEN _data.PaymentTypeName
                 //  END ASC,
                 //  CASE
                 //    WHEN (@SortColumn = 'PaymentTypeName' AND
                 //      @SortOrder = 'DESC') THEN _data.PaymentTypeName
                 //  END DESC,
                 //  CASE
                 //    WHEN (@SortColumn = 'Amount' AND
                 //      @SortOrder = 'ASC') THEN _data.Amount
                 //  END ASC,
                 //  CASE
                 //    WHEN (@SortColumn = 'Amount' AND
                 //      @SortOrder = 'DESC') THEN _data.Amount
                 //  END DESC

                 //  OFFSET @SkipRows ROWS
                 //  FETCH NEXT @PageSize ROWS ONLY
                 //  RETURN

                 //END
                 //GO

                 where T : class;









        /// <summary>
        /// Get a <typeparamref name="T"/> using raw sql string with parameters.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="sql">The sql string.</param>
        /// <param name="param">The paramters in the sql string.</param>
        /// <param name="transaction">The transaction to be performed.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Returns <see cref="Task"/> of <typeparamref name="T"/>.</returns>
        Task<T> QuerySingleAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
        where T : class, IEntity;
        Task<T> QuerySingleAsync<T>(string sql, bool isStoreProcedure, DynamicParameters? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
      where T : class, IEntity;
        Task<T> QuerySingleWithOutEntityAsync<T>(string sql, bool isStoreProcedure, DynamicParameters? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
      where T : class;


    }
}