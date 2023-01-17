var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOutputCache(options =>
{
    options.AddBasePolicy(builder =>
        builder.Expire(TimeSpan.FromSeconds(10)));
    
    options.AddPolicy("Expire20", builder =>
        builder.Expire(TimeSpan.FromSeconds(20)));
    
    options.AddPolicy("Expire30", builder =>
        builder.Expire(TimeSpan.FromSeconds(30)));
    
    options.AddPolicy("Query", builder =>
        {
            builder.SetVaryByQuery("city");
            builder.Expire(TimeSpan.FromSeconds(5));
        });
    
    options.AddPolicy("NoCache", builder => 
        builder.NoCache());
    
    options.AddPolicy("NoLock", builder => 
        builder.SetLocking(false));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseOutputCache();

app.UseAuthorization();

app.MapControllers();

app.Run();
