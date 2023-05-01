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

app.UseStaticFiles();

app.Run();
