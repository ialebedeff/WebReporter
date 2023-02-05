using Blazorise;
using Blazorise.Charts;
using CommunicationMessageQueueManager;
using CommunicationService;
using Entities;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudBlazor.Utilities;
using SmartMixWebReporter.Client.Pages.Report.ViewModels;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SmartMixWebReporter.Client.Pages.Consumption.ViewModels.Bunker
{

    public class BunkerConsumptionViewModel : ReportComponentBase
    {
        public BarChart<float>? _barChart;
        [Inject] public DatabaseConnectionInfo Database { get; set; } = null!;
        public IEnumerable<BunkerExpenditure>? Expenditures { get; set; }
        public IEnumerable<int>? Sensors { get; set; }
        public IReadOnlyCollection<string>? Labels { get; set; }
        public BarChartDataset<float>? Dataset { get; set; }
        public int? SelectedSensor { get; set; }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                Sensors = await GetLevelSensorNumbersAsync();
                SelectedSensor = Sensors?.FirstOrDefault() ?? 0;
            }
        }
        public new async Task ReloadDataAsync()
        {
            Expenditures = await GetBunkerExpendituresAsync(SelectedSensor);
            Dataset = await GenerateDatasetAsync(Expenditures);
            Labels = GenerateLabels(Expenditures);

            await InsertChartDataAsync(Labels, await GenerateDatasetAsync(Expenditures));
        }
        private async Task<IEnumerable<int>> GetLevelSensorNumbersAsync()
            => await Manager.Database.LevelSensors.GetAllSensorNumbersAsync(Database);
        private async Task<IEnumerable<BunkerExpenditure>> GetBunkerExpendituresAsync(int? levelSensorNumber)
        {
            if (FilterData.StartTime is null || 
                FilterData.EndTime is null || 
                levelSensorNumber is null)
            {
                return Enumerable.Empty<BunkerExpenditure>();
            }

            return await Manager.Database.LevelSensors.GetExpenditureByLevelSensorNumberAsync(
                (int)levelSensorNumber,
                (DateTime)FilterData.StartTime,
                (DateTime)FilterData.EndTime,
                isTrainMode: false,
                Database);
        }

        private Task<BarChartDataset<float>> GenerateDatasetAsync(IEnumerable<BunkerExpenditure> bunkerExpenditures)
        {
            return Task.FromResult(new BarChartDataset<float>()
            {
                Data = bunkerExpenditures
                .Select(expenditure => expenditure.Remains)
                .ToList()
            });
        }
        private IReadOnlyCollection<string> GenerateLabels(IEnumerable<BunkerExpenditure> bunkerExpenditures)
            => bunkerExpenditures
            .Select(expenditure => expenditure.Date.ToString())
            .ToList()
            .AsReadOnly();
        private async Task InsertChartDataAsync(IReadOnlyCollection<string> labels, BarChartDataset<float> barChartDataset)
            => await _barChart?.AddLabelsDatasetsAndUpdate(labels, barChartDataset);
    }
}
