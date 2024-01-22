using Application.Providers.JwtTokenProvider.Models;
using AutoMapper;

namespace Application.CQRS.Auth
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<Data.Entities.Employee, JwtUserData>()
				.ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Roles.Select(r => r.Name)));
		}
	}
}
