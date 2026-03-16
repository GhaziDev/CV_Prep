using System.Reflection.Metadata;
using cv_prep.Models;

namespace cv_prep.DTOs.Requests;

public record UserInfoRequest(User User_,Blob MainCv, Blob? CoverLetter);
