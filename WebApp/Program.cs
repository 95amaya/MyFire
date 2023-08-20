using Services;
using Services.CoreLibraries;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddSingleton<IDbConnectionManager, MySqlDbConnectionManager>((serviceProvider) =>
{
    var connectionString = builder.Configuration.GetConnectionString("MySqlLocal") ?? string.Empty;
    return new MySqlDbConnectionManager(connectionString);
});
builder.Services.AddSingleton<IBillTransactionDao, BillTransactionDaoDb>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();
