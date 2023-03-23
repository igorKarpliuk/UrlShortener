using Microsoft.IdentityModel.Tokens;
using UrlShortener.Core.Interfaces;
using UrlShortener.Core.Models;
using UrlShortener.Web.StartupExtensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=About}/{id?}");
app.MapGet("/{path}", async (
    string path,
    IGenericRepository<URL> urlRepository)
    =>
        {
            var url =  urlRepository.Get(u => u.ShortUrl == path).FirstOrDefault();
            if(url == null || string.IsNullOrEmpty(url.Url))
            {
                return Results.NotFound();
            }
            return Results.Redirect(url.Url);
        });
app.Run();
