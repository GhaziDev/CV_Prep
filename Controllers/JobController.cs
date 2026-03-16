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

namespace cv_prep.Controllers;




class JobController:Controller
{
    
    private readonly JobDb _db;

    private readonly UserDb _userDb;

    private readonly UserInfoDb _userInfoDb;


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



    /// implement a scheduler
    /// 



}