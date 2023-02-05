namespace AppConstants
{
    public class DatabaseBatchCommands
    {
        public string SelectBatchesFromApplication(string database, int lineNumber, int applicationId)
            => $@"SELECT * FROM {database}.`l{lineNumber}_batchs` as l 
                  WHERE l.id_application = {applicationId};";
        public string SelectBatchMats(string database, int lineNumber, int applicationId)
            => $@"SELECT * FROM {database}.`l{lineNumber}_batch_mats` AS m 
                  WHERE m.batch_id IN (SELECT (id_batch) FROM {database}.`l{lineNumber}_batchs` as l 
                  WHERE l.id_application = {applicationId});";
        public string SelectBatchMatsById(string database, int lineNumber, int batchId)
            => $@"SELECT * FROM {database}.`l{lineNumber}_batch_mats` AS m
                  WHERE m.batch_id = {batchId}";
    }
}