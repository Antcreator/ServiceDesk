using System.Text.Json;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;

namespace ServiceDesk.Util;

public class QueueService
{
    private readonly QueueClient _client;

    public QueueService()
    {
        _client = new QueueClient("UseDevelopmentStorage=true", "notifications-queue", new QueueClientOptions
        {
            MessageEncoding = QueueMessageEncoding.Base64
        });
    }

    public async Task<string?> GetMessageAsync()
    {
        QueueProperties props = await _client.GetPropertiesAsync();

        if (props.ApproximateMessagesCount == 0)
        {
            return null;
        }
        
        QueueMessage message = await _client.ReceiveMessageAsync();
        string text = message.MessageText;

        await DeleteMessageAsync(message);

        return text;
    }
    
    public async Task DeleteMessageAsync(QueueMessage message)
    {
        await _client.DeleteMessageAsync(message.MessageId, message.PopReceipt);
    }

    public async Task SendMessageAsync(object message)
    {
        var text = JsonSerializer.Serialize(message);

        await _client.SendMessageAsync(text);
    }
}
