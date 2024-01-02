using BuberDinner.Application.Authentication.Commands.Register;
using BuberDinner.Application.Authentication.Common;
using BuberDinner.Application.Authentication.Queries.Login;
using BuberDinner.Contracts.Authentication;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BuberDinner.Api.Controllers;

[ApiController]
[Route("auth")]
public class AuthenticationController : ApiController
{
    private readonly ISender _mediator;
    public AuthenticationController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var command = new RegisterCommand(
            request.FirstName, 
            request.LastName, 
            request.Email, 
            request.Password
        );
        ErrorOr<AuthenticationResult> authResult = await _mediator.Send(command);
        
        return authResult.Match(
            authResult => Ok(MapAuthResponse(authResult)),
            errors => Problem(errors)
        );
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var query = new LoginQuery(
            request.Email, 
            request.Password
        );
        
        var authResult = await _mediator.Send(query);



        return authResult.Match(
            authResult => Ok(MapAuthResponse(authResult)),
            errors => Problem(errors)
        );
    }

    private AuthenticationResponse MapAuthResponse(AuthenticationResult authResult)
    {
        return new AuthenticationResponse(
            authResult.User.Id, 
            authResult.User.Email, 
            authResult.User.LastName, 
            authResult.User.FirstName, 
            authResult.Token);
    }
}