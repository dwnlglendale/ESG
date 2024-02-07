using CarbonFootprint1.Data;
using CarbonFootprint1.Methods;
using CarbonFootprint1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;


namespace CarbonFootprint1.Controllers
{
    public class FootprintController : Controller
    {


        private readonly ApplicationDbContext? _db;
        private readonly UserManager<IdentityUser>? _userManager;
        private readonly SignInManager<IdentityUser>? _signInManager;
        private readonly RoleManager<IdentityRole>? _roleManager;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ISessionManagerService _sessionManagerService;
        private readonly ErrorLogs _errorLogs;

        public FootprintController(

             ApplicationDbContext db,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IWebHostEnvironment hostEnvironment,
            ISessionManagerService sessionManagerService,
            ErrorLogs errorLogs
            )
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _hostEnvironment = hostEnvironment;
            _sessionManagerService = sessionManagerService;
            _errorLogs = errorLogs;
        }

        [Authorize(Roles = "Requester")]
        public async Task<IActionResult> branchInfo(BranchDetails formObj)
        {
            try{
                //Check is session is active
                var sessionActiveState = _sessionManagerService.CheckBrowserSession();
                if (sessionActiveState == false)
                {
                    await _signInManager.SignOutAsync();
                    return RedirectToAction("branchInfo", "Footprint");
                }

                if (ModelState.IsValid)
                {
                    var user = _userManager.FindByNameAsync(User.Identity.Name).Result;
                    var userDetails = _db.ApplicationUserTable.Where(u => u.Id == user.Id).FirstOrDefault();
                    
                    formObj.BranchName = userDetails.BranchName;
                    formObj.BranchSize = Convert.ToDouble(userDetails.BranchSize);
                    formObj.BranchCode = userDetails.BranchCode;
                    formObj.Status = FormStatus.Draft;

                    _db.FootprintTable.Add(formObj);
                    _db.SaveChanges();
                }
                _errorLogs.LogUserActivity(User.Identity!.Name!, "Branch Info Submitted");
                return RedirectToAction("FormElectricityDetails", "Sidebar", new { id = formObj.CarbornFootprint });
            }
            catch (Exception ex)
            {
                _errorLogs.LogExceptions(ex, "BranchInfo not submitted");
                return RedirectToAction("Index", "Home");
            }
        }

