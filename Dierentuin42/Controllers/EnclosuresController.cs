using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Dierentuin42.Data;
using Dierentuin42.Models;
using Dierentuin42.Migrations;

namespace Dierentuin42.Controllers
{
    public class EnclosuresController : Controller
    {
        private readonly Dierentuin42Context _context;

        public EnclosuresController(Dierentuin42Context context)
        {
            _context = context;
        }

        // GET: Enclosures
        public async Task<IActionResult> Index()
        {
            var enclosures = await _context.Enclosure
                                  .Include(e => e.Zoo) 
                                  .ToListAsync();
            return View(enclosures);
        }

        // GET: Enclosures/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enclosure = await _context.Enclosure
                .Include(e => e.Animals) 
                .FirstOrDefaultAsync(m => m.Id == id);

            if (enclosure == null)
            {
                return NotFound();
            }

            return View(enclosure);
        }

        // GET: Enclosures/Create
        public IActionResult Create()
        {
            ViewBag.AvailableAnimals = _context.Animal
            .Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.Name
            })
            .ToList();
            return View(new Enclosure());
        }

        // POST: Enclosures/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Enclosure enclosure, List<int> selectedAnimalIds)
        {
            if (ModelState.IsValid)
            {
                if (selectedAnimalIds != null && selectedAnimalIds.Any())
                {
                    foreach (var animalId in selectedAnimalIds)
                    {
                        var animal = await _context.Animal.FindAsync(animalId);
                        if (animal != null)
                        {
                            enclosure.Animals.Add(animal); // Relatie toevoegen
                        }
                    }
                }

                _context.Add(enclosure);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Vul ViewBag opnieuw in bij fouten
            ViewBag.AvailableAnimals = _context.Animal
                .Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.Name
                })
                .ToList();

            return View(enclosure);
        }

        // GET: Enclosures/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Haal de enclosure inclusief de gekoppelde dieren op
            var enclosure = await _context.Enclosure
                .Include(e => e.Animals)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (enclosure == null)
            {
                return NotFound();
            }

            // Haal alle dieren op
            var allAnimals = await _context.Animal.ToListAsync();

            // Stel de ViewBag samen voor de beschikbare dieren
            ViewBag.AvailableAnimals = allAnimals
                .Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.Name,
                    Selected = enclosure.Animals.Any(animal => animal.Id == a.Id)
                })
                .ToList();

            return View(enclosure);
        }

        // POST: Enclosures/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Enclosure enclosure, List<int> selectedAnimalIds)
        {
            if (id != enclosure.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Haal de bestaande enclosure inclusief dieren op
                    var existingEnclosure = await _context.Enclosure
                        .Include(e => e.Animals)
                        .FirstOrDefaultAsync(e => e.Id == id);

                    if (existingEnclosure == null)
                    {
                        return NotFound();
                    }

                    // Update de enclosure-eigenschappen
                    existingEnclosure.Name = enclosure.Name;

                    // Werk de dieren bij
                    existingEnclosure.Animals.Clear();
                    if (selectedAnimalIds != null && selectedAnimalIds.Any())
                    {
                        foreach (var animalId in selectedAnimalIds)
                        {
                            var animal = await _context.Animal.FindAsync(animalId);
                            if (animal != null)
                            {
                                existingEnclosure.Animals.Add(animal);
                            }
                        }
                    }

                    _context.Update(existingEnclosure);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnclosureExists(enclosure.Id))
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

            // Vul ViewBag opnieuw bij fouten
            ViewBag.AvailableAnimals = _context.Animal
                .Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.Name,
                    Selected = selectedAnimalIds.Contains(a.Id)
                })
                .ToList();

            return View(enclosure);
        }

            // GET: Enclosures/Delete/5
            public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enclosure = await _context.Enclosure
                .FirstOrDefaultAsync(m => m.Id == id);
            if (enclosure == null)
            {
                return NotFound();
            }

            return View(enclosure);
        }

        // POST: Enclosures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var enclosure = await _context.Enclosure.FindAsync(id);
            if (enclosure != null)
            {
                _context.Enclosure.Remove(enclosure);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EnclosureExists(int id)
        {
            return _context.Enclosure.Any(e => e.Id == id);
        }
    }
}
