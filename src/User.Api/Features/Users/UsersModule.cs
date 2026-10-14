using System.Security.Claims;
using User.Api.DTOs;
using User.Api.Infrastructure;

namespace User.Api.Features.Users;

public static class UsersModule
{
    public static IEndpointRouteBuilder MapUsers(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/users")
            .WithTags("Users");

        group.MapPost("/register", async (RegisterRequest request, KeycloakAdminService keycloak) => {
            try
            {
                await keycloak.CreateUserAsync(request.Email, request.Password, request.FirstName, request.LastName);
                return Results.Created();
            }
            catch (Exception ex) when (ex.Message.Contains("User exists"))
            {
                return Results.Conflict("User with this email already exists");
            }
            })
            .WithTags("Register")
            .WithSummary("Register");

        group.MapPut("/me", async (UpdateProfileRequest request, ClaimsPrincipal user, KeycloakAdminService keycloak) => {
            var userId = user.FindFirst("sub")?.Value;

            if (userId is null)
            {
                return Results.Unauthorized();
            }

            await keycloak.UpdateUserAsync(userId, request.FirstName, request.LastName, request.Email);

            return Results.NoContent();
        })
        .WithTags("UpdateProfile")
        .WithSummary("UpdateProfile")
        .RequireAuthorization();

        group.MapPut("/me/password", async (ChangePasswordRequest request, ClaimsPrincipal user, KeycloakAdminService keycloak) => {
            var userId = user.FindFirst("sub")?.Value;
            var email = user.FindFirst("email")?.Value;

            if (userId is null || email is null)
            {
                return Results.Unauthorized();
            }

            try
            {
                await keycloak.VerifyPasswordAsync(email, request.CurrentPassword);
            }
            catch (UnauthorizedAccessException)
            {
                return Results.Problem("Current password is incorrect", statusCode: 400);
            }

            await keycloak.ChangePasswordAsync(userId, request.NewPassword);
            return Results.NoContent();
        })
        .WithName("ChangePassword")
        .WithSummary("ChangePassword")
        .RequireAuthorization();

        group.MapDelete("/me", async (ClaimsPrincipal user, KeycloakAdminService keycloak) => {
            var userId = user.FindFirst("sub")?.Value;

            if (userId is null)
            {
                return Results.Unauthorized();
            }

            await keycloak.DeleteUserAsync(userId);
            return Results.NoContent();
        })
        .WithName("DeleteAccount")
        .WithSummary("DeleteAccount")
        .RequireAuthorization();

        group.MapPost("/login", async (LoginRequest request, KeycloakAdminService keycloak, HttpContext ctx, ILogger<Program> logger) => {
            try
            {
                var (accessToken, refreshToken) = await keycloak.GetTokenAsync(request.Email, request.Password);

                ctx.Response.Cookies.Append("refresh_token", refreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = false,
                    SameSite = SameSiteMode.Lax,
                    Expires = DateTimeOffset.UtcNow.AddDays(30)
                });

                return Results.Ok(new {accessToken});
            }
            catch (InvalidOperationException ex)
            {
                return Results.Problem(ex.Message, statusCode: 403);
            }
            catch (UnauthorizedAccessException ex)
            {
                logger.LogError("Login failed: {Message}", ex.Message);
                return Results.Unauthorized();
            }
        })
        .WithName("Login")
        .WithSummary("Login");

        group.MapPost("refresh", async (KeycloakAdminService keycloak, HttpContext ctx) => {
            var refreshToken = ctx.Request.Cookies["refresh_token"];

            if (refreshToken is null)
            {
                return Results.Unauthorized();
            }

            try
            {
                var (accessToken, newRefreshToken) = await keycloak.RefreshTokenAsync(refreshToken);

                ctx.Response.Cookies.Append("refresh_token", newRefreshToken, new CookieOptions
                {
                    HttpOnly = true, Secure = false, SameSite = SameSiteMode.Lax, Expires = DateTimeOffset.UtcNow.AddDays(30)
                });

                return Results.Ok(new {accessToken});
            }
            catch (UnauthorizedAccessException)
            {
                ctx.Response.Cookies.Delete("refresh_token");
                return Results.Unauthorized();
            }
        })
        .WithTags("Refresh")
        .WithSummary("Refresh");

        group.MapPost("/logout", (HttpContext ctx) => {
                ctx.Response.Cookies.Delete("refresh_token");
                return Results.NoContent();
            })
            .WithTags("Logout")
            .WithSummary("Logout");

        return app;
    }
}
