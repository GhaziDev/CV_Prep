
using System.Reflection.Metadata;

namespace cv_prep.Models;

public class UserInfo
{

    public required User User_ {get;set;}
    public required Blob MainCv {get;set;}

    public Blob? CoverLetter {get;set;}
    
}