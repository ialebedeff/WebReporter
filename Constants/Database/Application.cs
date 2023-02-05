using Entities;

namespace AppConstants
{
    public class DatabaseApplicationCommands
    {
        public string GetApplicationById(string database, int lineNumber, int applicationId)
            => $@"SELECT * FROM {database}.`l{lineNumber}_applications` as app WHERE app.id_application = {applicationId};";
        public string GetApplicationsByDate => "SELECT * FROM {0}.`l{1}_applications` " +
                                               "WHERE `l{1}_applications`.start_time BETWEEN " +
                                               "'{2}' AND '{3}' " +
                                               "ORDER BY {0}.`l{1}_applications`.id_application DESC LIMIT 100;";
        public string GetApplicationsByDateWithSkipTake => "SELECT * FROM {0}.`l{1}_applications` " +
                                              "WHERE `l{1}_applications`.start_time BETWEEN " +
                                              "'{2}' AND '{3}' " +
                                              "ORDER BY {0}.`l{1}_applications`.id_application DESC LIMIT {4} OFFSET {5};";
        public string GetApplicationsWithSkipTake => "SELECT * FROM {0}.`l{1}_applications` " +
                                                     "ORDER BY {0}.`l{1}_applications`.id_application DESC " +
                                                     "LIMIT {2} OFFSET {3};";
        public string SelectApplicationsByFilter(
            string database, 
            int lineNumber, 
            FilterData filterData,
            int skip = 0, int take = int.MaxValue, string values = "t.*, r.*, u.user_name")
        {
            var command = $"SELECT {values} FROM {database}.`l{lineNumber}_applications` as t " +
                          $"INNER JOIN {database}.`l{lineNumber}_archive_recipe` AS r ON t.id_product = r.id_recipe " +
                          $"INNER JOIN {database}.users AS u ON t.creator_id = u.id ";

            if (filterData.StartTime is not null)
            {
                command = command.AddStartTime(((DateTime)filterData.StartTime).ToString("yyyy-MM-dd hh:mm:ss"));
            }
            if (filterData.EndTime is not null)
            {
                command = command.AddEndTime(((DateTime)filterData.EndTime).ToString("yyyy-MM-dd hh:mm:ss"));
            }
            if (filterData.SelectedCar is not null)
            {
                command = command.AddCar(filterData.SelectedCar.Id);
            }
            if (filterData.SelectedClient is not null)
            { 
                command = command.AddClient(filterData.SelectedClient.Id);
            }
            if (filterData.IsManual is not null  && filterData.IsManual == true)
            {
                command = command.AddOnlyNonManual();
            }
            if (filterData.Invoice is not null)
            {
                command = command.AddInvoice(filterData.Invoice);
            }
            if (filterData.IsTrainMode is not null)
            {
                command = command.AddTrainMode((bool)filterData.IsTrainMode);
            }
            if (filterData.IsCompleted is not null && filterData.IsCompleted == true)
            {
                command = command.AddOnlyCompletedApps();
            }
            if (filterData.IsSpeedApp is not null && filterData.IsSpeedApp == true)
            {
                command = command.AddOnlySpeedApp();
            }
            if (filterData.SelectedRecipes is not null && filterData.SelectedRecipes.Count() > 0)
            {
                command = command.AddRecipes(filterData.SelectedRecipes.Select(recipe => recipe.Id));
            }
            if (filterData.ApplicationNumber is not null)
            {
                command = command.AddApplicationNumber((int)filterData.ApplicationNumber);
            }
            if (filterData.MixerNumber is int mixerNumber)
            {
                command = command.AddMixerNumber(mixerNumber);
            }

            if (take is not int.MaxValue)
            { 
                command += $" LIMIT {take} OFFSET {skip}";
            }

            return command;
        }
    }
}