        [Authorize(Roles = "Requester")]
        public async Task<IActionResult> electricity(BranchDetails formObj, IFormCollection collectedFiles)
        {
            try
            {
                //Check is session is active
                var sessionActiveState = _sessionManagerService.CheckBrowserSession();
                if (sessionActiveState == false)
                {
                    await _signInManager.SignOutAsync();
                    return RedirectToAction("electricity", "Footprint");
                }


                if (ModelState.IsValid)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    var branchReciept = collectedFiles.Files.Where(f => f.Name == "attachments[]").ToList();


                    string uploadedFileName = string.Empty;
                    List<string> fileNames = new List<string>();
                    if (branchReciept != null)
                    {
                        foreach (var file in branchReciept)
                        {
                            var uId = Guid.NewGuid().ToString();
                            var originalFileName = Path.GetFileNameWithoutExtension(file.FileName);
                            var fileExtension = Path.GetExtension(file.FileName);
                            uploadedFileName = originalFileName + "_" + uId + fileExtension;
                            var uploadPath = Path.Combine(wwwRootPath, @"uploads\receipts");

                            using (var fileStreams = new FileStream(Path.Combine(uploadPath, uploadedFileName ), FileMode.Create))
                            {
                                file.CopyTo(fileStreams);
                                fileNames.Add("##" + @"uploads/receipts/" + uploadedFileName);
                            }
                        }
                    }

                    //lets cast the array back to string 

                    string newFileNameList = String.Join("", fileNames);

                    var newForm = _db.FootprintTable.Find(formObj.CarbornFootprint);
                    if (formObj.DocumentsPath == null)
                    {
                        newForm.DocumentsPath = newFileNameList;

                    }
                    else
                    {
                        newForm.DocumentsPath = formObj.DocumentsPath + newFileNameList;
                    }



                    //var newForm = _db.FootprintTable.Find(formObj.CarbornFootprint);
                    newForm.ElectricityConsumed = formObj.ElectricityConsumed;
                    newForm.ElectricityAmount = formObj.ElectricityAmount;
                    newForm.AlternativeSourceOfEnergy = formObj.AlternativeSourceOfEnergy;
                    newForm.MeterType = formObj.MeterType;
                    newForm.Status = FormStatus.Draft;

                    _db.FootprintTable.Update(newForm);
                    _db.SaveChanges();
                }
                _errorLogs.LogUserActivity(User.Identity!.Name!, "Electricity Form Submitted");
                return RedirectToAction("FormDieselDetails", "Sidebar", new { id = formObj.CarbornFootprint });
            }
            catch (Exception ex)
            {
                TempData["error"] = "Error Occurred, could not submit form";
                _errorLogs.LogExceptions(ex, "Electricity Form not submitted");
                return RedirectToAction("Index", "Home");
            }
        }

        [Authorize(Roles = "Requester")]
        public async  Task<IActionResult> airtravel(AirTravel formObj)
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
                if (ModelState.IsValid)
                {
                    //formObj.DomesticTravelCount = 0;
                    //formObj.InternationalTravelCount;
                    //formObj.DomesticTravelCost;
                    //formObj.InternationalTravelCost;
                    //newForm.DomDistanceTravelled = formObj.DomDistanceTravelled;
                    //newForm.IntDistanceTravelled = formObj.IntDistanceTravelled;
                    _db.AirTravelTable.Add(formObj);
                    _db.SaveChanges();

                    TempData["success"] = "FootPrint Added Successfully";
                }
            }
            catch(Exception ex)
                {
                TempData["error"] = "Error Occurred, could not submit form";
                _errorLogs.LogExceptions(ex, "Air Travel Form not saved");
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home", new { id = formObj.ID });
        }

        [Authorize(Roles = "Requester")]
        public async Task<IActionResult> diesel(BranchDetails formObj, IFormCollection collectedFiles)
        {
            //Check is session is active
            var sessionActiveState = _sessionManagerService.CheckBrowserSession();
            if (sessionActiveState == false)
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Index", "Home");
            }


            try{ 
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                var branchReciept = collectedFiles.Files.Where(f => f.Name == "attachments[]").ToList();


                string uploadedFileName = string.Empty;
                List<string> fileNames = new List<string>();
                if (branchReciept != null)
                {
                    foreach (var file in branchReciept)
                    {
                        var uId = Guid.NewGuid().ToString();
                        var originalFileName = Path.GetFileNameWithoutExtension(file.FileName);
                        var fileExtension = Path.GetExtension(file.FileName);
                        uploadedFileName = originalFileName + "_" + uId + fileExtension;
                        var uploadPath = Path.Combine(wwwRootPath, @"uploads\receipts");

                        using (var fileStreams = new FileStream(Path.Combine(uploadPath, uploadedFileName), FileMode.Create))
                        {
                            file.CopyTo(fileStreams);
                            fileNames.Add("##" + @"uploads/receipts/" + uploadedFileName);
                        }
                    }
                }

                //lets cast the array back to string 

                string newFileNameList = String.Join("", fileNames);

                var newForm = _db.FootprintTable.Find(formObj.CarbornFootprint);

                var updateForm = _db.FootprintTable.Find(newForm.CarbornFootprint);

                if (newForm.DocumentsPath == null)
                {
                    //newForm.DocumentsPath = formObj.DocumentsPath + newFileNameList;
                    newForm.DocumentsPath = newFileNameList;

                }
                else
                {
                    updateForm.DocumentsPath = newForm.DocumentsPath + newFileNameList;
                }

                //var newForm = _db.FootprintTable.Find(formObj.CarbornFootprint);
                newForm.AmountPaidForDieselGenerator = formObj.AmountPaidForDieselGenerator;
                _errorLogs.LogUserActivity("User", "user added value: " + formObj.AmountPaidForDieselGenerator);
                newForm.QuantityOfDieselPurchased = formObj.QuantityOfDieselPurchased;
                newForm.QuantityOfDieselConsumed = formObj.QuantityOfDieselConsumed;
                newForm.QuantityOfDieselLeft = formObj.QuantityOfDieselLeft;
                newForm.TotalRuningGeneratorHours = formObj.TotalRuningGeneratorHours;
                newForm.QuantityOfDieselConsumed = formObj.QuantityOfDieselConsumed;
                _db.FootprintTable.Update(newForm);
                _db.SaveChanges();

                _errorLogs.LogUserActivity("User", "Diesel Amount: " + formObj.AmountPaidForDieselGenerator + ", Diesel Purchased: " + formObj.QuantityOfDieselPurchased + ", Diesel Consumed: " + formObj.QuantityOfDieselConsumed + ", Diesel left: " + formObj.QuantityOfDieselLeft + ", Hours: " + formObj.TotalRuningGeneratorHours + ", Diesel Consumed: " + formObj.QuantityOfDieselConsumed);
            }
        }
            catch (Exception ex)
            {
                TempData["error"] = "Error Occurred, could not submit form";
                _errorLogs.LogExceptions(ex, "Fuel Form not submitted");
                return RedirectToAction("Index", "Home");
                
            }

            return RedirectToAction("FormWaterDetails", "Sidebar", new { id = formObj.CarbornFootprint });
        }

        [Authorize(Roles = "Requester")]
        public async Task<IActionResult> fuel(BranchDetails formObj)
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
                if (ModelState.IsValid)
                {
                    var newForm = _db.FootprintTable.Find(formObj.CarbornFootprint);


                    _db.FootprintTable.Update(newForm);
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = "Error Occurred, could not save details";
                _errorLogs.LogExceptions(ex, "Fuel Form not submitted");
                return RedirectToAction("Index", "Home");

            }
            return RedirectToAction("FormWaterDetails", "Sidebar", new { id = formObj.CarbornFootprint });
        }

        [Authorize(Roles = "Requester")]
        public async Task<IActionResult> water(BranchDetails formObj, IFormCollection collectedFiles)
        {
            if (ModelState.IsValid)
            {
                //Check is session is active
                var sessionActiveState = _sessionManagerService.CheckBrowserSession();
                if (sessionActiveState == false)
                {
                    await _signInManager.SignOutAsync();
                    return RedirectToAction("Index", "Home");
                }
                try { 
                string wwwRootPath = _hostEnvironment.WebRootPath;
                var branchReciept = collectedFiles.Files.Where(f => f.Name == "attachments[]").ToList();



                string uploadedFileName = string.Empty;
                List<string> fileNames = new List<string>();
                if (branchReciept != null)
                {
                    foreach (var file in branchReciept)
                    {
                        var uId = Guid.NewGuid().ToString();
                        var originalFileName = Path.GetFileNameWithoutExtension(file.FileName);
                        var fileExtension = Path.GetExtension(file.FileName);
                        uploadedFileName = originalFileName + "_" + uId + fileExtension;
                        var uploadPath = Path.Combine(wwwRootPath, @"uploads\receipts");

                        using (var fileStreams = new FileStream(Path.Combine(uploadPath, uploadedFileName), FileMode.Create))
                        {
                            file.CopyTo(fileStreams);
                            fileNames.Add("##" + @"uploads/receipts/" + uploadedFileName);
                        }
                    }
                }

                //lets cast the array back to string 

                string newFileNameList = String.Join("", fileNames);

                var newForm = _db.FootprintTable.Find(formObj.CarbornFootprint);

                var updateForm = _db.FootprintTable.Find(newForm.CarbornFootprint);

                if (newForm.DocumentsPath == null)
                {
                    //newForm.DocumentsPath = formObj.DocumentsPath + newFileNameList;
                    newForm.DocumentsPath = newFileNameList;

                }
                else
                {
                    updateForm.DocumentsPath = newForm.DocumentsPath + newFileNameList;
                }

                //var newForm = _db.FootprintTable.Find(formObj.CarbornFootprint);
                newForm.QuantityOfDrinkableWaterConsumed = formObj.QuantityOfDrinkableWaterConsumed;
                newForm.QuantityOfRegularWaterConsumed = formObj.QuantityOfRegularWaterConsumed;
                newForm.CostOfRegularWaterConsumed = formObj.CostOfRegularWaterConsumed;
                newForm.CostOfDrinkableWaterConsumed = formObj.CostOfDrinkableWaterConsumed;

                _db.FootprintTable.Update(newForm);
                _db.SaveChanges();
            }
              catch (Exception ex)
            {
                TempData["error"] = "Error Occurred, could not submit form";
                _errorLogs.LogExceptions(ex, "Water Form not submitted");
                return RedirectToAction("Index", "Home");

            }
        }
            return RedirectToAction("FormPaperDetails", "Sidebar", new { id = formObj.CarbornFootprint });
        }

        [Authorize(Roles = "Requester")]
        public async Task<IActionResult> paper(BranchDetails formObj, IFormCollection collectedFiles)
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
                if (ModelState.IsValid)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    var branchReciept = collectedFiles.Files.Where(f => f.Name == "attachments[]").ToList();



                    string uploadedFileName = string.Empty;
                    List<string> fileNames = new List<string>();
                    if (branchReciept != null)
                    {
                        foreach (var file in branchReciept)
                        {
                            var uId = Guid.NewGuid().ToString();
                            var originalFileName = Path.GetFileNameWithoutExtension(file.FileName);
                            var fileExtension = Path.GetExtension(file.FileName);
                            uploadedFileName = originalFileName + "_" + uId + fileExtension;
                            var uploadPath = Path.Combine(wwwRootPath, @"uploads\receipts");

                            using (var fileStreams = new FileStream(Path.Combine(uploadPath, uploadedFileName), FileMode.Create))
                            {
                                file.CopyTo(fileStreams);
                                fileNames.Add("##" + @"uploads/receipts/" + uploadedFileName);
                            }
                        }
                    }

                    //lets cast the array back to string 

                    string newFileNameList = String.Join("", fileNames);

                    var newForm = _db.FootprintTable.Find(formObj.CarbornFootprint);

                    var updateForm = _db.FootprintTable.Find(newForm.CarbornFootprint);

                    if (newForm.DocumentsPath == null)
                    {
                        //newForm.DocumentsPath = formObj.DocumentsPath + newFileNameList;
                        newForm.DocumentsPath = newFileNameList;

                    }
                    else
                    {
                        updateForm.DocumentsPath = newForm.DocumentsPath + newFileNameList;
                    }

                    //var newForm = _db.FootprintTable.Find(formObj.CarbornFootprint);
                    newForm.NumberOfPaperUsed = formObj.NumberOfPaperUsed;
                    newForm.CostOfPaperUsed = formObj.CostOfPaperUsed;


                    _db.FootprintTable.Update(newForm);
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = "Error Occurred, could not submit form";
                _errorLogs.LogExceptions(ex, "Paper Form not submitted");
                return RedirectToAction("Index", "Home");

            }
            return RedirectToAction("FormWasteDetails", "Sidebar", new { id = formObj.CarbornFootprint });
        }

        [Authorize(Roles = "Requester")]
        public async Task<IActionResult> waste(BranchDetails formObj, IFormCollection collectedFiles)
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
                if (ModelState.IsValid)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    var branchReciept = collectedFiles.Files.Where(f => f.Name == "attachments[]").ToList();




                    string uploadedFileName = string.Empty;
                    List<string> fileNames = new List<string>();
                    if (branchReciept != null)
                    {
                        foreach (var file in branchReciept)
                        {
                            var uId = Guid.NewGuid().ToString();
                            var originalFileName = Path.GetFileNameWithoutExtension(file.FileName);
                            var fileExtension = Path.GetExtension(file.FileName);
                            uploadedFileName = originalFileName + "_" + uId + fileExtension;
                            var uploadPath = Path.Combine(wwwRootPath, @"uploads\receipts");

                            //Find folder path if not found create
                            if (!System.IO.Directory.Exists(uploadPath))
                            {
                                System.IO.Directory.CreateDirectory(uploadPath);
                            }

                            using (var fileStreams = new FileStream(Path.Combine(uploadPath, uploadedFileName), FileMode.Create))
                            {
                                file.CopyTo(fileStreams);
                                fileNames.Add("##" + @"uploads/receipts/" + uploadedFileName);
                            }
                        }
                    }

                    //lets cast the array back to string 

                    string newFileNameList = String.Join("", fileNames);

                    var newForm = _db.FootprintTable.Find(formObj.CarbornFootprint);

                    var updateForm = _db.FootprintTable.Find(newForm.CarbornFootprint);

                    if (newForm.DocumentsPath == null)
                    {
                        //newForm.DocumentsPath = formObj.DocumentsPath + newFileNameList;
                        newForm.DocumentsPath = newFileNameList;

                    }
                    else
                    {
                        updateForm.DocumentsPath = newForm.DocumentsPath + newFileNameList;
                    }

                    newForm.MeansOfDisposal = formObj.MeansOfDisposal;
                    newForm.CostOfDisposal = formObj.CostOfDisposal;
                    newForm.QuantityOfDisposal = formObj.QuantityOfDisposal;
                    newForm.Status = FormStatus.Pending;

                    _db.FootprintTable.Update(newForm);
                    _db.SaveChanges();

                    //Initiate Email to Approver

                    var bDetails = formObj.BranchName;

                    var emailHelper = new Email();
                    string message = @"<!DOCTYPE html><html xmlns:v=""urn:schemas-microsoft-com:vml"" xmlns:o=""urn:schemas-microsoft-com:office:office"" lang=""en""><head><title></title><meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8""><meta name=""viewport"" content=""width=device-width,initial-scale=1""><!--[if mso]><xml><o:officedocumentsettings><o:pixelsperinch>96</o:pixelsperinch><o:allowpng></o:officedocumentsettings></xml><![endif]--><style>*{box-sizing:border-box}body{margin:0;padding:0}a[x-apple-data-detectors]{color:inherit!important;text-decoration:inherit!important}#MessageViewBody a{color:inherit;text-decoration:none}p{line-height:inherit}.desktop_hide,.desktop_hide table{mso-hide:all;display:none;max-height:0;overflow:hidden}.image_block img+div{display:none}@media (max-width:660px){.desktop_hide table.icons-inner{display:inline-block!important}.icons-inner{text-align:center}.icons-inner td{margin:0 auto}.row-content{width:100%!important}.mobile_hide{display:none}.stack .column{width:100%;display:block}.mobile_hide{min-height:0;max-height:0;max-width:0;overflow:hidden;font-size:0}.desktop_hide,.desktop_hide table{display:table!important;max-height:none!important}}</style></head><body style=""background-color:#fff;margin:0;padding:0;-webkit-text-size-adjust:none;text-size-adjust:none""><table class=""nl-container"" width=""100%"" border=""0"" cellpadding=""0"" cellspacing=""0"" role=""presentation"" style=""mso-table-lspace:0;mso-table-rspace:0;background-color:#fff""><tbody><tr><td><table class=""row row-1"" align=""center"" width=""100%"" border=""0"" cellpadding=""0"" cellspacing=""0"" role=""presentation"" style=""mso-table-lspace:0;mso-table-rspace:0""><tbody><tr><td><table class=""row-content stack"" align=""center"" border=""0"" cellpadding=""0"" cellspacing=""0"" role=""presentation"" style=""mso-table-lspace:0;mso-table-rspace:0;background-color:#f02828;color:#000;width:640px"" width=""640""><tbody><tr><td class=""column column-1"" width=""100%"" style=""mso-table-lspace:0;mso-table-rspace:0;font-weight:400;text-align:left;vertical-align:top;border-top:0;border-right:0;border-bottom:0;border-left:0""><table class=""image_block block-1"" width=""100%"" border=""0"" cellpadding=""0"" cellspacing=""0"" role=""presentation"" style=""mso-table-lspace:0;mso-table-rspace:0""><tr><td class=""pad"" style=""width:100%;padding-right:0;padding-left:0""><div class=""alignment"" align=""left"" style=""line-height:10px""><img src=""https://d1oco4z2z1fhwp.cloudfront.net/templates/default/1211/top-left.png"" style=""display:block;height:auto;border:0;width:205px;max-width:100%"" width=""205"" alt=""Alternate text"" title=""Alternate text""></div></td></tr></table><table class=""text_block block-2"" width=""100%"" border=""0"" cellpadding=""0"" cellspacing=""0"" role=""presentation"" style=""mso-table-lspace:0;mso-table-rspace:0;word-break:break-word""><tr><td class=""pad"" style=""padding-bottom:10px;padding-left:10px;padding-right:10px;padding-top:35px""><div style=""font-family:Verdana,sans-serif""><div class style=""font-size:12px;font-family:Verdana,Geneva,sans-serif;mso-line-height-alt:14.399999999999999px;color:#fff;line-height:1.2""><p style=""margin:0;font-size:14px;text-align:center;mso-line-height-alt:16.8px""><span style=""font-size:50px"">Hello Team!</span></p><p style=""margin:0;font-size:14px;text-align:center;mso-line-height-alt:16.8px""><span style=""font-size:50px"">A Footprint has been submitted &nbsp;</span></p></div></div></td></tr></table><table class=""text_block block-3"" width=""100%"" border=""0"" cellpadding=""10"" cellspacing=""0"" role=""presentation"" style=""mso-table-lspace:0;mso-table-rspace:0;word-break:break-word""><tr><td class=""pad""><div style=""font-family:sans-serif""><div class style=""font-size:12px;font-family:Poppins,Arial,Helvetica,sans-serif;mso-line-height-alt:18px;color:#fff;line-height:1.5""><p style=""margin:0;font-size:14px;text-align:center;mso-line-height-alt:21px""></p></div></div></td></tr></table><table class=""button_block block-4"" width=""100%"" border=""0"" cellpadding=""0"" cellspacing=""0"" role=""presentation"" style=""mso-table-lspace:0;mso-table-rspace:0""><tr><td class=""pad"" style=""padding-bottom:30px;padding-left:10px;padding-right:10px;padding-top:10px;text-align:center""><div class=""alignment"" align=""center""><!--[if mso]><v:roundrect xmlns:v=""urn:schemas-microsoft-com:vml"" xmlns:w=""urn:schemas-microsoft-com:office:word"" href=""https://carbon.apps.cbg.com.gh"" style=""height:38px;width:104px;v-text-anchor:middle"" arcsize=""11%"" stroke=""false"" fillcolor=""#000000""><w:anchorlock><v:textbox inset=""0px,0px,0px,0px""><center style=""color:#fff;font-family:Arial,sans-serif;font-size:14px""><![endif]--><a href=""https://carbon.apps.cbg.com.gh/"" target=""_blank"" style=""text-decoration:none;display:inline-block;color:#fff;background-color:#000;border-radius:4px;width:auto;border-top:0 solid transparent;font-weight:undefined;border-right:0 solid transparent;border-bottom:0 solid transparent;border-left:0 solid transparent;padding-top:5px;padding-bottom:5px;font-family:Poppins,Arial,Helvetica,sans-serif;font-size:14px;text-align:center;mso-border-alt:none;word-break:keep-all""><span style=""padding-left:35px;padding-right:35px;font-size:14px;display:inline-block;letter-spacing:normal""><span dir=""ltr"" style=""word-break:break-word""><span style=""line-height:28px"" dir=""ltr"" data-mce-style>Login</span></span></span></a><!--[if mso]><![endif]--></div></td></tr></table><table class=""image_block block-5"" width=""100%"" border=""0"" cellpadding=""0"" cellspacing=""0"" role=""presentation"" style=""mso-table-lspace:0;mso-table-rspace:0""><tr><td class=""pad"" style=""width:100%;padding-right:0;padding-left:0""><div class=""alignment"" align=""right"" style=""line-height:10px""><img src=""https://d1oco4z2z1fhwp.cloudfront.net/templates/default/1211/bottom-right.png"" style=""display:block;height:auto;border:0;width:209px;max-width:100%"" width=""209"" alt=""Alternate text"" title=""Alternate text""></div></td></tr></table></td></tr></tbody></table></td></tr></tbody></table><table class=""row row-2"" align=""center"" width=""100%"" border=""0"" cellpadding=""0"" cellspacing=""0"" role=""presentation"" style=""mso-table-lspace:0;mso-table-rspace:0""><tbody><tr><td><table class=""row-content stack"" align=""center"" border=""0"" cellpadding=""0"" cellspacing=""0"" role=""presentation"" style=""mso-table-lspace:0;mso-table-rspace:0;color:#000;width:640px"" width=""640""><tbody><tr><td class=""column column-1"" width=""100%"" style=""mso-table-lspace:0;mso-table-rspace:0;font-weight:400;text-align:left;padding-bottom:5px;padding-top:5px;vertical-align:top;border-top:0;border-right:0;border-bottom:0;border-left:0""><table class=""icons_block block-1"" width=""100%"" border=""0"" cellpadding=""0"" cellspacing=""0"" role=""presentation"" style=""mso-table-lspace:0;mso-table-rspace:0""><tr><td class=""pad"" style=""vertical-align:middle;color:#9d9d9d;font-family:inherit;font-size:15px;padding-bottom:5px;padding-top:5px;text-align:center""><table width=""100%"" cellpadding=""0"" cellspacing=""0"" role=""presentation"" style=""mso-table-lspace:0;mso-table-rspace:0""><tr><td class=""alignment"" style=""vertical-align:middle;text-align:center""><!--[if vml]><table align=""left"" cellpadding=""0"" cellspacing=""0"" role=""presentation"" style=""display:inline-block;padding-left:0;padding-right:0;mso-table-lspace:0;mso-table-rspace:0""><![endif]--><!--[if !vml]><!--><table class=""icons-inner"" style=""mso-table-lspace:0;mso-table-rspace:0;display:inline-block;margin-right:-4px;padding-left:0;padding-right:0"" cellpadding=""0"" cellspacing=""0"" role=""presentation""><!--<![endif]--><tr><td style=""vertical-align:middle;text-align:center;padding-top:5px;padding-bottom:5px;padding-left:5px;padding-right:6px""><a href=""https://www.designedwithbee.com/"" target=""_blank"" style=""text-decoration:none""><img class=""icon"" alt=""Designed with BEE"" src=""https://d15k2d11r6t6rl.cloudfront.net/public/users/Integrators/BeeProAgency/53601_510656/Signature/bee.png"" height=""32"" width=""34"" align=""center"" style=""display:block;height:auto;margin:0 auto;border:0""></a></td><td style=""font-family:Poppins,Arial,Helvetica,sans-serif;font-size:15px;color:#9d9d9d;vertical-align:middle;letter-spacing:undefined;text-align:center""><a href=""https://www.designedwithbee.com/"" target=""_blank"" style=""color:#9d9d9d;text-decoration:none"">Designed with BEE</a></td></tr></table></td></tr></table></td></tr></table></td></tr></tbody></table></td></tr></tbody></table></td></tr></tbody></table></body></html>";
                    var success = await emailHelper.SendEmailAsync(/*"erm@cbg.com.gh"*/"glenn.inkoom@cbg.com.gh", "CBG CARBON FOOTPRINT PORTAL", message);


                    _errorLogs.LogUserActivity(User.Identity.Name, "Submitted a footprint!");
                    TempData["success"] = "FootPrint Added Successfully";

                   
                }
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _errorLogs.LogExceptions(ex, "Footprint was not submitted");
                return RedirectToAction("Index", "Home");
            }
        }

        [Authorize(Roles = "Requester")]
        public async Task<IActionResult> update(BranchDetails updateForm, string? status, IFormCollection collectedFiles)
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
                var formObj = _db.FootprintTable.Find(updateForm.CarbornFootprint);

                formObj.PermanentStaffNumber = updateForm.PermanentStaffNumber;
                formObj.PermanentStaffNumber = updateForm.PermanentStaffNumber;
                formObj.NonPermanentStaffNumber = updateForm.NonPermanentStaffNumber;


                formObj.ElectricityConsumed = updateForm.ElectricityConsumed;
                formObj.ElectricityAmount = updateForm.ElectricityAmount;
                formObj.AlternativeSourceOfEnergy = updateForm.AlternativeSourceOfEnergy;
                formObj.MeterType = updateForm.MeterType;

                formObj.AmountPaidForDieselGenerator = updateForm.AmountPaidForDieselGenerator;
                formObj.QuantityOfDieselPurchased = updateForm.QuantityOfDieselPurchased;
                formObj.QuantityOfDieselConsumed = updateForm.QuantityOfDieselConsumed;
                formObj.QuantityOfDieselLeft = updateForm.QuantityOfDieselLeft;
                formObj.TotalRuningGeneratorHours = updateForm.TotalRuningGeneratorHours;
                formObj.QuantityOfDieselConsumed = updateForm.QuantityOfDieselConsumed;

                formObj.AmountPaidForDieselGenerator = updateForm.AmountPaidForDieselGenerator;
                formObj.QuantityOfDieselPurchased = updateForm.QuantityOfDieselPurchased;
                formObj.QuantityOfDieselConsumed = updateForm.QuantityOfDieselConsumed;
                formObj.QuantityOfDieselLeft = updateForm.QuantityOfDieselLeft;
                formObj.TotalRuningGeneratorHours = updateForm.TotalRuningGeneratorHours;

                formObj.QuantityOfDrinkableWaterConsumed = updateForm.QuantityOfDrinkableWaterConsumed;
                formObj.QuantityOfRegularWaterConsumed = updateForm.QuantityOfRegularWaterConsumed;
                formObj.CostOfRegularWaterConsumed = updateForm.CostOfRegularWaterConsumed;
                formObj.CostOfDrinkableWaterConsumed = updateForm.CostOfDrinkableWaterConsumed;

                formObj.NumberOfPaperUsed = updateForm.NumberOfPaperUsed;
                formObj.CostOfPaperUsed = updateForm.CostOfPaperUsed;

                formObj.MeansOfDisposal = updateForm.MeansOfDisposal;
                formObj.CostOfDisposal = updateForm.CostOfDisposal;
                formObj.QuantityOfDisposal = updateForm.QuantityOfDisposal;
                updateForm.Status = FormStatus.Pending;
                formObj.Status = updateForm.Status;

                string wwwRootPath = _hostEnvironment.WebRootPath;
                var branchReciept = collectedFiles.Files.Where(f => f.Name == "attachments[]").ToList();


                string uploadedFileName = string.Empty;
                List<string> fileNames = new List<string>();
                if (branchReciept != null)
                {
                    foreach (var file in branchReciept)
                    {
                        var uId = Guid.NewGuid().ToString();
                        var originalFileName = Path.GetFileNameWithoutExtension(file.FileName);
                        var fileExtension = Path.GetExtension(file.FileName);
                        uploadedFileName = originalFileName + "_" + uId + fileExtension;
                        var uploadPath = Path.Combine(wwwRootPath, @"uploads\receipts");

                        using (var fileStreams = new FileStream(Path.Combine(uploadPath, uploadedFileName), FileMode.Create))
                        {
                            file.CopyTo(fileStreams);
                            fileNames.Add("##" + @"uploads/receipts/" + uploadedFileName);
                        }
                    }
                }

                //lets cast the array back to string 

                string newFileNameList = String.Join("", fileNames);

                var newForm = _db.FootprintTable.Find(formObj.CarbornFootprint);
                if (formObj.DocumentsPath == null)
                {
                    newForm.DocumentsPath = newFileNameList;

                }
                else
                {
                    newForm.DocumentsPath = formObj.DocumentsPath + newFileNameList;
                }


                _db.FootprintTable.Update(newForm);
                //_db.SaveChanges();
                //TempData["success"] = "FootPrint Added Successfully";


                _db.SaveChanges();

                if (status == "update")
                {


                    formObj.Status = FormStatus.Pending;


                    _db.FootprintTable.Update(formObj);
                    _db.SaveChanges();
                    TempData["success"] = "Form Succesfully Updated";
                }
                else if (status == "return")
                {
                    formObj.Status = FormStatus.Returned;


                    _db.FootprintTable.Update(formObj);
                    _db.SaveChanges();
                    TempData["success"] = "Form Succesfully Updated";
                }
                _errorLogs.LogUserActivity(User.Identity!.Name, "Form Successfully Updated");
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _errorLogs.LogExceptions(ex, "Form not updated");
                TempData["error"] = "Form not updated";
                return RedirectToAction("Index", "Home");
            }

        }


        [Authorize(Roles = "Approver, Authorizer")]
        public async  Task<IActionResult> StatusChange(BranchDetails? addComment, int? id, string? status)
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

            var formStatus = _db.FootprintTable.Where(s => s.CarbornFootprint == id).FirstOrDefault();



            if (formStatus == null)
            {
                return NotFound();
            }

            if (status == "accept")
            {
                formStatus.Status = FormStatus.Approved;


                _db.FootprintTable.Update(formStatus);
                _db.SaveChanges();
                TempData["success"] = "Form Succesfully Accepted";
            }
            else if (status == "return")
            {
                formStatus.Status = FormStatus.Returned;
                formStatus.Comment = addComment.Comment;
                _db.FootprintTable.Update(formStatus);
                _db.SaveChanges();


                //var uD = _db.ApplicationUserTable.Find(id).Email;

                var userEmail= _db.ApplicationUserTable?.Where(ue => ue.BranchName == formStatus.BranchName)?.FirstOrDefault()?.Email;

               

                var emailHelper = new Email();
                string message = @"<!DOCTYPE html><html xmlns:v=""urn:schemas-microsoft-com:vml"" xmlns:o=""urn:schemas-microsoft-com:office:office"" lang=""en""><head><title></title><meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8""><meta name=""viewport"" content=""width=device-width,initial-scale=1""><!--[if mso]><xml><o:officedocumentsettings><o:pixelsperinch>96</o:pixelsperinch><o:allowpng></o:officedocumentsettings></xml><![endif]--><style>*{box-sizing:border-box}body{margin:0;padding:0}a[x-apple-data-detectors]{color:inherit!important;text-decoration:inherit!important}#MessageViewBody a{color:inherit;text-decoration:none}p{line-height:inherit}.desktop_hide,.desktop_hide table{mso-hide:all;display:none;max-height:0;overflow:hidden}.image_block img+div{display:none}@media (max-width:660px){.desktop_hide table.icons-inner{display:inline-block!important}.icons-inner{text-align:center}.icons-inner td{margin:0 auto}.row-content{width:100%!important}.mobile_hide{display:none}.stack .column{width:100%;display:block}.mobile_hide{min-height:0;max-height:0;max-width:0;overflow:hidden;font-size:0}.desktop_hide,.desktop_hide table{display:table!important;max-height:none!important}}</style></head><body style=""background-color:#fff;margin:0;padding:0;-webkit-text-size-adjust:none;text-size-adjust:none""><table class=""nl-container"" width=""100%"" border=""0"" cellpadding=""0"" cellspacing=""0"" role=""presentation"" style=""mso-table-lspace:0;mso-table-rspace:0;background-color:#fff""><tbody><tr><td><table class=""row row-1"" align=""center"" width=""100%"" border=""0"" cellpadding=""0"" cellspacing=""0"" role=""presentation"" style=""mso-table-lspace:0;mso-table-rspace:0""><tbody><tr><td><table class=""row-content stack"" align=""center"" border=""0"" cellpadding=""0"" cellspacing=""0"" role=""presentation"" style=""mso-table-lspace:0;mso-table-rspace:0;background-color:#f02828;color:#000;width:640px"" width=""640""><tbody><tr><td class=""column column-1"" width=""100%"" style=""mso-table-lspace:0;mso-table-rspace:0;font-weight:400;text-align:left;vertical-align:top;border-top:0;border-right:0;border-bottom:0;border-left:0""><table class=""image_block block-1"" width=""100%"" border=""0"" cellpadding=""0"" cellspacing=""0"" role=""presentation"" style=""mso-table-lspace:0;mso-table-rspace:0""><tr><td class=""pad"" style=""width:100%;padding-right:0;padding-left:0""><div class=""alignment"" align=""left"" style=""line-height:10px""><img src=""https://d1oco4z2z1fhwp.cloudfront.net/templates/default/1211/top-left.png"" style=""display:block;height:auto;border:0;width:205px;max-width:100%"" width=""205"" alt=""Alternate text"" title=""Alternate text""></div></td></tr></table><table class=""text_block block-2"" width=""100%"" border=""0"" cellpadding=""0"" cellspacing=""0"" role=""presentation"" style=""mso-table-lspace:0;mso-table-rspace:0;word-break:break-word""><tr><td class=""pad"" style=""padding-bottom:10px;padding-left:10px;padding-right:10px;padding-top:35px""><div style=""font-family:Verdana,sans-serif""><div class style=""font-size:12px;font-family:Verdana,Geneva,sans-serif;mso-line-height-alt:14.399999999999999px;color:#fff;line-height:1.2""><p style=""margin:0;font-size:14px;text-align:center;mso-line-height-alt:16.8px""><span style=""font-size:50px"">Hello!</span></p><p style=""margin:0;font-size:14px;text-align:center;mso-line-height-alt:16.8px""><span style=""font-size:50px"">A Footprint has been returned for correction &nbsp;</span></p></div></div></td></tr></table><table class=""text_block block-3"" width=""100%"" border=""0"" cellpadding=""10"" cellspacing=""0"" role=""presentation"" style=""mso-table-lspace:0;mso-table-rspace:0;word-break:break-word""><tr><td class=""pad""><div style=""font-family:sans-serif""><div class style=""font-size:12px;font-family:Poppins,Arial,Helvetica,sans-serif;mso-line-height-alt:18px;color:#fff;line-height:1.5""><p style=""margin:0;font-size:14px;text-align:center;mso-line-height-alt:21px""></p></div></div></td></tr></table><table class=""button_block block-4"" width=""100%"" border=""0"" cellpadding=""0"" cellspacing=""0"" role=""presentation"" style=""mso-table-lspace:0;mso-table-rspace:0""><tr><td class=""pad"" style=""padding-bottom:30px;padding-left:10px;padding-right:10px;padding-top:10px;text-align:center""><div class=""alignment"" align=""center""><!--[if mso]><v:roundrect xmlns:v=""urn:schemas-microsoft-com:vml"" xmlns:w=""urn:schemas-microsoft-com:office:word"" href=""http://carbon.apps.cbg.com.gh"" style=""height:38px;width:104px;v-text-anchor:middle"" arcsize=""11%"" stroke=""false"" fillcolor=""#000000""><w:anchorlock><v:textbox inset=""0px,0px,0px,0px""><center style=""color:#fff;font-family:Arial,sans-serif;font-size:14px""><![endif]--><a href=""https://carbon.apps.cbg.com.gh/"" target=""_blank"" style=""text-decoration:none;display:inline-block;color:#fff;background-color:#000;border-radius:4px;width:auto;border-top:0 solid transparent;font-weight:undefined;border-right:0 solid transparent;border-bottom:0 solid transparent;border-left:0 solid transparent;padding-top:5px;padding-bottom:5px;font-family:Poppins,Arial,Helvetica,sans-serif;font-size:14px;text-align:center;mso-border-alt:none;word-break:keep-all""><span style=""padding-left:35px;padding-right:35px;font-size:14px;display:inline-block;letter-spacing:normal""><span dir=""ltr"" style=""word-break:break-word""><span style=""line-height:28px"" dir=""ltr"" data-mce-style>Login</span></span></span></a><!--[if mso]><![endif]--></div></td></tr></table><table class=""image_block block-5"" width=""100%"" border=""0"" cellpadding=""0"" cellspacing=""0"" role=""presentation"" style=""mso-table-lspace:0;mso-table-rspace:0""><tr><td class=""pad"" style=""width:100%;padding-right:0;padding-left:0""><div class=""alignment"" align=""right"" style=""line-height:10px""><img src=""https://d1oco4z2z1fhwp.cloudfront.net/templates/default/1211/bottom-right.png"" style=""display:block;height:auto;border:0;width:209px;max-width:100%"" width=""209"" alt=""Alternate text"" title=""Alternate text""></div></td></tr></table></td></tr></tbody></table></td></tr></tbody></table><table class=""row row-2"" align=""center"" width=""100%"" border=""0"" cellpadding=""0"" cellspacing=""0"" role=""presentation"" style=""mso-table-lspace:0;mso-table-rspace:0""><tbody><tr><td><table class=""row-content stack"" align=""center"" border=""0"" cellpadding=""0"" cellspacing=""0"" role=""presentation"" style=""mso-table-lspace:0;mso-table-rspace:0;color:#000;width:640px"" width=""640""><tbody><tr><td class=""column column-1"" width=""100%"" style=""mso-table-lspace:0;mso-table-rspace:0;font-weight:400;text-align:left;padding-bottom:5px;padding-top:5px;vertical-align:top;border-top:0;border-right:0;border-bottom:0;border-left:0""><table class=""icons_block block-1"" width=""100%"" border=""0"" cellpadding=""0"" cellspacing=""0"" role=""presentation"" style=""mso-table-lspace:0;mso-table-rspace:0""><tr><td class=""pad"" style=""vertical-align:middle;color:#9d9d9d;font-family:inherit;font-size:15px;padding-bottom:5px;padding-top:5px;text-align:center""><table width=""100%"" cellpadding=""0"" cellspacing=""0"" role=""presentation"" style=""mso-table-lspace:0;mso-table-rspace:0""><tr><td class=""alignment"" style=""vertical-align:middle;text-align:center""><!--[if vml]><table align=""left"" cellpadding=""0"" cellspacing=""0"" role=""presentation"" style=""display:inline-block;padding-left:0;padding-right:0;mso-table-lspace:0;mso-table-rspace:0""><![endif]--><!--[if !vml]><!--><table class=""icons-inner"" style=""mso-table-lspace:0;mso-table-rspace:0;display:inline-block;margin-right:-4px;padding-left:0;padding-right:0"" cellpadding=""0"" cellspacing=""0"" role=""presentation""><!--<![endif]--><tr><td style=""vertical-align:middle;text-align:center;padding-top:5px;padding-bottom:5px;padding-left:5px;padding-right:6px""><a href=""https://www.designedwithbee.com/"" target=""_blank"" style=""text-decoration:none""><img class=""icon"" alt=""Designed with BEE"" src=""https://d15k2d11r6t6rl.cloudfront.net/public/users/Integrators/BeeProAgency/53601_510656/Signature/bee.png"" height=""32"" width=""34"" align=""center"" style=""display:block;height:auto;margin:0 auto;border:0""></a></td><td style=""font-family:Poppins,Arial,Helvetica,sans-serif;font-size:15px;color:#9d9d9d;vertical-align:middle;letter-spacing:undefined;text-align:center""><a href=""https://www.designedwithbee.com/"" target=""_blank"" style=""color:#9d9d9d;text-decoration:none"">Designed with BEE</a></td></tr></table></td></tr></table></td></tr></table></td></tr></tbody></table></td></tr></tbody></table></td></tr></tbody></table></body></html>";
                var success = await emailHelper.SendEmailAsync(userEmail + "@cbg.com.gh", "CBG CARBON FOOTPRINT PORTAL", message);
                TempData["success"] = "Form Succesfully Returned";
            }
            else if (status == "update")
            {


                formStatus.Status = FormStatus.Pending;


                _db.FootprintTable.Update(formStatus);
                _db.SaveChanges();
                TempData["success"] = "Form Succesfully Updated";
            }


            return RedirectToAction("AllFootprint", "Sidebar");
        }

    }



    internal class HttpStatusCodeResult : ActionResult
    {
        private HttpStatusCode badRequest;

        public HttpStatusCodeResult(HttpStatusCode badRequest)
        {
            this.badRequest = badRequest;
        }
    }
}



