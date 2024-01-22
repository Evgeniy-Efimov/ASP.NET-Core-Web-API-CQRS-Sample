using Application.CQRS.Employee.Commands;
using Application.CQRS.Employee.DTOs;
using Application.CQRS.Employee.Events;
using AutoMapper;

namespace Application.CQRS.Employee
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<Data.Entities.Employee, EmployeeDto>()
				.ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Roles.Select(r => r.Name)));

			CreateMap<CreateEmployeeCommand, Data.Entities.Employee>();

			CreateMap<Data.Entities.Employee, EmployeeCreatedEvent>()
				.ForMember(dest => dest.Employee, opt => opt.MapFrom(src => src));
		}
	}
}
