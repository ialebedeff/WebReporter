namespace SmartMixWebReporter.Client.Pages.Filter
{
    public class FilterData
    {
        public DateTime? StartTime { get; set; } = DateTime.Parse("21.06.2021");
        public DateTime? EndTime { get; set; } = DateTime.Now;
        public bool IsFirstRun { get; set; } = true;
        public bool IsComponentLoad { get; set; }
        public bool IsFirstApplicationRun { get; set; } = true;
    }
}
