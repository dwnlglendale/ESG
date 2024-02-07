using CarbonFootprint1.Data;
using CarbonFootprint1.Methods;
using CarbonFootprint1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;


namespace CarbonFootprint1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext? _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ISessionManagerService _sessionManagerService;
        private readonly SignInManager<IdentityUser> _signInManager;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ISessionManagerService sessionManagerService, SignInManager<IdentityUser> signInManager)
        {
            _logger = logger;
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _sessionManagerService = sessionManagerService;
            _signInManager = signInManager;
        }

        //[Authorize]
        [Authorize(Roles = "Administrator, Approver, Requester, PNT, Authorizer")]
        public async Task<IActionResult> Index(BranchDetails details)
        {
            //Check is session is active
            var sessionActiveState = _sessionManagerService.CheckBrowserSession();
            if (sessionActiveState == false)
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Index");
            }

            //Get UserName and Display on HeaderPartial
            var perBranch = _db.FootprintTable.Where(p => p.Status == FormStatus.Approved);
            var user = _userManager.FindByNameAsync(User.Identity.Name).Result;
            var userDetails = _db.ApplicationUserTable.Where(u => u.UserName == user.UserName).FirstOrDefault();

            var userRole = userDetails.UserPosition;

            var FullName = userDetails.FullName;
            var RoleName = userRole;
            var BranchName = userDetails.BranchName;
            var Department = userDetails.Department;



            HttpContext.Session.SetString("FullName", FullName);
            HttpContext.Session.SetString("RoleName", RoleName);

            if (userDetails.BranchName == null)
            {
                HttpContext.Session.SetString("Department", Department);
            }
            else
            {
                HttpContext.Session.SetString("BranchName", BranchName);
            }


            //Utility Consumption and Cost
            var totalElectricityConsumption = perBranch.Sum(tec => tec.ElectricityConsumed);
            var totalAmountSpentOnElectricity = perBranch.Sum(tea => tea.ElectricityAmount);

            var hydro = totalElectricityConsumption * 0.31;
            var gas = totalElectricityConsumption * 0.69;
            var totalElec = (hydro + gas) / 1015;




            var totalDieselGeneratorConsumption = perBranch.Sum(tdgc => tdgc.QuantityOfDieselConsumed);
            var totalDieselGeneratorCost = perBranch.Sum(tdgc => tdgc.AmountPaidForDieselGenerator);
            var totalDiesel = perBranch.Sum(d2 => d2.QuantityOfDieselConsumed);

            var totalDieselkgs = totalDiesel * 2.67;
            var totalDieselCo2 = (totalDiesel * 2.67) / 1015;







            var totalDieselCost = totalDieselGeneratorCost ;

            var totalPaperCost = perBranch.Sum(tpc => tpc.CostOfPaperUsed);
            var totalPaperConsumption = perBranch.Sum(tpc => tpc.NumberOfPaperUsed);

            var totalDrinkWaterCost = perBranch.Sum(tpc => tpc.CostOfDrinkableWaterConsumed);
            var totalRegWaterCost = perBranch.Sum(tpc => tpc.CostOfRegularWaterConsumed);
            var totalRegWaterConsumption = perBranch.Sum(tpc => tpc.QuantityOfRegularWaterConsumed);
            var totalDrinkWaterConsumption = perBranch.Sum(tpc => tpc.QuantityOfDrinkableWaterConsumed);

            var totalWaterCost = totalDrinkWaterCost + totalRegWaterCost;


            var totalWasteCost = perBranch.Sum(twc => twc.CostOfDisposal);
            var totalWasteMade = perBranch.Sum(twm => twm.QuantityOfDisposal);

            var totalUtilityCost = totalAmountSpentOnElectricity + totalDieselGeneratorCost + totalPaperCost + totalWaterCost;

            
            

            

            //CO2 Emission
            var totalDieselGeneratorCO2 = perBranch.Sum(tdvc => tdvc.QuantityOfDieselConsumed);
            

            var totalCO2Emission = totalDieselCo2 + totalElec;



            //ViewBags for Dashboard
            ViewBag.TotalCO2Emission = totalDieselkgs;
            ViewBag.TotalCO2Emissionkgs = totalDieselkgs;
            ViewBag.TotalUtilityCost = totalUtilityCost;
            ViewBag.TotalElectricityUsage = totalElectricityConsumption;
            ViewBag.TotalDieselUsage = totalDieselGeneratorConsumption;
            ViewBag.TotalWasteUsage = totalWasteMade;
            ViewBag.TotalPaperUsage = totalPaperConsumption;

            ViewBag.TotalPaperCost = totalPaperCost;
            ViewBag.TotalWasteCost = totalWasteCost;
            ViewBag.TotalWaterCost = totalWaterCost;
            ViewBag.TotalElectricityCost = totalAmountSpentOnElectricity;
            ViewBag.TotalDieselCost = totalDieselGeneratorCost;


            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public double CalculateAmountSpent(BranchDetails branchDetails)
        {
            var amountSpentOnElectricity = branchDetails.ElectricityAmount;
            //var amountSpentOnDiesel = branchDetails.AmountPaidForDiesel;
            //var amountSpentOnPetrol = branchDetails.AmountPaidForPetrol;

            var totalAmountSpent = Convert.ToDouble(/*amountSpentOnDiesel +*/ amountSpentOnElectricity/* + amountSpentOnPetrol*/);

            return totalAmountSpent;
        }

        public double CalculateTotalCO2Emission(BranchDetails branchDetails)
        {
            var perBranch = _db.FootprintTable.Where(p => p.Status == FormStatus.Approved);


            //branchDetails.FuelCO2Emission = branchDetails.DieselGeneratorCO2Emission + branchDetails.DieselVehicleCO2Emission + branchDetails.PetrolCO2Emission;

            var totalCo2Emission = Convert.ToDouble(branchDetails.DieselGeneratorCO2Emission/* + branchDetails.FuelCO2Emission*/);

            return totalCo2Emission;
        }

    }
}