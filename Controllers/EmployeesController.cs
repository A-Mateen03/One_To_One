using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Database_RelationShips.Data;
using Database_RelationShips.Models;
using Database_RelationShips.ViewModel;

namespace Database_RelationShips.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeesController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: Employees
        public async Task<IActionResult> Index()
        {
            return _context.Employee != null ?
                        View(await _context.Employee.Include(e => e.EmployeeAddress).ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Employee'  is null.");
        }

        public  ActionResult CreateEmployee()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateEmployee(EmployeeViewModel emp)
        {
            if (ModelState.IsValid)
            {
                Employee employee = new Employee();
                //employee.EmployeeID = emp.EmployeeID;
                employee.Name = emp.Name;
                employee.PhoneNumber = emp.PhoneNumber;
                _context.Employee.Add(employee);
                _context.SaveChanges();

                EmployeeAddress employeeAddress = new EmployeeAddress();
                employeeAddress.EmployeeID = employee.EmployeeID;
                employeeAddress.Address = emp.Address;
                _context.EmployeeAddress.Add(employeeAddress);
                _context.SaveChanges(); 
            return RedirectToAction(nameof(Index));
            }

            return View();
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> DetailsAll(int? id, EmployeeViewModel employee)
        {
            if (id == null || _context.Employee == null)
            {
                return NotFound();
            }
            
            employee.Name = await _context.Employee.Where(m => m.EmployeeID == id).Select(m => m.Name).FirstOrDefaultAsync();
            employee.PhoneNumber = await _context.Employee.Where(m => m.EmployeeID == id).Select(m => m.PhoneNumber).FirstOrDefaultAsync();
            employee.Address = await _context.EmployeeAddress.Where(m => m.EmployeeID == id).Select(m => m.Address).FirstOrDefaultAsync();




            //var employee = await _context.Employee.FirstOrDefaultAsync(m => m.EmployeeID == id);
            //var address = await _context.EmployeeAddress.FirstOrDefaultAsync(m => m.EmployeeID == id);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> EditAll(int? id, EmployeeViewModel employee)
        {

            employee.EmployeeID = await _context.Employee.Where(m => m.EmployeeID == id).Select(m => m.EmployeeID).FirstOrDefaultAsync();
            employee.Name = await _context.Employee.Where(m => m.EmployeeID == id).Select(m => m.Name).FirstOrDefaultAsync();
            employee.PhoneNumber = await _context.Employee.Where(m => m.EmployeeID == id).Select(m => m.PhoneNumber).FirstOrDefaultAsync();
            employee.Address = await _context.EmployeeAddress.Where(m => m.EmployeeID == id).Select(m => m.Address).FirstOrDefaultAsync();



            if (id == null || _context.Employee == null)
            {
                return NotFound();
            }

            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        [HttpPost]
        // [ValidateAntiForgeryToken]
        public IActionResult EditAllPost( EmployeeViewModel employee)
        {
            //if (ModelState.IsValid)
            //{

            //    return RedirectToAction(nameof(Index));
            //}
            //return View(employee);

            if (ModelState.IsValid)
            {
                try
                {
                    // Assuming you have the updated employee information in the 'employee' object
                    var employeeID = employee.EmployeeID;

                    // Retrieve the existing Employee record
                    var existingEmployee = _context.Employee.FirstOrDefault(e => e.EmployeeID == employeeID);

                    if (existingEmployee != null)
                    {
                        // Update the Employee properties with new values
                        existingEmployee.Name = employee.Name;
                        existingEmployee.PhoneNumber = employee.PhoneNumber;

                        // Save changes to the Employee table
                        _context.SaveChanges();
                    }
                    else
                    {
                        // Handle the case where the Employee with the given ID does not exist
                        return NotFound();
                    }

                    // Retrieve the existing EmployeeAddress record
                    var existingEmployeeAddress = _context.EmployeeAddress.FirstOrDefault(ea => ea.EmployeeID == employeeID);

                    if (existingEmployeeAddress != null)
                    {
                        // Update the EmployeeAddress properties with new values
                        existingEmployeeAddress.Address = employee.Address;

                        // Save changes to the EmployeeAddress table
                        _context.SaveChanges();
                    }
                    else
                    {
                        // If EmployeeAddress doesn't exist, you can create a new one and associate it with the Employee
                        var newEmployeeAddress = new EmployeeAddress
                        {
                            EmployeeID = employeeID,
                            Address = employee.Address
                        };

                        // Add the new EmployeeAddress record
                        _context.EmployeeAddress.Add(newEmployeeAddress);

                        // Save changes to the EmployeeAddress table
                        _context.SaveChanges();
                    }

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Handle any exceptions that occur during the update process
                    // You can log the exception and return an appropriate response
                    return RedirectToAction(nameof(Index)); // Or return a different view with an error message
                }
            }

            // If ModelState is not valid, return to the edit view with validation errors
            return View(employee);

        }



        public async Task<IActionResult> DeleteAll(int? id, EmployeeViewModel employee)
        {
            if (id == null || _context.Employee == null)
            {
                return NotFound();
            }

            employee.EmployeeID = await _context.Employee.Where(m => m.EmployeeID == id).Select(m => m.EmployeeID).FirstOrDefaultAsync();
            employee.Name = await _context.Employee.Where(m => m.EmployeeID == id).Select(m => m.Name).FirstOrDefaultAsync();
            employee.PhoneNumber = await _context.Employee.Where(m => m.EmployeeID == id).Select(m => m.PhoneNumber).FirstOrDefaultAsync();
            employee.Address = await _context.EmployeeAddress.Where(m => m.EmployeeID == id).Select(m => m.Address).FirstOrDefaultAsync();


            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }
        [HttpPost, ActionName("DeleteAll")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAllConfirmed(int id)
        {
            if (_context.Employee == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Employee'  is null.");
            }
            var employee = await _context.Employee.FindAsync(id);
            if (employee != null)
            {
                _context.Employee.Remove(employee);
            }

            await _context.SaveChangesAsync();
            
            var employeeAddress = await _context.EmployeeAddress.FindAsync(id);
            if (employeeAddress != null)
            {
                _context.EmployeeAddress.Remove(employeeAddress);
            }
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }





























        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Employee == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee
                .FirstOrDefaultAsync(m => m.EmployeeID == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeID,Name,PhoneNumber")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Employee == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeID,Name,PhoneNumber")] Employee employee)
        {
            if (id != employee.EmployeeID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.EmployeeID))
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
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Employee == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee
                .FirstOrDefaultAsync(m => m.EmployeeID == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Employee == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Employee'  is null.");
            }
            var employee = await _context.Employee.FindAsync(id);
            if (employee != null)
            {
                _context.Employee.Remove(employee);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
          return (_context.Employee?.Any(e => e.EmployeeID == id)).GetValueOrDefault();
        }
    }
}
