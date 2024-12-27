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
                .Include(c=>c.Animals)
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
    }
}
