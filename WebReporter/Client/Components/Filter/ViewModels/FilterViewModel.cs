using CommunicationMessageQueueManager;
using CommunicationService;
using Entities;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace WebReporter.Client.Components.Filter.ViewModels
{
    public class FilterData
    {
        public DateTime? StartTime { get; set; } = DateTime.Now.AddDays(-7);
        public DateTime? EndTime { get; set; } = DateTime.Now;
        public Entities.Car? SelectedCar { get; set; }
        public Entities.Client? SelectedClient { get; set; }
        public Entities.Component? SelectedComponent { get; set; }
    }
   
    public class FilterViewModel : ViewModelBase
    {
        public IEnumerable<Entities.Client>? Clients { get; set; }
        public IEnumerable<Entities.Component>? Components { get; set; }
        public IEnumerable<Entities.Car>? Cars { get; set; }
        public IEnumerable<Entities.Recipe>? Recipes { get; set; }
        private async Task LoadAllClientsAsync()
            => Clients = await Manager.Database.Clients.GetAllClientsAsync(Database);
        private async Task LoadAllComponentsAsync()
            => Components = await Manager.Database.Components.GetAllComponentsAsync(Database);
        private async Task LoadAllCarsAsync()
            => Cars = await Manager.Database.Cars.GetAllCarsAsync(Database);
        private async Task LoadAllRecipes()
            => Recipes = await Manager.Database.Recipes.GetAllRecipesAsync(Database);
        protected override async Task OnInitializedAsync()
        {
            await Signal.StartAsync();

            await LoadAllCarsAsync();
            await LoadAllComponentsAsync();
            await LoadAllRecipes();
            await LoadAllRecipes();
        }
    }

    public class ViewModelBase : ComponentBase
    {
        [Inject] public SignalClient Signal { get; set; } = null!;
        [Inject] public RequestManager Manager { get; set; } = null!;
        [Inject] public DatabaseConnectionInfo Database { get; set; } = null!;
        [Inject] public FilterData Filter { get; set; } = null!;
    }
}
