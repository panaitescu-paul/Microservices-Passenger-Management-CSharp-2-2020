using Confluent.Kafka;

namespace api.Models
{
    public class KafkaConfig : IKafkaConfig
    {
        public bool Enabled { get; set; }
        public ProducerConfig ProducerConfig { get; set; }
        public KafkaTopics KafkaTopics { get; set; }
    }

    public interface IKafkaConfig
    {
        bool Enabled { get; set; }
        ProducerConfig ProducerConfig { get; set; }
        KafkaTopics KafkaTopics { get; set; }
    }
    // ReSharper disable once ClassNeverInstantiated.Global
    public class KafkaTopics : IKafkaTopics
    {
        public string Create { get; set; }
        public string Update { get; set; }
        public string Delete { get; set; }
    }

    public interface IKafkaTopics
    {
        string Create { get; set; }
        string Update { get; set; }
        string Delete { get; set; }
    }
}