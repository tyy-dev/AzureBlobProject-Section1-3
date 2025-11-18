using AzureUdemy;

WebApplication app = new Startup(WebApplication.CreateBuilder(args)).ConfigureServices().ConfigureApp();
app.Run();
