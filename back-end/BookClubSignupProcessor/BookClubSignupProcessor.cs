using System;
using BookClubSignupProcessor.Models;
using BookClubSignupProcessor.TableAccess;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace BookClubSignupProcessor
{
  public static class BookClubSignupProcessor
  {
    [FunctionName("BookClubSignupProcessor")]
    public static async void Run([QueueTrigger("bookclubsignups")] BookClubSignupRequest request, ILogger log)
    {
      var tableAccess = new StorageTableAccess("BookClucbSignups");
      await tableAccess.Insert(AdaptRequest(request));
    }

    private static BookClubSignupEntity AdaptRequest(BookClubSignupRequest request)
    {
      var ticks = DateTime.Now.Ticks;
      return new BookClubSignupEntity
      {
        GenreSubscription = $"{request.Genre}",
        UniqueName = $"{request.Name}:{ticks}",
        Name = request.Name,
        Email = request.Email,
        Genre = request.Genre
      };
    }
  }
}
