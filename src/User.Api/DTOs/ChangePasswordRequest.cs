namespace User.Api.DTOs;

public record ChangePasswordRequest(string CurrentPassword, string NewPassword);
