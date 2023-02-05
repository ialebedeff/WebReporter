using AppConstants;
using AutoConverter;
using Converters;
using Entities;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;

namespace CommunicationMessageQueueManager
{
    public class BatchesDatabaseRequests : DatabaseRequest
    {
        public BatchesDatabaseRequests(MessageQueueManager messageQueueManager) : base(messageQueueManager)
        {
        }

        public async Task<IEnumerable<BatchMat>> GetBatchMatsAsync(
           int applicationId,
           DatabaseConnectionInfo databaseConnectionInfo)
        {
            var message = await ExecuteDatabaseCommandResultAsync(new DatabaseCommand()
            {
                Command = string.Format(Constants.DatabaseCommands.Batchs.SelectBatchMats(
                                databaseConnectionInfo.Database, 1, applicationId)),
                Connection = databaseConnectionInfo
            });

            if (message is null)
            {
                return Enumerable.Empty<BatchMat>();
            }

            return DataConverter.ConvertToBatchMats(message.Data);
        }

        public async Task<IEnumerable<Batch>> GetBatchesFromApplicationAsync(
            int applicationId,
            DatabaseConnectionInfo databaseConnectionInfo)
        {
            var message = await ExecuteDatabaseCommandResultAsync(new DatabaseCommand()
            {
                Command = string.Format(Constants.DatabaseCommands.Batchs.SelectBatchesFromApplication(
                   databaseConnectionInfo.Database, 1, applicationId)),
                Connection = databaseConnectionInfo
            });

            if (message is null)
            {
                return Enumerable.Empty<Batch>();
            }

            return RecordsConverter.ConvertTo<Batch>(message.Data);
        }
    }
    public class RecipesDatabaseRequests : DatabaseRequest
    {
        public RecipesDatabaseRequests(MessageQueueManager messageQueueManager) : base(messageQueueManager)
        {
        }

        public async Task<IEnumerable<Recipe>> GetRecipesAsync(
           IEnumerable<int> ids,
           DatabaseConnectionInfo databaseConnectionInfo)
        {
            var message = await ExecuteDatabaseCommandResultAsync(new DatabaseCommand()
            {
                Command = Constants.DatabaseCommands.Recipes.SelectRecipesByIds(
                    databaseConnectionInfo.Database, 1, ids),
                Connection = databaseConnectionInfo
            });

            if (message is null)
            {
                return Enumerable.Empty<Recipe>();
            }

            return DataConverter.ConvertToRecipes(message.Data);
        }

        public async Task<IEnumerable<Recipe>> GetAllRecipesAsync(
            DatabaseConnectionInfo database)
        {
            var message = await ExecuteDatabaseCommandResultAsync(new DatabaseCommand()
            {
                Command = Constants.DatabaseCommands.Recipes.SelectAllRecipes(database.Database),
                Connection = database
            });

            if (message is null)
            {
                return Enumerable.Empty<Recipe>();
            }

            return DataConverter.ConvertToRecipes(message.Data);
        }
        public async Task<IEnumerable<RecipeExpenditure>> GetRecipesExpendituresByFilterAsync(
            FilterData filterData, DatabaseConnectionInfo database)
        {
            var message = await ExecuteDatabaseCommandResultAsync(new DatabaseCommand()
            {
                Command = Constants.DatabaseCommands.Recipes.SelectRecipesExpenditures(database.Database, 1, filterData),
                Connection = database
            });

            if (message is null)
            {
                return Enumerable.Empty<RecipeExpenditure>();
            }

            return RecordsConverter.ConvertTo<RecipeExpenditure>(message.Data);
        }

        public async Task<IEnumerable<Recipe>> GetRecipesByNameAsync(
            string recipeName, DatabaseConnectionInfo database)
        {
            var message = await ExecuteDatabaseCommandResultAsync(new DatabaseCommand()
            {
                Command = Constants.DatabaseCommands.Recipes.SelectRecipeIdsByName(database.Database, 1, recipeName),
                Connection = database
            });

            if (message is null)
            {
                return Enumerable.Empty<Recipe>();
            }

            return DataConverter.ConvertToRecipes(message.Data);
        }
        public async Task<IEnumerable<Recipe>> GetAllArchiveRecipesAsync(
            DatabaseConnectionInfo database)
        {
            var message = await ExecuteDatabaseCommandResultAsync(new DatabaseCommand()
            {
                Command = Constants.DatabaseCommands.Recipes.SelectAllArchiveRecipes(database.Database, 1),
                Connection = database
            });

            if (message is null)
            {
                return Enumerable.Empty<Recipe>();
            }

            return DataConverter.ConvertToRecipes(message.Data);
        }

