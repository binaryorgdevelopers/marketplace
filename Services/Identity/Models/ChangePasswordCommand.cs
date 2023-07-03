namespace Identity.Models;

public record ChangePasswordCommand(string Email, string OldPassword, string NewPassword);