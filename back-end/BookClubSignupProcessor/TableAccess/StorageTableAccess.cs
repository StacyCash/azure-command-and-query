using Microsoft.Azure.Cosmos.Table;
using System.Threading.Tasks;

namespace BookClubSignupProcessor.TableAccess
{
  public class StorageTableAccess
  {
    private const string _connectionString = "<Your Azure Storage Account Connection String>";
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
