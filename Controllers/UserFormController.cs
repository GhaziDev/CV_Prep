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

namespace cv_prep.Controllers;

public class UserInfoController : Controller
{

        private readonly ContextDb _db;

     public UserInfoController(ContextDb db)
    {
        _db = db;
        
    }


    public async Task<ActionResult<string>> CreateUserInfo(UserInfoRequest request)
    {


        try{

            if (request.MainCv.Length == 0)
            {
                return BadRequest("CV was not provided");
            }


        

        var latestId = _db.UserInfo.Last().Id;
        
        
        UserInfo user = new UserInfo
        {

            Id= latestId+1,
            
            User_ = _db.User.Find(User.FindFirstValue(ClaimTypes.NameIdentifier))!,
            MainCv = request.MainCv,
            CoverLetter= request.CoverLetter


        };

        return Ok("Your Info has been saved!");

    





        }

        catch(Exception e)
        {
           Log.Error($"{e}");
           return Problem("Internal Server Error");
        }     
        
    }

    public async Task<ActionResult<UserInfoResponse>> GetUserInfo()
    {

        try{
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userInfo = await _db.UserInfo.FirstOrDefaultAsync(ui=>ui.User_.Id==int.Parse(userId!));
        if (userInfo!=null)
        {
            
            return new UserInfoResponse(userInfo.User_,userInfo.MainCv,userInfo.CoverLetter);

            
        }

        return null;
        }
        catch(Exception e){
            Log.Error($"{e}");
            return Problem("Internal Server Error");
        }
        
        
    }


    
}

