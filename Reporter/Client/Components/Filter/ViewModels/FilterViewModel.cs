using Entities;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using static MudBlazor.Colors;

namespace WebReporter.Client.Components.Filter.ViewModels
{
    public class RecipeConsumptionFilterOptions : FilterOptions
    {
        public RecipeConsumptionFilterOptions()
        {
            IsDateEnabled = true;
            IsTrainModeEnabled = true;
            //IsRecipeSelectorEnabled = true;
            IsRecipeEnabled = true;
        }
    }

    public class DefaultFilterOptions : FilterOptions
    {
        public DefaultFilterOptions()
        {
            IsApplicationNumberEnabled = true;
            IsInvoiceEnabled = true;
            IsMixerNumberEnabled = true;
            IsComponentEnabled = true;
            IsClientEnabled = true;
            IsCarEnabled = true;
            IsTrainModeEnabled = true;
            IsManualEnabled = true;
            IsSpeedAppEnabled = true;
            IsCompletedAppEnabled = true;
            IsRecipeEnabled = true;
            //IsRecipeSelectorEnabled = true;
            IsBunkerEnabled = true;
            IsDateEnabled = true;
        }
    }

    public class BunkerConsumptionFilterOptions : FilterOptions
    {
        public BunkerConsumptionFilterOptions()
        {
            IsDateEnabled = true;
            IsBunkerEnabled = true;
            IsTrainModeEnabled = true;
        }
    }
    public class ApplicationFilterOptions : FilterOptions
    {
        public ApplicationFilterOptions()
        {
            IsClientEnabled = true;
            IsCarEnabled = true;
            IsTrainModeEnabled = true;
            IsManualEnabled = true;
            IsSpeedAppEnabled = true;
            IsCompletedAppEnabled = true;
            IsRecipeEnabled = true;
            IsBunkerEnabled = false;
            IsDateEnabled = true;
            IsInvoiceEnabled = true;
            IsApplicationNumberEnabled = true;
            IsMixerNumberEnabled = true;
        }
    }
    public class FilterOptions
    {
        public bool IsApplicationNumberEnabled { get; set; }
        public bool IsInvoiceEnabled { get; set; }
        public bool IsMixerNumberEnabled { get; set; }
        public bool IsComponentEnabled { get; set; }
        public bool IsClientEnabled { get; set; }
        public bool IsCarEnabled { get; set; }
        public bool IsTrainModeEnabled { get; set; }
        public bool IsManualEnabled { get; set; }
        public bool IsSpeedAppEnabled { get; set; }
        public bool IsCompletedAppEnabled { get; set; }
        public bool IsRecipeEnabled { get; set; }
        public bool IsRecipeSelectorEnabled { get; set; }
        public bool IsBunkerEnabled { get; set; }
        public bool IsDateEnabled { get; set; }
    }
    public class FilterViewModel : ViewModelBase
    {
        [Parameter] public new FilterData? Filter { get; set; }
        [Parameter] public EventCallback ReloadData { get; set; }
        [Parameter] public FilterOptions Options { get; set; } = null!;
        protected override async Task OnInitializedAsync()
        {
            await Signal.StartAsync();

            if (!ApplicationData.IsLoaded)
            {
                await ApplicationData
                    .LoadAsync()
                    .ConfigureAwait(false);
            }
        }

        public async Task LoadRecipesByNameAsync(string recipeName)
        {
            if (Filter is not null && !string.IsNullOrEmpty(recipeName))
            {
                Filter.SelectedRecipes = await Manager.Database.Recipes
                    .GetRecipesByNameAsync(recipeName, Database);
            }
            else if (Filter is not null) { Filter.SelectedRecipes = null; }
        }
        public Task<IEnumerable<string>> SearchByRecipesAsync(string value)
        {
            if (string.IsNullOrEmpty(value))
                return Task.FromResult<IEnumerable<string>>(Enumerable.Empty<string>());
            if (ApplicationData.Recipes is null)
                return Task.FromResult<IEnumerable<string>>(Enumerable.Empty<string>());

            return Task.FromResult(ApplicationData.Recipes
                .Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => x.Name));
        }
    }
}
