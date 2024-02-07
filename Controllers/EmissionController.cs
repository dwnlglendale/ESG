using CarbonFootprint1.Data;
using CarbonFootprint1.Methods;
using CarbonFootprint1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;



namespace CarbonFootprint1.Controllers
{
    public class EmissionController : Controller
    {
        private readonly ApplicationDbContext? _db;
        private readonly UserManager<IdentityUser>? _userManager;
        private readonly SignInManager<IdentityUser>? _signInManager;
        private readonly RoleManager<IdentityRole>? _roleManager;
        private readonly ISessionManagerService _sessionManagerService;
        private readonly ErrorLogs _errorLogs;

        public EmissionController(

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

        [Authorize(Roles = "Approver, Requester, Authorizer")]
        public async  Task<IActionResult> EmissionAnalytics(int? id)
        {
            try
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

                var branchDetails = _db.FootprintTable.Find(id);
                if (branchDetails == null)
                {
                    return NotFound();
                }


                //ViewBags (Total, Electricity, Fuel, and Diesel)
                var cO2Emissions = CalculateEmissions(branchDetails);
                ViewBag.CO2Emission = cO2Emissions;

                var electricityCO2Emissions = CalculateElectricityEmissions(branchDetails);
                ViewBag.ElectricityCO2Emission = electricityCO2Emissions;

                var fuelCO2Emissions = CalculateFuelEmissions(branchDetails);
                ViewBag.FuelCO2Emission = fuelCO2Emissions;

                var dieselCO2Emissions = CalculateDieselEmissions(branchDetails);
                ViewBag.DieselCO2Emission = dieselCO2Emissions;

                var totalUtilityCost = CalculateAmountSpent(branchDetails);
                ViewBag.TotalUtilityCost = totalUtilityCost;


                return View();
            }
            catch (Exception ex)
            {
                _errorLogs.LogExceptions(ex, "Emission Calculations have not been calculated");
                return RedirectToAction("Index", "Home");
            }
        }

        public double CalculateEmissions(BranchDetails branchDetails)
        {
           //Electricity C02 Calculations
                var percentageHydro = Convert.ToDouble(0.31 * branchDetails.ElectricityConsumed);  //calculate hydro and gas percentage
                var percentageGas = Convert.ToDouble(0.69 * branchDetails.ElectricityConsumed);

                var hydroElectricityCO2Emissions = Convert.ToDouble(percentageHydro * 0.018);       //calculate total emissions for electricity
                var gasElectricityCO2Emissions = Convert.ToDouble(percentageGas * 0.50);


                var totalElectricityCO2Emissions = hydroElectricityCO2Emissions + gasElectricityCO2Emissions;


                var totalEmission = (totalElectricityCO2Emissions) / 1015;
                return totalEmission;
            
         
        }

        public double CalculateElectricityEmissions(BranchDetails branchDetails)
        {
            //Electricity C02 Calculations
            var percentageHydro = (0.31 * branchDetails.ElectricityConsumed);  //calculate hydro and gas percentage
            var percentageGas = (0.69 * branchDetails.ElectricityConsumed);

            var hydroElectricityCO2Emissions = Convert.ToDouble(percentageHydro * 0.018);       //calculate total emissions for electricity
            var gasElectricityCO2Emissions = Convert.ToDouble(percentageGas * 0.50);

            var totalElectricityConsumed = percentageGas + percentageHydro;
            var totalElectricityCO2Emissions = hydroElectricityCO2Emissions + gasElectricityCO2Emissions;

            return totalElectricityCO2Emissions;
        }

        public double CalculateFuelEmissions(BranchDetails branchDetails)
        {
            ////Diesel C02 Calculations
            var vehicleDiesel = Convert.ToDouble(branchDetails.QuantityOfDieselConsumed * 2.67);      //Calculate Vehicle Diesel Consumption

            ////Petrol CO2 Calculations
            //var petrolC02Emissions = Convert.ToDouble(branchDetails.PetrolQuantityConsumed * 2.4);  //Calaculate Vehicle Petrol Consumption


            var totalFuelCO2Emissions = /*vehicleDiesel + petrolC02Emissions*/ vehicleDiesel;

            return totalFuelCO2Emissions;
        }

        public double CalculateDieselEmissions(BranchDetails branchDetails)
        {
            ////Diesel CO2 Calculations
            //var vehicleDiesel = Convert.ToDouble(branchDetails.DieselQuantityConsumed * 2.67);
            var generatorDiesel = Convert.ToDouble(branchDetails.QuantityOfDieselConsumed * 2.67);
            var totalDieselCO2Emissions = /*vehicleDiesel + */ generatorDiesel ;

            return generatorDiesel;
        }

        public double CalculateAmountSpent(BranchDetails branchDetails)
        {
            var amountSpentOnElectricity = branchDetails.ElectricityAmount;
            var amountSpentOnDiesel = branchDetails.AmountPaidForDieselGenerator;
            var amountSpentOnWater = (branchDetails.CostOfDrinkableWaterConsumed + branchDetails.CostOfRegularWaterConsumed);
            var amountSpentOnWaste = branchDetails.CostOfDisposal;

            var totalAmountSpent = Convert.ToDouble(amountSpentOnDiesel + amountSpentOnElectricity + amountSpentOnWater + amountSpentOnWaste);

            return totalAmountSpent;
        }

        
    }
}
