using System;
using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;

namespace cv_prep.Models;


[PrimaryKey("Id")]
public class Job
{
    public required int Id;

    public required string Description;

    public required List<string> AtsKeywords;
    public required List<string> Suggestions;
    

    public required string Url;

    public required string Role;

    public required DateOnly? ClosingDate;

    public required string Location;


    public required User _User;

}