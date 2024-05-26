using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RingoMedia_test.Data;
using RingoMedia_test.Models;

namespace RingoMedia_test.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly AppDbcontext _context;

        public DepartmentsController(AppDbcontext context)
        {
            _context = context;
        }

        // GET: Departments
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
            {
                var rootDepartments = await _context.Departments
                    .Where(d => d.ParentDepartmentId == null)
                    .ToListAsync();
                return View(rootDepartments);
            }
            else
            {
                var selectedDepartment = await _context.Departments
                    .Include(d => d.SubDepartments)
                    .FirstOrDefaultAsync(d => d.DepartmentId == id);

                if (selectedDepartment == null)
                {
                    return NotFound();
                }

                ViewBag.ParentDepartments = await GetParentDepartments(selectedDepartment);
                return View("Details", selectedDepartment);
            }
        }

        // GET: Departments/Create
        public IActionResult Create()
        {
            ViewBag.Departments = new SelectList(_context.Departments, "DepartmentId", "DepartmentName");
            return View();
        }

        // POST: Departments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DepartmentId,DepartmentName,DepartmentLogo,ParentDepartmentId")] Department department)
        {
            if (ModelState.IsValid)
            {
                _context.Add(department);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Departments = new SelectList(_context.Departments, "DepartmentId", "DepartmentName", department.ParentDepartmentId);
            return View(department);
        }

        private async Task<List<Department>> GetParentDepartments(Department department)
        {
            var parents = new List<Department>();
            var current = department;
            while (current.ParentDepartmentId != null)
            {
                current = await _context.Departments
                    .FirstOrDefaultAsync(d => d.DepartmentId == current.ParentDepartmentId);
                if (current != null)
                {
                    parents.Add(current);
                }
            }
            parents.Reverse();
            return parents;
        }

    }
}
