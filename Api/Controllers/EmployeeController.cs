using Application.CQRS.Employee.Commands;
using Application.CQRS.Employee.Queries;
using Application.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class EmployeeController : ControllerBase
	{
		private readonly IMediator _mediator;

		public EmployeeController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet]
		[Authorize(Roles = Roles.User)]
		public async Task<IActionResult> GetEmployees(CancellationToken cancellationToken = default)
		{
			var result = await _mediator.Send(new GetEmployeesQuery(), cancellationToken);

			return Ok(result);
		}

		[HttpPost]
		[Authorize(Roles = Roles.Admin)]
		public async Task<IActionResult> CreateEmployee(CreateEmployeeCommand request, CancellationToken cancellationToken = default)
		{
			var result = await _mediator.Send(request, cancellationToken);

			return Ok(result);
		}
	}
}