namespace AppConstants
{
    public class DatabaseCommands
    {
        public DatabaseApplicationCommands Applications { get; set; } = new();
        public DatabaseLevelSensorCommands LevelSensors { get; set; } = new();
        public DatabaseRecipeCommands Recipes { get; set; } = new();
        public DatabaseBatchCommands Batchs { get; set; } = new();
        public DatabaseClientsCommand Clients { get; set; } = new();
        public DatabaseComponentCommands Components { get; set; } = new();
        public DatabaseCarsCommands Cars { get; set; } = new();
    }
}