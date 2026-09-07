using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shop.Application.DTOs.OrderDTOs;
using Shop.Domain.Models.Mongo;
using Shop.Infrastructure.Configuration;
using MongoDB.Driver;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Shop.Api.Services;

public class RabbitMqMongoOrderConsumer : BackgroundService
{
    private readonly ILogger<RabbitMqMongoOrderConsumer> _logger;
    private readonly RabbitMqSettings _rabbitMqSettings;
    private readonly IMongoCollection<MongoOrder> _ordersCollection;
    
    private IConnection? _connection;
    private IChannel? _channel;

    public RabbitMqMongoOrderConsumer(
        ILogger<RabbitMqMongoOrderConsumer> logger,
        IOptions<RabbitMqSettings> rabbitOptions,
        IOptions<MongoDbSettings> mongoOptions)
    {
        _logger = logger;
        _rabbitMqSettings = rabbitOptions.Value;

        var mongoClient = new MongoClient(mongoOptions.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(mongoOptions.Value.DatabaseName);
        _ordersCollection = mongoDatabase.GetCollection<MongoOrder>(mongoOptions.Value.OrdersCollectionName);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory
        {
            HostName = _rabbitMqSettings.Host,
            Port = _rabbitMqSettings.Port
        };

        _connection = await factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();

        await _channel.QueueDeclareAsync(
            queue: "Orders",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        var consumer = new AsyncEventingBasicConsumer(_channel);
        
        consumer.ReceivedAsync += async (sender, e) =>
        {
            var body = e.Body.ToArray();
            var json = Encoding.UTF8.GetString(body);
            var orderDto = JsonSerializer.Deserialize<OrderCreateDTO>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            
            if (orderDto != null)
            {
                var mongoOrder = new MongoOrder
                {
                    Id = orderDto.Id,
                    UserId = orderDto.UserId,
                    Address = new MongoAddress
                    {
                        City = orderDto.Address.City,
                        Street = orderDto.Address.Street
                    },
                    Products = orderDto.Products.Select(p => new MongoProductItem
                    {
                        ProductId = p.ProductId,
                        Qty = p.Qty,
                        Price = p.Price
                    }).ToList()
                };

                await _ordersCollection.InsertOneAsync(mongoOrder, cancellationToken: stoppingToken);
                _logger.LogInformation($"Successfully saved Order {mongoOrder.Id} to MongoDB!");
            }
                
            await Task.CompletedTask;
        };

        await _channel.BasicConsumeAsync(
            queue: "Orders",
            autoAck: true,
            consumer: consumer);
            
        _logger.LogInformation("RabbitMQ MongoOrder Consumer started. Waiting for Orders...");
        
        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_channel != null)
            await _channel.CloseAsync();
            
        if (_connection != null)
            await _connection.CloseAsync();
            
        await base.StopAsync(cancellationToken);
    }
}
