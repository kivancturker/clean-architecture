using BuberDinner.Domain.Entities;

namespace BuberDinner.Application.Common.Interfacces.Persistance;

public interface IUserRepository 
{
    User? GetUserByEmail(string email);
    void Add(User user);
}