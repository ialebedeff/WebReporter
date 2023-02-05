using Entities;
using MudBlazor;
using WebReporter.Client.Components.Filter.ViewModels;

namespace Reporter.Client.Pages.Consumption.Material.ViewModels
{
    public class ConsumptionByMaterialViewModel : ViewModelBase
    {
        public int ResultsCount { get; set; }
        public MudTable<Entities.Application>? ApplicationTable { get; set; }
        public MudTable<Entities.ComponentExpenditure>? ComponentsTable { get; set; }
        public Task ReloadApplicationAsync()
           => ApplicationTable?
             .ReloadServerData()?
             .ContinueWith(task => ComponentsTable?
             .ReloadServerData()) ?? Task.CompletedTask;

        public async Task<TableData<Entities.Application>> ReloadApplications(TableState state)
        {
            if (Filter.StartTime is not null && Filter.EndTime is not null)
            {
                ApplicationData.Applications = await Manager.Database.Applications
                    .GetApplicationsAsync(
                    Filter,
                    state.Page * state.PageSize,
                    state.PageSize,
                    Database);

                ResultsCount = await Manager.Database.Applications
                    .GetApplicationsCountAsync(
                    Filter, Database);

                foreach (var application in ApplicationData.Applications)
                {
                    application.Client = ApplicationData.Clients?
                        .FirstOrDefault(client => client.Id == application.ClientId);
                    application.Car = ApplicationData.Cars?
                        .FirstOrDefault(car => car.Id == application.CarId);
                }

                return new TableData<Entities.Application>() { TotalItems = ResultsCount, Items = ApplicationData.Applications };
            }

            return new TableData<Entities.Application> { Items = Enumerable.Empty<Entities.Application>() };
        }

        public async Task<TableData<ComponentExpenditure>> ReloadRecipeExpenditures(TableState state)
        {
            if (Filter.StartTime is not null && Filter.EndTime is not null)
            {
                var rawExpenditures = await Manager.Database.Components.GetComponentExpendituresAsync(Filter, Database);

                return new TableData<ComponentExpenditure>() { TotalItems = rawExpenditures.Count(), Items = rawExpenditures };
            }

            return new TableData<ComponentExpenditure> { Items = Enumerable.Empty<ComponentExpenditure>() };
        }
    }
}
