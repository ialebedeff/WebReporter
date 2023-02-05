using CommunicationMessageQueueManager;
using Entities;
using Microsoft.AspNetCore.Components;

namespace WebReporter.Client.Components.Filter.ViewModels
{
    public class ApplicationData
    {
        public ApplicationData(
            RequestManager requestManager, 
            DatabaseConnectionInfo databaseConnectionInfo) {
            RequestManager = requestManager;
            Database = databaseConnectionInfo;

        }
        [Inject] public RequestManager RequestManager { get; set; } = null!;
        [Inject] public DatabaseConnectionInfo Database { get; set; } = null!;

        public IEnumerable<Entities.Client>? Clients { get; set; }
        public IEnumerable<Entities.Component>? Components { get; set; }
        public IEnumerable<Entities.Car>? Cars { get; set; }
        public IEnumerable<Entities.Recipe>? Recipes { get; set; }
        public IEnumerable<Entities.Recipe>? ArchiveRecipes { get; set; }
        public IEnumerable<Application>? Applications { get; set; }
        public IEnumerable<int>? Sensors { get; set; }
        public IEnumerable<RecipeExpenditure>? RecipeExpenditures { get; set; }

        public bool IsLoaded { get; set; }
        public Task LoadAsync()
        {
            return Task.WhenAll(new Task[]
            {
                LoadAllRecipes(),
                LoadAllCarsAsync(),
                LoadAllSensorsAsync(),
                LoadAllClientsAsync(),
                LoadAllComponentsAsync(),
            }).ContinueWith(task => {
                IsLoaded = true;
            });
        }
        private async Task LoadAllClientsAsync()
           => Clients = await RequestManager.Database.Clients.GetAllClientsAsync(Database);
        private async Task LoadAllComponentsAsync()
            => Components = await RequestManager.Database.Components.GetAllComponentsAsync(Database);
        private async Task LoadAllCarsAsync()
            => Cars = await RequestManager.Database.Cars.GetAllCarsAsync(Database);
        private async Task LoadAllRecipes()
            => Recipes = await RequestManager.Database.Recipes.GetAllRecipesAsync(Database);
        private async Task LoadAllArchiveRecipesAsync()
            => ArchiveRecipes = await RequestManager.Database.Recipes.GetAllArchiveRecipesAsync(Database);
        private async Task LoadAllSensorsAsync()
            => Sensors = await RequestManager.Database.LevelSensors.GetAllSensorNumbersAsync(Database);
    }
}
