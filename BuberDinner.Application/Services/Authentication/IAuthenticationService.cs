using ErrorOr;

namespace BuberDinner.Application.Services.Authentication;


public interface IAuthenticationService 
{
    ErrorOr<AuthenticationResult> Register(string firstName, string LastName, string email, string password);
    ErrorOr<AuthenticationResult> Login(string firstName, string password);
}