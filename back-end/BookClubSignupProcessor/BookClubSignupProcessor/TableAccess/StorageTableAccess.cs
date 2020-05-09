using Microsoft.Azure.Cosmos.Table;
using System.Threading.Tasks;

namespace BookClubSignupProcessor.TableAccess
{
  public class StorageTableAccess
  {
    private const string _connectionString = "DefaultEndpointsProtocol=http;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;";
    private readonly string _tableName;

    public StorageTableAccess(string tableName)
    {
      _tableName = tableName;
    }

    public async Task Insert(BookClubSignupEntity entity)
    {
      // Get a reference to our table
      var table = await GetTableAsync();

      // Create an insert operation for the entity we wish to store
      var insert = TableOperation.Insert(entity);

      // Execute that operation using our table reference
      await table.ExecuteAsync(insert);
    }

    private async Task<CloudTable> GetTableAsync()
    {
      // Using our connection string, get access to our CloudStorageAccount
      var storageAccount = CloudStorageAccount.Parse(_connectionString);

      // From the storage account get a table client
      var tableClient = storageAccount.CreateCloudTableClient();

      // Use this table client to get a reference to our table
      var table = tableClient.GetTableReference(_tableName);

      // Create the table if it doesn't exist
      await table.CreateIfNotExistsAsync();

      // Return the table reference
      return table;
    }
  }
}