        public async Task<Recipe?> GetRecipeOrDefaultAsync(
            int id,
            DatabaseConnectionInfo databaseConnection)
        {
            var message = await ExecuteDatabaseCommandResultAsync(new DatabaseCommand()
            {
                Command = Constants.DatabaseCommands.Recipes.SelectRecipeById(
                    databaseConnection.Database, 1, id),
                Connection = databaseConnection
            });

            if (message is null)
            {
                return default;
            }

            return DataConverter.ConvertToRecipe(message.Data.FirstOrDefault());
        }
    }
    public class ApplicationsDatabaseRequests : DatabaseRequest
    {
        public ApplicationsDatabaseRequests(MessageQueueManager messageQueueManager) : base(messageQueueManager)
        {
        }
        public async Task<IEnumerable<Application>> GetApplicationsAsync(DatabaseConnectionInfo databaseConnectionInfo)
           => await GetApplicationsAsync(0, int.MaxValue, databaseConnectionInfo);
        public async Task<IEnumerable<Application>> GetApplicationsAsync(
           int skip,
           int take,
           DatabaseConnectionInfo databaseConnectionInfo)
        {
            var message = await ExecuteDatabaseCommandResultAsync(new DatabaseCommand()
            {
                Command = string.Format(Constants.DatabaseCommands.Applications.GetApplicationsWithSkipTake,
                                        databaseConnectionInfo.Database, 1, take, skip),
                Connection = databaseConnectionInfo
            });

            if (message is null)
            {
                return Enumerable.Empty<Application>();
            }

            return DataConverter.ConvertToApplications(message.Data);
        }

        public async Task<IEnumerable<Application>> GetApplicationsAsync(
            DateTime startTime,
            DateTime endTime,
            DatabaseConnectionInfo databaseConnectionInfo)
        {
            var message = await ExecuteDatabaseCommandResultAsync(new DatabaseCommand()
            {
                Command = string.Format(Constants.DatabaseCommands.Applications.GetApplicationsByDate,
                                        databaseConnectionInfo.Database, 1,
                                        startTime.ToString("yyyy-MM-dd hh:mm:ss"),
                                        endTime.ToString("yyyy-MM-dd hh:mm:ss")),
                Connection = databaseConnectionInfo
            });

            if (message is null)
            {
                return Enumerable.Empty<Application>();
            }

            return RecordsConverter.ConvertTo<Application>(message.Data);
        }

        public async Task<IEnumerable<Application>> GetApplicationsAsync(
            DateTime startTime,
            DateTime endTime,
            int skip, int take,
            DatabaseConnectionInfo databaseConnectionInfo)
        {
            var message = await ExecuteDatabaseCommandResultAsync(new DatabaseCommand()
            {
                Command = string.Format(Constants.DatabaseCommands.Applications.GetApplicationsByDateWithSkipTake,
                                        databaseConnectionInfo.Database, 1,
                                        startTime.ToString("yyyy-MM-dd hh:mm:ss"),
                                        endTime.ToString("yyyy-MM-dd hh:mm:ss"), skip, take),
                Connection = databaseConnectionInfo
            });

            if (message is null)
            {
                return Enumerable.Empty<Application>();
            }

            return RecordsConverter.ConvertTo<Application>(message.Data);
        }
        public async Task<int> GetApplicationsCountAsync(
            FilterData filterData,
            DatabaseConnectionInfo databaseConnectionInfo)
        {
            var message = await ExecuteDatabaseCommandResultAsync(new DatabaseCommand()
            {
                Command = Constants.DatabaseCommands.Applications.SelectApplicationsByFilter(
                    databaseConnectionInfo.Database, 1, filterData, values: "COUNT(*)"),
                Connection = databaseConnectionInfo
            });

            if (message is null)
            {
                return 0;
            }

            return DataConverter.ConvertToCount(message.Data);
        }
        public async Task<IEnumerable<Application>> GetApplicationsAsync(
            FilterData filterData, int skip, int take,
            DatabaseConnectionInfo databaseConnectionInfo)
        {
            var message = await ExecuteDatabaseCommandResultAsync(new DatabaseCommand()
            {
                Command = Constants.DatabaseCommands.Applications.SelectApplicationsByFilter(
                    databaseConnectionInfo.Database, 1, filterData, skip, take),
                Connection = databaseConnectionInfo
            });

            if (message is null)
            {
                return Enumerable.Empty<Application>();
            }

            return RecordsConverter.ConvertTo<Application>(message.Data);
        }
        public async Task<Application?> GetApplicationAsync(
            int applicationId,
            DatabaseConnectionInfo database)
        {
            var message = await ExecuteDatabaseCommandResultAsync(new DatabaseCommand()
            {
                Command = Constants.DatabaseCommands.Applications.GetApplicationById(
                        database.Database, 1, applicationId),
                Connection = database
            });

            if (message is null)
            {
                return null;
            }

            return RecordsConverter.ConvertTo<Application>(message.Data.FirstOrDefault());
        }
        public Task<Application?> this[DatabaseConnectionInfo database, int applicationId]
            => GetApplicationAsync(applicationId, database);
        public Task<IEnumerable<Application>> this[DatabaseConnectionInfo database, FilterData fitlerData]
            => GetApplicationsAsync(fitlerData, 0, int.MaxValue, database);
    }
    public class ClientsDatabaseRequests : DatabaseRequest
    {
        public ClientsDatabaseRequests(MessageQueueManager messageQueueManager) : base(messageQueueManager)
        {
        }

