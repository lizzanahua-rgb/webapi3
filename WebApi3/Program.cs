using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using WebApi3.Data;
using WebApi3.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//parametrizacion de DB
var connectionString =builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseMySQL(connectionString);//para usar mysql como motor de base de datos
});

var app = builder.Build();

//creacion BD
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
    
    //aqui mostramos datos
    var blogs = context.Blogs.ToList();//se transformo en un inser
    foreach (var blog in blogs )
    {
        Console.WriteLine("Consulta 1: " + blog.Url);
    }
    
    //consulta 2 de 1 
    var blog2 = context.Blogs
        .Single(b=>b.BlogId==1);
    Console.WriteLine("Consulta 2: " + blog2.Url);//como un where

    //consulta 3
    var blogs3 = context.Blogs
        .Where(b => b.Url.Contains("u"))//lo que encuentre en el db con esa especificacion
        .ToList();
    foreach (var blog in blogs )
    {
        Console.WriteLine("Consulta 3: " + blog.Url);
    }
    
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.MapScalarApiReference();//Agregado para usar scalar
app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}