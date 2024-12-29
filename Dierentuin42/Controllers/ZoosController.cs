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
using System.Linq.Expressions;

namespace Dierentuin42.Controllers
{
    public class ZoosController : Controller
    {
        private readonly Dierentuin42Context _context;

        public ZoosController(Dierentuin42Context context)
        {
            _context = context;
        }

        // GET: Zoos (ZOEKEN)
        public async Task<IActionResult> Index(string searchText)
        {
            var zoos = _context.Zoo.AsQueryable();

            if (!string.IsNullOrEmpty(searchText))
            {
                zoos = zoos.Where(z => z.Name.Contains(searchText));
            }

            return View(await zoos.ToListAsync());
        }



        // GET: Zoos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zoo = await _context.Zoo
                .Include(z => z.Enclosures)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (zoo == null)
            {
                return NotFound();
            }

            return View(zoo);
        }

        // GET: Zoos/Create
        public IActionResult Create()
        {
            ViewBag.availableEnclosures = _context.Enclosure
                .Select(e => new SelectListItem
                {
                    Value = e.Id.ToString(),
                    Text = e.Name
                })
                .ToList();

            return View(new Zoo());
        }

        // POST: Zoos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Zoo zoo, List<int> selectedEnclosuresIds)
        {
            if (ModelState.IsValid)
            {
                if(selectedEnclosuresIds != null && selectedEnclosuresIds.Any()) 
                {
                    foreach(var enclosureId in selectedEnclosuresIds) 
                    {
                           var enclosure = await _context.Enclosure.FindAsync(enclosureId);
                        if(enclosure != null) 
                        { 
                            zoo.Enclosures.Add(enclosure);
                        }
                    }
                }
                _context.Add(zoo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.availableEnclosures = _context.Enclosure
                .Select(e => new SelectListItem
                {
                    Value = e.Id.ToString(),
                    Text= e.Name
                });
            return View(zoo);
        }

        // GET: Zoos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zoo = await _context.Zoo
                .Include(z => z.Enclosures)
                .FirstOrDefaultAsync(z => z.Id == id);
            if (zoo == null)
            {
                return NotFound();
            }

            var allEnclosures = await _context.Enclosure.ToListAsync();

            ViewBag.AvailableEnclosures = allEnclosures
                .Select(e => new SelectListItem
                {
                    Value = e.Id.ToString(),
                    Text = e.Name,
                    Selected = zoo.Enclosures.Any(enclosure => enclosure.Id == e.Id)
                })
                .ToList();

            return View(zoo);
        }

        // POST: Zoos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Zoo zoo, List<int> selectedEnclosureIds)
        {
            if (id != zoo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingZoo = await _context.Zoo
                        .Include(z => z.Enclosures)
                        .FirstOrDefaultAsync(z => z.Id == id);

                    if (existingZoo == null)
                    {
                        return NotFound();
                    }

                    existingZoo.Name = zoo.Name;

                    existingZoo.Enclosures.Clear();

                    if (selectedEnclosureIds != null && selectedEnclosureIds.Any())
                    {
                        foreach (var enclolsureId in selectedEnclosureIds)
                        {
                            var enclosure = await _context.Enclosure.FindAsync(enclolsureId);
                            if (enclosure != null)
                            {
                                existingZoo.Enclosures.Add(enclosure);
                            }
                        }
                    }
                    _context.Update(existingZoo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ZooExists(zoo.Id))
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
            ViewBag.AvailableEnclosures = _context.Enclosure
                .Select(e => new SelectListItem
                {
                    Value = e.Id.ToString(),
                    Text = e.Name,
                    Selected = selectedEnclosureIds.Contains(e.Id)
                })
                .ToList();

            return View(zoo);
        }

        // GET: Zoos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zoo = await _context.Zoo
                .FirstOrDefaultAsync(m => m.Id == id);
            if (zoo == null)
            {
                return NotFound();
            }

            return View(zoo);
        }

        // POST: Zoos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var zoo = await _context.Zoo.FindAsync(id);
            if (zoo != null)
            {
                _context.Zoo.Remove(zoo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ZooExists(int id)
        {
            return _context.Zoo.Any(e => e.Id == id);
        }
    }
}
