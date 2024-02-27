using GeekShopping.CartAPI.Repository;
using GeekShopping.OrderAPI.Messages;
using GeekShopping.OrderAPI.Model;
using GeekShopping.OrderAPI.RabbitMQSender;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json;

namespace GeekShopping.OrderAPI.MessageConsumer
{
    public class RabbitMQCheckoutConsumer : BackgroundService
    {
        private readonly OrderRepository _repository; 
        private readonly IConnection _connection; 
        private readonly IModel _channel;
        private readonly IRabbitMQMessageSender _rabbitMQMessageSender;

        public RabbitMQCheckoutConsumer(OrderRepository repository,  IRabbitMQMessageSender rabbitMQMessageSender)
        {
            _repository = repository;
            _rabbitMQMessageSender = rabbitMQMessageSender;

            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest",
            };

            _connection = factory.CreateConnection();

            _channel = _connection.CreateModel();
            _channel.QueueDeclare("Checkoutqueue", false, false, arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (chanel, evt) =>
            {
                var content = Encoding.UTF8.GetString(evt.Body.ToArray());
                CheckoutHeaderVO vo = JsonSerializer.Deserialize<CheckoutHeaderVO>(content);
                ProcessOrder(vo).GetAwaiter().GetResult();

                //Remove mensagem da lista no RabbitMQ
                _channel.BasicAck(evt.DeliveryTag, false);
            };
            _channel.BasicConsume("Checkoutqueue", false, consumer);

            return Task.CompletedTask;
        }

        private async Task ProcessOrder(CheckoutHeaderVO vo)
        {
            OrderHeader order = new OrderHeader()
            {
                UserId = vo.UserId,
                FirstName = vo.FirstName,
                LastName = vo.LastName,
                OrderDetails = new List<OrderDetail>(),
                CardNumber = vo.CardNumber,
                CouponCode = vo.CouponCode ?? string.Empty,
                CVV = vo.CVV,
                DiscountAmount = vo.DiscountAmount,
                Email = vo.Email,
                ExpiryMonthYear = vo.ExpiryMonthYear,
                OrderTime = DateTime.Now,
                PurchaseAmount = vo.PurchaseAmount, 
                PaymentStatus = false,
                Phone = vo.Phone,
                DateTime = vo.DateTime,
            };

            foreach(var details in vo.CartDetails)
            {
                OrderDetail detail = new OrderDetail()
                {
                    ProductId = details.ProductId,
                    ProductName = details.Product.Name,
                    Price = details.Product.Price,
                    Count = details.Count
                };

                order.CartTotalItens += details.Count;

                order.OrderDetails.Add(detail);
            }

            await _repository.AddOrder(order);

            PaymentVO payment = new PaymentVO() {
                Name = order.FirstName + " " + order.LastName ,
                CardNumber = order.CardNumber,  
                CVV = order.CVV,
                ExpiryMonthYear = order.ExpiryMonthYear,
                OrderId = order.Id,
                PurchaseAmount = order.PurchaseAmount,   
                Email = order.Email, 
            };

            try
            {
                _rabbitMQMessageSender.SendMessage(payment, "OrderPaymentProcessQueue");
            }
            catch (Exception)
            {
                //Log
                throw;
            }
        }
    }
}