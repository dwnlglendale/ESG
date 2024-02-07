using CarbonFootprint1.Data;
using CarbonFootprint1.Methods;
using CarbonFootprint1.Models;
using CarbonFootprint1.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CarbonFootprint1.Controllers
{
    public class RoleController : Controller
    {
        private readonly ApplicationDbContext? _db;
        private readonly UserManager<IdentityUser>? _userManager;
        private readonly SignInManager<IdentityUser>? _signInManager;
        private readonly RoleManager<IdentityRole>? _roleManager;
        private readonly ISessionManagerService _sessionManagerService;

        public RoleController(
             ApplicationDbContext db,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ISessionManagerService sessionManagerService
            )
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _sessionManagerService = sessionManagerService;
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> RoleManagement()
        {
            //Check is session is active
            var sessionActiveState = _sessionManagerService.CheckBrowserSession();
            if (sessionActiveState == false)
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Index", "Home");
            }

            var roles = _roleManager.Roles.ToList();
            return View(roles);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Index()
        {
            //Check is session is active
            var sessionActiveState = _sessionManagerService.CheckBrowserSession();
            if (sessionActiveState == false)
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        //GET METHOD
        [Authorize(Roles = "Administrator")]
        public async  Task<IActionResult> CreateNewRole()
        {
            //Check is session is active
            var sessionActiveState = _sessionManagerService.CheckBrowserSession();
            if (sessionActiveState == false)
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        //POST METHOD
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> CreateNewRole(RoleVM obj)
        {
            if (ModelState.IsValid)
            {
             // Create Role In Db
                var role = CreateRole();

                role.Name = obj.RoleInput.Name;
                role.Description = obj.RoleInput.Description;
                role.Status = "Active";
                role.DateCreated = DateTime.Now;
                role.CreatedBy = User.Identity?.Name;

                IdentityResult result = await _roleManager.CreateAsync(role);

                if (result.Succeeded)
                {
                    ModelState.Clear();
                    TempData["success"] = "Role Created Successfully";
                    return RedirectToAction("RoleManagement");
                }
                else
                {
                    TempData["error"] = "Ooops... Error Creating Role";
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return RedirectToAction("RoleManagement");
            }
            return View(obj);
        }



        private Role CreateRole()
        {


            try
            {
                return Activator.CreateInstance<Role>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }


    }
}
