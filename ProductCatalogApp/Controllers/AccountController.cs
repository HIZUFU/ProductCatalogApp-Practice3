using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductCatalogApp.Data;
using ProductCatalogApp.Models;

namespace ProductCatalogApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher<AppUser> _passwordHasher;

        public AccountController(
            ApplicationDbContext context,
            IPasswordHasher<AppUser> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        // GET: Account/Register
        [AllowAnonymous]
        public IActionResult Register()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Register(
            RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string username = model.Username.Trim();

            bool usernameExists = await _context.Users
                .AnyAsync(user => user.Username == username);

            if (usernameExists)
            {
                ModelState.AddModelError(
                    nameof(model.Username),
                    "Пользователь с таким именем уже существует.");

                return View(model);
            }

            var userRole = await _context.Roles
                .FirstOrDefaultAsync(role => role.Name == "User");

            if (userRole == null)
            {
                return Problem(
                    "Роль User не найдена в базе данных.");
            }

            var user = new AppUser
            {
                Username = username,
                RoleId = userRole.Id
            };

            user.PasswordHash = _passwordHasher.HashPassword(
                user,
                model.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            await SignInUserAsync(
                user,
                userRole.Name,
                rememberMe: false);

            return RedirectToAction("Index", "Home");
        }

        // GET: Account/Login
        [AllowAnonymous]
        public IActionResult Login()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(
            LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string username = model.Username.Trim();

            var user = await _context.Users
                .Include(user => user.Role)
                .FirstOrDefaultAsync(
                    user => user.Username == username);

            if (user == null)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Неверное имя пользователя или пароль.");

                return View(model);
            }

            var verificationResult =
                _passwordHasher.VerifyHashedPassword(
                    user,
                    user.PasswordHash,
                    model.Password);

            if (verificationResult ==
                PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Неверное имя пользователя или пароль.");

                return View(model);
            }

            if (user.Role == null)
            {
                return Problem(
                    "Пользователю не назначена роль.");
            }

            if (verificationResult ==
                PasswordVerificationResult.SuccessRehashNeeded)
            {
                user.PasswordHash = _passwordHasher.HashPassword(
                    user,
                    model.Password);

                await _context.SaveChangesAsync();
            }

            await SignInUserAsync(
                user,
                user.Role.Name,
                model.RememberMe);

            return RedirectToAction("Index", "Home");
        }

        // POST: Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction(nameof(Login));
        }

        // GET: Account/AccessDenied
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        private async Task SignInUserAsync(
            AppUser user,
            string roleName,
            bool rememberMe)
        {
            var claims = new List<Claim>
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    user.Id.ToString()),

                new Claim(
                    ClaimTypes.Name,
                    user.Username),

                new Claim(
                    ClaimTypes.Role,
                    roleName)
            };

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            var authenticationProperties =
                new AuthenticationProperties
                {
                    IsPersistent = rememberMe
                };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                authenticationProperties);
        }
    }
}