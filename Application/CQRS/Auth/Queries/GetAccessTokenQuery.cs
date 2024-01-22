using Application.Providers.JwtTokenProvider.Interfaces;
using Application.Providers.JwtTokenProvider.Models;
using AutoMapper;
using Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace Application.CQRS.Auth.Queries
{
	public record GetAccessTokenQuery : IRequest<string>
	{
		[JsonIgnore] 
		public string UserIdentity { get; init; }
	}

	internal class GetAccessTokenQueryHandler : IRequestHandler<GetAccessTokenQuery, string>
	{
		private readonly IJwtTokenProvider _jwtTokenProvider;
		private readonly IAppDbContext _context;
		private readonly IMapper _mapper;

		public GetAccessTokenQueryHandler(IJwtTokenProvider jwtTokenProvider, IAppDbContext context, IMapper mapper)
		{
			_jwtTokenProvider = jwtTokenProvider;
			_context = context;
			_mapper = mapper;
		}

		public async Task<string> Handle(GetAccessTokenQuery request, CancellationToken cancellationToken)
		{
			var employee = await _context.Employees
				.Include(e => e.Roles)
				.FirstOrDefaultAsync(e => e.Login == request.UserIdentity, cancellationToken);

			if (employee == null)
			{
				throw new Exception($"User {request.UserIdentity} was not found");
			}

			var userData = _mapper.Map<JwtUserData>(employee);
			var token = _jwtTokenProvider.GetToken(userData);

			return token;
		}
	}
}
