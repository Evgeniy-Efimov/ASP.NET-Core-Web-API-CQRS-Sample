using System.Net;

namespace Api.Middlewares
{
	public class ExceptionHandlingMiddleware
	{
		private const string RequestIdHeader = "web-app-request-id";

		private readonly RequestDelegate _requestDelegate;

		public ExceptionHandlingMiddleware(RequestDelegate requestDelegate)
		{
			_requestDelegate = requestDelegate;
		}

		public async Task Invoke(HttpContext context)
		{
			try
			{
				context.Request.Headers.Add(RequestIdHeader, Guid.NewGuid().ToString());

				await this._requestDelegate(context);
			}
			catch (Exception ex)
			{
				await HandleExceptionAsync(context, ex);
			}
		}

		private Task HandleExceptionAsync(HttpContext context, Exception exception)
		{
			var result = "";

			var requestId = context.Request.Headers[RequestIdHeader];

			switch (exception)
			{
				default:
					context.Response.ContentType = "text/plain";
					context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
					result = $"Something went wrong. Request id: {requestId}";

					//TODO: Add logs

					break;
			}

			return context.Response.WriteAsync(result);
		}
	}
}
