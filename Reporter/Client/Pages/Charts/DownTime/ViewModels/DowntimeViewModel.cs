using Entities;
using WebReporter.Client.Components.Filter.ViewModels;

namespace Reporter.Client.Pages.Charts.DownTime.ViewModels
{
    public class DowntimeViewModel : ViewModelBase
    {
        public IEnumerable<Entities.Application> Applications { get; set; }

        protected override Task OnInitializedAsync()
            => ReloadDataAsync();
        public Task ReloadDataAsync()
        {
            _ = ChartJS.UpdateCharts();
            _ = LoadDataAsync();
            _ = ReloadChartAsync();

            return Task.CompletedTask;
        }
        private async Task LoadDataAsync()
        {
            Applications = await Manager.Database.Applications[Database, Filter];
        }

        private Task ReloadChartAsync()
        {
            if (Applications is not null)
            {
                var times = Applications.Select(application => (application.EndTime - application.StartTime));
                var startTime = Applications.Min(application => application.StartTime);
                var endTime = Applications.Max(application => application.EndTime);
                var workedTime = new TimeSpan(
                    times.Sum(t => t.Days),
                    times.Sum(t => t.Hours),
                    times.Sum(t => t.Minutes),
                    times.Sum(t => t.Seconds));

                var allTime = new TimeSpan(((DateTime)Filter.EndTime - (DateTime)Filter.StartTime).Ticks);

                _ = ChartJS.DowntimePieChart(new long[] { workedTime.Ticks, allTime.Ticks - workedTime.Ticks }, new string[]
                {   $"Рабочее время: {string.Format("{0:##.#}", workedTime.TotalHours)} часов",
                    $"Время простоя: {string.Format("{0:##.#}",allTime.TotalHours - workedTime.TotalHours)} часов" },
                    new string[] { "Рабочее время", "Время простоя" });
            }

            return Task.CompletedTask;
        }
    }
}
