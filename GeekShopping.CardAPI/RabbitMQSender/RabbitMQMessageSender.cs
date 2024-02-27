using GeekShopping.CardAPI.Messages;
using GeekShopping.MessageBus;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace GeekShopping.CardAPI.RabbitMQSender
{
    public class RabbitMQMessageSender : IRabbitMQMessageSender
    {
        private readonly string _hostName;
        private readonly string _password;
        private readonly string _userName;
        private IConnection _connection;

        public RabbitMQMessageSender()
        {
            _hostName = "localhost";
            _password = "guest";
            _userName = "guest";
        }

        public void SendMessage(BaseMessage message, string queueName)
        {
            if (ConnectionExists())
            {
                using var channel = _connection.CreateModel();
                channel.QueueDeclare(queueName, false, false, arguments: null);//Definindo uma fila 
                byte[] body = GetMessageAsByteArray(message);
                channel.BasicPublish(exchange:"", routingKey: queueName, basicProperties: null, body: body);
            }
        }

        private byte[] GetMessageAsByteArray(BaseMessage message)
        {
            
            var options = new JsonSerializerOptions()
            {
                //Para serializar a classe herdada e a classes base
                WriteIndented = true,
            };

            var json = JsonSerializer.Serialize<CheckoutHeaderVO>((CheckoutHeaderVO)message, options);
            var body = Encoding.UTF8.GetBytes(json);
            return body;
        }
        private void CreateConnection()
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _hostName,
                    UserName = _userName,
                    Password = _password
                };
                _connection = factory.CreateConnection();
            }
            catch (Exception)
            {
                //Log exception
                throw;
            }
        }

        private bool ConnectionExists()
        {
            if (_connection != null) return true;
            CreateConnection();
            return _connection != null;
        }
    }
}