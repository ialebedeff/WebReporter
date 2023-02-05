using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using CommunicationMessageHandler;
using CommunicationMessageQueueManager;
using CommunicationService;
using Entities;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor.Services;
using SmartMixWebReporter.Client;
using SmartMixWebReporter.Client.Pages.Filter;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddMudServices();


var signalClient = new SignalClient(
    string.Format("{0}{1}",builder.HostEnvironment.BaseAddress, "proxy") , connection => {
        connection.On<Message<List<Dictionary<string, object>>>>("DatabaseExecutionResult", (message) =>
        {
            MessageQueueManager.Instance?.InsertUnworkedMessage(message);
        });
    });

var messageQueue = new MessageQueueManager(signalClient);
var requestManager = new RequestManager(messageQueue);

builder.Services.AddBlazorise(options =>
{
    options.Immediate = true;
}).AddBootstrapProviders()
    .AddFontAwesomeIcons(); 
builder.Services.AddSingleton<SignalClient>(signalClient);
builder.Services.AddSingleton<MessageQueueManager>(messageQueue);
builder.Services.AddSingleton<RequestManager>(requestManager);
builder.Services.AddSingleton<DatabaseConnectionInfo>(new DatabaseConnectionInfo()
{
    Database = "210203_mirastroy",
    User = "root",
    Password = "1q2w3e4r5T",
    Server = "localhost"
});
builder.Services.AddScoped<Entities.FilterData>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
