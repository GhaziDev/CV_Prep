using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using cv_prep.DTOs.Responses;
using cv_prep.Models;
using cv_prep.DTOs.Requests;
using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Http.HttpResults;
using Serilog;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Serilog.Core;
using Microsoft.AspNetCore.Authorization;
using Amazon.Scheduler;
using Amazon.Scheduler.Model;


namespace cv_prep.Controllers;




class JobController:Controller
{
    
    private readonly JobDb _db;

    private readonly UserDb _userDb;

    private readonly UserInfoDb _userInfoDb;

    private readonly SchedulerLogDb _schedulerLogDb;


    public JobController(JobDb db, UserDb userDb, UserInfoDb userInfoDb)
    {
         

        _userDb = userDb;
        _db = db;
        this._userInfoDb = userInfoDb;

    

        
    }



    [HttpGet("list")]
    [Authorize]
    
    public async Task<ActionResult<List<JobResponse>>> ListJobs()
    {


        var getUser = _userDb.User.Find(ClaimTypes.NameIdentifier);

        var getJobs = _db.Job.Where(job=>job._User==getUser);

        var JobResponseList = new List<JobResponse>();

        foreach(var job in getJobs)
        {
            JobResponseList.Add(new JobResponse(job.Id,job.Cv, job.CoverLetter, job.Description , job.Url, job.Role, job.ClosingDate, job.Location));



        }

        return  JobResponseList;



        
    }



public async Task<ActionResult<string>> StartJob(JobRequest request)
    {

        var _user = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);


        var schedulerLog = await _schedulerLogDb.SchedulerLog.FirstOrDefaultAsync(u=>u._User==_user);

        var NoOfTimesExec = schedulerLog!.NoOfExec;
        var trackExec = schedulerLog!.TrackExec;
        var recordedHour = schedulerLog.RecordedHour;
        var requestedHour = schedulerLog.RequestedHour;
// the above should be done on updating the scheduler, not starting new one.
        var schedulerClient = new AmazonSchedulerClient();


        if(requestedHour<=recordedHour && trackExec >= NoOfTimesExec)
        {
            return BadRequest("out of range requested hours");
        }

        else{

        var schedulerRequest = new CreateScheduleRequest{
             Name= "job-every-5-min",
    ScheduleExpression = "rate(1 hour)",
    GroupName = "default",
    Target = new Target { Arn = "lambda arn", RoleArn = "scheduler arn", Input = "json data" },

        };

        // the target lambda would have to run the OpenAI API for AI AGENT.


        var response = await schedulerClient.CreateScheduleAsync(schedulerRequest);



        return Ok("Scheduler has been created!");

        }


        //check if requested hour more than recorded hour, and number of execution times has been exceeded
        // if so, don't start the schedule until the next hour
        //if next hour came, reset the no of execution time, and create and run the scheduler

        // current architecture would use the same lambda for all users, because it has small user base.'




        
        
    }


    public Task<ActionResult<string>> StopScheduler()
    {
        //
        
    }



    /// implement a scheduler
    /// 



}