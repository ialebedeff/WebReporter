namespace AppConstants
{
    public class DatabaseCarsCommands
    {
        public string SelectAllCars(string database)
            => $@"SELECT * FROM {database}.`cars`;";
    }
}