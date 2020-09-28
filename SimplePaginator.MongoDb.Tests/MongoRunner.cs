using Mongo2Go;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimplePaginator.MongoDb.Tests
{
    public class MongoRunner
    {
        public IMongoQueryable<int> Data => Collection.AsQueryable().Select(a => a.Value);

        private MongoDbRunner Runner;
        IMongoCollection<TestValue> Collection;
        public MongoRunner()
        {
            Runner = MongoDbRunner.Start();
            var mongoClient = new MongoClient(Runner.ConnectionString);
            var db = mongoClient.GetDatabase("test");
            Collection = db.GetCollection<TestValue>("test");
            Collection.InsertMany(Enumerable.Range(1, 200).Select(a => new TestValue { Value = a }));
        }
    }
}
