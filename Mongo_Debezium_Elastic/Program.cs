using Mongo_Monstache_Elastic.Data;
using Mongo_Monstache_Elastic.Data.Models;
using MongoDB.Driver;
using Nest;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton(sp =>
{
    var settings = new ConnectionSettings(new Uri(builder.Configuration.GetConnectionString("Elastic")!)).DefaultIndex("usersdb.users");

    return new ElasticClient(settings);
});

builder.Services.AddSingleton<IMongoClient>(sp =>
{
    return new MongoClient(builder.Configuration.GetConnectionString("Database"));
});
builder.Services.AddScoped<DbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();
SeedUsers(app);


app.Run();

void SeedUsers(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<DbContext>();
    var filter = Builders<User>.Filter.Empty;
    if (dbContext.Users.CountDocuments(filter, null) > 0) return;
    dbContext.Users.InsertManyAsync(dbContext.FakedUsers()).GetAwaiter().GetResult();
}