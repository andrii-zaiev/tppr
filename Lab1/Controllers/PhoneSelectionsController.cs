using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lab1.DataAccess;
using Lab1.DataAccess.Models;

namespace Lab1.Controllers
{
    public class PhoneSelectionsController : Controller
    {
        private readonly PhoneDatabaseContext _context;

        public PhoneSelectionsController(PhoneDatabaseContext context)
        {
            _context = context;
        }

        // GET: PhoneSelections
        public async Task<IActionResult> Index()
        {
            var phoneDatabaseContext = _context.PhoneSelections.Include(p => p.Phone).Include(p => p.User);
            return View(await phoneDatabaseContext.ToListAsync());
        }

        // GET: PhoneSelections/Create
        public IActionResult Create()
        {
            ViewData["PhoneId"] = new SelectList(_context.Phones, nameof(Phone.Id), nameof(Phone.Name));
            ViewData["UserId"] = new SelectList(_context.Users, nameof(DataAccess.Models.User.Id), nameof(DataAccess.Models.User.Name));
            return View();
        }

        // POST: PhoneSelections/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,PhoneId")] PhoneSelection phoneSelection)
        {
            if (ModelState.IsValid)
            {
                phoneSelection.Id = Guid.NewGuid();
                _context.Add(phoneSelection);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PhoneId"] = new SelectList(_context.Phones, nameof(Phone.Id), nameof(Phone.Name), phoneSelection.PhoneId);
            ViewData["UserId"] = new SelectList(_context.Users, nameof(DataAccess.Models.User.Id), nameof(DataAccess.Models.User.Name), phoneSelection.UserId);
            return View(phoneSelection);
        }

        // GET: PhoneSelections/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phoneSelection = await _context.PhoneSelections.FindAsync(id);
            if (phoneSelection == null)
            {
                return NotFound();
            }
            ViewData["PhoneId"] = new SelectList(_context.Phones, nameof(Phone.Id), nameof(Phone.Name), phoneSelection.PhoneId);
            ViewData["UserId"] = new SelectList(_context.Users, nameof(DataAccess.Models.User.Id), nameof(DataAccess.Models.User.Name), phoneSelection.UserId);
            return View(phoneSelection);
        }

        // POST: PhoneSelections/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,UserId,PhoneId")] PhoneSelection phoneSelection)
        {
            if (id != phoneSelection.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(phoneSelection);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhoneSelectionExists(phoneSelection.Id))
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
            ViewData["PhoneId"] = new SelectList(_context.Phones, nameof(Phone.Id), nameof(Phone.Name), phoneSelection.PhoneId);
            ViewData["UserId"] = new SelectList(_context.Users, nameof(DataAccess.Models.User.Id), nameof(DataAccess.Models.User.Name), phoneSelection.UserId);
            return View(phoneSelection);
        }

        // GET: PhoneSelections/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phoneSelection = await _context.PhoneSelections
                .Include(p => p.Phone)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (phoneSelection == null)
            {
                return NotFound();
            }

            return View(phoneSelection);
        }

        // POST: PhoneSelections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var phoneSelection = await _context.PhoneSelections.FindAsync(id);
            _context.PhoneSelections.Remove(phoneSelection);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PhoneSelectionExists(Guid id)
        {
            return _context.PhoneSelections.Any(e => e.Id == id);
        }
    }
}
