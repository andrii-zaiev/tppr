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

namespace Lab1.Controllers
{
    public class PhonesController : Controller
    {
        private readonly PhoneDatabaseContext _context;

        public PhonesController(PhoneDatabaseContext context)
        {
            _context = context;
        }

        // GET: Phones
        public async Task<IActionResult> Index()
        {
            return View(await _context.Phones.ToListAsync());
        }

        // GET: Phones/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phone = await _context.Phones
                .Include(p => p.PhoneParameterValues)
                .ThenInclude(v => v.ParameterValue)
                .ThenInclude(v => v.Parameter)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (phone == null)
            {
                return NotFound();
            }

            return View(phone);
        }

        // GET: Phones/Create
        public IActionResult Create()
        {
            ViewBag.Parameters = _context.Parameters.OrderBy(p => p.Name).ToList();
            ViewBag.ParameterValues = _context.ParameterValues
                .ToList()
                .GroupBy(pv => pv.ParameterId)
                .ToDictionary(g => g.Key,
                    g => new SelectList(g.OrderBy(v => v.Value), nameof(ParameterValue.Id), nameof(ParameterValue.ValueText)));
            return View(new CreatePhoneModel());
        }

        // POST: Phones/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePhoneModel model)
        {
            if (ModelState.IsValid)
            {
                var phone = new Phone
                {
                    Id = Guid.NewGuid(),
                    Name = model.Name
                };
                _context.Add(phone);

                IEnumerable<PhoneParameterValue> phoneParameterValues = model.ParameterValueIds
                    .Select(id => new PhoneParameterValue
                    {
                        Id = Guid.NewGuid(),
                        ParameterValueId = id,
                        PhoneId = phone.Id
                    });
                
                _context.PhoneParameterValues.AddRange(phoneParameterValues);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Parameters = _context.Parameters.OrderBy(p => p.Name).ToList();
            ViewBag.ParameterValues = _context.ParameterValues
                .ToList()
                .GroupBy(pv => pv.ParameterId)
                .ToDictionary(g => g.Key,
                    g => new SelectList(g.OrderBy(v => v.Value), nameof(ParameterValue.Id), nameof(ParameterValue.ValueText)));
            return View(model);
        }

        // GET: Phones/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phone = await _context.Phones
                .Include(p => p.PhoneParameterValues)
                .ThenInclude(v => v.ParameterValue)
                .ThenInclude(v => v.Parameter)
                .SingleAsync(p => p.Id == id);
            if (phone == null)
            {
                return NotFound();
            }

            Dictionary<Guid, Guid> phoneParameterValues =
                phone.PhoneParameterValues.ToDictionary(v => v.ParameterValue.ParameterId, v => v.ParameterValueId);

            ViewBag.Parameters = _context.Parameters.OrderBy(p => p.Name).ToList();
            ViewBag.ParameterValues = _context.ParameterValues
                .ToList()
                .GroupBy(pv => pv.ParameterId)
                .ToDictionary(g => g.Key,
                    g => new SelectList(g.OrderBy(v => v.Value), nameof(ParameterValue.Id), nameof(ParameterValue.ValueText),
                        phoneParameterValues.TryGetValue(g.Key, out var value) ? (object)value : null));
            return View(new EditPhoneModel
            {
                Id = phone.Id,
                Name = phone.Name,
                ParameterValueIds = phone.PhoneParameterValues.Select(v => v.ParameterValueId).ToList()
            });
        }

        // POST: Phones/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, EditPhoneModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            var phone = await _context.Phones.FindAsync(id);

            if (ModelState.IsValid)
            {
                try
                {
                    phone.Name = model.Name;
                    _context.Update(phone);
                    var oldValues = await _context.PhoneParameterValues.Where(pv => pv.PhoneId == id).ToListAsync();
                    _context.PhoneParameterValues.RemoveRange(oldValues);
                    IEnumerable<PhoneParameterValue> newValues = model.ParameterValueIds
                        .Select(valueId => new PhoneParameterValue
                        {
                            Id = Guid.NewGuid(),
                            ParameterValueId = valueId,
                            PhoneId = phone.Id
                        });

                    _context.PhoneParameterValues.AddRange(newValues);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhoneExists(phone.Id))
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

            Dictionary<Guid, Guid> phoneParameterValues =
                phone.PhoneParameterValues.ToDictionary(v => v.ParameterValue.ParameterId, v => v.ParameterValueId);

            ViewBag.Parameters = _context.Parameters.OrderBy(p => p.Name).ToList();
            ViewBag.ParameterValues = _context.ParameterValues
                .ToList()
                .GroupBy(pv => pv.ParameterId)
                .ToDictionary(g => g.Key,
                    g => new SelectList(g.OrderBy(v => v.Value), nameof(ParameterValue.Id), nameof(ParameterValue.ValueText),
                        phoneParameterValues.TryGetValue(g.Key, out var value) ? (object)value : null));
            return View(new EditPhoneModel
            {
                Id = phone.Id,
                Name = phone.Name,
                ParameterValueIds = phone.PhoneParameterValues.Select(v => v.ParameterValueId).ToList()
            });
        }

        // GET: Phones/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phone = await _context.Phones
                .FirstOrDefaultAsync(m => m.Id == id);
            if (phone == null)
            {
                return NotFound();
            }

            return View(phone);
        }

        // POST: Phones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var phone = await _context.Phones.FindAsync(id);
            _context.Phones.Remove(phone);
            var values = _context.PhoneParameterValues.Where(v => v.PhoneId == id).ToList();
            _context.PhoneParameterValues.RemoveRange(values);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PhoneExists(Guid id)
        {
            return _context.Phones.Any(e => e.Id == id);
        }
    }
}
