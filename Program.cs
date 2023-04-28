using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddControllersWithViews();
builder.Services.AddMvcCore();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseFileServer(new FileServerOptions()
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), @"node_modules")),
    RequestPath = new PathString("/node_modules"),
    EnableDirectoryBrowsing = true
});

app.UseFileServer(new FileServerOptions()
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), @"Styles")),
    RequestPath = new PathString("/Styles"),
    EnableDirectoryBrowsing = true
});

app.UseFileServer(new FileServerOptions()
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), @"dist")),
    RequestPath = new PathString("/dist"),
    EnableDirectoryBrowsing = true
});

app.UseStaticFiles();

app.Run();
