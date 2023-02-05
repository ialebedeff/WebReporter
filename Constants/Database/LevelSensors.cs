using Entities;

namespace AppConstants
{
    public class DatabaseLevelSensorCommands
    {
        public string SelectAllBunkers(string database, int lineNumber)
            => $@"select DISTINCT number from {database}.`l{lineNumber}_level_sensors`;";
        public string SelectBunkerExpenditures(
            string database,
            int lineNumber,
            int sensorNumber,
            string started,
            string completed,
            bool isTrainMode, int skip = 0, int take = int.MaxValue)
            => $@"select * from {database}.`l{lineNumber}_level_sensors` as ls 
                  where ls.number = {sensorNumber}
                  and ls.time >= '{started}'
                  and ls.time <= '{completed}'                 
                  and ls.train_mode = {Convert.ToInt32(isTrainMode)}
                  order by ls.time LIMIT {take} OFFSET {skip}";

       

        public string SelectAllBunkerExpenditure(
            string database,
            int lineNumber,
            DateTime started,
            DateTime completed,
            bool isTrainMode)
            => $@"select * from {database}.`l{lineNumber}_level_sensors` as ls 
                  and ls.time >= '{started}'
                  and ls.time <= '{completed}'               
                  and ls.train_mode = {Convert.ToInt32(isTrainMode)}
                  order by ls.time";
    }
}