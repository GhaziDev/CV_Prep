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
    
    
    private readonly ContextDb _db;




    public JobController(ContextDb db)
    {
         

      
        _db = db;
       

    

        
    }



    [HttpGet("list")]
    [Authorize]
    
    public async Task<ActionResult<List<JobResponse>>> ListJobs()
    {


        var getUser = _db.User.Find(ClaimTypes.NameIdentifier);

        var getJobs = _db.Job.Where(job=>job._User==getUser);

        var JobResponseList = new List<JobResponse>();

        foreach(var job in getJobs)
        {
            JobResponseList.Add(new JobResponse(job.Id,job.AtsKeywords, job.Description , job.Url, job.Role, job.ClosingDate, job.Location));



        }

        return  JobResponseList;



        
    }




[HttpPost("start")]
[Authorize]
public async Task<ActionResult<string>> StartJob(JobRequest request)
    {

        var _user = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);


        var schedulerLog = await _db.SchedulerLog.FirstOrDefaultAsync(u=>u._User==_user);

// the above should be done on updating the scheduler, not starting new one.
        var schedulerClient = new AmazonSchedulerClient();



    

        var schedulerRequest = new CreateScheduleRequest{
             Name= "job-every-1-hour",
    ScheduleExpression = "rate(1 hour)",
    GroupName = "default",
    Target = new Target { Arn = "lambda arn", RoleArn = "scheduler arn", Input = "json data" },

        };

        // the target lambda would have to run the OpenAI API for AI AGENT.


        var response = await schedulerClient.CreateScheduleAsync(schedulerRequest);



        return Ok("Scheduler has been created!");



        //check if requested hour more than recorded hour, and number of execution times has been exceeded
        // if so, don't start the schedule until the next hour
        //if next hour came, reset the no of execution time, and create and run the scheduler

        // current architecture would use the same lambda for all users, because it has small user base.'




        
        
    }


    [HttpDelete("stop")]
    [Authorize]
    public async Task<ActionResult<string>> StopScheduler()
    {

         var _user = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var scheduler = await _db.SchedulerLog.FirstOrDefaultAsync(u=>u._User==_user);

        if(scheduler!=null){
        _db.SchedulerLog.Remove(scheduler);
        return Ok("Scheduler has been stopped and deleted");
        }

        return BadRequest("No Scheduler to stop");



        
    }


    [HttpPut("edit")]
    [Authorize]

    public async Task<ActionResult<string>> EditSchedulerTiming(EditSchedulerRequest request)
    {
        var _user = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

       
        var newNoOfExec = request.NoOfExec;
        var scheduler = await _db.SchedulerLog.FirstAsync(u=>u._User==_user);

        if (scheduler != null)
        {
            scheduler.NoOfExec = newNoOfExec;
            _db.SaveChanges();
            return Ok("Scheduler timing has been updated");
        }

        return NotFound("Scheduler does not exist");

        



    }


    public async Task<ActionResult<string>> StoreJobs(JobRequest request)
    {
        // lambda function would send requests here

        try{
        int id =  _db.Job.LastOrDefault()!.Id+1;

        var userId = await _db.User.FindAsync(int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!));
        if(userId!=null){
        var requestedJob = new Job {Id=id,AtsKeywords=request.AtsKeywords,Description = request.Description, Url = request.Url, Role = request.Role, ClosingDate = request.ClosingDate, Location = request.Location, _User =  userId, Suggestions = request.Suggestions};


        var job = await _db.Job.AddAsync(requestedJob);

        return Ok("Job has been retrieved and stored");
        }

        return Unauthorized("User not authorized");
        }
        catch(Exception e)
        {
            Log.Error($"{e}");


            return Problem("Internal Server Error");

            
        }
        
    }






    /// implement a scheduler
    /// 



}