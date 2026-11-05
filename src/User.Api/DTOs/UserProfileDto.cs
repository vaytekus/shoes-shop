namespace User.Api.DTOs;

public record UserProfileDto(string FirstName, string LastName, string Email, string? AvatarUrl);
