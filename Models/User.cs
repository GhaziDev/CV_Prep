namespace cv_prep.Models;

using System;
using Microsoft.EntityFrameworkCore;

[PrimaryKey("Id")]
public class User
{
    public int Id {get;set;}
    public required string FirstName {get;set;}
    public required string LastName {get;set;}
    public required string Email {get;set;}
    public required string Code {get;set;} // must be hashed like passwords
    public required string VerificationToken {get;set;}
    public required string Country {get;set;}
    public required bool IsVerified {get;set;}




}
