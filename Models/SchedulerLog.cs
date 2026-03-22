using System;
using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;

namespace cv_prep.Models;



[Index(nameof(_User), IsUnique = true)]

public class SchedulerLog
{

    public required DateOnly RecordedHour; //when
    public required DateOnly RequestedHour; // on change/edit

    public required int NoOfExec; //how many times per hour.
    public required int TrackExec; //tells how many times the scheduler has been running overall.

    public required string SchedulerArn; // AWS lambda arn.



    public required int _User; // user Id


}