using System.Text;

namespace AppConstants
{
    public static class DatabaseFilterExtensions
    {
        public static string AddCar(this string command, int carId)
            => AddFilter(command, "id_car", carId);
        public static string AddMixerNumber(this string command, int mixerNumber)
            => AddFilter(command, "mix_num", mixerNumber);
        public static string AddApplicationNumber(this string command, int applicationId)
           => AddFilter(command, "id_application", applicationId);
        public static string AddRecipes(this string command, IEnumerable<int> recipesIds)
            => AddFilter(command, "id_recipe", $"({string.Join(",", recipesIds)})", " IN ", useQuotes: false);
        public static string AddClient(this string command, int clientId)
            => AddFilter(command, "id_client", clientId);
        public static string AddRecipe(this string command, int recipeId)
            => AddFilter(command, "id_recipe", recipeId);
        public static string AddStartTime(this string command, string dateTime)
            => AddFilter(command, "start_time", dateTime, ">");
        public static string AddEndTime(this string command, string dateTime)
           => AddFilter(command, "end_time", dateTime, "<");
        public static string AddCreator(this string command, int creatorId)
            => AddFilter(command, "creator_id", creatorId);
        public static string AddTrainMode(this string command, bool isTrainMode)
            => AddFilter(command, "trainMode", isTrainMode);
        public static string AddOperator(this string command, int operatorId)
         => AddFilter(command, "user_id", operatorId);
        public static string AddOnlyNonManual(this string command)
            => AddFilter(command, "id_recipe", 0, "!=");
        public static string AddOnlySpeedApp(this string command)
            => AddFilter(command, "speed_app", 1);
        public static string AddOnlyDeletedApps(this string command)
            => AddFilter(command, "deleted", 1);
        public static string AddOnlyCompletedApps(this string command)
            => AddFilter(command, "complete", 1);
        public static string AddInvoice(this string command, string invoice)
           => AddFilter(command, "invoice", invoice);

        private static string AddFilter(
            this string command, 
            string key, 
            object value, 
            string operation = "=", bool useQuotes = true)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(command);

            if (!builder.ToString().Contains("as t"))
            {
                builder.Append(" as t ");
            }

            if (!builder.ToString().Contains("WHERE"))
            {
                builder.Append(" WHERE ");
            }
            else
            {
                builder.Append(" AND ");
            }

            builder.Append(string.Format("t.{0} {1} ", key, operation));

            if (value is int)
            {
                builder.Append(value);
            }
            else if ((value is DateTime || value is string) && useQuotes)
            {
                builder.Append($"'{value}'");
            }
            else if ((value is DateTime || value is string) && !useQuotes)
            {
                builder.Append($"{value}");
            }
            else if (value is bool)
            {
                builder.Append(Convert.ToInt32(value));
            }
            
            return builder.ToString();
        }
    }
}