using CoffeeShop.Models;
using CoffeeShop.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

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

    [HttpPost]
    public IActionResult Post(Coffee coffee)
    {
        try
        {
            if (!_coffeeRepo.Insert(coffee))
            {
                return BadRequest(coffee);
            }

            return CreatedAtAction("Get", new { id = coffee.Id }, coffee);
        }
        catch (SqlException ex)
        {
            if (ex.Message.Contains("FOREIGN KEY constraint"))
            {
                return BadRequest("That bean variety doesn't exist, you can't do that!");
            }
            
            return StatusCode(500, ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, Coffee coffee)
    {
        try
        {
            if (id != coffee.Id || !_coffeeRepo.Update(coffee))
            {
                return BadRequest();
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        try
        {
            var found = _coffeeRepo.GetById(id) is not null;
            if (!found)
            {
                return NotFound();
            }

            if (!_coffeeRepo.Delete(id))
            {
                return BadRequest();
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
