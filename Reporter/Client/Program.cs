using CommunicationMessageQueueManager;
using CommunicationService;
using Entities;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor.Services;
using Reporter.Client;
using Reporter.Client.wwwroot;
using WebReporter.Client.Components.Filter.ViewModels;

var builder = WebAssemblyHostBuilder.CreateDefault(args);


var signalClient = new SignalClient(
    string.Format("{0}{1}", builder.HostEnvironment.BaseAddress, "proxy"), connection => {
        connection.On<Message<List<Dictionary<string, object>>>>("DatabaseExecutionResult", (message) =>
        {
            MessageQueueManager.Instance?.InsertUnworkedMessage(message);
        });
    });

var messageQueue = new MessageQueueManager(signalClient);
var requestManager = new RequestManager(messageQueue);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddSingleton<SignalClient>(signalClient);
builder.Services.AddSingleton<MessageQueueManager>(messageQueue);
builder.Services.AddSingleton<RequestManager>(requestManager);
builder.Services.AddSingleton<DatabaseConnectionInfo>(new DatabaseConnectionInfo()
{
    Database = "most",
    User = "root",
    Password = "1q2w3e4r5T",
    Server = "localhost"
});
builder.Services.AddMudServices();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddSingleton<FilterData>();
builder.Services.AddSingleton<ApplicationData>();
builder.Services.AddSingleton<ChartJS>();

await builder.Build().RunAsync();
