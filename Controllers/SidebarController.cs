using CarbonFootprint1.Data;
using CarbonFootprint1.Methods;
using CarbonFootprint1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace CarbonFootprint1.Controllers
{
    public class SidebarController : Controller
    {
        private readonly ApplicationDbContext? _db;
        private readonly UserManager<IdentityUser>? _userManager;
        private readonly SignInManager<IdentityUser>? _signInManager;
        private readonly RoleManager<IdentityRole>? _roleManager;
        private readonly ISessionManagerService _sessionManagerService;
        private readonly ErrorLogs _errorLogs;

        public SidebarController(

             ApplicationDbContext db,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ISessionManagerService sessionManagerService,
            ErrorLogs errorLogs
            )
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _sessionManagerService = sessionManagerService;
            _errorLogs = errorLogs;
        }

        [Authorize(Roles = "Admin, PNT, Requester, Aprover, Authorizer")]
        public async Task<IActionResult> Dashboard()
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


        [Authorize(Roles = "Requester")]
        public async Task<IActionResult> FormBranchDetails()
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

        [Authorize(Roles = "Requester")]
        public async Task<IActionResult> FormElectricityDetails(int? id)
        {

            //Check is session is active
            var sessionActiveState = _sessionManagerService.CheckBrowserSession();
            if (sessionActiveState == false)
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Index", "Home");
            }

            if (id == null)
            {
                return NotFound();
            }

            var idFromDB = _db.FootprintTable.Find(id);
            return View(idFromDB);
        }

        [Authorize(Roles = "Requester")]
        public async Task<IActionResult> FormDieselDetails(int? id)
        {
            //Check is session is active
            var sessionActiveState = _sessionManagerService.CheckBrowserSession();
            if (sessionActiveState == false)
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Index", "Home");
            }


            if (id == null)
            {
                return NotFound();
            }

            var idFromDB = _db.FootprintTable.Find(id);
            return View(idFromDB);
        }

        [Authorize(Roles = "Requester")]
        public async Task<IActionResult> FormFuelDetails(int? id)
        {
            //Check is session is active
            var sessionActiveState = _sessionManagerService.CheckBrowserSession();
            if (sessionActiveState == false)
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Index", "Home");
            }

            if (id == null)
            {
                return NotFound();
            }

            var idFromDB = _db.FootprintTable.Find(id);
            return View(idFromDB);
        }

        [Authorize(Roles = "Requester")]
        public async Task<IActionResult> FormWaterDetails(int? id)
        {
            //Check is session is active
            var sessionActiveState = _sessionManagerService.CheckBrowserSession();
            if (sessionActiveState == false)
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Index", "Home");
            }

            if (id == null)
            {
                return NotFound();
            }

            var idFromDB = _db.FootprintTable.Find(id);
            return View(idFromDB);
        }

        [Authorize(Roles = "Requester")]
        public async Task<IActionResult> FormPaperDetails(int? id)
        {
            //Check is session is active
            var sessionActiveState = _sessionManagerService.CheckBrowserSession();
            if (sessionActiveState == false)
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Index", "Home");
            }

            if (id == null)
            {
                return NotFound();
            }

            var idFromDB = _db.FootprintTable.Find(id);
            return View(idFromDB);
        }

        [Authorize(Roles = "Requester")]
        public async Task<IActionResult> FormWasteDetails(int? id)
        {
            //Check is session is active
            var sessionActiveState = _sessionManagerService.CheckBrowserSession();
            if (sessionActiveState == false)
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Index", "Home");
            }

            if (id == null)
            {
                return NotFound();
            }

            var idFromDB = _db.FootprintTable.Find(id);

            return View(idFromDB);
        }

        [Authorize(Roles = "Approver, Authorizer")]
        public async Task<IActionResult> AllFootprint()
        {
            //Check is session is active
            var sessionActiveState = _sessionManagerService.CheckBrowserSession();
            if (sessionActiveState == false)
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Index", "Home");
            }

            IEnumerable<BranchDetails> allFootprint = _db.FootprintTable.Where(p => p.Status == FormStatus.Pending);
            return View(allFootprint);
        }

        [Authorize(Roles = "Approver, Authorizer")]
        public async Task<IActionResult> ApprovedFootprint()
        {
            try {
                _errorLogs.LogUserActivity(User.Identity!.Name!,"Viewed Approved Footprint");
                IEnumerable<BranchDetails> approvedFootprint = _db.FootprintTable.Where(a => a.Status == FormStatus.Approved);
                return View(approvedFootprint);
            }
           
            catch(Exception ex)
            {
                _errorLogs.LogExceptions(ex, "Approved Footprint view not found");
                return View();
            }
        }

        [Authorize(Roles = "Requester")]
        public async Task<IActionResult> ReturnedFootprint()
        {

            //Check is session is active
            var sessionActiveState = _sessionManagerService.CheckBrowserSession();
            if (sessionActiveState == false)
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Index", "Home");
            }

            try
            {
                string? branch = _db.ApplicationUserTable.Where(u => u.UserName == User.Identity.Name).Select(u => u.BranchName).FirstOrDefault();
                _errorLogs.LogUserActivity(User.Identity!.Name, "User Viewed Returned Footprint");
                IEnumerable<BranchDetails> returnedFootprint = _db.FootprintTable.Where(r => r.Status == FormStatus.Returned && r.BranchName == branch);
                return View(returnedFootprint);
            }
            catch(Exception ex)
            {
                _errorLogs.LogExceptions(ex, "Returned Footprint not Found");
                TempData["error"]= "Page not available";
                return RedirectToAction("Index", "Home");
            }
        }

         [Authorize(Roles = "Requester")]
        public async Task<IActionResult> AviationFootprint(BranchDetails details)
        {
            //Check is session is active
            var sessionActiveState = _sessionManagerService.CheckBrowserSession();
            if (sessionActiveState == false)
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Index", "Home");
            }

            IEnumerable<AirTravel> returnedFootprint = _db.AirTravelTable.Where(r => r.Status == FormStatus.Pending);

            

            return View(returnedFootprint);
        }

        [Authorize(Roles = "Approver, Authorizer")]
        public async Task<IActionResult> Footprint(int? id)
        {
            //Check is session is active
            var sessionActiveState = _sessionManagerService.CheckBrowserSession();
            if (sessionActiveState == false)
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Index", "Home");
            }

            if (id == null || id == 0)
            {
                return NotFound();
            }

            var footprintId = _db.FootprintTable.Where(f => f.CarbornFootprint == id).FirstOrDefault();

            if(footprintId == null)
            {
                return NotFound();
            }

            return View(footprintId);

        }

        [Authorize(Roles = "Approver, Authorizer")]
        public async Task<IActionResult> ApprovedView(int? id)
        {
            //Check is session is active
            var sessionActiveState = _sessionManagerService.CheckBrowserSession();
            if (sessionActiveState == false)
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Index", "Home");
            }

            if (id == null || id == 0)
            {
                return NotFound();
            }

            var footprintId = _db.FootprintTable.Where(f => f.CarbornFootprint == id).FirstOrDefault();

            if (footprintId == null)
            {
                return NotFound();
            }

            return View(footprintId);

        }

        [Authorize(Roles = "Requester")]
        public async Task<IActionResult> EditReturnedFootprint(int? id)
        {
            //Check if session is active
            var sessionActiveState = _sessionManagerService.CheckBrowserSession();
            if (sessionActiveState == false)
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Index", "Home");
            }

            if (id == null || id == 0)
            {
                return NotFound();
            }

            var footprintId = _db.FootprintTable.Where(f => f.CarbornFootprint == id).FirstOrDefault();

            if (footprintId == null)
            {
                return NotFound();
            }

            return View(footprintId);
        }

        public async Task<IActionResult> PendingSub(int? id)
        {
            //Check if session is active
            var sessionActiveState = _sessionManagerService.CheckBrowserSession();
            if (sessionActiveState == false)
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Index", "Home");
            }

            try
            {
                string? branch = _db.ApplicationUserTable.Where(u => u.UserName == User.Identity.Name).Select(u => u.BranchName).FirstOrDefault();
                _errorLogs.LogUserActivity(User.Identity!.Name, "User Viewed Returned Footprint");
                IEnumerable<BranchDetails> returnedFootprint = _db.FootprintTable.Where(r => r.Status == FormStatus.Draft && r.BranchName == branch);
                return View(returnedFootprint);
            }
            catch (Exception ex)
            {
                _errorLogs.LogExceptions(ex, "Returned Footprint not Found");
                TempData["error"] = "Page not available";
                return RedirectToAction("Index", "Home");
            }
        }


        [Authorize(Roles = "PNT")]
        public IActionResult FormAviationDetails(int? id)
        {
          
            return View(/*idFromDB*/);
        }


    }
}
