# Introduction
*Output Caching* was one of my favorite features in .Net Framework. Unfortunately  output caching was not available *.Net Core* until .Net 7 version.

The output caching middleware enables caching of HTTP responses. .Net saves the cached entries on the server. Thus, you have more control over caching.

The output caching middleware can be used in all types of ASP.NET Core apps: Minimal API, Web API with controllers, MVC, and Razor Pages.

# Basics
Add output cache middleware to services and then to the processing pipeline as below.

    builder.Services.AddOutputCache();
	.....
    app.UseOutputCache();
    
Add `[OutputCache]` attribute to your controller.

	[HttpGet(Name = "GetWeatherForecast Default")]
    [OutputCache]
    public IEnumerable<WeatherForecast> GetDefault()
    {
    ....
    }
For a `minimal  API`, you can configure ir as below.

    app.MapGet("/cached", Gravatar.WriteGravatar).CacheOutput();
    app.MapGet("/attribute", [OutputCache] (context) => 
        Gravatar.WriteGravatar(context));

As you can see, it is very easy to add output caching. The next step is configuration.

# Configuration
There are simply two places for configuring the output cache.
- `AddOutputCache`
- `OutputCacheAttribute`

## AddOutputCache
Add your policies while calling `AddOutputCache`.

	builder.Services.AddOutputCache(options =>
	{
	    options.AddBasePolicy(builder =>
	        builder.Expire(TimeSpan.FromSeconds(10)));
    
	    options.AddPolicy("Expire20", builder =>
	        builder.Expire(TimeSpan.FromSeconds(20)));
    
	    options.AddPolicy("Expire30", builder =>
	        builder.Expire(TimeSpan.FromSeconds(30)));    
	});

`AddBasePolicy` is the default policy. If nothing is specified, this policy will be used. 
`builder.Expire(TimeSpan.FromSeconds(10)` specifies when the cached entry will expire.

For `Controllers`, you can specify your policy in the attribute.

    [HttpGet("get-bypolicy", Name = "GetWeatherForecast Policy")]
    [OutputCache(PolicyName = "Expire20")]
    public IEnumerable<WeatherForecast> GetPolicy()
    {
	....
	}

For a `minimal API`, you can use as below.

    app.MapGet("/20", Gravatar.WriteGravatar).CacheOutput("Expire20");
	app.MapGet("/30", [OutputCache(PolicyName = "Expire30")] (context) => 
    Gravatar.WriteGravatar(context));

# Cache Key
You can control your cache key by below options.

-   [SetVaryByQuery](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.outputcaching.outputcachepolicybuilder.setvarybyquery)  - Specify one or more query string names to add to the cache key.
-   [SetVaryByHeader](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.outputcaching.outputcachepolicybuilder.setvarybyheader)  - Specify one or more HTTP headers to add to the cache key.
- [VaryByValue](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.outputcaching.outputcachepolicybuilder.varybyvalue)- Specify a value to add to the cache key. The following example uses a value that indicates whether the current server time in seconds is odd or even. A new response is generated only when the number of seconds goes from odd to even or even to odd.

For example, you want the cache temperatures by city. You can achieve this by using SetVaryByQuery. You can specify this in either `AddOutputCache` or  `OutputCache` attribute.

    [HttpGet("{city}", Name = "GetWeatherForecast Query")]
    [OutputCache(VaryByQueryKeys = new[] { "city" })]
    public IEnumerable<WeatherForecast> GetByQuery(string city)
    {
    ...
    }

# What can you cache?
By default, output caching follows these rules:

-   Only HTTP 200 responses are cached.
-   Only HTTP GET or HEAD requests are cached.
-   Responses that set cookies aren't cached.
-   Responses to authenticated requests aren't cached.

# Conclusion
Output caching is a very simple and powerful feature of .Net 7. With a little effort, you can improve the performance of your applications.

# References
- https://learn.microsoft.com/en-us/aspnet/core/performance/caching/output?view=aspnetcore-7.0
- https://learn.microsoft.com/en-us/aspnet/core/performance/caching/overview?view=aspnetcore-7.0

