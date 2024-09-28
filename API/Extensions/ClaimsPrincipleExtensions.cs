using System;
using System.Runtime.CompilerServices;
using System.Security.Authentication;
using System.Security.Claims;
using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

// looks like a useful digression

// class for getting user account information
public static class ClaimsPrincipleExtensions
{
    // the current user is read from the entity AppUser as a ClaimsPrinciple
    public static async Task<AppUser> GetUserByEmail(this UserManager<AppUser> userManager, ClaimsPrincipal user)
    {
        var userToReturn = await userManager.Users.FirstOrDefaultAsync(x => x.Email == user.GetEmail());

        if (userToReturn == null) throw new AuthenticationException("Email claim not found");

        return userToReturn;
    }

    public static async Task<AppUser> GetUserByEmailWithAddress(this UserManager<AppUser> userManager, ClaimsPrincipal user)
    {
        var userToReturn = await userManager.Users
            .Include(x => x.Address)
            .FirstOrDefaultAsync(x => x.Email == user.GetEmail());

        if (userToReturn == null) throw new AuthenticationException("Email claim not found");

        return userToReturn;
    }

    public static string GetEmail(this ClaimsPrincipal user)        // 'this' keyword extends to a class
    {
        // get email of current user and check if not null
        var email = user.FindFirstValue(ClaimTypes.Email) ?? throw new AuthenticationException("Email claim not found");

        return email;
    }
}