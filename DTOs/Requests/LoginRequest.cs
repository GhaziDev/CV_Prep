namespace cv_prep.DTOs.Requests;


public record LoginRequest(string Email, string? OtpCode);