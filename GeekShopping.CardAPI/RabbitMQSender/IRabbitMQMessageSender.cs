using GeekShopping.MessageBus;

namespace GeekShopping.CardAPI.RabbitMQSender
{
    public interface IRabbitMQMessageSender
    {
        void SendMessage(BaseMessage baseMessage, string queueName);
    }
}