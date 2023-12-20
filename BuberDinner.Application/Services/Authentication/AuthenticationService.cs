using BuberDinner.Application.Common.Interfacces.Persistance;
using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Domain.Entities;

namespace BuberDinner.Application.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository  _userRepository;

    public AuthenticationService(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
    }

    public AuthenticationResult Register(string firstName, string LastName, string email, string password)
    {
        // 1. Validate that user doesn't exist
        var user = _userRepository.GetUserByEmail(email);
        if (user is not null)
        {
            throw new Exception("User already exists");
        }
        // 2. Create user (Generate unique id) and persist to DB
        user = new User
        {
            FirstName = firstName,
            LastName = LastName,
            Email = email,
            Password = password
        };
        _userRepository.Add(user);
        // 3. Create Jwt token
        Guid userId = Guid.NewGuid();
        var token = _jwtTokenGenerator.GenerateToken(user);
        
        return new AuthenticationResult(user, token);
    }

    public AuthenticationResult Login(string email, string password)
    {
        // 1. Validate that user exists
        var user = _userRepository.GetUserByEmail(email);
        if (user is null)
        {
            throw new Exception("User doesn't exist");
        
        }
        // 2. Validate that password is correct
        if (user.Password != password)
        {
            throw new Exception("Password is incorrect");
        }
        // 3. Create Jwt token
        var token = _jwtTokenGenerator.GenerateToken(user);

        return new AuthenticationResult(user, token);
    }
}