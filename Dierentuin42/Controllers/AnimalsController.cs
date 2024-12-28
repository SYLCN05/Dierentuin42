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

namespace Dierentuin42.Controllers
{
    public class AnimalsController : Controller
    {
        private readonly Dierentuin42Context _context;

        public AnimalsController(Dierentuin42Context context)
        {
            _context = context;
        }

        // GET: Animals
        public async Task<IActionResult> Index()
        {
            var dierentuin42Context = _context.Animal.Include(a => a.Category).Include(a => a.Enclosure);
            return View(await dierentuin42Context.ToListAsync());
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

        public async Task<IActionResult>Search() 
        {
        
            return View();
        }

        public async Task<IActionResult> SearchResults(String SearchNaam)
        {

            return View("Index", await _context.Animal.Where(s => s.Name.Equals(SearchNaam)).ToListAsync());
        }

        public async Task<IActionResult> Sunrise()
        {
            ViewBag.Action = "Sunrise";
            var animals = await _context.Animal.ToListAsync();
            var awakeAnimals = animals.Where(a => a.IsAwake(true) || a.AnimalActivityPattern == Animal.ActivityPattern.Cathemeral).ToList();
            return View("AnimalStatus", awakeAnimals);
        }

        public async Task<IActionResult> Sunset()
        {
            ViewBag.Action = "Sunset";
            var animals = await _context.Animal.ToListAsync();
            var awakeAnimals = animals.Where(a => a.IsAwake(false) || a.AnimalActivityPattern == Animal.ActivityPattern.Cathemeral).ToList();
            return View("AnimalStatus", awakeAnimals);
        }

        public async Task<IActionResult> FeedingTime()
        {
            ViewBag.Action = "Feeding Time";
            var animals = await _context.Animal.ToListAsync();

            var feedingTimes = animals.Select(a => new
            {
                Animal = a.Name,
                Prey = a.Prey,
                FeedingTime = a.GetFeedingTime()
            }).ToList();

            Console.WriteLine("Feeding Times:");
            foreach (var feedingTime in feedingTimes)
            {
                Console.WriteLine($"Animal: {feedingTime.Animal}, Prey: {feedingTime.Prey}, FeedingTime: {feedingTime.FeedingTime}");
            }

            return View("FeedingTime", feedingTimes);
        }








    }
}
