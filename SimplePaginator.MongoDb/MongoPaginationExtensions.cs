using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver.Linq;
using System;
using System.Threading.Tasks;

namespace SimplePaginator.MongoDb
{
    public static class MongoPaginationExtensions
    {
        ///<inheritdoc cref="MongoPaginationService.Paginate{T}(IMongoQueryable{T}, int, int, Func{IMongoQueryable{T}, int})"/>
        public static IPaginationResult<IMongoQueryable<T>> Paginate<T>(this IMongoQueryable<T> query, int page, int pageSize, Func<IMongoQueryable<T>, int> countFunction)
        {
            var entriesCount = countFunction(query);
            return CreateResult(page, pageSize, query, entriesCount);
        }

        ///<inheritdoc cref="MongoPaginationService.PaginateAsync{T}(IMongoQueryable{T}, int, int)" />
        public static async Task<IPaginationResult<IMongoQueryable<T>>> PaginateAsync<T>(this IMongoQueryable<T> query, int page, int pageSize)
        {
            var entriesCount = await query.CountAsync();
            return CreateResult(page, pageSize, query, entriesCount);
        }

        public static async Task<IPaginationResult<IMongoQueryable<T>>> PaginateAsync<T>(this IMongoQueryable<T> query, int page, int pageSize, Func<IMongoQueryable<T>, Task<int>> asyncCountFunction)
        {
            var entriesCount = await asyncCountFunction(query);
            return CreateResult(page, pageSize, query, entriesCount);
        }

        private static IPaginationResult<IMongoQueryable<T>> CreateResult<T>(int page, int pageSize, IMongoQueryable<T> query, int entriesCount)
        {
            return new PaginationResult<IMongoQueryable<T>>(page, pageSize, query.Skip((page - 1) * pageSize).Take(pageSize), entriesCount);
        }

        

    }
}
