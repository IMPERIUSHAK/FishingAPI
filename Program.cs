using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore;
using listip.data;
using System.Net.Http;
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<listipDbContext>(options => {
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    if (string.IsNullOrEmpty(connectionString))
        throw new InvalidOperationException("Connection string 'DefaultConnection' is missing.");

    options.UseMySQL(connectionString);
});

var app = builder.Build();

app.MapGet("/", async (HttpContext context, listipDbContext db) =>
{
    using var httpClient = new HttpClient();

    
     var ip = context.Request.Headers["X-Forwarded-For"].ToString();

    
    if (string.IsNullOrEmpty(ip))
    {
        ip = context.Connection.RemoteIpAddress?.ToString();
    }
    
    var locationResponse = await httpClient.GetStringAsync($"http://ip-api.com/json/{ip}");

    
    db.IP.Add(new listip.IP { Adress = ip });
    await db.SaveChangesAsync();

   
    context.Response.ContentType = "text/html";
    await context.Response.WriteAsync($"<h2>Congrats! Connected to the server.info saved:<br>{locationResponse}</h2>");
});


//метод app run перехватывает абсолютно все маршруты поэтому лучше разделять запросы по отдельным маршрутам
// app.Run(async context =>
// {
//     using var httpClient = new HttpClient();
//     var ipAddress = await httpClient.GetStringAsync("https://api.ipify.org");

//     await context.Response.WriteAsync($"You got hacked your IP: {ipAddress}");
// });
app.MapGet("/adresses", async (listipDbContext db) =>
{
    var adresses = await db.IP.ToListAsync();
    return Results.Json(adresses);
});

app.Run();

///
    // Получаем IP-адрес пользователя локальной сети
    //var ipAddress = context.Request.Headers["X-Forwarded-For"].FirstOrDefault() 
                    //?? context.Connection.RemoteIpAddress?.ToString();
    