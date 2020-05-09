using Microsoft.Azure.Cosmos.Table;

namespace BookClubSignupProcessor.TableAccess
{
  public class BookClubSignupEntity : TableEntity
  {
    [IgnoreProperty]
    public string GenreSubscription
    {
      get { return PartitionKey; }
      set { PartitionKey = value; }
    }

    [IgnoreProperty]
    public string UniqueName
    {
      get { return RowKey; }
      set { RowKey = value; }
    }

    public string Name { get; set; }
    public string Email { get; set; }
    public string Genre { get; set; }
  }
}