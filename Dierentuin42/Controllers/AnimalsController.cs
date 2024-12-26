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
        public async Task<IActionResult> Index(
    string searchText,
    string filterName,
    string filterSpecies,
    string filterCategory,
    string filterSize,
    string filterDiet,
    string filterActivityPattern,
    string filterPrey,
    string filterEnclosure,
    string filterSecurity,
    string sortColumn,
    string sortOrder)
        {
            var animals = _context.Animal
                .Include(a => a.Category)
                .Include(a => a.Enclosure)
                .AsQueryable();

            // FILTEREN OP SPECIFIEKE VELDEN
            if (!string.IsNullOrEmpty(filterName))
            {
                animals = animals.Where(a => a.Name.Equals(filterName));
            }

            if (!string.IsNullOrEmpty(filterSpecies))
            {
                animals = animals.Where(a => a.Species.Equals(filterSpecies));
            }

            if (!string.IsNullOrEmpty(filterCategory))
            {
                animals = animals.Where(a => a.Category.Name.Equals(filterCategory));
            }

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

            if (!string.IsNullOrEmpty(filterSecurity) && Enum.TryParse(filterSecurity, out Animal.SecurityLevel security))
            {
                animals = animals.Where(a => a.SecurityRequirement == security);
            }

            if (!string.IsNullOrEmpty(filterPrey))
            {
                animals = animals.Where(a => a.Prey.Equals(filterPrey));
            }

            if (!string.IsNullOrEmpty(filterEnclosure))
            {
                animals = animals.Where(a => a.Enclosure.Name.Equals(filterEnclosure));
            }

            // ZOEKEN OP MEERDER VELDEN GELIJKTIJDIG
            if (!string.IsNullOrEmpty(searchText))
            {
                Animal.ActivityPattern? activityPattern = null;
                Animal.SecurityLevel? securityLevel = null;
                Animal.DietaryClass? dietaryClass = null;
                Animal.Size? animalSize = null;  

                if (Enum.TryParse(searchText, out Animal.ActivityPattern parsedActivityPattern))
                {
                    activityPattern = parsedActivityPattern;
                }

                if (Enum.TryParse(searchText, out Animal.SecurityLevel parsedSecurityLevel))
                {
                    securityLevel = parsedSecurityLevel;
                }

                if (Enum.TryParse(searchText, out Animal.DietaryClass parsedDietaryClass))
                {
                    dietaryClass = parsedDietaryClass;
                }

                if (Enum.TryParse(searchText, out Animal.Size parsedSize))
                {
                    animalSize = parsedSize;  
                }

                animals = animals.Where(a =>
                    a.Name.Contains(searchText) ||
                    a.Species.Contains(searchText) ||
                    a.Prey.Contains(searchText) ||
                    a.Category.Name.Contains(searchText) ||
                    a.Enclosure.Name.Contains(searchText) ||

                    (activityPattern.HasValue && a.AnimalActivityPattern == activityPattern.Value) ||
                    (securityLevel.HasValue && a.SecurityRequirement == securityLevel.Value) ||
                    (dietaryClass.HasValue && a.AnimalDiet == dietaryClass.Value) ||
                    (animalSize.HasValue && a.AnimalSize == animalSize.Value) 
                );
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

    }
}
