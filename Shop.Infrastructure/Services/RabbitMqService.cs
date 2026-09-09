using Microsoft.Extensions.Options;
using Shop.Application.Interfaces.Services;
using Shop.Infrastructure.Configuration;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Shop.Infrastructure.Services;

public class RabbitMqService : IQueueService
{
    private readonly RabbitMqSettings _rabbitMqSettings;

    public RabbitMqService(IOptions<RabbitMqSettings> options)
    {
        _rabbitMqSettings = options.Value;
    }

    public async Task PublishAsync<T>(string queue, T message, CancellationToken cancellationToken = default)
    {
        var factory = new ConnectionFactory()
        {
            HostName = _rabbitMqSettings.Host,
            Port = _rabbitMqSettings.Port
        };

        await using var connection = await factory.CreateConnectionAsync(cancellationToken);
        
        await using var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);
        
        await channel.QueueDeclareAsync(
            queue: queue,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );
        
        var json = JsonSerializer.Serialize(message);
        
        var body = Encoding.UTF8.GetBytes(json);
        
        var properties = new BasicProperties
        {
            Persistent = true
        };
        
        await channel.BasicPublishAsync(
             exchange: "",
             routingKey: queue,
             mandatory: false,
             basicProperties: properties,
             body: body,
             cancellationToken: cancellationToken
        );
    }
}
