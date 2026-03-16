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

namespace cv_prep.Controllers;

public class UserController : Controller
{

     private readonly UserDb _db;

     public UserController(UserDb db)
    {
        _db = db;
    }
    
    [HttpGet]
    public async Task<ActionResult<GetUserResponse>> Index(int id)
    {
        var user = await _db.User.FirstOrDefaultAsync(u => u.Id == id);
        if (user is null) return NotFound();

        var dto = new GetUserResponse(user.FirstName, user.LastName, user.Email);
        return Ok(dto);
        
    }

    public async Task<ActionResult<string>> Create(CreateUserRequest user)
    {

        if (user == null)
        {
            return ValidationProblem();
        }

        var userInst = new User
        {
            FirstName =user.FirstName,
            LastName=user.LastName,
            Email=user.Email,
            Country=user.Country,
            Code="",
            VerificationToken="",
            IsVerified=false
        };

        var CreateUser = _db.User.Add(userInst);
        await _db.SaveChangesAsync();
        return "User has been created successfully";

        
    }



    public async Task<ActionResult<string>> Login(LoginRequest userInfo)
    {


        try{

        if(userInfo.Email.Length>0 && userInfo.OtpCode?.Length==0)
        {

            return Unauthorized("Requires OTP Code");

            
        }

        if(userInfo.Email.Length>0 && userInfo.OtpCode?.Length > 0)
        {

        
          User? Email = await _db.User.FirstOrDefaultAsync(u=>u.Email==userInfo.Email);

            if (Email==null)
            {
                return BadRequest("Wrong Email");
            }

        User? OtpCode = await _db.User.FirstOrDefaultAsync(u=>u.Code==userInfo.OtpCode);

            if (OtpCode == null)
            {
                return BadRequest("Wrong code, Please try again");
            }

            // login here and return a success response.

            //first create a claim:

            List<Claim> claim =new(){
            
                new (ClaimTypes.Email,userInfo.Email),
                new (ClaimTypes.NameIdentifier, _db.User.First(u=>u.Email == userInfo.Email).Id.ToString())
            };


                
            ClaimsIdentity identity = new(claim,CookieAuthenticationDefaults.AuthenticationScheme);

            ClaimsPrincipal principal = new(identity);

                
            

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,principal);
            return Ok("You are now logged in");



            
        }

        return BadRequest("Insert Your Email Address");
        }
        catch(Exception e)
        {

            Log.Error($"{e}");
            return Problem("Internal Server Error");
        }

        
    }
}
