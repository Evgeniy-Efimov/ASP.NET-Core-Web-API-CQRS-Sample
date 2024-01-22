using Application.CQRS.Employee.Events;
using AutoMapper;
using Data.Context;
using FluentValidation;
using MediatR;

namespace Application.CQRS.Employee.Commands
{
	public class CreateEmployeeCommand : IRequest<Guid>
	{
		public string Name { get; set; }
		public string Login { get; set; }
		public string Email { get; set; }
		public string Department { get; set; }
		public string Location { get; set; }
	}

	public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, Guid>
	{
		private readonly IMapper _mapper;
		private readonly IAppDbContext _context;
		private readonly IPublisher _publisher;

		public CreateEmployeeCommandHandler(IMapper mapper, 
			IAppDbContext context,
			IPublisher publisher)
		{
			_mapper = mapper;
			_context = context;
			_publisher = publisher;
		}

		public async Task<Guid> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
		{
			var newEmployee = _mapper.Map<Data.Entities.Employee>(request);

			await _context.Employees.AddAsync(newEmployee, cancellationToken);

			await _context.SaveChangesAsync(cancellationToken);

			var newEmployeeEvent = _mapper.Map<EmployeeCreatedEvent>(newEmployee);
			_ = _publisher.Publish(newEmployeeEvent);

			return newEmployee.Id;
		}
	}

	public sealed class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
	{
		public CreateEmployeeCommandValidator()
		{
			RuleFor(x => x.Name).NotEmpty();
			RuleFor(x => x.Email).NotEmpty();
			RuleFor(x => x.Login).NotEmpty();
		}
	}
}
