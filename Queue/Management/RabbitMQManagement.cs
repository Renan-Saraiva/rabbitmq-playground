using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Queue.Management
{
    public class RabbitMQManagement : IQueueManagement
    {
        private ConnectArgs ConnectArgs { get; set; }
        private IModel Channel { get; set; }
        private IConnection Connection { get; set; }
        private EventingBasicConsumer EventingBasicConsumer { get; set; }

        public RabbitMQManagement(ConnectArgs args)
        {
            this.ConnectArgs = args;
            Connect();
        }

        private void Connect()
        {
            CreateConnection();
            CreateChannel();
        }

        private void CreateConnection()
        {
            var factory = new ConnectionFactory() { HostName = this.ConnectArgs.Host };
            this.Connection = factory.CreateConnection();
        }

        private void CreateChannel()
        {
            this.Channel = Connection.CreateModel();
        }

        public void CreateQueue(string Queue)
        {
            this.Channel.QueueDeclare(queue: Queue,
                            durable: false,
                            exclusive: false,
                            autoDelete: false,
                            arguments: null);
        }

        public void SetQueueConsumer<T>(string Queue, Action<T> Callback)
        {
            this.EventingBasicConsumer = new EventingBasicConsumer(this.Channel);
            this.EventingBasicConsumer.Received += (model, ea) => Callback(Deserialize<T>(ea.Body.ToArray()));
            this.Channel.BasicConsume(queue: Queue, autoAck: true, consumer: this.EventingBasicConsumer);
        }

        public void Produce<T>(T Body, string Queue)
        {
            this.Channel.BasicPublish(exchange: "", routingKey: Queue, basicProperties: null, body: Serialize(Body));
        }

        private byte[] Serialize(object value)
        {
            return JsonSerializer.SerializeToUtf8Bytes(value);
        }

        private T Deserialize<T>(byte[] value)
        {
            return JsonSerializer.Deserialize<T>(value);
        }
    }
}
