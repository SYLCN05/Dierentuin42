using Microsoft.AspNetCore.Mvc;
using Dierentuin42.Data;
using Dierentuin42.Models;
using Microsoft.EntityFrameworkCore;

namespace Dierentuin42.Controllers.Api
{
    [Route("api/animals")]
    [ApiController]
    public class AnimalsApiController : ControllerBase
    {
        private readonly Dierentuin42Context _context;

        public AnimalsApiController(Dierentuin42Context context)
        {
            _context = context;
        }

        // GET: api/animals
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Animal>>> GetAnimals()
        {
            var result = await _context.Animal
                .Include(a => a.Category)
                .Include(a => a.Enclosure)
                .ToListAsync();
            return Ok(result);
        }

        // GET: api/animals/id
        [HttpGet("{id}")]
        public async Task<ActionResult<Animal>> GetAnimal(int id)
        {
            var animal = await _context.Animal
                .Include(a => a.Category)
                .Include(a => a.Enclosure)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (animal == null)
            {
                return NotFound();
            }
            return Ok(animal);
        }

        // POST: api/animals
        [HttpPost]
        public async Task<ActionResult<Animal>> CreateAnimal(Animal animal)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _context.Add(animal);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAnimal), new { id = animal.Id }, animal);
        }

        // PUT: api/animals/id
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAnimal(int id, Animal animal)
        {
            if (id != animal.Id)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                _context.Update(animal);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await AnimalExists(id))
                {
                    return NotFound();
                }
                   
                throw;
            }

            return NoContent();
        }

        // DELETE: api/animals/id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnimal(int id)
        {
            var animal = await _context.Animal.FindAsync(id);
            if (animal == null)
            {
                return NotFound();
            }
            _context.Animal.Remove(animal);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // GET: api/animals/search
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Animal>>> SearchAnimals([FromQuery] string name)
        {
            var results = await _context.Animal
                .Where(s => s.Name.Equals(name))
                .ToListAsync();
            return Ok(results);
        }

        private async Task<bool> AnimalExists(int id)
        {
            return await _context.Animal.AnyAsync(e => e.Id == id);
        }
    }
}
