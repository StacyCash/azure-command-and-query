using Newtonsoft.Json;
using Azure.Storage.Queues;
using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SignupApi.QueueHelper
{
  public class QueueAccess
  {
    private const string _connectionString = "<Your Azure Storage Account Connection String Here>";
    private readonly string _queueName;

    public QueueAccess(string queueName)
    {
      _queueName = queueName;
    }

    public async Task<string> Add(object message)
    {
      // Get a queue client
      var queueClient = await GetQueueClient();

      // Encode our message as JSON
      var json = EncodeMessage(message);

      // Send the encoded message
      var result = await queueClient.SendMessageAsync(json);

      // Do something with the result
      return result.Value.MessageId;
    }

    private async Task<QueueClient> GetQueueClient()
    {
      // Get a queue service client using the connection string property from our code
      var queueServiceClient = new QueueServiceClient(_connectionString);

      // Use the queue client to get a reference to a queue
      var queueClient = queueServiceClient.GetQueueClient(_queueName);

      // Check the queue exists, and if it doesn't create it (requires Azure.Storage.Queues version `12.3.0` or higher)
      await queueClient.CreateIfNotExistsAsync();

      // Return the `QueueClient`
      return queueClient;
    }

    private string EncodeMessage(object message)
    {
      string messageAsString = JsonConvert.SerializeObject(message);
      return ToBase64(messageAsString);
    }

    private string ToBase64(string input)
    {
      // Get a byte array from the input string
      byte[] inputAsBytes = Encoding.UTF8.GetBytes(input);

      // Convert those bytes to a Base64 string
      return Convert.ToBase64String(inputAsBytes);
    }
  }
}
