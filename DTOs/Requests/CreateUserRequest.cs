namespace cv_prep.DTOs.Requests;




public record CreateUserRequest(string FirstName, string LastName, string Email, string Country, bool IsVerified, string Code, string VerificationToken);

