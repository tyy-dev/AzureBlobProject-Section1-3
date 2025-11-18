using Azure.Storage.Blobs;
using AzureUdemy.Services;

namespace AzureUdemy;

public class Startup(WebApplicationBuilder builder)
{
    public WebApplicationBuilder Builder { get; init; } = builder;

    public IServiceCollection Services => this.Builder.Services;
    public ConfigurationManager Configuration => this.Builder.Configuration;

    public Startup ConfigureServices()
    {
        this.Services.AddControllersWithViews();

        this.Services.AddSingleton(_ => new BlobServiceClient(this.Configuration.GetValue<string>("BlobConnection")));

        this.Services.AddSingleton<IContainerService, ContainerService>();
        this.Services.AddSingleton<IBlobService, BlobService>();

        return this;
    }

    public WebApplication ConfigureApp()
    {
        WebApplication app = this.Builder.Build();
        this.Configure(app);
        return app;
    }

    public void Configure(WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
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
    }
}