        public async Task<IEnumerable<Client>> GetAllClientsAsync(
            DatabaseConnectionInfo database)
        {
            var message = await ExecuteDatabaseCommandResultAsync(
                new DatabaseCommand()
                {
                    Command = Constants.DatabaseCommands.Clients.SelectAllClients(database.Database),
                    Connection = database
                });

            if (message is null)
            {
                return Enumerable.Empty<Client>();
            }

            return RecordsConverter.ConvertTo<Client>(message.Data);
        }
    }
    public class CarsDatabaseRequests : DatabaseRequest
    {
        public CarsDatabaseRequests(MessageQueueManager messageQueueManager) : base(messageQueueManager)
        {
        }

        public async Task<IEnumerable<Car>> GetAllCarsAsync(
            DatabaseConnectionInfo databaseConnectionInfo)
        {
            var message = await ExecuteDatabaseCommandResultAsync(new DatabaseCommand()
            {
                Command = string.Format(Constants.DatabaseCommands.Cars.SelectAllCars(databaseConnectionInfo.Database)),
                Connection = databaseConnectionInfo
            });

            if (message is null)
            {
                return Enumerable.Empty<Car>();
            }

            return RecordsConverter.ConvertTo<Car>(message.Data);
        }
    }
    public class ComponentsDatabaseRequests : DatabaseRequest
    {
        public ComponentsDatabaseRequests(MessageQueueManager messageQueueManager) : base(messageQueueManager)
        {
        }

        public async Task<IEnumerable<ComponentExpenditure>> GetComponentExpendituresAsync(
            FilterData filterData,
            DatabaseConnectionInfo database)
        {
            var message = await ExecuteDatabaseCommandResultAsync(new DatabaseCommand()
            {
                Command = string.Format(Constants.DatabaseCommands.Components.SelectComponentExpenditures(
                   database.Database, 1, filterData)),
                Connection = database
            });

            if (message is null)
            {
                return Enumerable.Empty<ComponentExpenditure>();
            }

            return RecordsConverter.ConvertTo<ComponentExpenditure>(message.Data);
        }

        public async Task<IEnumerable<Component>> GetComponentsFromRecipeAsync(
            int recipeId,
            DatabaseConnectionInfo databaseConnection)
        {
            var message = await ExecuteDatabaseCommandResultAsync(new DatabaseCommand()
            {
                Command = string.Format(Constants.DatabaseCommands.Recipes.SelectComponenentsFromRecipe(
                    databaseConnection.Database, 1, recipeId)),
                Connection = databaseConnection
            });

            if (message is null)
            {
                return Enumerable.Empty<Component>();
            }

            return DataConverter.ConvertToComponents(message.Data);
        }

        public async Task<IEnumerable<Component>> GetAllComponentsAsync(
            DatabaseConnectionInfo database)
        {
            var message = await ExecuteDatabaseCommandResultAsync(new DatabaseCommand()
            {
                Command = Constants.DatabaseCommands.Components.SelectAllComponents(database.Database),
                Connection = database
            });

            if (message is null)
            {
                return Enumerable.Empty<Component>();
            }

            return DataConverter.ConvertToComponents(message.Data);
        }
    }
    public class LevelSensorsDatabaseRequests : DatabaseRequest
    {
        public LevelSensorsDatabaseRequests(MessageQueueManager messageQueueManager) : base(messageQueueManager)
        {
        }

