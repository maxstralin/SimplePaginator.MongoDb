using MongoDB.Driver.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SimplePaginator
{
    public interface IMongoPaginationService : IPaginationService
    {
        /// <summary>
        /// Paginate an <seealso cref="IMongoQueryable"/> with a default page count implemented
        /// </summary>
        /// <typeparam name="T">Entry type</typeparam>
        /// <param name="query">The query to paginate</param>
        /// <param name="page">Page number, starting at 1</param>
        /// <param name="pageSize">Number of entries per page</param>
        /// <returns>Paginated query with page numbers calculated</returns>
        Task<IPaginationResult<IMongoQueryable<T>>> PaginateAsync<T>(IMongoQueryable<T> query, int page, int pageSize);

        /// <summary>
        /// Paginate an IQueryable with a page count returned using a custom count function
        /// </summary>
        /// <typeparam name="T">Entry type</typeparam>
        /// <param name="query">The query to paginate</param>
        /// <param name="page">Page number, starting at 1</param>
        /// <param name="pageSize">Number of entries per page</param>
        /// <param name="countFunction">Custom count function</param>
        /// <returns>Paginated query with page numbers calculated</returns>
        IPaginationResult<IMongoQueryable<T>> Paginate<T>(IMongoQueryable<T> query, int page, int pageSize, Func<IMongoQueryable<T>, int> countFunction);
        /// <summary>
        /// Paginate an IQueryable with a page count returned using a custom async count function
        /// </summary>
        /// <typeparam name="T">Entry type</typeparam>
        /// <param name="query">The query to paginate</param>
        /// <param name="page">Page number, starting at 1</param>
        /// <param name="pageSize">Number of entries per page</param>
        /// <param name="asyncCountFunction">Custom async count function</param>
        /// <returns>Paginated query with page numbers calculated</returns>
        Task<IPaginationResult<IMongoQueryable<T>>> PaginateAsync<T>(IMongoQueryable<T> query, int page, int pageSize, Func<IMongoQueryable<T>, Task<int>> asyncCountFunction);
    }
}