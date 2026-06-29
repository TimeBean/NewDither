using Dither.ServiceDefaults;
using Service.WebSite.Domain.Service;
using Service.Website.Infrastructure.Service;
using Tailwind;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddControllersWithViews();
builder.UseTailwindCli();

builder.Services.AddHttpClient<IDitherService, HttpDitherService>("DitherApiClient", client =>
    {
        client.BaseAddress = new Uri("http+https://ditherApi");
    })
    .AddServiceDiscovery();

builder.Services.AddHttpClient<IQuoteService, HttpQuoteService>("QuoteApiClient", client =>
    {
        client.BaseAddress = new Uri("http+https://quoteApi");
    })
    .AddServiceDiscovery();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();