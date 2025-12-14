using ApiFaceUnah;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

//Get connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

//Add the DbContext and configure MYSQL 
builder.Services.AddDbContext<DBContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);

var port = Environment.GetEnvironmentVariable("WEBSITES_PORT") ?? "8080";
builder.WebHost.UseUrls($"http://*:{port}");

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//verify database connection on startup
try
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<DBContext>();
    db.Database.CanConnect();

    Console.WriteLine("Conectado a Mysql correctamente");

}
catch (Exception ex)
{
    Console.WriteLine("Error al conectar a Mysql:" + ex.Message);
}

app.Run();
