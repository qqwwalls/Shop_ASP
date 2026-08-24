using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMqReader;

public sealed class User
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("RabbitMQ Consumer is starting...");

        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
            Port = 5672
        };

        var connection = await factory.CreateConnectionAsync();
        var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(
            queue: "Users",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (sender, e) =>
        {
            var body = e.Body.ToArray();

            var json = Encoding.UTF8.GetString(body);

            var message = JsonSerializer.Deserialize<User>(json);

            Console.WriteLine($"\n[x] Received New Message!");
            if (message != null)
            {
                Console.WriteLine($"    Email: {message.Email}");
                Console.WriteLine($"    Password: {message.Password}");
            }
            else
            {
                Console.WriteLine($"    Failed to deserialize message: {json}");
            }
            
            await Task.CompletedTask;
        };

        await channel.BasicConsumeAsync(
            queue: "Users",
            autoAck: true,
            consumer: consumer
        );

        Console.WriteLine("[*] Waiting for messages in 'Users' queue. Press [ENTER] to exit.");
        Console.ReadLine();
    }
}
