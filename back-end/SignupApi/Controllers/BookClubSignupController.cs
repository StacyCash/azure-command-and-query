using SignupApi.QueueHelper;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SignupApi.Models;

namespace SignupApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BookClubSignupController : ControllerBase
	{
		[HttpPost]
		public async Task Post([FromBody] BookClubSignupRequest request)
		{
			await new QueueAccess("bookclubsignups").Add(request);
		}
	}
}
