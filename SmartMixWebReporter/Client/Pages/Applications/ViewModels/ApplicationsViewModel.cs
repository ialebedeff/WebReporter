using CommunicationMessageQueueManager;
using CommunicationService;
using Entities;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using SmartMixWebReporter.Client.Pages.Filter;
using SmartMixWebReporter.Client.Pages.Report.ViewModels;

namespace SmartMixWebReporter.Client.Pages.Applications.ViewModels
{
    public class ApplicationsViewModel : ReportComponentBase
    {
        [Inject] SignalClient SignalClient { get; set; } = null!;
        [Inject] RequestManager RequestManager { get; set; } = null!;
        [Inject] DatabaseConnectionInfo Database { get; set; } = null!;
        public List<Application> Applications { get; set; }
            = new List<Application>();
        protected async override Task OnInitializedAsync()
            => await ReloadDataAsync();

        protected override async Task ReloadDataAsync()
        {
            if (FilterData.StartTime is not null && FilterData.EndTime is not null)
            {
                AddLoadingTask(SignalClient.StartAsync());

                await InvokeAsync(async () => Applications = new List<Application>(
                        await RequestManager.Database.Applications.GetApplicationsAsync(
                        (DateTime)FilterData.StartTime, (DateTime)FilterData.EndTime, Database)));
                await AwaitLoadingAsync();
                await InvokeAsync(StateHasChanged);
            }
        }
    }
}
