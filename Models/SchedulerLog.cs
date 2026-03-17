using System;
using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;

namespace cv_prep.Models;



[Index(nameof(_User), IsUnique = true)]

public class SchedulerLog
{

    public required DateOnly RecordedHour;
    public required DateOnly RequestedHour;

    public required int NoOfExec;
    public required int TrackExec;

    public required string SchedulerArn;



    public required int _User;


}