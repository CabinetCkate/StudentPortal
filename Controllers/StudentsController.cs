using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPortal.Data;
using StudentPortal.Models;
using StudentPortal.ViewModels;

namespace StudentPortal.Controllers;

// [Authorize] бұл контроллердің барлық әрекетін кірген пайдаланушылармен шектейді.
[Authorize]
public class StudentsController : Controller
{
    private readonly AppDbContext _context;

    public StudentsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var students = await _context.Students
            .AsNoTracking()
            .OrderBy(student => student.FullName)
            .ToListAsync();
        return View(students);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var student = await _context.Students.AsNoTracking()
            .FirstOrDefaultAsync(item => item.Id == id.Value);
        return student is null ? NotFound() : View(student);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public IActionResult Create() => View(new StudentFormViewModel());

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(StudentFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        _context.Students.Add(new Student
        {
            FullName = model.FullName,
            Email = model.Email,
            Group = model.Group,
            EnrollmentDate = model.EnrollmentDate
        });
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var student = await _context.Students.AsNoTracking()
            .FirstOrDefaultAsync(item => item.Id == id.Value);
        if (student is null)
        {
            return NotFound();
        }

        return View(new StudentFormViewModel
        {
            FullName = student.FullName,
            Email = student.Email,
            Group = student.Group,
            EnrollmentDate = student.EnrollmentDate
        });
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, StudentFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var student = await _context.Students.FindAsync(id);
        if (student is null)
        {
            return NotFound();
        }

        student.FullName = model.FullName;
        student.Email = model.Email;
        student.Group = model.Group;
        student.EnrollmentDate = model.EnrollmentDate;
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var student = await _context.Students.AsNoTracking()
            .FirstOrDefaultAsync(item => item.Id == id.Value);
        return student is null ? NotFound() : View(student);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, bool confirm = false)
    {
        var student = await _context.Students.FindAsync(id);
        if (student is null)
        {
            return NotFound();
        }

        if (!confirm)
        {
            return View(student);
        }

        _context.Students.Remove(student);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
