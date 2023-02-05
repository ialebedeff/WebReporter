using CommunicationMessageQueueManager;
using CommunicationService;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using SmartMixWebReporter.Client.Pages.Filter;

namespace SmartMixWebReporter.Client.Pages.Report.ViewModels
{
    public class ReportComponentBase : ReporterComponentBase
    {
        public bool IsLoading => LoadingData.Any(x => !x.IsCompleted);
        public List<Task> LoadingData { get; set; }
            = new List<Task>();
        public void AddLoadingTask(Task task)
            => LoadingData.Add(task);
        public async Task AwaitLoadingAsync()
            => await Task.WhenAll(LoadingData);
        protected virtual Task ReloadDataAsync()
            => Task.CompletedTask;
    }

    public class ReporterComponentBase : MudComponentBase
    {
        [Inject] public RequestManager Manager { get; set; } = null!;
        [Inject] public SignalClient SignalClient { get; set; } = null!;
        [Inject] public FilterData FilterData { get; set; } = null!;
    }
}
