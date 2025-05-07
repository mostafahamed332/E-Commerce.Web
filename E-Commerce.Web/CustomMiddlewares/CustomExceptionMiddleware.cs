using Domain.Exceptions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;
using Shared.ErrorModule;
using System.Text.Json;

namespace E_Commerce.Web.CustomMiddlewares
{
	public class CustomExceptionMiddleware
	{
		private readonly RequestDelegate next;
		private readonly ILogger<CustomExceptionMiddleware> logger;

		public CustomExceptionMiddleware(RequestDelegate Next, ILogger<CustomExceptionMiddleware> logger)
		{
			next = Next;
			this.logger = logger;
		}


		public async Task Invoke(HttpContext httpContext)
		{
			try
			{
				await next.Invoke(httpContext);
				if(httpContext.Response.StatusCode == StatusCodes.Status404NotFound)
				{
					var Response = new ErrorToReturn()
					{
						StatusCode = httpContext.Response.StatusCode,
						ErrorMessage = $"End point {httpContext.Request.Path} is not found"
					};

					var ResponseToReturn = JsonSerializer.Serialize(Response);

					await httpContext.Response.WriteAsync(ResponseToReturn);
				}
			}
			catch(Exception ex)
			{
				logger.LogError(ex, "Something is wrong");
				httpContext.Response.StatusCode = ex switch
				{
					NotFoundException => StatusCodes.Status404NotFound,
					_ => StatusCodes.Status500InternalServerError,
				};

				httpContext.Response.ContentType = "application/json";

				var Response = new ErrorToReturn()
				{
					StatusCode = httpContext.Response.StatusCode,
					ErrorMessage = ex.Message,
				};

				var ResponseToReturn = JsonSerializer.Serialize(Response);

				await httpContext.Response.WriteAsync(ResponseToReturn);


			}
		}
	}
}
