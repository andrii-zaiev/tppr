using System;
using System.Linq;
using System.Threading.Tasks;
using Lab1.DataAccess;
using Lab1.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Lab1.Controllers
{
    public class ParameterValuesController : Controller
    {
        private readonly PhoneDatabaseContext _context;

        public ParameterValuesController(PhoneDatabaseContext context)
        {
            _context = context;
        }

        // GET: ParameterValues
        public async Task<IActionResult> Index()
        {
            var phoneDatabaseContext = _context.ParameterValues.Include(p => p.Parameter);
            return View(await phoneDatabaseContext.ToListAsync());
        }

        // GET: ParameterValues/Create
        public IActionResult Create()
        {
            ViewData["ParameterId"] = new SelectList(_context.Parameters, nameof(Parameter.Id), nameof(Parameter.Name));
            return View();
        }

        // POST: ParameterValues/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ParameterId,Value,ValueText")] ParameterValue parameterValue)
        {
            if (ModelState.IsValid)
            {
                parameterValue.Id = Guid.NewGuid();
                _context.Add(parameterValue);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ParameterId"] = new SelectList(_context.Parameters, nameof(Parameter.Id), nameof(Parameter.Name), parameterValue.ParameterId);
            return View(parameterValue);
        }

        // GET: ParameterValues/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parameterValue = await _context.ParameterValues.FindAsync(id);
            if (parameterValue == null)
            {
                return NotFound();
            }
            ViewData["ParameterId"] = new SelectList(_context.Parameters, nameof(Parameter.Id), nameof(Parameter.Name));
            return View(parameterValue);
        }

        // POST: ParameterValues/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,ParameterId,Value,ValueText")] ParameterValue parameterValue)
        {
            if (id != parameterValue.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(parameterValue);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParameterValueExists(parameterValue.Id))
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
            ViewData["ParameterId"] = new SelectList(_context.Parameters, nameof(Parameter.Id), nameof(Parameter.Name), parameterValue.ParameterId);
            return View(parameterValue);
        }

        // GET: ParameterValues/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parameterValue = await _context.ParameterValues
                .Include(p => p.Parameter)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (parameterValue == null)
            {
                return NotFound();
            }

            return View(parameterValue);
        }

        // POST: ParameterValues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var parameterValue = await _context.ParameterValues.FindAsync(id);
            _context.ParameterValues.Remove(parameterValue);
            var phoneValues = await _context.PhoneParameterValues.Where(pv => pv.ParameterValueId == id).ToListAsync();
            _context.PhoneParameterValues.RemoveRange(phoneValues);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ParameterValueExists(Guid id)
        {
            return _context.ParameterValues.Any(e => e.Id == id);
        }
    }
}
