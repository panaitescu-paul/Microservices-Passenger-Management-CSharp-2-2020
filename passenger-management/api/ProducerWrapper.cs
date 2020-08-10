using System;
using System.Threading.Tasks;
using api.Models;
using Confluent.Kafka;

namespace api
{
    public class ProducerWrapper
    {
        private readonly IProducer<Null, string> _producer;

        public ProducerWrapper(IKafkaConfig config)
        {
            _producer = !config.Enabled ? null : new ProducerBuilder<Null, string>(config.ProducerConfig).Build();
        }

        public async Task WriteMessage(string topicName, string message)
        {
            if (_producer == null)
            {
                Console.WriteLine($"Kafka: Not Configured. Skipping publishing of '{message}' to '{topicName}'.");
                return;
            }
            
            var deliveryResult = await _producer.ProduceAsync(topicName, new Message<Null, string>
            {
                Value = message
            });
            Console.WriteLine(
                $"Kafka: Delivered '{deliveryResult.Value}' to '{deliveryResult.TopicPartitionOffset}'");
        }
    }
}