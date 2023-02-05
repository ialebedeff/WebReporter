using BridgeService;
using Microsoft.AspNetCore.ResponseCompression;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddRazorPages();
builder.Services.AddSignalR(options => {
    options.MaximumReceiveMessageSize = 1024 * 1024 * 32;
    options.MaximumParallelInvocationsPerClient = 100;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();



app.MapHub<BridgeHub>("/proxy");
app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
