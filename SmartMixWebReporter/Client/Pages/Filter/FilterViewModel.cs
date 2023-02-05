using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace SmartMixWebReporter.Client.Pages.Filter
{
    public class FilterViewModel : MudComponentBase
    {
        [Parameter] public DateTime? StartTime { get; set; }
        [Parameter] public DateTime? EndTime { get; set; }
        [Parameter] public EventCallback<MouseEventArgs> OnAccept { get; set; }

    }
}
