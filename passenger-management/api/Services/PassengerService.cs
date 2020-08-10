using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using api.Models;
using MongoDB.Driver;
using JsonConvert = Newtonsoft.Json.JsonConvert;

namespace api.Services
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class PassengerService
    {
        private readonly IMongoCollection<Passenger> _passengers;
        private readonly ProducerWrapper _producer;
        private readonly IKafkaTopics _kafkaTopics;

        public PassengerService(IPassengersDatabaseSettings settings, IKafkaConfig kafkaConfig)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _passengers = database.GetCollection<Passenger>(settings.PassengersCollectionName);

            if (kafkaConfig != null)
            {
                _producer = new ProducerWrapper(kafkaConfig);
                _kafkaTopics = kafkaConfig.KafkaTopics;
            }
        }

        public List<Passenger> Get(bool includeDisabled = false)
        {
            return _passengers.Find(passenger => passenger.Enabled || includeDisabled).ToList();
        }

        public Passenger Get(string id, bool includeDisabled = false)
        {
            return _passengers.Find(passenger => passenger.Id == id && (passenger.Enabled || includeDisabled))
                .FirstOrDefault();
        }

        // ReSharper disable once UnusedMethodReturnValue.Global
        public async Task<Passenger> Create(Passenger passenger)
        {
            passenger.Enabled = true;
            _passengers.InsertOne(passenger);
            
            await _producer.WriteMessage(_kafkaTopics.Create, JsonConvert.SerializeObject(passenger));

            return passenger;
        }

        public async Task Update(string id, Passenger passengerIn, bool includeDisabled = false)
        {
            passengerIn.Id = id;
            passengerIn.Enabled = true;
            _passengers.ReplaceOne(passenger => passenger.Id == id && (passenger.Enabled || includeDisabled),
                passengerIn);
            
            await _producer.WriteMessage(_kafkaTopics.Update, JsonConvert.SerializeObject(passengerIn));
        }

        public async Task Delete(Passenger passengerIn)
        {
            passengerIn.Enabled = false;
            _passengers.ReplaceOne(passenger => passenger.Id == passengerIn.Id, passengerIn);
            
            await _producer.WriteMessage(_kafkaTopics.Delete, JsonConvert.SerializeObject(passengerIn));
        }

        public async Task Delete(string id)
        {
            await Delete(Get(id));
        }
    }
}