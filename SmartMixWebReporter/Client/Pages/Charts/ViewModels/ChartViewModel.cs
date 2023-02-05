using CommunicationMessageQueueManager;
using CommunicationService;
using Entities;
using Humanizer;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using SmartMixWebReporter.Client.Pages.Report.ViewModels;

namespace SmartMixWebReporter.Client.Pages.Charts.ViewModels
{
    public class ChartData
    {
        public string[]? Labels { get; set; }
        public List<ChartSeries> Series { get; set; } = new();
    }
    public class ChartViewModel : ReportComponentBase
    {
        [Inject] RequestManager RequestManager { get; set; } = null!;
        [Inject] DatabaseConnectionInfo Database { get; set; } = null!;
        public IEnumerable<Application>? Applications { get; set; }
        public ChartData ChartData { get; set; } = new();

        protected override async Task OnInitializedAsync()
            => await ReloadDataAsync();

        protected override async Task ReloadDataAsync()
        {
            Applications = await RequestManager.Database.Applications.GetApplicationsAsync(
             (DateTime)FilterData.StartTime, (DateTime)FilterData.EndTime, Database);

            ConvertToSeries();

            await InvokeAsync(StateHasChanged);
        }
        private void ConvertToSeries()
        {
            if (Applications is null)
            {
                return;
            }

            ChartData.Labels = Applications
                .Select(app => app.StartTime.ToString("yyyy.MM.dd"))
                .Distinct()
                .ToArray();

            var orderedVolumes = new List<double>();
            var completedVolumes = new List<double>();

            var ordered = new ChartSeries() { Name = "Заказанный" };
            var completed = new ChartSeries() { Name = "Выполненный" };

            foreach (var label in ChartData.Labels)
            {
                orderedVolumes.Add(Applications
                    .Where(app => app.StartTime.ToString("yyyy.MM.dd") == label)
                    .Select(app => (double)app.Volume)
                    .Sum());

                completedVolumes.Add(Applications
                    .Where(app => app.StartTime.ToString("yyyy.MM.dd") == label)
                    .Select(app => (double)app.VolumeCurrent)
                    .Sum());
            }
            ChartData.Labels = ChartData.Labels
                .Select(label => string.Format("{0}", label, Convert.ToDateTime(label).Humanize()))
                .ToArray();

            ordered.Data = orderedVolumes.ToArray();
            completed.Data = completedVolumes.ToArray();

            ChartData.Series.Add(ordered);
            ChartData.Series.Add(completed);
        }
    }
}
