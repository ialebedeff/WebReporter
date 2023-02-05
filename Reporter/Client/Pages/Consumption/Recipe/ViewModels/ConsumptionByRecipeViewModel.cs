using Entities;
using MudBlazor;
using Org.BouncyCastle.Security.Certificates;
using WebReporter.Client.Components.Filter.ViewModels;

namespace Reporter.Client.Pages.Consumption.Recipe.ViewModels
{
    public class ConsumptionByRecipeViewModel : ViewModelBase
    {
        public int ResultsCount { get; set; }
        public MudTable<Entities.Application>? ApplicationTable { get; set; }
        public MudTable<RecipeExpenditure>? RecipeExpenditureTable { get; set; }
        public Task ReloadApplicationAsync()
        {
            _ = ApplicationTable?.ReloadServerData();
            _ = RecipeExpenditureTable?.ReloadServerData();
            _ = CreateRecipeExpendituresChartAsync();

            return Task.CompletedTask;
        }

        protected override Task OnInitializedAsync()
            => ReloadApplicationAsync();
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

        public async Task<TableData<RecipeExpenditure>> ReloadRecipeExpenditures(TableState state)
        {
            if (Filter.StartTime is not null && Filter.EndTime is not null)
            {
                ApplicationData.RecipeExpenditures = await Manager.Database.Recipes
                    .GetRecipesExpendituresByFilterAsync(Filter, Database);

                return new TableData<RecipeExpenditure>() { TotalItems = ApplicationData.RecipeExpenditures.Count(), Items = ApplicationData.RecipeExpenditures };
            }

            return new TableData<RecipeExpenditure> { Items = Enumerable.Empty<RecipeExpenditure>() };
        }

        public async Task CreateRecipeExpendituresChartAsync()
        {
            ApplicationData.RecipeExpenditures = await Manager.Database.Recipes
                        .GetRecipesExpendituresByFilterAsync(Filter, Database);

            if (ApplicationData.RecipeExpenditures is not null)
            {
                var volume = ApplicationData.RecipeExpenditures.Select(expenditure => expenditure.Volume);
                var recipes = ApplicationData.RecipeExpenditures.Select(expenditure => string.Join("", expenditure.RecipeName.Take(20)));
                var volumeFact = ApplicationData.RecipeExpenditures.Select(expenditure => expenditure.VolumeFact);
                var labels = new string[] { "Объём", "Объём по факту" };

                await ChartJS.UpdateCharts();
                await ChartJS.OrderedVolumeChart(volume, volumeFact, recipes, labels);
            }
        }
    }
}
