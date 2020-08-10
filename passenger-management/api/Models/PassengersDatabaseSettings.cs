// ReSharper disable UnusedMember.Global

namespace api.Models
{
    public class PassengersDatabaseSettings : IPassengersDatabaseSettings
    {
        public string PassengersCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IPassengersDatabaseSettings
    {
        string PassengersCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}