using Dierentuin42.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<Dierentuin42Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Dierentuin42Context") ?? throw new InvalidOperationException("Connection string 'Dierentuin42Context' not found.")));

builder.Services.AddControllers();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Seed data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<Dierentuin42Context>();
    DataSeeder.Seed(services, context);  // Hier wordt de seeder aangeroepen
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.MapControllers();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
