using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lab1.DataAccess;
using Lab1.DataAccess.Models;
using Lab1.Models;
using Lab1.Services;

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
            ViewData["UserId"] = new SelectList(_context.Users, nameof(DataAccess.Models.User.Id), nameof(DataAccess.Models.User.Name));
            return View();
        }

        // POST: PhoneSelections/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId")] Guid userId)
        {
            if (ModelState.IsValid)
            {
                var result = FindBestPhone();

                var phoneSelection = new PhoneSelection
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    PhoneId = result.bestPhoneId
                };
                _context.PhoneSelections.Add(phoneSelection);
                await _context.SaveChangesAsync();
                return View("Result", new SelectionResultModel
                {
                    SelectedPhone = _context.Phones.Find(result.bestPhoneId),
                    ParetoOptimalPhones =
                        _context.Phones.Where(p => result.paretoOptimalPhoneIds.Contains(p.Id)).ToList()
                });
            }
            ViewData["UserId"] = new SelectList(_context.Users.OrderBy(u => u.Name), nameof(DataAccess.Models.User.Id), nameof(DataAccess.Models.User.Name), userId);
            return View(new PhoneSelection { UserId = userId });
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

        private (Guid bestPhoneId, List<Guid> paretoOptimalPhoneIds) FindBestPhone()
        {
            var phones = _context.Phones
                .Include(p => p.PhoneParameterValues)
                .ThenInclude(pv => pv.ParameterValue)
                .ThenInclude(pvv => pvv.Parameter)
                .ToList()
                .Select(p => new Alternative
                {
                    Id = p.Id,
                    Criteria = p.PhoneParameterValues.ToDictionary(pv => pv.ParameterValue.ParameterId, pv =>
                        new Criterion
                        {
                            Id = pv.ParameterValue.ParameterId,
                            Optimality = pv.ParameterValue.Parameter.Optimality,
                            Value = pv.ParameterValue.Value
                        })
                })
                .ToList();

            var decisionService = new DecisionService();

            var normalized = decisionService.Normalize(phones);

            var paretoOptimal = decisionService.GetParetoOptimal(normalized);

            var optimal = decisionService.GetOptimalByLinearAdditiveConvolution(paretoOptimal);

            return (optimal.Id, paretoOptimal.Select(a => a.Id).ToList());
        }
    }
}
