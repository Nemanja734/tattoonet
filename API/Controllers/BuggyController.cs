using System;
using System.Security.Claims;
using API.DTOs;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

// endpoint for our client to make error pages
public class BuggyController : BaseApiController
{
    [HttpGet("unauthorized")]
    public IActionResult GetUnauthorized()
    {
        return Unauthorized();
    }

    [HttpGet("badrequest")]
    public IActionResult GetBadRequest()
    {
        return BadRequest("Not a good request");
    }

    [HttpGet("notfound")]
    public IActionResult GetNotFound()
    {
        return NotFound();
    }

    [HttpGet("internalerror")]
    public IActionResult GetInternalError()
    {
        throw new Exception("This is a test exception");
    }

    [HttpPost("validationerror")]
    public IActionResult GetValidationError(CreateProductDto product)
    {
        return Ok();
    }

    // testing authentication of an endpoint
    // this endpoint should only work if the user has cookies enabled
    [Authorize]
    [HttpGet("secret")]
    public IActionResult GetSecret()
    {
        // FindFirst gets information contained in the cookie to authorize that user
        var name = User.FindFirst(ClaimTypes.Name)?.Value;      // Name == username (email)
        var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;      // NameIdentifier == User ID stored in the database

        return Ok("Hello " + name + " with the id of " + id);
    }
}
