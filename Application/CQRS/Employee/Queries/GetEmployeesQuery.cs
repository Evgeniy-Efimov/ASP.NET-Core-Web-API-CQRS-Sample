using Application.CQRS.Employee.DTOs;
using AutoMapper;
using Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Employee.Queries
{
	public class GetEmployeesQuery : IRequest<IEnumerable<EmployeeDto>>
	{
	}

	public class GetEmployeesQueryHandler : IRequestHandler<GetEmployeesQuery, IEnumerable<EmployeeDto>>
	{
		private readonly IMapper _mapper;
		private readonly IAppDbContext _dbContext;

		public GetEmployeesQueryHandler(IMapper mapper, IAppDbContext dbContext)
		{
			_mapper = mapper;
			_dbContext = dbContext;
		}

		public async Task<IEnumerable<EmployeeDto>> Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
		{
			var employees = await _dbContext.Employees.AsNoTracking().ToListAsync(cancellationToken);

			return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
		}
	}
}
