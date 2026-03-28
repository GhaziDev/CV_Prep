
using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;

namespace cv_prep.Models;

[PrimaryKey("Id")]
public class UserInfo
{

    public required int Id;

    public required User User_ {get;set;}
    public required byte[] MainCv {get;set;}

    public byte[]? CoverLetter {get;set;}

    public string[]? DesiredRoles {get;set;}

    public int NoOfTimesExec {get;set;}
    
}