using Dierentuin42.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// DATABASE CONTEXT CONFHIGURATIE
builder.Services.AddDbContext<Dierentuin42Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Dierentuin42Context")
        ?? throw new InvalidOperationException("Connection string 'Dierentuin42Context' not found.")));

// CONTAINER SERVICE
builder.Services.AddControllersWithViews();

var app = builder.Build();

// DATA SEEDER
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var context = services.GetRequiredService<Dierentuin42Context>();

        // DATABASE CHECK EN LAATSTE WIJZIGINGEN MIGREREN
        context.Database.Migrate();

        // OFFICIEEL DATA SEEDEN
        DataSeeder.Seed(services, context);
    }
    catch (Exception ex)
    {
        // TEST FOUT LOG
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Error seeden database");
    }
}

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection(); 
app.UseStaticFiles();      

app.UseRouting();          

app.UseAuthorization();    

// STNADAARDROUTE
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
