using System;
using System.Net;
using System.Text.Json;
using ebanx_api.AccountRecords;
using Microsoft.AspNetCore.Mvc;

namespace ebanx_api
{
	[ApiController]
	public class Controller : ControllerBase
	{
		[HttpPost("reset")]
		public IActionResult Reset()
		{
			Services.ResetAccounts();
			return new OkObjectResult(nameof(HttpStatusCode.OK));
		}

		[HttpGet("balance")]
		public IActionResult GetBalance(string account_id)
		{
			var account = Services.GetAccount(account_id);

			if (account is null)
			{
				return new NotFoundObjectResult(0);
			}

			return new OkObjectResult(account.Balance);
		}

		[HttpPost("event")]
		public IActionResult Event(AccountRecords.AccountDto account)
		{
			try
			{
                var result = Services.UpsertAccountAndBalance(account);

                return new ObjectResult(result) { StatusCode = 201 };
            }
			catch
			{
				return new NotFoundObjectResult(0);
			}
		}
	}
}

