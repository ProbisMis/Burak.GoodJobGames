using System.Collections.Generic;

namespace GoodJobGames.Utilities.ConfigModels
{
    public class DataStorageSection
    {
        public List<DataStorage> DataStorageCollection { get; set; } = new List<DataStorage>();
        public DataStorageTypes SelectedDataStorageType { get; set; }
    }

    public class DataStorage
    {
        public DataStorageTypes DataStorageType { get; set; }
        public string DataStorageName { get; set; }
        public string ConnectionString { get; set; }
    }

    public enum DataStorageTypes
    {
        SqlServer = 1,
        RDS = 2
    }
}
