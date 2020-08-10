using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

// ReSharper disable UnusedMember.Global

namespace api.Models
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Passenger: ICloneable
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public string Id { get; set; }

        [BsonElement("Enabled")] public bool Enabled { get; set; }

        [BsonElement("Cpr")] public string Cpr { get; set; }

        [BsonElement("FirstName")] public string FirstName { get; set; }

        [BsonElement("LastName")] public string LastName { get; set; }

        [BsonElement("Age")] public decimal Age { get; set; }

        [BsonElement("Gender")] public string Gender { get; set; }

        [BsonElement("PassportInfo")] public string PassportInfo { get; set; }

        [BsonElement("Nationality")] public string Nationality { get; set; }

        protected bool Equals(Passenger other)
        {
            return Id == other.Id && Enabled == other.Enabled && Cpr == other.Cpr && FirstName == other.FirstName &&
                   LastName == other.LastName && Age == other.Age && Gender == other.Gender &&
                   PassportInfo == other.PassportInfo && Nationality == other.Nationality;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(Passenger)) return false;
            return Equals((Passenger) obj);
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(Id);
            hashCode.Add(Enabled);
            hashCode.Add(Cpr);
            hashCode.Add(FirstName);
            hashCode.Add(LastName);
            hashCode.Add(Age);
            hashCode.Add(Gender);
            hashCode.Add(PassportInfo);
            hashCode.Add(Nationality);
            return hashCode.ToHashCode();
        }

        public object Clone()
        {
            return new Passenger
            {
                Id = Id,
                Enabled = Enabled,
                Cpr = Cpr,
                FirstName = FirstName,
                LastName = LastName,
                Age = Age,
                Gender = Gender,
                PassportInfo = PassportInfo,
                Nationality = Nationality
            };
        }
    }
}