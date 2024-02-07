using CarbonFootprint1.Data;
using CarbonFootprint1.Methods;
using CarbonFootprint1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CarbonFootprint1.Controllers
{
    public class ReportController : Controller
    {

        private readonly ApplicationDbContext? _db;
        private readonly UserManager<IdentityUser>? _userManager;
        private readonly SignInManager<IdentityUser>? _signInManager;
        private readonly RoleManager<IdentityRole>? _roleManager;
        private readonly ISessionManagerService _sessionManagerService;

        public ReportController(

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

        [Authorize(Roles = "Approver, Authorizer")]
        public async Task<IActionResult> PerBranch()
        {
            //Check is session is active
            var sessionActiveState = _sessionManagerService.CheckBrowserSession();
            if (sessionActiveState == false)
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Index", "Home");
            }
            IEnumerable<BranchDetails> perBranch = _db.FootprintTable.Where(p => p.Status == FormStatus.Approved);
            return View(perBranch);
        }

        [Authorize(Roles = "Approver, Authorizer")]
        public async Task<IActionResult> Electricity()
        {
            //Check is session is active
            var sessionActiveState = _sessionManagerService.CheckBrowserSession();
            if (sessionActiveState == false)
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Index", "Home");
            }

            IEnumerable<BranchDetails> perBranch = _db.FootprintTable.Where(p => p.Status == FormStatus.Approved);
            foreach(var branch in perBranch)
            {
                branch.TotalStaffNumber = branch.PermanentStaffNumber + branch.NonPermanentStaffNumber;

                

                var percentageHydro = (0.31 * branch.ElectricityConsumed);  //calculate hydro and gas percentage
                var percentageGas = (0.69 * branch.ElectricityConsumed);

                var hydroElectricityCO2Emissions = Convert.ToDouble(percentageHydro * 0.018);       //calculate total emissions for electricity
                var gasElectricityCO2Emissions = Convert.ToDouble(percentageGas * 0.50);


                var totalElectricityCO2Emissions = (hydroElectricityCO2Emissions + gasElectricityCO2Emissions) / 1015;
                branch.ElectricityCO2Emission = Convert.ToDouble(totalElectricityCO2Emissions.ToString("F4"));

                var totalElectricityCO2EmissionKGS = hydroElectricityCO2Emissions + gasElectricityCO2Emissions;
                branch.ElectricityCO2KGS = Convert.ToDouble(totalElectricityCO2EmissionKGS.ToString("F2"));

                var totalStaffElectricity = Convert.ToDouble(branch.ElectricityCO2Emission / branch.TotalStaffNumber);
                branch.StaffElectricityEmission = Convert.ToDouble( totalStaffElectricity.ToString("F4")) ;

                var staffElectricityUsage = Convert.ToDouble(branch.ElectricityConsumed/branch.TotalStaffNumber);
                branch.StaffElectricityConsumption = Convert.ToDouble(staffElectricityUsage.ToString("F3"));
            }
            return View(perBranch);
        }

        [Authorize(Roles = "Approver, Authorizer")]
        public async Task<IActionResult> Water()
        {
            //Check is session is active
            var sessionActiveState = _sessionManagerService.CheckBrowserSession();
            if (sessionActiveState == false)
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Index", "Home");
            }

            IEnumerable<BranchDetails> perBranch = _db.FootprintTable.Where(p => p.Status == FormStatus.Approved);

            foreach(var branch in perBranch)
            {
                branch.TotalStaffNumber = branch.PermanentStaffNumber + branch.NonPermanentStaffNumber;
            }

            return View(perBranch);
        }

        [Authorize(Roles = "Approver, Authorizer")]
        public async Task<IActionResult> Diesel()
        {
            //Check is session is active
            var sessionActiveState = _sessionManagerService.CheckBrowserSession();
            if (sessionActiveState == false)
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Index", "Home");
            }

            IEnumerable<BranchDetails> perBranch = _db.FootprintTable.Where(p => p.Status == FormStatus.Approved);
            foreach (var branch in perBranch)
            {
                branch.TotalStaffNumber = branch.PermanentStaffNumber + branch.NonPermanentStaffNumber;

                //Diesel CO2 Calculations
                //var vehicleDiesel = Convert.ToDouble(branch.DieselQuantityConsumed * 2.67);
                var generatorDiesel = Convert.ToDouble(branch.QuantityOfDieselConsumed * 2.67);
                var totalDieselCO2Emissions = /*vehicleDiesel +*/ generatorDiesel / 1015;

                branch.DieselGeneratorCO2Emission = Convert.ToDouble((totalDieselCO2Emissions).ToString("F3"));
            }
            return View(perBranch);
        }
        //public IActionResult Petrol()
        //{
        //    IEnumerable<BranchDetails> perBranch = _db.FootprintTable.Where(p => p.Status == FormStatus.Approved);
        //    foreach(var branch in perBranch)
        //    {
        //        //var vehiclePetrol = Convert.ToDouble(branch.PetrolQuantityConsumed * 2.4);
        //        var petrolCO2Emission = Convert.ToDouble((vehiclePetrol) / 1015);

        //        branch.PetrolCO2Emission = Convert.ToDouble(petrolCO2Emission.ToString("F3"));
        //    }
        //    return View(perBranch);
        //}

        [Authorize(Roles = "Approver, Authorizer")]
        public async Task<IActionResult> Waste()
        {
            //Check is session is active
            var sessionActiveState = _sessionManagerService.CheckBrowserSession();
            if (sessionActiveState == false)
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Index", "Home");
            }

            IEnumerable<BranchDetails> perBranch = _db.FootprintTable.Where(p => p.Status == FormStatus.Approved);
            foreach(var branch in perBranch)
            {
                branch.TotalStaffNumber = branch.PermanentStaffNumber + branch.NonPermanentStaffNumber;
            }
            return View(perBranch);
        }

        [Authorize(Roles = "Approver, Authorizer")]
        public async Task<IActionResult> Paper()
        {
            //Check is session is active
            var sessionActiveState = _sessionManagerService.CheckBrowserSession();
            if (sessionActiveState == false)
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Index", "Home");
            }

            IEnumerable<BranchDetails> perBranch = _db.FootprintTable.Where(p => p.Status == FormStatus.Approved);
            foreach (var branch in perBranch)
            {
                branch.TotalStaffNumber = branch.PermanentStaffNumber + branch.NonPermanentStaffNumber;
            }
            return View(perBranch);
        }

        [Authorize(Roles = "Approver, Authorizer")]
        public async Task<IActionResult> Aviation()
        {
            //Check is session is active
            var sessionActiveState = _sessionManagerService.CheckBrowserSession();
            if (sessionActiveState == false)
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Index", "Home");
            }

            IEnumerable<BranchDetails> perBranch = _db.FootprintTable.Where(p => p.Status == FormStatus.Approved);
            return View(perBranch);
        }

  
    }
}
