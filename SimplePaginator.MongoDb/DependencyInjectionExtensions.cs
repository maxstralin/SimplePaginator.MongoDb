using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimplePaginator.MongoDb
{
    public static class DependencyInjectionExtensions
    {
        /// <summary>
        /// Adds the default implementation of <seealso cref="IMongoPaginationService"/>
        /// </summary>
        /// <param name="addBaseInterface">If true, will also register itself as <seealso cref="IPaginationService"/></param>
        public static void AddSimpleMongoPaginator(this IServiceCollection services, bool addBaseInterface = false)
        {
            services.AddTransient<IMongoPaginationService, MongoPaginationService>();
            if (addBaseInterface) services.AddTransient<IPaginationService, MongoPaginationService>();
        }
    }
}
