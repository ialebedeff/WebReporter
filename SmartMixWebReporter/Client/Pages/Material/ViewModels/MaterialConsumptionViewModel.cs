using Entities;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using SmartMixWebReporter.Client.Pages.Report.ViewModels;
using System.Linq;

namespace SmartMixWebReporter.Client.Pages.Material.ViewModels
{
    public class ComponentViewModel
    {
        public ComponentViewModel(
            Component component)
        {
            Component = component;
        }
        public Component Component { get; set; }
        public List<BatchMat> BatchMats { get; set; }
            = new List<BatchMat>();
    }
    public class MaterialConsumptionViewModel : ReporterComponentBase
    {
        [Inject] DatabaseConnectionInfo Database { get; set; } = null!;
        public IEnumerable<Application> Applications { get; set; }
            = Enumerable.Empty<Application>();
        public IList<ComponentViewModel> Components { get; set; }
            = new List<ComponentViewModel>();
        public List<ChartSeries> Series { get; set; }
            = new List<ChartSeries>();
        protected override async Task OnInitializedAsync()
        {
            await SignalClient.StartAsync();
            await ReloadDataAsync();
        }


        public async Task ReloadDataAsync()
        {
            Applications = await Manager.Database.Applications.GetApplicationsAsync((DateTime)FilterData.StartTime, (DateTime)FilterData.EndTime, Database);

            foreach (var application in Applications)
            {
                application.Components = await Manager.Database.Components.GetComponentsFromRecipeAsync(application.RecipeId, Database);
                application.Batches = await Manager.Database.Batches.GetBatchesFromApplicationAsync(application.Id, Database);

                var mats = await Manager.Database.Batches.GetBatchMatsAsync(application.Id, Database);

                foreach (var batch in application.Batches)
                    batch.Mats.AddRange(mats.Where(mat => batch.Id == mat.BatchId));
            }

            foreach (var application in Applications)
            {
                foreach (var component in application.Components)
                {
                    Components.Add(new ComponentViewModel(component));
                }
            }

            Components = Components
                .DistinctBy(c => c.Component.OldId)
                .ToList();

            foreach (var application in Applications)
            {
                foreach (var batch in application.Batches)
                {
                    foreach (var component in Components)
                    {
                        component.BatchMats.AddRange(
                            batch.Mats.Where(mat => mat.ComponentId == component.Component.Id));
                    }
                }
            }

            InitializeSeries();

            await InvokeAsync(StateHasChanged);
        }

        private void InitializeSeries()
        {
            Series.Clear();

            foreach (var component in Components)
            {
                Series.Add(new ChartSeries()
                {
                    Name = component.Component.Name,
                    Data = new double[3] { 
                        component.BatchMats.Sum(c => (double)c.MassAuto),
                        component.BatchMats.Sum(c => (double)c.MassManual),
                        component.BatchMats.Sum(c => (double)c.MassCorrection),
                    }
                });
            }


        }
    }
}
