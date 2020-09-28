# SimplePagination.MongoDb

Extension of [SimplePaginator](https://github.com/maxstralin/SimplePaginator), see more documentation there.

Enables better pagination for `IMongoQueryable<T>`. Main difference is that there's a default count implemented for `PaginateAsync(page, pageSize)` through the MongoDb driver's `CountAsync()`

## Installation

Use Visual Studio Package Manager or 
```bash
Install-Package SimplePaginator.MongoDb
```

## Usage
Generally, you should use the async methods: `PaginateAsync()`.  
Have a look at the tests for more examples.

### Extension methods
```csharp
IMongoQueryable source = db.GetCollection<DbClass>("some-collection").AsQueryable();

//Returns an IPaginationResult with the first 15 entries, where the page count is calculated using the MongoDb driver's CountAsync() function 
IPaginationResult paginated = source.PaginateAsync(page: 1, pageSize: 15);
```

You can also use an async custom function
```
//Returns an IPaginationResult with the first 15 entries, where the page count is calculated using a custom async function.
IPaginationResult paginated = await source.PaginateAsync(page: 1, pageSize: 15, (q) => Task.FromResult(50));
```

### Dependency injection
Behind the scenes, the default service simply calls the extensions methods so there's no practical difference.
**Startup.cs**
```csharp
services.AddSimpleMongoPaginator(); //Adds IMongoPaginationService to DI
services.AddSimpleMongoPaginator(true); //Adds IMongoPaginationService AND also registers it as IPaginationService to DI
```

```csharp
//Injected through DI
public void Example(IMongoPaginationService pagination) {
 //Presumably you have some MongoDb services here which return an IMongoQueryable<T>
}
```

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

Please make sure to update tests as appropriate.

## License
[MIT](https://choosealicense.com/licenses/mit/)