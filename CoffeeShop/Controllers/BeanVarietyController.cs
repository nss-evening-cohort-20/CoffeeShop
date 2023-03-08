using Microsoft.AspNetCore.Mvc;
using CoffeeShop.Models;
using CoffeeShop.Repositories;
using Microsoft.Data.SqlClient;

namespace CoffeeShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BeanVarietyController : ControllerBase
    {
        private readonly IBeanVarietyRepository _beanVarietyRepository;

        public BeanVarietyController(IBeanVarietyRepository beanVarietyRepository)
        {
            _beanVarietyRepository = beanVarietyRepository;
        }

        // https://localhost:5001/api/beanvariety/
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_beanVarietyRepository.GetAll());
        }

        // https://localhost:5001/api/beanvariety/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var variety = _beanVarietyRepository.Get(id);
            if (variety == null)
            {
                return NotFound();
            }
            return Ok(variety);
        }

        // https://localhost:5001/api/beanvariety/
        [HttpPost]
        public IActionResult Post(BeanVariety beanVariety)
        {
            _beanVarietyRepository.Add(beanVariety);
            return CreatedAtAction("Get", new { id = beanVariety.Id }, beanVariety);
        }

        // https://localhost:5001/api/beanvariety/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, BeanVariety beanVariety)
        {
            if (id != beanVariety.Id)
            {
                return BadRequest();
            }

            _beanVarietyRepository.Update(beanVariety);
            return NoContent();
        }

        // https://localhost:5001/api/beanvariety/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var found = _beanVarietyRepository.Get(id);
            if (found == null) { return NotFound(); }
            try
            {
                _beanVarietyRepository.Delete(id);

            }
            catch (SqlException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {

                throw;
            }

            return NoContent();
        }
    }
}