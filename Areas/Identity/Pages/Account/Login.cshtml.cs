// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;
using Shyjus.BrowserDetection;
using CarbonFootprint1.Data;
using CarbonFootprint1.Methods;

namespace CarbonFootprint1.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly IBrowserDetector _browserDetector;
        private readonly ApplicationDbContext? _db;
        private readonly ErrorLogs _errorLogs;
        

        public LoginModel(
            SignInManager<IdentityUser> signInManager,
            ILogger<LoginModel> logger,
            UserManager<IdentityUser> userManager,
            ApplicationDbContext db,
            IBrowserDetector browserDetector,
            ErrorLogs errorLogs

            )
        {
            _signInManager = signInManager;
            _logger = logger;
            _userManager = userManager;
            _browserDetector = browserDetector;
            _db = db;
            _errorLogs = errorLogs;
        }

    
        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

       
        public string ReturnUrl { get; set; }

       
        [TempData]
        public string ErrorMessage { get; set; }

     
        public class InputModel
        {
        
            [Required]
            //[EmailAddress]
            public string Username { get; set; }

         
            [Required]
            public string Password { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
           
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            try {
                _errorLogs.LogUserActivity(Input.Username, "Passed first try");

                returnUrl ??= Url.Content("~/");

                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();


                if (ModelState.IsValid)
                {
                    var model = new InputModel();
                    model.Username = Input.Username;
                    model.Password = Input.Password;
                    IdentityUser userlog = null;

                    try

                    {

                        userlog = await _userManager.FindByNameAsync(Input.Username);
                        
                        //var roles = await _userManager.GetRolesAsync(userlog);
                    }
                    catch (Exception ex)
                    {
                        _errorLogs.LogExceptions(ex, "User not assigned to role, or does not exist");
                        TempData["error"] = "User does not exist, contact IT support";
                        return RedirectToAction("Index", "Home");

                    }

                    string payloadString = JsonConvert.SerializeObject(model);
                    HttpResponseMessage message = null;
                    HttpContent c = new StringContent(payloadString, Encoding.UTF8, "application/json");

                    if (userlog == null)
                    {
                        TempData["error"] = "User does not exist, contact IT support";
                        _errorLogs.LogResponse("Error", "User does not exist in the database", "Login", Input.Username, "Carbon FP");
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        try
                        {
                            var user = _db.ApplicationUserTable.Where(u => u.UserName == Input.Username).FirstOrDefault();
                            _errorLogs.LogUserActivity(userlog.UserName, $"user object{user}");
                            if (user.Status == "Active")
                            {
                                _errorLogs.LogUserActivity(userlog.UserName, $"active user{user}");

                                try
                                {
                                    using (var client = new HttpClient())
                                    {
                                        try
                                        {
                                            client.BaseAddress = new Uri(Data.EndPoints.Constant.endPoint);
                                            message = await client.PostAsync($"AD_Authentication", c);

                                            _errorLogs.LogUserActivity(userlog.UserName, $"https message {message}");

                                        }
                                        catch (Exception ex)
                                        {
                                            ModelState.AddModelError(string.Empty, ex.Message);
                                        }

                                        try
                                        {
                                            if (message.IsSuccessStatusCode)
                                            {
                                                _errorLogs.LogUserActivity(userlog.UserName, $"IsSuccessStatusCode {message}");
                                                var T = message.Content.ReadAsStringAsync().Result;
                                                if (T == "Success")
                                                {

                                                    var test = (_signInManager == null);

                                                    await _signInManager.SignInAsync(userlog, isPersistent: false, null);
                                                    var browserData = _browserDetector.Browser;
                                                    var browserinfoNow = browserData.Name + browserData.Version + browserData.DeviceType + browserData.OS;
                                                    HttpContext.Session.SetString("browserInfo", browserinfoNow);
                                                    HttpContext.Session.SetString("isLoggedIn", "YES");

                                                    _errorLogs.LogUserActivity(userlog.UserName, "User Login Successful");
                                                    return LocalRedirect(returnUrl);
                                                }
                                                else if (T == "Invalid UserName or Password" || T == "INVALID USER NAME")
                                                {
                                                    TempData["error"] = "Invalid Username or Password";
                                                    _errorLogs.LogUserActivity(userlog.UserName, "User login unsuccesful");
                                                    return RedirectToAction("Index", "Home");
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            _errorLogs.LogExceptions(ex, "Incorrect credentials");
                                            ModelState.AddModelError(string.Empty, ex.Message);
                                        }

                                    }
                                }
                                catch (Exception ex)
                                {
                                    _errorLogs.LogExceptions(ex, "HTTP message not available");
                                }
                            }
                            else
                            {
                                _errorLogs.LogUserActivity(userlog.UserName, "User account has been disabled");
                                TempData["error"] = "User has been disabled";
                                return RedirectToAction("Index", "Home");


                            }
                        }
                        catch (Exception ex)
                        {
                            _errorLogs.LogExceptions(ex, $"user object not found");
                        }

                    }
                }

                else
                {
                    TempData["error"] = "An Error Occurred";
                    _errorLogs.LogUserActivity(Input.Username, "ModelState Invalid");
                    return RedirectToAction("Index", "Home");
                }

                // If we got this far, something failed, redisplay form
                TempData["error"] = "An Error Occurred";
                _errorLogs.LogUserActivity(Input.Username, "If statement not hit");
                return RedirectToAction("ErrorPage", "User");
            }
            
            catch(Exception ex)
            {

                _errorLogs.LogExceptions(ex, "");
                return null;
            }
            
            
        }
    }
}
