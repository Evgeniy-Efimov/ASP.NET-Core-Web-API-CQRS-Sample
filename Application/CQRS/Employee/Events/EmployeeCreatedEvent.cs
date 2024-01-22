using Application.CQRS.Employee.DTOs;
using AutoMapper;
using MediatR;

namespace Application.CQRS.Employee.Events
{
	public class EmployeeCreatedEvent : INotification
	{
		public EmployeeDto Employee { get; set; }
	}

	public class EmployeeCreatedEventHandler : INotificationHandler<EmployeeCreatedEvent>
	{
		public EmployeeCreatedEventHandler(IMapper mapper)
		{
		}

		public Task Handle(EmployeeCreatedEvent employeeCreatedEvent, CancellationToken cancellationToken)
		{
			//TODO: Add notification using provider

			return Task.CompletedTask;
		}
	}
}