        public async Task<IEnumerable<int>> GetAllSensorNumbersAsync(
            DatabaseConnectionInfo databaseConnectionInfo)
        {
            var message = await ExecuteDatabaseCommandResultAsync(new DatabaseCommand()
            {
                Command = Constants.DatabaseCommands.LevelSensors.SelectAllBunkers(
                    databaseConnectionInfo.Database, 1),
                Connection = databaseConnectionInfo
            });

            if (message is null)
            {
                return Enumerable.Empty<int>();
            }

            return DataConverter.ConvertToLevelSensorNumbers(message.Data);
        }
        public async Task<int> GetExpenditureCountAsync(
            int levelSensorNumber,
            DateTime startTime,
            DateTime endTime,
            bool? isTrainMode,
            DatabaseConnectionInfo databaseConnectionInfo)
        {
            var message = await ExecuteDatabaseCommandResultAsync(new DatabaseCommand()
            {
                Command = Constants.DatabaseCommands.LevelSensors.SelectBunkerExpenditures(
                    databaseConnectionInfo.Database, 1, levelSensorNumber,
                    startTime.ToString("yyyy-MM-dd hh:mm:ss"),
                    endTime.ToString("yyyy-MM-dd hh:mm:ss"), isTrainMode ?? false)
                .Replace("*", "COUNT(*)"),
                Connection = databaseConnectionInfo
            });

            if (message is null)
            {
                return 0;
            }

            return DataConverter.ConvertToCount(message.Data);
        }
        public async Task<IEnumerable<BunkerExpenditure>> GetExpenditureByLevelSensorNumberAsync(
            int levelSensorNumber,
            DateTime startTime,
            DateTime endTime,
            bool? isTrainMode,
            DatabaseConnectionInfo databaseConnectionInfo,
            int skip = 0, int take = int.MaxValue)
        {
            var message = await ExecuteDatabaseCommandResultAsync(new DatabaseCommand()
            {
                Command = Constants.DatabaseCommands.LevelSensors.SelectBunkerExpenditures(
                    databaseConnectionInfo.Database, 1, levelSensorNumber,
                    startTime.ToString("yyyy-MM-dd hh:mm:ss"),
                    endTime.ToString("yyyy-MM-dd hh:mm:ss"), isTrainMode ?? false, skip, take),
                Connection = databaseConnectionInfo
            });

            if (message is null)
            {
                return Enumerable.Empty<BunkerExpenditure>();
            }

            return await DataConverter.ConvertToBunkerExpenditures(message.Data);
        }
    }
    public class DatabaseRequest : RequestBase
    {
        public DatabaseRequest(MessageQueueManager messageQueueManager) : base(messageQueueManager) 
        {
     
        }
        public async Task<Message<List<Dictionary<string, object>>>?> ExecuteDatabaseCommandResultAsync(DatabaseCommand databaseCommand)
            => await MessageQueueManager.LongPollAsync<DatabaseCommand, List<Dictionary<string, object>>>("ExecuteDatabaseCommand", databaseCommand);
        public async Task ExecuteDatabaseCommandAsync(DatabaseCommand databaseCommand)
            => await MessageQueueManager.SendAsync<DatabaseCommand>("ExecuteDatabaseCommand", databaseCommand);
    }
    public class DatabaseRequestManager : RequestBase
    {
        public DatabaseRequestManager(MessageQueueManager messageQueueManager) : base(messageQueueManager)
        {
            Clients = new ClientsDatabaseRequests(messageQueueManager);
            Batches = new BatchesDatabaseRequests(messageQueueManager);
            Applications = new ApplicationsDatabaseRequests(messageQueueManager);
            Components = new ComponentsDatabaseRequests(messageQueueManager);
            LevelSensors = new LevelSensorsDatabaseRequests(messageQueueManager);
            Recipes = new RecipesDatabaseRequests(messageQueueManager);
            Cars = new CarsDatabaseRequests(messageQueueManager);
        }

        public ClientsDatabaseRequests Clients { get; set; }
        public BatchesDatabaseRequests Batches { get; set; }
        public ApplicationsDatabaseRequests Applications { get; set; }
        public ComponentsDatabaseRequests Components { get; set; }
        public LevelSensorsDatabaseRequests LevelSensors { get; set; }
        public RecipesDatabaseRequests Recipes { get; set; }
        public CarsDatabaseRequests Cars { get; set; }
    }
}