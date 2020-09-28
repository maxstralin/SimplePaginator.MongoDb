using Microsoft.Extensions.DependencyInjection;
using Mongo2Go;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Moq;
using SimplePaginator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimplePaginator.MongoDb.Tests
{
    public class ServiceTests : IClassFixture<MongoRunner>
    {
        private IMongoQueryable<int> Data { get; }
        private IMongoPaginationService PaginationService { get; } = new MongoPaginationService();

        public ServiceTests(MongoRunner runner)
        {
            Data = runner.Data;
        }

        [Theory]
        [InlineData(1, 50, 1, 50)]
        [InlineData(1, 25, 1, 25)]
        [InlineData(2, 25, 26, 50)]
        [InlineData(2, 50, 51, 100)]
        public async Task BasicPagination(int page, int pageSize, int start, int end)
        {
            var expected = Enumerable.Range(start, end - start + 1);

            var result = await PaginationService.PaginateAsync(Data, page, pageSize);

            Assert.Equal(page, result.Page);
            Assert.Equal(pageSize, result.PageSize);
            Assert.Equal(expected, result.Entries.AsEnumerable());
        }

        [Theory]
        [InlineData(25, 50, 2)]
        [InlineData(10, 50, 5)]
        [InlineData(12, 50, 5)]
        [InlineData(20, 50, 3)]
        public void PageCount(int pageSize, int totalEntries, int expectedPages)
        {
            var range = Enumerable.Range(1, totalEntries).AsQueryable();
            var result = PaginationService.Paginate(range, 1, pageSize);

            Assert.Equal(expectedPages, result.PageCount);
        }

        [Theory]
        [InlineData(200, 2)]
        [InlineData(90, 5)]
        public void CustomCountPageCount(int pageSize, int expectedPages)
        {
            var range = Enumerable.Range(1, 10).AsQueryable();

            var result = PaginationService.Paginate(range, 1, pageSize, (x) => 400);

            Assert.Equal(expectedPages, result.PageCount);
        }

        [Theory]
        [InlineData(200, 2)]
        [InlineData(90, 5)]
        public async Task CustomAsyncCountPageCount(int pageSize, int expectedPages)
        {
            var range = Enumerable.Range(1, 10).AsQueryable();

            var result = await PaginationService.PaginateAsync(range, 1, pageSize, (x) => Task.FromResult(400));

            Assert.Equal(expectedPages, result.PageCount);
        }

        [Fact]
        public void CorrectlyDerived()
        {
            Assert.IsAssignableFrom<PaginationService>(PaginationService);
            Assert.IsAssignableFrom<IPaginationService>(PaginationService);
            Assert.IsAssignableFrom<IMongoPaginationService>(PaginationService);

            Assert.IsType<MongoPaginationService>(PaginationService);
        }

        [Fact]
        public void DependencyInjectionAddsPaginationService()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSimpleMongoPaginator();
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var foundService = serviceProvider.GetRequiredService<IMongoPaginationService>();

            Assert.NotNull(foundService);
            Assert.IsAssignableFrom<IMongoPaginationService>(foundService);
            Assert.IsType<MongoPaginationService>(foundService);
        }

        [Fact]
        public void DependencyInjectionDoesntAddBaseImplementation()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSimpleMongoPaginator();
            var serviceProvider = serviceCollection.BuildServiceProvider();

            Assert.Throws<InvalidOperationException>(() => serviceProvider.GetRequiredService<IPaginationService>());
        }

        [Fact]
        public void DependencyInjectionWithBase()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSimpleMongoPaginator(true);
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var foundBaseService = serviceProvider.GetRequiredService<IPaginationService>();

            Assert.NotNull(foundBaseService);
            Assert.IsAssignableFrom<IMongoPaginationService>(foundBaseService);
            Assert.IsAssignableFrom<IPaginationService>(foundBaseService);
            Assert.IsType<MongoPaginationService>(foundBaseService);
        }

    }
}
