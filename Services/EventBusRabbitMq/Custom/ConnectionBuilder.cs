using System.Collections.Immutable;
using System.Net.Sockets;
using Microsoft.Extensions.Logging;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace EventBusRabbitMq.Custom;

public class ConnectionBuilder : IConnectionBuilder
{
    private readonly IConnectionFactory _connectionFactory;
    private readonly ILogger<ConnectionBuilder> _logger;
    private IConnection _connection;
    private readonly int _retryCount;
    private bool Disposed;

    private object _syncRoot = default;

    public ConnectionBuilder(IConnectionFactory connectionFactory, ILogger<ConnectionBuilder> logger,
        int retryCount = 5)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
        _retryCount = retryCount;
    }

    public IModel CreateModel()
    {
        if (!IsConnected)
            throw new InvalidOperationException("No RabbitMQ connection are available to perform this action");
        return _connection.CreateModel();
    }

    public void Dispose()
    {
        if (Disposed) return;
        Disposed = true;
        try
        {
            _connection.ConnectionShutdown -= OnConnectionShutdown!;
            _connection.CallbackException -= OnCallbackException!;
            _connection.ConnectionBlocked -= OnConnectionBlocked!;
            _connection.Dispose();
        }
        catch (IOException ex)
        {
            _logger.LogCritical(ex.ToString());
        }
    }

    public bool IsConnected => _connection is { IsOpen: true } && !Disposed;

    public bool TryConnect()
    {
        _logger.LogInformation("RabbitMQ is trying to ReConnect");

        lock (_syncRoot)
        {
            var policy = Policy.Handle<SocketException>()
                .Or<BrokerUnreachableException>()
                .WaitAndRetry(
                    _retryCount,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    (exception, span) =>
                    {
                        _logger.LogInformation("RabbitMQ client couldn't connect after {TimeOut}s ({ExceptionMessage})",
                            $"{span.TotalSeconds:N1}", exception.Message);
                    });

            policy.Execute(() => _connection = _connectionFactory.CreateConnection());

            if (IsConnected)
            {
                _connection.ConnectionShutdown += OnConnectionShutdown;
                _connection.CallbackException += OnCallbackException;
                _connection.ConnectionBlocked += OnConnectionBlocked;

                _logger.LogInformation(
                    "RabbitMQ Client acquired a persistent connection to '{HostName}' and is subscribed to failure events",
                    _connection.Endpoint.HostName);

                return true;
            }
            else
            {
                _logger.LogCritical("FATAL ERROR: RabbitMQ connections could not be created and opened");

                return false;
            }
        }
    }


    private void OnConnectionBlocked(object sender, ConnectionBlockedEventArgs e)
    {
        if (Disposed) return;

        _logger.LogWarning("A RabbitMQ connection is shutdown. Trying to re-connect..."); 

        TryConnect();
    }

    void OnCallbackException(object sender, CallbackExceptionEventArgs e)
    {
        if (Disposed) return;

        _logger.LogWarning("A RabbitMQ connection throw exception. Trying to re-connect...");

        TryConnect();
    }

    void OnConnectionShutdown(object sender, ShutdownEventArgs reason)
    {
        if (Disposed) return;

        _logger.LogWarning("A RabbitMQ connection is on shutdown. Trying to re-connect...");

        TryConnect();
    }
}

public interface IConnectionBuilder
{
    IModel CreateModel();
    bool IsConnected { get; }
    bool TryConnect();
}

