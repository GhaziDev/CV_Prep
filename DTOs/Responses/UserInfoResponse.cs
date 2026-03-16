using System;
using System.Reflection.Metadata;
using cv_prep.Models;

namespace cv_prep.DTOs.Responses;


public record UserInfoResponse(User User_,Blob MainCv, Blob Coverletter);