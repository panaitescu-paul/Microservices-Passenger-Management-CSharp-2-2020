using System;
using System.Collections;
using System.Threading.Tasks;
using api.Models;
using api.Services;
using MongoDB.Driver;
using NUnit.Framework;

namespace PassengerTests
{
    public class PassengerServiceTest
    {
        private readonly PassengersDatabaseSettings _databaseSettings = new PassengersDatabaseSettings
        {
            PassengersCollectionName = "Passengers",
            ConnectionString = "mongodb://172.17.0.2:27017",
            DatabaseName = "PassengerDbTest"
        };

        private readonly Passenger _testPassenger = new Passenger
        {
            Id = "5ea6d6a76ed3550a3147dac3",
            Cpr = "1234567891",
            FirstName = "Temp",
            LastName = "1",
            Age = 21,
            Gender = "Male",
            PassportInfo = "Available for 5 more year",
            Nationality = "Testanian"
        };

        private PassengerService _passengerService;

        [SetUp]
        public void Setup()
        {
            _passengerService = new PassengerService(_databaseSettings, null);
        }

        [TearDown]
        public void Teardown()
        {
            // Drop the passenger collection
            var mongoClient = new MongoClient(_databaseSettings.ConnectionString);
            var database = mongoClient.GetDatabase(_databaseSettings.DatabaseName);
            database.DropCollection(_databaseSettings.PassengersCollectionName);
        }

        [Test]
        public async Task TestCreate()
        {
            var actual = await _passengerService.Create(_testPassenger);
            Assert.AreEqual(_testPassenger, actual, "Create returned a different passenger");
            var actualDb = _passengerService.Get(_testPassenger.Id);
            Assert.AreEqual(_testPassenger, actualDb, "Database returned a different passenger");
        }

        [Test]
        public async Task TestRead()
        {
            await TestCreate();

            var actual = _passengerService.Get(_testPassenger.Id);
            Assert.AreEqual(_testPassenger, actual, "Database returned a different passenger");
        }

        [Test]
        public async Task TestReadAll()
        {
            var passengers = new ArrayList();

            for (var i = 0; i < 3; i++)
            {
                var passenger = _testPassenger.Clone() as Passenger;
                passenger.Id = passenger.Id.Substring(0, 23) + i;
                passengers.Add(passenger);
            }

            foreach (Passenger passenger in passengers)
            {
                await _passengerService.Create(passenger);
            }

            var passengersDb = _passengerService.Get();
            Console.WriteLine(passengers);
            Console.WriteLine(passengersDb);
            
            Assert.AreEqual(passengers, passengersDb);
        }

        [Test]
        public async Task TestUpdate()
        {
            var newAge = _testPassenger.Age + 1;
            
            await TestCreate();

            var passenger = _passengerService.Get(_testPassenger.Id);

            passenger.Age = newAge;

            await _passengerService.Update(passenger.Id, passenger);

            var actual = _passengerService.Get(_testPassenger.Id);
            
            Assert.AreNotEqual(_testPassenger, actual, "Database update did not modify the passenger");
            Assert.AreEqual(passenger, actual, "Database returned a different passenger than the modified one");

        }

        [Test]
        public async Task TestDelete()
        {
            await TestCreate();

            await _passengerService.Delete(_testPassenger.Id);

            var actualDisabled = _passengerService.Get(_testPassenger.Id, true);
            
            Assert.AreEqual(actualDisabled.Enabled, false);

            var actual = _passengerService.Get(_testPassenger.Id);
            
            Assert.AreEqual(null, actual);

        }
    }
}