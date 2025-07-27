namespace RabbitMQ.Core.Settings;

public sealed record RabbitMqSettings : IRabbitMqSettings
{
    public required string HostName { get; init; }
    public required string Username { get; init; }
    public required string Password { get; init; }
}

public interface IRabbitMqSettings
{
    string HostName { get; init; }
    string Username { get; init; }
    string Password { get; init; }
}