using Entities;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using WebReporter.Client.Components.Filter.ViewModels;

namespace Reporter.Client.Dialogs.Application
{
    public class BatchInformationViewModel : ViewModelBase
    {
        public BatchInformationViewModel(Batch batch, IEnumerable<BatchMat> batchMats)
        { 
            Batch = batch;
            BatchMats = batchMats;
        }
        public Batch Batch { get; private set; }
        public bool IsEnabled { get; set; }
        public IEnumerable<BatchMat> BatchMats { get; private set; }
    }
    public class ApplicationDialogViewModel : ViewModelBase
    {
        [Parameter] public Entities.Application Application { get; set; } = null!;
        public IEnumerable<Batch>? Batches { get; set; }
        public IEnumerable<BatchMat>? BatchMats { get; set; }
        public IEnumerable<BatchInformationViewModel>? BatchInformations { get; set; }
        public MudChip SelectedChip { get; set; }
        protected override async Task OnInitializedAsync()
        {
            Batches = await Manager.Database.Batches.GetBatchesFromApplicationAsync(Application.Id, Database);
            BatchMats = await Manager.Database.Batches.GetBatchMatsAsync(Application.Id, Database);

            BatchInformations = Batches.Select(batch => new BatchInformationViewModel(batch, BatchMats.Where(mat => mat.BatchId == batch.Id)));
        }
    }
}
