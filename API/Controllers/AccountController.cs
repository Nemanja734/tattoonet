using System;
using System.Security.Claims;
using API.DTOs;
using API.Extensions;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

// More on how to use Identity to secure a Web API backend for SPAs (Singe Page Application): https://learn.microsoft.com/en-us/aspnet/core/security/?view=aspnetcore-8.0

// SignInManager comes from Identity and manages users
public class AccountController(SignInManager<AppUser> signInManager) : BaseApiController        
{
    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterDto registerDto)
    {
        // AppUser implements IdentityUser Framework
        var user = new AppUser
        {
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            Email = registerDto.Email,
            UserName = registerDto.Email
        };

        // create new user with Identity method
        var result = await signInManager.UserManager.CreateAsync(user, registerDto.Password);

        // check if new user was created
        if (!result.Succeeded) 
        {
            // get every error at registration (no name, no email, unsecure password)
            foreach (var error in result.Errors)
            {
                // ModelState describes the current state, in this case it saves the errors in its dictionary
                // the errors are coming from Identity Framework
                ModelState.AddModelError(error.Code, error.Description);
            }

            return ValidationProblem();
        }

        return Ok();
    }

    [Authorize]     // user is logged in
    [HttpPost("logout")]
    public async Task<ActionResult> Logout()
    {
        // log user out using the IdentityFramework
        await signInManager.SignOutAsync();

        return NoContent();
    }

    // get user information of the currently logged in user
    [HttpGet("user-info")]
    public async Task<ActionResult> GetUserInfo()
    {
        if (User.Identity?.IsAuthenticated == false) return NoContent();

        // User is an object of ClaimsPrinciple here
        var user = await signInManager.UserManager.GetUserByEmailWithAddress(User);

        return Ok(new
        {
            user.FirstName,
            user.LastName,
            user.Email,
            Address = user.Address.ToDto()
        });
    }

    // check if user is authenticated (logged in)
    [HttpGet]
    public ActionResult GetAuthState()
    {
        return Ok(new {IsAuthenticated = User.Identity?.IsAuthenticated ?? false});
    }

    [Authorize]
    [HttpPost("address")]
    public async Task<ActionResult<Address>> CreateOrUpdateAddress(AddressDto addressDto)
    {
        var user = await signInManager.UserManager.GetUserByEmailWithAddress(User);

        if (user.Address == null)
        {
            user.Address = addressDto.ToEntity();
        }
        else
        {
            user.Address.UpdateFromDto(addressDto);
        }

        var result = await signInManager.UserManager.UpdateAsync(user);

        if (!result.Succeeded) return BadRequest("Problem updating user address");

        return Ok(user.Address.ToDto());
    }
}