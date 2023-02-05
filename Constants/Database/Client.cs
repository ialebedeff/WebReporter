namespace AppConstants
{
    public class DatabaseClientsCommand
    {
        public string SelectAllClients(string database)
            => $@"SELECT * FROM {database}.`client`;";
        public string SelectClientById(string database, int clientId)
            => $@"SELECT * FROM {database}.`client` as c 
                  WHERE c.id_client = {clientId};";
    }
}