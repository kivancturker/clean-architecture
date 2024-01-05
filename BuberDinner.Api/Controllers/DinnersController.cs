using Microsoft.AspNetCore.Mvc;

namespace BuberDinner.Api.Controllers;

[Route("[controller]")]
public class DinnersController : ApiController 
{
    public IActionResult ListDinners()
    {
        return Ok(Array.Empty<string>());
    }
}