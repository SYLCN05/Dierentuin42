using Dierentuin42.Controllers;
using Dierentuin42.Data;
using Microsoft.AspNetCore.Mvc;
using Dierentuin42.Models;
using Microsoft.EntityFrameworkCore;

namespace Dierentuin42.Api
{
    [Route("api/enclosures")]
    [ApiController]
    public class EnclosuresApiController : ControllerBase
    {
        private readonly Dierentuin42Context _context;

        public EnclosuresApiController(Dierentuin42Context context)
        {
            _context = context;
        }

        
        //: Get api/enclosures
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Enclosure>>> GetEnclosures() 
        {
            var results = await _context.Enclosure
                .Include(e=> e.Zoo)
                .ToListAsync();
            return Ok(results);

        }

        //: Get api/enclosures/id
        [HttpGet("{id}")]

        public async Task<ActionResult<Enclosure>> GetEnclosure(int id) 
        {
            var enclosure = await _context.Enclosure
                .Include(e => e.Zoo)
                .Include(e=> e.Animals)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (enclosure == null) 
            {
                return NotFound();
            }

            return Ok(enclosure);
        }

        //: Post api/enclosures
        [HttpPost]
        public async Task<ActionResult<Enclosure>> CreateEnclosure(Enclosure enclosure) 
        {
           if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            _context.Enclosure.Add(enclosure);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEnclosure), new { id = enclosure.Id }, enclosure);   
        }

        //: Put api/enclosures/id
        [HttpPut("{id}")]

        public async Task<ActionResult<Category>> UpdateCategory(int id, Enclosure enclosure) 
        {
             if(id != enclosure.Id) 
             {
                return BadRequest();
             }
             if (!ModelState.IsValid) 
             {
                return BadRequest(ModelState);
             }
            try 
            {
                _context.Update(enclosure);
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateConcurrencyException) 
            {
                if(! await EnclosureExists(id)) 
                {
                    return NotFound();
                }
                throw;
            }
            return NoContent();

        }

        //: Delete api/enclosures/id
        [HttpDelete("{id}")]
        public async Task<ActionResult<Enclosure>> DeleteEnclosure(int id) 
        {
            var enclosure = await _context.Enclosure.FindAsync(id);

            if(enclosure == null) 
            {
                return NotFound();
            }

            _context.Remove(enclosure);
            await _context.SaveChangesAsync();
            return NoContent();
               
        }

        //: Search api/enclosures/search?
        [HttpGet("search")]

        public async Task<ActionResult<IEnumerable<Enclosure>>> SearchEnclosure([FromQuery] string name, string zoo, Enclosure.Climate climate, Enclosure.HabitatType habitat, Enclosure.SecurityLevel security, double space) 
        {
            var results = await _context.Enclosure
                .Include(e=> e.Animals)
                .Include(e=> e.Zoo)
                .Where(e => e.Name.Equals(name) &&
                       e.Zoo.Name.Equals(zoo) &&
                       e.EnclosureClimate == climate &&
                       e.EnclosureHabitatType == habitat &&
                       e.EnclosureSecurityLevel == security &&
                       e.Size == space 
                )
                .ToListAsync();
            return Ok(results);

        }
        
        private async Task<bool> EnclosureExists(int id) 
        {
            return await _context.Enclosure.AnyAsync(e => e.Id == id);
        }
        
    }
}
