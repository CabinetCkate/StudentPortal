using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentPortal.Models;
using StudentPortal.ViewModels;

namespace StudentPortal.Controllers;

[AllowAnonymous]
public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ILogger<AccountController> _logger;

    public AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ILogger<AccountController> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Register() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Тіркелу формасы жарамсыз: {ErrorCount} валидация қатесі.", ModelState.ErrorCount);
            return View(model);
        }

        var user = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName
        };

        // UserManager пайдаланушыны Identity ережелерімен және құпиясөз хэшімен сақтайды.
        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                var message = error.Code is "DuplicateEmail" or "DuplicateUserName"
                    ? "Бұл email бұрыннан тіркелген"
                    : error.Description;
                var key = error.Code is "DuplicateEmail" or "DuplicateUserName"
                    ? nameof(model.Email)
                    : string.Empty;

                ModelState.AddModelError(key, message);
            }

            _logger.LogWarning(
                "Тіркелу сәтсіз аяқталды. Identity қате кодтары: {ErrorCodes}",
                string.Join(", ", result.Errors.Select(error => error.Code)));
            return View(model);
        }

        var roleResult = await _userManager.AddToRoleAsync(user, "Student");
        if (!roleResult.Succeeded)
        {
            var rollback = await _userManager.DeleteAsync(user);
            foreach (var error in roleResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            _logger.LogWarning(
                "Student рөлін тіркелген пайдаланушыға беру сәтсіз аяқталды. Қате кодтары: {ErrorCodes}; тазалау сәтті: {RollbackSucceeded}",
                string.Join(", ", roleResult.Errors.Select(error => error.Code)),
                rollback.Succeeded);
            return View(model);
        }

        await _signInManager.SignInAsync(user, isPersistent: false);
        return RedirectToAction("Index", "Students");
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Кіру формасы жарамсыз: {ErrorCount} валидация қатесі.", ModelState.ErrorCount);
            return View(model);
        }

        // SignInManager cookie-ді қауіпсіз жасайды; қате email мен құпиясөзді ажыратпаймыз.
        var result = await _signInManager.PasswordSignInAsync(
            model.Email,
            model.Password,
            model.RememberMe,
            lockoutOnFailure: true);

        if (result.Succeeded)
        {
            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return LocalRedirect(returnUrl);
            }

            return RedirectToAction("Index", "Students");
        }

        _logger.LogWarning("Кіру сәтсіз аяқталды.");
        ModelState.AddModelError(string.Empty, "Email немесе құпиясөз қате");
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login", "Account");
    }

    [HttpGet]
    public IActionResult AccessDenied() => View();
}
