using Dierentuin42.Controllers;
using Dierentuin42.Data;
using Microsoft.AspNetCore.Mvc;
using Dierentuin42.Models;
using Microsoft.EntityFrameworkCore;

namespace Dierentuin42.Api
{
    [Route("api/categories")]
    [ApiController]
    public class CategoriesApiController : ControllerBase
    {
        private readonly CategoriesController _webcontroller;
        private readonly Dierentuin42Context _context;
        public CategoriesApiController(Dierentuin42Context context)
        {
            _context = context;
            _webcontroller = new CategoriesController(context);
        }

        //: Get api/categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            var result = await _context.Category
            .Include(c => c.Animals)
            .ToListAsync();
            return Ok(result);
        }

        //: Get api/categories/id
        [HttpGet("{id}")]

        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _context.Category
                .Include(c => c.Animals)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        //: Post api/categroies
        [HttpPost]
        public async Task<ActionResult<Category>> CreateCategory(Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _context.Category.Add(category);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
        }

        //: Put api/categories/id
        [HttpPut("{id}")]

        public async Task<IActionResult> UpdateCategory(int id, Category category)
        {
            if (id != category.Id)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                _context.Update(category);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CategoryExists(id))
                {
                    return NotFound();
                }
                throw;
            }
            return NoContent();
        }

        //: Delete api/categories/id
        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteCategory(int id) 
        {
            var Category = await _context.Category.FindAsync(id);
            if (Category == null) 
            {
                return NotFound();
            }
            _context.Category.Remove(Category);
            await _context.SaveChangesAsync();  
            return NoContent();
        }

        //: Get api/categories/search?name
        [HttpGet("search")]

        public async Task<ActionResult<IEnumerable<Category>>> SearchCategory([FromQuery]string name) 
        {
            var results = await _context.Category
                .Where(c => c.Name.Equals(name))
                .ToListAsync();
            return Ok(results);

        }

        private async Task<bool> CategoryExists(int id) 
        {
            return await _context.Category.AnyAsync(c => c.Id == id);
        }
    }
}
