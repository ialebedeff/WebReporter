using Entities;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Reporter.Client.Dialogs.Application;
using WebReporter.Client.Components.Filter.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Reporter.Client.Pages.Application
{
    public class ApplicationViewModel : ViewModelBase
    {
        public MudTable<Entities.Application>? Table { get; set; } = null!;
        public IEnumerable<Entities.Application>? Applications { get; set; }
        public int ResultsCount { get; set; }
        public Task ReloadApplicationAsync()
            => Table?.ReloadServerData() ?? Task.CompletedTask;

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

        public async Task OpenApplicationInfoDialogAsync(Entities.Application application)
        {
            await DialogService.ShowAsync<ApplicationDialog>($"Информация по заявке: {application.Id}",
                parameters: new DialogParameters() {
                    ["Application"] = application
                }, options: new DialogOptions() {FullWidth = true, MaxWidth = MaxWidth.Medium});
        }
    }
}
