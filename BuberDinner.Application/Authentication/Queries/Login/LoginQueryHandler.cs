using BuberDinner.Application.Authentication.Common;
using BuberDinner.Application.Common.Interfacces.Persistance;
using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Domain.Common.Errors;
using ErrorOr;
using MediatR;

namespace BuberDinner.Application.Authentication.Queries.Login;

public class LoginQueryHandler : 
    IRequestHandler<LoginQuery, ErrorOr<AuthenticationResult>>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;

    public LoginQueryHandler(
        IJwtTokenGenerator jwtTokenGenerator, 
        IUserRepository userRepository)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(
        LoginQuery query, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        // 1. Validate that user exists
        var user = _userRepository.GetUserByEmail(query.Email);
        if (user is null)
        {
            // throw new Exception("User doesn't exist");
            return Errors.Authentication.InvalidCredentials;
        }
        // 2. Validate that password is correct
        if (user.Password != query.Password)
        {
            // throw new Exception("Password is incorrect");
            return Errors.Authentication.InvalidCredentials;
        }
        // 3. Create Jwt token
        var token = _jwtTokenGenerator.GenerateToken(user);

        return new AuthenticationResult(user, token);
    }
}