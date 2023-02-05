using Entities;
using System.Data;
using System.Reflection;

namespace Converters
{
    public static partial class DataConverter
    {
        public static string GetString(Dictionary<string, object>? value, string key)
        {
            if (value is null)
                return string.Empty;
            if (!value.ContainsKey(key))
                return string.Empty;

            return Convert.ToString(value[key]) ?? String.Empty;
        }

        public static int GetInt(Dictionary<string, object>? value, string key)
        {
            if (value is null)
                return 0;
            if (!value.ContainsKey(key))
                return 0;

            try
            {
                return Convert.ToInt32(Convert.ToString(value[key]));
            }
            catch
            {
                return 0;
            }
        }
        public static float GetFloat(Dictionary<string, object>? value, string key)
        {
            if (value is null)
                return 0f;
            if (!value.ContainsKey(key))
                return 0f;

            return float.Parse(Convert.ToString(value[key]) ?? String.Empty, System.Globalization.CultureInfo.InvariantCulture);
        }
        public static int ConvertToCount(List<Dictionary<string, object>>? values)
        {
            if (values is null || values.Count == 0)
                return 0;

            return GetInt(values[0], values[0].First().Key);
        }
    }
    public static partial class DataConverter
    {
        public static IEnumerable<Recipe> ConvertToRecipes(List<Dictionary<string, object>>? values)
        {
            if (values is null)
            {
                return Enumerable.Empty<Recipe>();
            }

            var recipes = new List<Recipe>();

            foreach (var record in values)
            {
                recipes.Add(ConvertToRecipe(record));
            }

            return recipes;
        }

        public static IEnumerable<Batch> ConvertToBatches(List<Dictionary<string, object>> records)
        {
            if (records is null)
            {
                return Enumerable.Empty<Batch>();
            }

            var batch = new List<Batch>();

            foreach (var record in records)
            {
                batch.Add(ConvertToBatch(record));
            }

            return batch;
        }

        public static BatchMat ConvertToBatchMat(Dictionary<string, object> record)
        {
            return new BatchMat()
            {
                BatchId = GetInt(record, "batch_id"),
                BunkerNumber = GetInt(record, "bunker_num"),
                ComponentId = GetInt(record, "id_component"),
                MassAuto = GetFloat(record, "mass_auto"),
                MassManual = GetFloat(record, "mass_manual"),
                MassCorrection = GetFloat(record, "mass_correction"),
            };
        }

        public static IEnumerable<BatchMat> ConvertToBatchMats(List<Dictionary<string, object>> records)
        {
            if (records is null)
            {
                return Enumerable.Empty<BatchMat>();
            }

            var batchs = new List<BatchMat>();

            foreach (var record in records)
            {
                batchs.Add(ConvertToBatchMat(record));
            }

            return batchs;
        }

        public static Batch ConvertToBatch(Dictionary<string, object> record)
        {
            return new Batch()
            {
                ApplicationId = GetInt(record, "id_application"),
                Id = GetInt(record, "id_batch"),
                MixerNumber = GetInt(record, "mix_num"),
                Volume = GetFloat(record, "batch_volume"),
                VolumeFact = GetFloat(record, "batch_volume_fact")
            };
        }

        public static IEnumerable<Component> ConvertToComponents(List<Dictionary<string, object>> records)
        {
            if (records is null)
            {
                return Enumerable.Empty<Component>();
            }

            var components = new List<Component>();

            foreach (var record in records)
            {
                components.Add(ConvertToComponent(record));
            }

            return components;
        }


        public static Component ConvertToComponent(Dictionary<string, object> record)
        {
            return new Component()
            {
                Name = GetString(record, "name"),
                Humidity = GetFloat(record, "humidity"),
                Id = GetInt(record, "id_component"),
                OldId = GetInt(record, "old_id"),
            };
        }

        public static Recipe? ConvertToRecipe(Dictionary<string, object>? record)
        {
            if (record is null)
            {
                return null;
            }

            return new Recipe()
            {
                Id = GetInt(record, "id_recipe"),
                Name = GetString(record, "name"),
                OldId = GetInt(record, "old_id")
            };
        }

        public static async Task<List<BunkerExpenditure>> ConvertToBunkerExpenditures(List<Dictionary<string, object>>? values)
        {
            var startValue = 0f;
            var expenditures = new List<BunkerExpenditure>();

            if (values is null)
            {
                return expenditures;
            }

            if (values.Count > 0)
            {
                foreach (var record in values)
                {
                    var item = await ConvertToBunkerExpenditure(record);

                    float expenditure = startValue - item.Expenditures;

                    item.Remains = item.Expenditures;

                    if (item.ApplicationId == 0) // Заявка с номером 0 - загрузка в бункер
                    {
                        item.Expenditures = 0;
                        item.Load -= expenditure;
                        item.Type = BunkerLevelType.Loading;
                    }

                    else
                    {
                        item.Expenditures = expenditure;
                        item.Type = BunkerLevelType.Consumption;
                    }

                    startValue -= expenditure;

                    expenditures.Add(item);
                }
            }

            expenditures[0].Expenditures = 0;

            return expenditures;
        }

        public static async Task<BunkerExpenditure> ConvertToBunkerExpenditure(Dictionary<string, object> record)
        {
            return await Task.FromResult(new BunkerExpenditure()
            {
                ApplicationId = GetInt(record, "application_id"),
                BatchId = GetInt(record, "batch_num"),
                Date = Convert.ToDateTime(record["time"].ToString()),
                Expenditures = GetFloat(record, "value"),
                Id = GetInt(record, "id"),
                SensorNumber = GetInt(record, "number"),
                Remains = GetFloat(record, ""),
            });
        }


        public static List<int> ConvertToLevelSensorNumbers(List<Dictionary<string, object>>? values)
        {
            var numbers = new List<int>();

            if (values is null)
            {
                return numbers;
            }

            foreach (var record in values)
            {
                numbers.Add(GetInt(record, "number"));
            }

            return numbers;
        }
        public static List<Application> ConvertToApplications(List<Dictionary<string, object>>? values)
        {
            var applications = new List<Application>();

            if (values is null)
            {
                return applications;
            }

            foreach (var value in values)
            {
                applications.Add(new Application()
                {
                    Id = GetInt(value, "id_application"),
                    RecipeId = GetInt(value, "id_recipe"),
                    Invoice = GetString(value, "invoice"),
                    Volume = float.Parse(GetString(value, "volume"), System.Globalization.CultureInfo.InvariantCulture),
                    IsDeleted = Convert.ToBoolean(Convert.ToString(value["deleted"])),
                    IsCompleted = Convert.ToBoolean(Convert.ToString(value["complete"])),
                    StartTime = Convert.ToDateTime(Convert.ToString(value["start_time"])),
                    EndTime = Convert.ToDateTime(Convert.ToString(value["end_time"])),
                    ApplicationCreateTime = Convert.ToDateTime(Convert.ToString(value["create_time"])),
                    BatchPhase = GetInt(value, "batch_phase"),
                    IsRunning = Convert.ToBoolean(GetString(value, "running")),
                    IsSpeedUp = Convert.ToBoolean(GetString(value, "speed_app")),
                    VolumeFact = float.Parse(GetString(value, "volume_fact"), System.Globalization.CultureInfo.InvariantCulture),
                    VolumeCurrent = float.Parse(GetString(value, "volume_cur"), System.Globalization.CultureInfo.InvariantCulture),
                    MixerNumber = GetInt(value, "mix_num"),
                });
            }

            return applications;
        }
    }

    
}