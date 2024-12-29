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
    public class CategoriesController : Controller
    {
        private readonly Dierentuin42Context _context;

        public CategoriesController(Dierentuin42Context context)
        {
            _context = context;
        }

        // GET: Categories (ZOEKEN EN FILTEREN)
        public async Task<IActionResult> Index(string searchText, string filterName)
        {
            var categoriesQuery = _context.Category.AsQueryable();

            if (!string.IsNullOrEmpty(searchText))
            {
                categoriesQuery = categoriesQuery.Where(c => c.Name.Contains(searchText));
            }

            if (!string.IsNullOrEmpty(filterName))
            {
                categoriesQuery = categoriesQuery.Where(c => c.Name == filterName);
            }

            var categories = await categoriesQuery.ToListAsync();

            ViewData["CategoryNames"] = await _context.Category
                .Select(c => c.Name)
                .Distinct()
                .ToListAsync();

            return View(categories);
        }


        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category
                .Include(c => c.Animals)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            ViewBag.AvailableAnimals = _context.Animal
                .Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.Name
                })
                .ToList();

            return View(new Category());
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category, List<int> selectedAnimalIds)
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
                            category.Animals.Add(animal);
                        }
                    }
                }

                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.AvailableAnimals = _context.Animal
                .Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.Name
                })
                .ToList();

            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category
                .Include(c => c.Animals)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            var allAnimals = await _context.Animal.ToListAsync();

            ViewBag.AvailableAnimals = allAnimals
                .Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.Name,
                    Selected = category.Animals.Any(animal => animal.Id == a.Id) 
                })
                .ToList();

            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category, List<int> selectedAnimalIds)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingCategory = await _context.Category
                        .Include(c => c.Animals)
                        .FirstOrDefaultAsync(c => c.Id == id);

                    if (existingCategory == null)
                    {
                        return NotFound();
                    }

                    existingCategory.Name = category.Name;

                    existingCategory.Animals.Clear();
                    if (selectedAnimalIds != null && selectedAnimalIds.Any())
                    {
                        foreach (var animalId in selectedAnimalIds)
                        {
                            var animal = await _context.Animal.FindAsync(animalId);
                            if (animal != null)
                            {
                                existingCategory.Animals.Add(animal);
                            }
                        }
                    }

                    _context.Update(existingCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
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

            ViewBag.AvailableAnimals = _context.Animal
                .Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.Name,
                    Selected = selectedAnimalIds.Contains(a.Id)
                })
                .ToList();

            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Category.FindAsync(id);
            if (category != null)
            {
                _context.Category.Remove(category);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Category.Any(e => e.Id == id);
        }

    }
}
