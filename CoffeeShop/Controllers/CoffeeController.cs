using CoffeeShop.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeShop.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CoffeeController : ControllerBase
{
    private readonly ICoffeeRepository _coffeeRepo;

    public CoffeeController(ICoffeeRepository coffeeRepo)
    {
        _coffeeRepo = coffeeRepo;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_coffeeRepo.GetAll());
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var found = _coffeeRepo.GetById(id);

        if (found == null)
        {
            return NotFound();
        }
        return Ok(found);
    }
}
