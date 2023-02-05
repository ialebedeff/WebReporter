using Entities;
using MudBlazor;
using System.Data;
using WebReporter.Client.Components.Filter.ViewModels;

namespace Reporter.Client.Pages.Consumption.Bunker.ViewModels
{
    public class ConsumptionByBunkerViewModel : ViewModelBase
    {
        public IEnumerable<BunkerExpenditure>? Expenditures { get; set; }
        public bool IsChartUpdating { get; set; }
        public bool IsChartLoaded { get; set; }
        public bool IsDataLoaded { get; set; }
        public int ResultsCount { get; set; }
        public MudTable<BunkerExpenditure>? Table { get; set; }
        public Task ReloadServerDataAsync()
        {
            Filter.SelectedSensor = ApplicationData.Sensors?.FirstOrDefault();

            _ = Table?.ReloadServerData();
            _ = ReloadChartsAsync();

            return Task.CompletedTask;
        }

        protected override Task OnInitializedAsync()
            => ReloadServerDataAsync();
        public async Task<TableData<BunkerExpenditure>> ReloadExpendituresAsync(TableState state)
        {
            if (Filter.StartTime is not null && Filter.EndTime is not null && Filter.SelectedSensor is not null)
            {
                ResultsCount = await Manager.Database.LevelSensors.GetExpenditureCountAsync(
                    (int)Filter.SelectedSensor,
                    (DateTime)Filter.StartTime,
                    (DateTime)Filter.EndTime,
                    Filter.IsTrainMode, Database);

                var expenditures = await Manager.Database.LevelSensors.GetExpenditureByLevelSensorNumberAsync(
                    (int)Filter.SelectedSensor, 
                    (DateTime)Filter.StartTime, 
                    (DateTime)Filter.EndTime, 
                    Filter.IsTrainMode, Database,
                    state.PageSize * state.Page,
                    state.PageSize);

                IsDataLoaded = true;
                
                await InvokeAsync(StateHasChanged);

                return new TableData<BunkerExpenditure> { Items = expenditures, TotalItems = ResultsCount };
            }

            return new TableData<BunkerExpenditure> { Items = Enumerable.Empty<BunkerExpenditure>() };
        }

        public async Task ReloadChartsAsync()
        {
            Expenditures = await Manager.Database.LevelSensors.GetExpenditureByLevelSensorNumberAsync(
                    (int)Filter.SelectedSensor,
                    (DateTime)Filter.StartTime,
                    (DateTime)Filter.EndTime,
                    Filter.IsTrainMode, Database,
                    0, int.MaxValue);

            _ = ChartJS.UpdateCharts();
            _ = LoadChartsAsync();
            _ = LoadRemainsChartAsync();
            _ = InvokeAsync(StateHasChanged);
        }
        private async Task LoadRemainsChartAsync()
        {
            if (Expenditures is not null && Filter.SelectedSensor is not null)
            {
                var labels = Expenditures.Select(expenditure => expenditure.Date.ToString("dd.MM.yyyy"));
                var categories = new int[] { (int)Filter.SelectedSensor };
                var values = Expenditures
                   .Select(expenditure => new
                   {
                       Remains = expenditure.Remains,
                   });
                var remainsDataset = new Dictionary<int, IEnumerable<BunkerExpenditure>>();

                remainsDataset.Add((int)Filter.SelectedSensor, values.Select(exp => new BunkerExpenditure()
                {
                    Remains = exp.Remains
                }));

                await ChartJS.BunkerRemainsChart(remainsDataset, labels, categories);
            }
        }

        private async Task LoadChartsAsync()
        {
            if (Expenditures is not null && Filter.SelectedSensor is not null)
            {
                var labels = Expenditures
                    .Select(expenditure => expenditure.Date.ToString("dd.MM.yyyy"))
                    .Distinct();

                var categories = new int[] { 
                    (int)Filter.SelectedSensor 
                };

                var values = Expenditures
                    .Select(expenditure => new {
                        Expenditures = expenditure.Expenditures,
                        Date = expenditure.Date.ToString("dd.MM.yyyy")})
                    .GroupBy(expenditure => expenditure.Date);

                var expendituresDataset = new Dictionary<int, IEnumerable<BunkerExpenditure>>(); 

                expendituresDataset.Add((int)Filter.SelectedSensor, values.Select(exp => new BunkerExpenditure()
                {
                    Expenditures = exp.Sum(exp => exp.Expenditures)
                }));

                await ChartJS.BunkerExpendituresChart(expendituresDataset, labels, categories);
            }
        }
    }
}
