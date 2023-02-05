using WebReporter.Client.Components.Filter.ViewModels;

namespace Reporter.Client.Pages.Charts.OrderedVolume.ViewModels
{
    public class OrderedVolumeViewModel : ViewModelBase
    {
        public async Task LoadApplicationAsync()
        {
            ApplicationData.Applications = await Manager.Database.Applications[Database, Filter];

            var orderedVolumes = ApplicationData.Applications
                .Select(application => new { application.Volume, application.EndTime })
                .GroupBy(value => value.EndTime.ToString("dd.MM.yyyy"))
                .Select(value => value.Sum(item => item.Volume));

            var currentVolumes = ApplicationData.Applications
                .Select(application => new { application.VolumeCurrent, application.EndTime })
                .GroupBy(value => value.EndTime.ToString("dd.MM.yyyy"))
                .Select(value => value.Sum(item => item.VolumeCurrent));

            var labels = new string[] { "Объём заказанный", "Объём выполненный" };

            var dates = ApplicationData.Applications
                .Select(application => application.EndTime.ToString("dd.MM.yyyy"))
                .Distinct();

            await ChartJS.UpdateCharts();
            await ChartJS.OrderedVolumeChart(orderedVolumes, currentVolumes, dates, labels);
        }

        protected override Task OnInitializedAsync()
            => LoadApplicationAsync();
    }
}
