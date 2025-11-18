namespace AzureUdemy;

public interface IStartupServices
{
    IStartupConfigured ConfigureServices(Action<IServiceCollection, ConfigurationManager> configureServices);
    IStartupApp ConfigureApp(Action<WebApplication> configureApp);
}

public interface IStartupConfigured
{
    IStartupApp ConfigureApp(Action<WebApplication> configureApp);
}

public interface IStartupApp
{
    void Run();
}
public class StartupBuilder(WebApplicationBuilder builder) : IStartupServices, IStartupConfigured, IStartupApp
{
    private WebApplication? _app;
    public WebApplicationBuilder Builder { get; init; } = builder;

    public IStartupConfigured ConfigureServices(Action<IServiceCollection, ConfigurationManager> configureServices)
    {
        configureServices(this.Builder.Services, this.Builder.Configuration);
        return this;
    }

    public IStartupApp ConfigureApp(Action<WebApplication> configureApp)
    {
        this._app = this.Builder.Build();
        configureApp(_app);
        return this;
    }

    public void Run() => this._app!.Run();
}
