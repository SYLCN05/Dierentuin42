using Dierentuin42.Data;
using Dierentuin42.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dierentuin42.Api
{
    [Route("api/zoos")]
    [ApiController]

    public class ZooApiController : ControllerBase
    {
        private readonly Dierentuin42Context _context;

        public ZooApiController(Dierentuin42Context context)
        {
            _context = context;
        }

        //: Get api/zoos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Zoo>>> GetZoos() 
        {
            var results = await _context.Zoo
                        .Include(z=> z.Enclosures)
                        .ToListAsync();
            return Ok(results);
        }

        //: Get api/zoo
        [HttpGet("{id}")]
         
        public async Task<ActionResult<Zoo>> GetZoo(int id) 
        {
            var result = await _context.Zoo
              .Include(z => z.Enclosures)
              .FirstOrDefaultAsync(z => z.Id == id);

            if (result == null) 
            {
                return NotFound();
            }
            return Ok(result);
        }

        //: Post api/zoos
        [HttpPost]

        public async Task<ActionResult<Zoo>> CreateZoo(Zoo zoo) 
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            _context.Zoo.Add(zoo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetZoo), new {id = zoo.Id}, zoo);
        }


        //: Put api/zoos
        [HttpPut("{id}")]

        public async Task<ActionResult<Zoo>> UpdateZoo(int id, Zoo zoo) 
        {
            if(id != zoo.Id) 
            {
                return BadRequest();
            }
            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            try
            {
                _context.Update(zoo);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) 
            {
                if (! await ZooExists(id)) 
                {
                    return NotFound();
                }
                throw;
            }
            return NoContent();
        }

        //: Delete api/zoo
        [HttpDelete("{id}")]
        public async Task<ActionResult<Zoo>> DeleteZoo(int id) 
        {
            var zoo = await _context.Zoo.FindAsync(id);

            if (zoo == null) 
            {
                return NotFound();
            }

            _context.Remove(zoo);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        //: Search api/zoo/search?
        [HttpGet("search")]

        public async Task<ActionResult<IEnumerable<Zoo>>> SearchZoo([FromQuery] string name) 
        {
            var zoo = await _context.Zoo
                .Include(z=> z.Enclosures)
                .Where(
                z => z.Name.Equals(name)
                )
                .ToListAsync();

           return Ok(zoo);

                
        }

        private async Task<bool> ZooExists(int id) 
        {
           return await _context.Zoo.AnyAsync(z=> z.Id == id);
        } 

    }
}
