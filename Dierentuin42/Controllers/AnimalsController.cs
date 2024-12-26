using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Dierentuin42.Data;
using Dierentuin42.Models;
using Microsoft.CodeAnalysis.Elfie.Model.Strings;
using System.Linq.Expressions;

namespace Dierentuin42.Controllers
{
    public class AnimalsController : Controller
    {
        private readonly Dierentuin42Context _context;

        public AnimalsController(Dierentuin42Context context)
        {
            _context = context;
        }

        // GET: Animals (FILTERS/SORTEREN)
        public async Task<IActionResult> Index(string filterName, string filterSpecies, string filterCategory,
            string filterSize, string filterDiet, string filterActivityPattern, string filterPrey,
            string filterEnclosure, string filterSecurity, string sortColumn, string sortOrder)
        {
            var animals = _context.Animal.Include(a => a.Category).Include(a => a.Enclosure).AsQueryable();

            // FILTEREN
            if (!string.IsNullOrEmpty(filterName))
            {
                animals = animals.Where(a => a.Name.Contains(filterName));
            }

            if (!string.IsNullOrEmpty(filterSpecies))
            {
                animals = animals.Where(a => a.Species.Contains(filterSpecies));
            }

            if (!string.IsNullOrEmpty(filterCategory))
            {
                animals = animals.Where(a => a.Category.Name.Contains(filterCategory));
            }

            // FILTERS VOOR ELK VELD
            if (!string.IsNullOrEmpty(filterSize) && Enum.TryParse(filterSize, out Animal.Size size))
            {
                animals = animals.Where(a => a.AnimalSize == size);
            }

            if (!string.IsNullOrEmpty(filterDiet) && Enum.TryParse(filterDiet, out Animal.DietaryClass diet))
            {
                animals = animals.Where(a => a.AnimalDiet == diet);
            }

            if (!string.IsNullOrEmpty(filterActivityPattern) && Enum.TryParse(filterActivityPattern, out Animal.ActivityPattern activity))
            {
                animals = animals.Where(a => a.AnimalActivityPattern == activity);
            }

            if (!string.IsNullOrEmpty(filterPrey))
            {
                animals = animals.Where(a => a.Prey.Contains(filterPrey));
            }

            if (!string.IsNullOrEmpty(filterEnclosure))
            {
                animals = animals.Where(a => a.Enclosure.Name.Contains(filterEnclosure));
            }

            if (!string.IsNullOrEmpty(filterSecurity) && Enum.TryParse(filterSecurity, out Animal.SecurityLevel security))
            {
                animals = animals.Where(a => a.SecurityRequirement == security);
            }

            // SORTEREN
            if (!string.IsNullOrEmpty(sortColumn))
            {
                var param = Expression.Parameter(typeof(Animal), "x");
                var property = Expression.Property(param, sortColumn);
                var lambda = Expression.Lambda<Func<Animal, object>>(Expression.Convert(property, typeof(object)), param);
                animals = sortOrder == "asc" ? animals.OrderBy(lambda) : animals.OrderByDescending(lambda);
            }

            // UNIEKE WAARDES VOOR FILTER
            ViewData["Names"] = await _context.Animal.Select(a => a.Name).Distinct().ToListAsync();
            ViewData["Species"] = await _context.Animal.Select(a => a.Species).Distinct().ToListAsync();
            ViewData["Prey"] = await _context.Animal.Select(a => a.Prey).Distinct().ToListAsync();
            ViewData["Categories"] = await _context.Category.Select(c => c.Name).Distinct().ToListAsync();
            ViewData["Sizes"] = Enum.GetValues(typeof(Animal.Size)).Cast<Animal.Size>().ToList();
            ViewData["Diets"] = Enum.GetValues(typeof(Animal.DietaryClass)).Cast<Animal.DietaryClass>().ToList();
            ViewData["ActivityPatterns"] = Enum.GetValues(typeof(Animal.ActivityPattern)).Cast<Animal.ActivityPattern>().ToList();
            ViewData["Enclosures"] = await _context.Enclosure.Select(e => e.Name).Distinct().ToListAsync();
            ViewData["SecurityLevels"] = Enum.GetValues(typeof(Animal.SecurityLevel)).Cast<Animal.SecurityLevel>().ToList();

            // GEEF TERUG
            return View(await animals.ToListAsync());
        }



        // GET: Animals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animal = await _context.Animal
                .Include(a => a.Category)
                .Include(a => a.Enclosure)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (animal == null)
            {
                return NotFound();
            }

            return View(animal);
        }

        // GET: Animals/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name");
            ViewData["EnclosureId"] = new SelectList(_context.Enclosure, "Id", "Name");
            return View();
        }

        // POST: Animals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Species,CategoryId,AnimalSize,AnimalDiet,AnimalActivityPattern,Prey,EnclosureId,SecurityRequirement")] Animal animal)
        {
            if (ModelState.IsValid)
            {
                _context.Add(animal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Id", animal.CategoryId);
            ViewData["EnclosureId"] = new SelectList(_context.Set<Enclosure>(), "Id", "Id", animal.EnclosureId);
            return View(animal);
        }

        // GET: Animals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animal = await _context.Animal.FindAsync(id);
            if (animal == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", animal.CategoryId);
            ViewData["EnclosureId"] = new SelectList(_context.Set<Enclosure>(), "Id", "Name", animal.EnclosureId);
            return View(animal);
        }

        // POST: Animals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Species,CategoryId,AnimalSize,AnimalDiet,AnimalActivityPattern,Prey,EnclosureId,SecurityRequirement")] Animal animal)
        {
            if (id != animal.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(animal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnimalExists(animal.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", animal.CategoryId);
            ViewData["EnclosureId"] = new SelectList(_context.Set<Enclosure>(), "Id", "Name", animal.EnclosureId);
            return View(animal);
        }

        // GET: Animals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animal = await _context.Animal
                .Include(a => a.Category)
                .Include(a => a.Enclosure)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (animal == null)
            {
                return NotFound();
            }

            return View(animal);
        }

        // POST: Animals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var animal = await _context.Animal.FindAsync(id);
            if (animal != null)
            {
                _context.Animal.Remove(animal);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnimalExists(int id)
        {
            return _context.Animal.Any(e => e.Id == id);
        }

        // Get: Animals/SearchView
        public async Task<IActionResult>Search() 
        {
        
            return View();
        }

        // POST: Animals/SearchResult
        public async Task<IActionResult> SearchResults(String SearchNaam)
        {

            return View("Index", await _context.Animal.Where(s => s.Name.Equals(SearchNaam)).ToListAsync());
        }


    }
}
