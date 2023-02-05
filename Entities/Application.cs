using AutoConverter;


namespace Entities
{
    public class EntityBase<TKey> where TKey : IEquatable<TKey>
    {
        public TKey Id { get; set; } = default!;
    }
    public class Component : EntityBase<int>
    {
        public string Name { get; set; }
        public float Amount { get; set; }
        public int MixOrder { get; set; }
        public double Humidity { get; set; }
        public int OldId { get; set; }
    }
    public class ComponentExpenditure
    {
        [Column("component_name")] public string ComponentName { get; set; } = null!;
        [Column("component_type")] public string ComponentType { get; set; } = null!;
        [Column("component_old_id")] public int ComponentOldId { get; set; }
        [Column("component_id")] public int ComponentId { get; set; }
        [Column("mass_need")] public float MassNeed { get; set; }
        [Column("mass_humidity_correction")] public float MassHumidityCorrection { get; set; }
        [Column("mass_manual_correction")] public float MassManualCorrection { get; set; }
        [Column("mass_real_correction")] public float MassRealCorrection { get; set; }
        [Column("mass_real_manual")] public float MassRealManual { get; set; }
        [Column("mass_real")] public float MassReal { get; set; }
        [Column("mass_real_auto")] public float MassRealAuto { get; set; }
        /// <summary>
        /// Расход номинальный
        /// </summary>
        public float MassNominalConsumption => MassNeed + MassManualCorrection + MassHumidityCorrection;
        /// <summary>
        /// Расход фактический
        /// </summary>
        public float MassFactConsumption => MassRealAuto + MassRealManual;
        public float AutoBalanceError => MassRealAuto > 0.001 ? MassRealAuto - MassNominalConsumption - MassManualCorrection - MassHumidityCorrection : 0;
        //public float FactBalanceErrorPercent => MassNominalConsumption > 0.01 ? CheckValue(FactBalanceError) : 0;
    }
    public class Car
    {
        [Column("id")] public int Id { get; set; }
        [Column("id_car")] public string Number { get; set; }
        [Column("volume")] public string Volume { get; set; }
    }
    public class BatchMat : EntityBase<int>
    {
        public int BatchId { get; set; }
        public int BunkerNumber { get; set; }
        public int ComponentId { get; set; }
        public float MassAuto { get; set; }
        public float MassManual { get; set; }
        public float MassCorrection { get; set; }
    }

    public class Batch : EntityBase<int>
    {
        [Column("id_batch")] public new int Id { get; set; }
        [Column("id_application")] public int ApplicationId { get; set; }
        [Column("batch_volume")] public float Volume { get; set; }
        [Column("batch_volume_fact")] public float VolumeFact { get; set; }
        [Column("start_time")] public DateTime StartTime { get; set; }
        [Column("end_time")] public DateTime EndTime { get; set; }
        [Column("mix_num")] public int MixerNumber { get; set; }
        [Column("temperature")] public float Temperature { get; set; }
        [Column("manual")] public bool IsManual { get; set; }
        [Column("actual_mixing_time")] public int ActualMixingTime { get; set; }
        [Column("actual_humidity")] public float ActualHumidity { get; set; }
        [Column("actual_current")] public int ActualCurrent { get; set; }
        [Column("is_duplicate")] public bool IsDuplicate { get; set; }
        [Column("pure_time")] public int PureTime { get; set; }

        public List<BatchMat> Mats { get; set; }
            = new List<BatchMat>();
    }
    public class Client
    {
        [Column("id_client")] public int Id { get; set; }
        [Column("name")] public string? Name { get; set; }
        [Column("address")] public string? Address { get; set; }
    }

    public class MaterialExpenditure
    { 
        public string ComponentName { get; set; }
        public string ComponentType { get; set; }
        public float ExpenditureNominal { get; set; }
        public float ExpenditureFact { get; set; }
        
    }
    public class Application : EntityBase<int>
    {
        [Column("id_application")] public new int Id { get; set; }
        [Column("invoice")] public string Invoice { get; set; } = null!;
        [Column("volume")] public float Volume { get; set; }
        [Column("volume_cur")] public float VolumeCurrent { get; set; }
        [Column("volume_fact")] public float VolumeFact { get; set; }
        [Column("start_time")] public DateTime StartTime { get; set; }
        [Column("end_time")] public DateTime EndTime { get; set; }
        [Column("create_time")]  public DateTime ApplicationCreateTime { get; set; }
        [Column("complete")] public bool IsCompleted { get; set; }
        [Column("deleted")] public bool IsDeleted { get; set; }
        [Column("id_recipe")] public int RecipeId { get; set; }
        [Column("mix_num")] public int MixerNumber { get; set; }
        [Column("batch_phase")] public int BatchPhase { get; set; }
        [Column("trainMode")] public bool IsTrainMode { get; set; }
        [Column("running")] public bool IsRunning { get; set; }
        [Column("speed_app")] public bool IsSpeedUp { get; set; }
        [Column("id_application")] public int LastLayer { get; set; }
        [Column("id_client")] public int ClientId { get; set; }
        [Column("id_car")] public int CarId { get; set; }
        [Column("name")] public string? RecipeName { get; set; }
        [Column("user_name")] public string? CreatorName { get; set; }
        public Client? Client { get; set; }
        public Recipe? Recipe { get; set; }
        public Car? Car { get; set; }
        public IEnumerable<Component> Components { get; set; }
             = new List<Component>();
        public IEnumerable<Batch> Batches { get; set; }
            = new List<Batch>();
    }
    public class BunkerExpenditure
    {
        public BunkerExpenditure() { }
        public int Id { get; set; }
        /// <summary>
        /// Время
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Состояние бункера
        /// </summary>
        public BunkerLevelType Type { get; set; }
        /// <summary>
        /// Номер датчика
        /// </summary>
        public int SensorNumber { get; set; }

        /// <summary>
        /// Загрузка
        /// </summary>
        public float Load { get; set; }
        /// <summary>
        /// Расход
        /// </summary>
        public float Expenditures { get; set; }

        /// <summary>
        /// Остаток
        /// </summary>
        public float Remains { get; set; }
        /// <summary>
        /// Id заявки (Номер)
        /// </summary>
        public int ApplicationId { get; set; }
        /// <summary>
        /// Номер замеса в заявке
        /// </summary>
        public int BatchId { get; set; }
    }
    public enum BunkerLevelType
    {
        Consumption,
        Loading
    }

    public class FilterData
    {
        public DateTime? StartTime { get; set; } = DateTime.Now.AddDays(-7);
        public DateTime? EndTime { get; set; } = DateTime.Now;
        public Car? SelectedCar { get; set; }
        public Client? SelectedClient { get; set; }
        public int? SelectedSensor { get; set; }
        public Component? SelectedComponent { get; set; }
        public IEnumerable<Recipe>? SelectedRecipes { get; set; }
        public Recipe? SelectedRecipe { get; set; }
        public string? SelectedRecipeName { get; set; }
        public bool? IsTrainMode { get; set; }
        public bool? IsManual { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsCompleted { get; set; }
        public bool? IsSpeedApp { get; set; }
        public string? Invoice { get; set; }
        public int? MixerNumber { get; set; }
        public int? ApplicationNumber { get; set; }
    }

    public class RecipeExpenditure
    {
        [Column("volume_fact")] public float VolumeFact { get; set; } 
        [Column("volume")] public float Volume { get; set; } 
        [Column("name")] public string RecipeName { get; set; } = null!;
    }

    public class Recipe
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int OldId { get; set; }
    }
}
