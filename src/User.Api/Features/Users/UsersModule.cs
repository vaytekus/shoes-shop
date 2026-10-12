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

        return app;
    }
}
