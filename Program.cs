using agileworks_1.Interceptors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging.Abstractions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddControllersWithViews();
builder.Services.AddMvcCore(c => c.Filters.Add(new TicketsCounter()));

// TODO: interceptor ?

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

/*app.UseFileServer(new FileServerOptions()
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), @"styles")),
    RequestPath = new PathString("/styles"),
    EnableDirectoryBrowsing = true
});

app.UseFileServer(new FileServerOptions()
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), @"dist")),
    RequestPath = new PathString("/dist"),
    EnableDirectoryBrowsing = true
});

app.UseFileServer(new FileServerOptions()
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), @"fonts")),
    RequestPath = new PathString("/fonts"),
    EnableDirectoryBrowsing = true
});*/

app.UseStaticFiles();

app.Run();
