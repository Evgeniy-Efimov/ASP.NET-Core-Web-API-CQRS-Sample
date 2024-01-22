using Application.CQRS.Auth.Queries;
using Application.Providers.HttpContextProvider.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS;

namespace Api.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AuthController : ControllerBase
	{
		private readonly IMediator _mediator;
		private readonly IHttpContextProvider _httpContextProvider;

		public AuthController(IMediator mediator, IHttpContextProvider httpContextProvider)
		{
			_mediator = mediator;
			_httpContextProvider = httpContextProvider;
		}

		//IIS domain auth
		[Authorize(IISServerDefaults.AuthenticationScheme)]
		[HttpGet("access-token")]
		public async Task<string> LogIn()
		{
			var command = new GetAccessTokenQuery
			{
				UserIdentity = _httpContextProvider.GetUserLogin()
			};

			return await _mediator.Send(command);
		}
	}
}
