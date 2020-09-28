using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplePaginator.MongoDb
{
    public class MongoPaginationService : PaginationService, IMongoPaginationService
    {
        ///<inheritdoc />
        public Task<IPaginationResult<IMongoQueryable<T>>> PaginateAsync<T>(IMongoQueryable<T> query, int page, int pageSize) => query.PaginateAsync(page, pageSize);

        ///<inheritdoc/>
        public IPaginationResult<IMongoQueryable<T>> Paginate<T>(IMongoQueryable<T> query, int page, int pageSize, Func<IMongoQueryable<T>, int> countFunction) => query.Paginate(page, pageSize, countFunction);

        ///<inheritdoc/>
        public Task<IPaginationResult<IMongoQueryable<T>>> PaginateAsync<T>(IMongoQueryable<T> query, int page, int pageSize, Func<IMongoQueryable<T>, Task<int>> asyncCountFunction) => query.PaginateAsync(page, pageSize, asyncCountFunction);
    }
}
