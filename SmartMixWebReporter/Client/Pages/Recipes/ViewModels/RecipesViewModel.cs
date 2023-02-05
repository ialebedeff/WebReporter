
using ChartJs.Blazor.BarChart;
using ChartJs.Blazor.Common;
using Entities;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudBlazor.Extensions;
using Org.BouncyCastle.Crypto.Tls;
using SmartMixWebReporter.Client.Pages.Report.ViewModels;
using System.Data;
using ChartJs.Blazor;
using ChartJs.Blazor.BarChart;
using ChartJs.Blazor.Common;

namespace SmartMixWebReporter.Client.Pages.Recipes.ViewModels
{
    public class RecipeReport
    {
        public RecipeReport(Recipe recipe, IEnumerable<Application> applications)
        {
            Recipe = recipe;
            Applications = applications;
        }
        public Recipe Recipe { get; set; }
        public IEnumerable<Application> Applications { get; set; }
    }
    public class RecipesViewModel : ReportComponentBase
    {
        [Inject] DatabaseConnectionInfo Database { get; set; } = null!;
        public IEnumerable<Application> Applications { get; set; }
            = new List<Application>();
        public List<Recipe> Recipes { get; set; }
            = new List<Recipe>();
        public List<RecipeReport> Reports { get; set; }
            = new List<RecipeReport>();
        public List<ChartSeries> Series { get; set; }
            = new List<ChartSeries>();
        public BarConfig ChartConfig { get; set; }
        public string[] Labels { get; set; }
            = new string[0];
        protected override async Task OnInitializedAsync()
        {
            await SignalClient.StartAsync();

            Applications = await Manager.Database.Applications.GetApplicationsAsync((DateTime)FilterData.StartTime, (DateTime)FilterData.EndTime, Database);

            var recipes = (await Manager.Database.Recipes.GetRecipesAsync(Applications
                .Select(application => application.RecipeId), Database))
                .DistinctBy(recipe => recipe.OldId);

            Recipes.AddRange(recipes);

            foreach (var recipe in Recipes)
            {
                Reports.Add(new RecipeReport(
                    recipe, Applications.Where(application => application.RecipeId == recipe.Id)));
            }

            await InvokeAsync(InitializeChart);
            await InvokeAsync(StateHasChanged);
        }

        public void InitializeChart()
        {
            ChartConfig.Options = new BarOptions()
            {
                Animation = new ChartJs.Blazor.Common.Animation()
                {
                    Duration = 100,
                    Easing = ChartJs.Blazor.Common.Enums.Easing.Linear
                },
                Legend = new Legend() { 
                    
                },
                Hover = new ChartJs.Blazor.Common.Hover()
                {
                    AnimationDuration = 100,
                    Intersect = true,
                    Mode = ChartJs.Blazor.Common.Enums.InteractionMode.Dataset,
                },
                Responsive = true,
                ResponsiveAnimationDuration = 100,
                Title = new ChartJs.Blazor.Common.OptionsTitle()
                {
                    Text = "Объём по рецептам"
                }
            };

            ChartConfig.Data.Labels.Clear();
            ChartConfig.Data.Datasets.Clear();

            List<BarDataset<float>> dataSets = new List<BarDataset<float>>();

            var randomizer = new Random();

            foreach (var report in Reports)
            {
                var color = String.Format("#{0:X6}", randomizer.Next(0x1000000)); // = "#A197B9"

                BarDataset<float> ordered = new BarDataset<float>();
                BarDataset<float> fact = new BarDataset<float>();

                ordered.Label = report.Recipe.Name;
                ordered.Add(report.Applications.Sum(application => application.Volume));
                ordered.BackgroundColor = color;

                fact.Label = string.Format("{0} Факт", report.Recipe.Name);
                fact.Add(report.Applications.Sum(application => application.VolumeFact));
                fact.BackgroundColor = color;
                
                dataSets.Add(ordered);
                dataSets.Add(fact);
            }

            ChartConfig.Options.Legend = new Legend()
            {
                Display = true,
                FullWidth = false,
                Position = ChartJs.Blazor.Common.Enums.Position.Bottom,
            };

            ChartConfig.Data.Datasets.AddRange(dataSets);
        }

    }
}
