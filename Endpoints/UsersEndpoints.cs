using System.Runtime.CompilerServices;
using DocsService.Services;
using DocsService.Contracts;

namespace DocsService.Endpoints
{
    public static class UsersEndpoints
    {
        public static IEndpointRouteBuilder MapUsersEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("register", Register);
            app.MapPost("login", Login);
            return app;
        }


        private static async Task<IResult> Register(RegisterUserRequests request, UserService usersService)
        {
            await usersService.Register(request.UserName, request.Email, request.Password,
                request.FirstName, request.LastName, request.LastName,
                request.Position, request.DocumentNumber);
            return Results.Ok();
        }

        private static async Task<IResult> Login(LoginUsersRequest request, UserService usersService, HttpContext context)
        {
            var token = await usersService.Login(request.Email, request.Password);
            context.Response.Cookies.Append("acookies", token);
            return Results.Ok();
        }
    }
}
