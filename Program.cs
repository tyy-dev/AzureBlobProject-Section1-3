using AzureUdemy;
using Azure.Storage.Blobs;
using AzureUdemy.Services;

new StartupBuilder(WebApplication.CreateBuilder(args)).ConfigureServices((IServiceCollection services, ConfigurationManager config) =>
{
    services.AddControllersWithViews();

    services.AddSingleton(_ => new BlobServiceClient(config.GetValue<string>("BlobConnection")));

    services.AddSingleton<IContainerService, ContainerService>();
    services.AddSingleton<IBlobService, BlobService>();

}).ConfigureApp((WebApplication app) =>
{
    if (app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
    }

    app.UseHttpsRedirection();

    app.UseStaticFiles();
    app.UseDefaultFiles();

    app.UseRouting();
    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
}).Run();
