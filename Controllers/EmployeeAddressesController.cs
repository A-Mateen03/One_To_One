using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Database_RelationShips.Data;
using Database_RelationShips.Models;

namespace Database_RelationShips.Controllers
{
    public class EmployeeAddressesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeAddressesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: EmployeeAddresses
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.EmployeeAddress.Include(e => e.Employee);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: EmployeeAddresses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.EmployeeAddress == null)
            {
                return NotFound();
            }

            var employeeAddress = await _context.EmployeeAddress
                .Include(e => e.Employee)
                .FirstOrDefaultAsync(m => m.EmployeeID == id);
            if (employeeAddress == null)
            {
                return NotFound();
            }

            return View(employeeAddress);
        }

        // GET: EmployeeAddresses/Create
        public IActionResult Create()
        {
            ViewData["EmployeeID"] = new SelectList(_context.Employee, "EmployeeID", "EmployeeID");
            return View();
        }

        // POST: EmployeeAddresses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeID,Address")] EmployeeAddress employeeAddress)
        {
            if (ModelState.IsValid)
            {
                // Check if there is an existing EmployeeAddress record for the provided EmployeeID
                var existingEmployeeAddress = await _context.EmployeeAddress
                    .FirstOrDefaultAsync(ea => ea.EmployeeID == employeeAddress.EmployeeID);

                if (existingEmployeeAddress == null)
                {
                    // No existing EmployeeAddress record for the provided EmployeeID, proceed with saving
                    _context.Add(employeeAddress);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // An EmployeeAddress record already exists for this EmployeeID, add an error message
                    ModelState.AddModelError("EmployeeAddress.EmployeeID", "EmployeeAddress record already exists for this EmployeeID.");
                    ViewBag.NotificationMessage = "EmployeeAddress record already exists for this EmployeeID.";

                }
            }

            ViewData["EmployeeID"] = new SelectList(_context.Employee, "EmployeeID", "EmployeeID", employeeAddress.EmployeeID);
            return View(employeeAddress);
        }



        // GET: EmployeeAddresses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.EmployeeAddress == null)
            {
                return NotFound();
            }

            var employeeAddress = await _context.EmployeeAddress.FindAsync(id);
            if (employeeAddress == null)
            {
                return NotFound();
            }
            ViewData["EmployeeID"] = new SelectList(_context.Employee, "EmployeeID", "EmployeeID", employeeAddress.EmployeeID);
            return View(employeeAddress);
        }

        // POST: EmployeeAddresses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeID,Address")] EmployeeAddress employeeAddress)
        {
            if (id != employeeAddress.EmployeeID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employeeAddress);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeAddressExists(employeeAddress.EmployeeID))
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
            ViewData["EmployeeID"] = new SelectList(_context.Employee, "EmployeeID", "EmployeeID", employeeAddress.EmployeeID);
            return View(employeeAddress);
        }

        // GET: EmployeeAddresses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.EmployeeAddress == null)
            {
                return NotFound();
            }

            var employeeAddress = await _context.EmployeeAddress
                .Include(e => e.Employee)
                .FirstOrDefaultAsync(m => m.EmployeeID == id);
            if (employeeAddress == null)
            {
                return NotFound();
            }

            return View(employeeAddress);
        }

        // POST: EmployeeAddresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.EmployeeAddress == null)
            {
                return Problem("Entity set 'ApplicationDbContext.EmployeeAddress'  is null.");
            }
            var employeeAddress = await _context.EmployeeAddress.FindAsync(id);
            if (employeeAddress != null)
            {
                _context.EmployeeAddress.Remove(employeeAddress);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeAddressExists(int id)
        {
          return (_context.EmployeeAddress?.Any(e => e.EmployeeID == id)).GetValueOrDefault();
        }
    }
}






