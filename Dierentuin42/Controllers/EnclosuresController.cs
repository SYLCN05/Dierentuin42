﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Dierentuin42.Data;
using Dierentuin42.Models;
using Dierentuin42.Migrations;
using System.Linq.Expressions;

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
        public async Task<IActionResult> Index(
            string searchText,
            string filterZoo,
            Enclosure.Climate? filterClimate,
            Enclosure.HabitatType? filterHabitatType,
            Enclosure.SecurityLevel? filterSecurityLevel,
            string filterName,
            double? filterSize,
            string sortColumn,
            string sortOrder)
        {
            var enclosures = _context.Enclosure.Include(e => e.Zoo).AsQueryable();

            // FILTEREN OP SPECIFIEKE VELDEN
            enclosures = ApplyFilters(enclosures, filterZoo, filterName, filterClimate, filterHabitatType,
                filterSecurityLevel, filterSize);

            // ZOEKEN OP MEERDER VELDEN GELIJKTIJDIG (waarbij enums correct worden behandeld)
            if (!string.IsNullOrEmpty(searchText))
            {
                enclosures = ApplySearchFilter(enclosures, searchText);
            }

            // UNIEKE WAARDES VOOR FILTER
            await SetUpViewData();

            return View(await enclosures.ToListAsync());
        }

        // FILTER LOGICA
        private IQueryable<Enclosure> ApplyFilters(
            IQueryable<Enclosure> enclosures,
            string filterZoo,
            string filterName,
            Enclosure.Climate? filterClimate,
            Enclosure.HabitatType? filterHabitatType,
            Enclosure.SecurityLevel? filterSecurityLevel,
            double? filterSize)
        {
            if (!string.IsNullOrEmpty(filterZoo))
            {
                enclosures = enclosures.Where(e => e.Zoo.Name == filterZoo);
            }

            if (!string.IsNullOrEmpty(filterName))
            {
                enclosures = enclosures.Where(e => e.Name.Equals(filterName));
            }

            if (filterClimate.HasValue)
            {
                enclosures = enclosures.Where(e => e.EnclosureClimate == filterClimate.Value);
            }

            if (filterHabitatType.HasValue)
            {
                enclosures = enclosures.Where(e => e.EnclosureHabitatType == filterHabitatType.Value);
            }

            if (filterSecurityLevel.HasValue)
            {
                enclosures = enclosures.Where(e => e.EnclosureSecurityLevel == filterSecurityLevel.Value);
            }

            if (filterSize.HasValue)
            {
                enclosures = enclosures.Where(e => e.Size == filterSize);
            }

            return enclosures;
        }

        // ZOEKEN OP MEERDER VELDEN
        private IQueryable<Enclosure> ApplySearchFilter(IQueryable<Enclosure> enclosures, string searchText)
        {
            Enclosure.Climate? climate = null;
            Enclosure.SecurityLevel? securityLevel = null;
            Enclosure.HabitatType? habitatType = null;

            // Probeer enums te parsen
            if (Enum.TryParse(searchText, out Enclosure.Climate parsedClimate))
            {
                climate = parsedClimate;
            }

            if (Enum.TryParse(searchText, out Enclosure.SecurityLevel parsedSecurityLevel))
            {
                securityLevel = parsedSecurityLevel;
            }

            if (Enum.TryParse(searchText, out Enclosure.HabitatType parsedHabitatType))
            {
                habitatType = parsedHabitatType;
            }

            return enclosures.Where(e =>
                e.Name.Contains(searchText) ||
                e.Zoo.Name.Contains(searchText) ||
                (climate.HasValue && e.EnclosureClimate == climate.Value) ||
                (securityLevel.HasValue && e.EnclosureSecurityLevel == securityLevel.Value) ||
                (habitatType.HasValue && e.EnclosureHabitatType == habitatType.Value) ||
                e.Size.ToString().Contains(searchText)
            );
        }

        // VIEWDATA INSTELLEN VOOR FILTER
        private async Task SetUpViewData()
        {
            ViewData["Zoos"] = await _context.Zoo.ToListAsync();
            ViewData["Names"] = await _context.Enclosure.Select(e => e.Name).Distinct().ToListAsync();
            ViewData["Sizes"] = await _context.Enclosure.Select(e => e.Size).Distinct().ToListAsync();
            ViewData["Climates"] = Enum.GetValues(typeof(Enclosure.Climate)).Cast<Enclosure.Climate>().ToList();
            ViewData["HabitatTypes"] = Enum.GetValues(typeof(Enclosure.HabitatType)).Cast<Enclosure.HabitatType>().ToList();
            ViewData["SecurityLevels"] = Enum.GetValues(typeof(Enclosure.SecurityLevel)).Cast<Enclosure.SecurityLevel>().ToList();
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
                    Selected = enclosure.Animals.Any(animal => animal.Id == a.Id) // Selecteer dieren die al gekoppeld zijn
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

                    // Werk de bestaande enclosure bij
                    existingEnclosure.Name = enclosure.Name;
                    existingEnclosure.EnclosureClimate = enclosure.EnclosureClimate;
                    existingEnclosure.EnclosureHabitatType = enclosure.EnclosureHabitatType;
                    existingEnclosure.EnclosureSecurityLevel = enclosure.EnclosureSecurityLevel;
                    existingEnclosure.Size = enclosure.Size;

                    // Werk de dieren bij
                    existingEnclosure.Animals.Clear();
                    if (selectedAnimalIds != null && selectedAnimalIds.Any())
                    {
                        foreach (var animalId in selectedAnimalIds)
                        {
                            var animal = await _context.Animal.FindAsync(animalId);
                            if (animal != null)
                            {
                                existingEnclosure.Animals.Add(animal); // Voeg de geselecteerde dieren toe
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
                    Selected = selectedAnimalIds.Contains(a.Id) // Markeer geselecteerde dieren
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
                .Include(e => e.Animals)
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
            var enclosure = await _context.Enclosure
                .Include(e => e.Animals)  
                .FirstOrDefaultAsync(e => e.Id == id);

            if (enclosure != null)
            {
                foreach (var animal in enclosure.Animals)
                {
                    animal.EnclosureId = null;  
                }

                _context.Enclosure.Remove(enclosure);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }


        private bool EnclosureExists(int id)
        {
            return _context.Enclosure.Any(e => e.Id == id);
        }

        public IActionResult Footer() 
        { 
            return PartialView("_Footer"); 
        
        }
    }
}
