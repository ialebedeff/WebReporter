using CommunicationMessageQueueManager;
using CommunicationService;
using Entities;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Reporter.Client.wwwroot;

namespace WebReporter.Client.Components.Filter.ViewModels
{
    public class ViewModelBase : ComponentBase
    {
        [Inject] public IDialogService DialogService { get; set; } = null!;
        [Inject] public ApplicationData ApplicationData { get; set; } = null!;
        [Inject] public SignalClient Signal { get; set; } = null!;
        [Inject] public RequestManager Manager { get; set; } = null!;
        [Inject] public DatabaseConnectionInfo Database { get; set; } = null!;
        [Inject] public FilterData Filter { get; set; } = null!;
        [Inject] public ChartJS ChartJS { get; set; } = null!;
    }
}
