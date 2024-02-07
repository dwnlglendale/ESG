using CarbonFootprint1.Areas.Identity.Pages.Account;
using CarbonFootprint1.Data;
using CarbonFootprint1.Methods;
using CarbonFootprint1.Models;
using CarbonFootprint1.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CarbonFootprint1.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ISessionManagerService _sessionManagerService;
        private readonly ErrorLogs _errorLogs;

        public UserController(

            ApplicationDbContext db,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IUserStore<IdentityUser> userStore,
            ILogger<RegisterModel> logger,
            RoleManager<IdentityRole> roleManager,
            ISessionManagerService sessionManagerService
,           ErrorLogs errorLogs
            )
        {
            _db = db;
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _roleManager = roleManager;
            _sessionManagerService = sessionManagerService;
            _errorLogs = errorLogs;

        }


        [Authorize(Roles = "Administrator, Approver")]
        public async Task<IActionResult> UserManagement()
        {
            try {
                //Check is session is active
                var sessionActiveState = _sessionManagerService.CheckBrowserSession();
                if (sessionActiveState == false)
                {
                    await _signInManager.SignOutAsync();
                    return RedirectToAction("Index", "Home");
                }

                IEnumerable<ApplicationUser> allUsers = _db.ApplicationUserTable.ToList();
                return View(allUsers);
            }
            catch (Exception ex)
            {
                _errorLogs.LogExceptions(ex, "Error Showing User Management Page");
                return View();

            }
           
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> AddNewUser()
        {
            try {
                //Check is session is active
                var sessionActiveState = _sessionManagerService.CheckBrowserSession();
                if (sessionActiveState == false)
                {
                    await _signInManager.SignOutAsync();
                    return RedirectToAction("Index", "Home");
                }

                UserVM userVM = new()
                {
                    UserInput = new(),
                    RoleList = _db.PositionsTable.Where(x => x.Position_Status == "Active").Select(x => x.Position_Name).Select(i => new SelectListItem
                    {
                        Text = i,
                        Value = i
                    }).ToList()
                };


                ViewBag.branchName = _db.BranchesTable.Select(x => x.BranchName).Select(i => new SelectListItem
                {
                    Text = i,
                    Value = i
                }).ToList();

                ViewBag.departmentName = _db.DepartmentsTable.Select(x => x.DepartmentName).Select(i => new SelectListItem
                {
                    Text = i,
                    Value = i
                }).ToList();

                ViewBag.branchSize = _db.BranchesTable.Select(x => x.BranchSize).Select(i => new SelectListItem
                {
                    Text = i,
                    Value = i
                }).ToList();


                ViewBag.positions = _db.PositionsTable.Select(x => x.Position_Name).Select(i => new SelectListItem
                {
                    Text = i,
                    Value = i
                }).ToList();

                return View(userVM);
            }
            catch (Exception ex)
            {
                _errorLogs.LogExceptions(ex, "User creation page not found");
                return RedirectToAction("UserManagement");
            }
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> AddNewUser(UserVM userObj, string id)
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

                
                var user = CreateUser();

                await _userStore.SetUserNameAsync(user, userObj.UserInput.UserName, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, userObj.UserInput.UserName, CancellationToken.None);

                user.FullName = userObj.UserInput.UserName;
                user.CreatedBy = User.Identity.Name;
                user.DateCreated = DateTime.Now;
                user.StaffNumber = userObj.UserInput.StaffID;
                user.UserPosition = userObj.UserInput.UserPosition;
                if (userObj.UserInput.BDSelection == "Branch")
                {
                    user.BranchName = userObj.UserInput.Branch;
                    user.BranchCode = _db.BranchesTable.Where(b => b.BranchName == userObj.UserInput.Branch).FirstOrDefault().BraanchCode;  //userObj.UserInput.BranchCode;
                    user.BranchSize = _db.BranchesTable.Where(b => b.BranchName == userObj.UserInput.Branch).FirstOrDefault().BranchSize; //userObj.UserInput.BranchSize;
                }
                else
                {
                    user.Department = userObj.UserInput.Department;
                }


                user.Status = userObj.UserInput.Status;

                //user.BranchID = userObj.UserInput.Branch;

                var result = await _userManager.CreateAsync(user);

                if (result.Succeeded)
                {
                    var roles = _db.PositionRolesTable.Where(p => p.Postion == userObj.UserInput.UserPosition).ToList();
                    string role = "";

                    foreach (var roleUser in roles)
                    {
                        role = roleUser.Roles;
                        await _userManager.AddToRoleAsync(user, role);

                    }
                }
                TempData["success"] = "User Created Successfully";
                _errorLogs.LogUserActivity(User.Identity.Name!, "User Created Successfully");
                return RedirectToAction("UserManagement");
            }
            catch (Exception ex)
            {
                _errorLogs.LogExceptions(ex, "User not created");
                return RedirectToAction("UserManagement");
            }
        }
        [Authorize(Roles = "Administrator")]
        private ApplicationUser CreateUser()
        {

            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch (Exception ex)
            {
                _errorLogs.LogExceptions(ex, "Could not create user");
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }


        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteUser(string? id)
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

            var userInDb = _db.ApplicationUserTable.Where(u => u.Id == id).FirstOrDefault();
            if (userInDb == null)
            {
                return NotFound();
            }

            _db.ApplicationUserTable.Remove(userInDb);
            _db.SaveChanges();
            TempData["success"] = "Succesfully Deleted";



            return RedirectToAction("Usermanagement");
        }


        //GET METHOD FOR EDITING USER
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> EditUser(string? id)
        {
            try {
                //Check Session
                var sessionActiveState = _sessionManagerService.CheckBrowserSession();
                if (sessionActiveState == false)
                {
                    await _signInManager.SignOutAsync();
                    return RedirectToAction("Index", "Home");
                }

                //Get user details from DB
                var userData = await _userManager.FindByIdAsync(id);
                var userRole = await _userManager.GetRolesAsync(userData);
                var appUser = _db.ApplicationUserTable.Find(id);
                var userBranch = appUser.BranchName;
                var userDepartment = appUser.Department;
                var userStatus = appUser.Status;
                if (userData == null)
                {
                    return NotFound();
                }

                var model = new UserInputModel
                {
                    UserName = userData.UserName,
                    Branch = userBranch,
                    Department = userDepartment,
                    UserPosition = appUser.UserPosition,
                    BDSelection = appUser.BDSelection,

                };

                // Get all Roles 
                var user = _db.ApplicationUserTable.Where(u => u.Id == id).FirstOrDefault();
                var userRoles = _db.UserRoles.Where(u => u.UserId == id).Select(x => x.RoleId).ToList();
                var item = await _roleManager.Roles.Select(x => new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id,
                    Selected = userRoles.Contains(x.Id)
                }).ToListAsync();

                //Get all Positions
                ViewBag.userPosition = _db.PositionsTable.Where(x => x.Position_Status == "Active").Select(x => x.Position_Name).Select(i => new SelectListItem()
                {
                    Text = i,
                    Value = i,
                }).ToList();

                UserVM userVM = new UserVM()
                {
                    UserInput = model,
                    RoleList = item,
                    AppUser = (ApplicationUser)userData,
                };

                
                //Get branch and dept list 
                ViewBag.branchName = _db.BranchesTable.Select(x => x.BranchName).Select(i => new SelectListItem
                {
                    Text = i,
                    Value = i
                }).ToList();

                ViewBag.departmentName = _db.DepartmentsTable.Select(x => x.DepartmentName).Select(i => new SelectListItem
                {
                    Text = i,
                    Value = i
                }).ToList();

                
                return View(userVM);
            }
            catch (Exception ex)
            {
                _errorLogs.LogExceptions(ex, "User was not edited successfully");
                return View();
            }
        }

        //POST METHOD FOR EDITING USER
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize (Roles = "Administrator")]
        public async Task <IActionResult> EditUser(UserVM editUserObj)
        {
            try
            {
                //Check if Session is still active 
                var sessionActiveState = _sessionManagerService.CheckBrowserSession();
                if (sessionActiveState == false)
                {
                    await _signInManager.SignOutAsync();
                    return RedirectToAction("Index", "Home");
                }

                var userData = await _userManager.FindByNameAsync(editUserObj.UserInput.UserName);
                var AppUserData = _db.ApplicationUserTable.FirstOrDefault(u => u.UserName == editUserObj.UserInput.UserName);


                if (AppUserData == null)
                {
                    TempData["error"] = "Username must not change";
                    return RedirectToAction("EditUser", "User");
                }


                //Remove Roles
                var roles = await _userManager.GetRolesAsync(userData);
                foreach (var role in roles)
                {
                    await _userManager.RemoveFromRoleAsync(userData, role);
                }
                //Add new roles
                var newRoles = _db.PositionRolesTable.Where(p => p.Postion == editUserObj.UserInput.UserPosition).ToList();
                string newAddRole = "";
                foreach (var roleUser in newRoles)
                {
                    newAddRole = roleUser.Roles;
                    await _userManager.AddToRoleAsync(userData, newAddRole);
                }

                //Update the user roles
                var newAppuserData = AppUserData;
                newAppuserData.BDSelection = editUserObj.UserInput.BDSelection;
                newAppuserData.UserPosition = editUserObj.UserInput.UserPosition;

                if (editUserObj.UserInput.BDSelection == "Branch")
                {
                    //find branch name 
                    var userBranch = _db.BranchesTable.Where(b => b.BranchName == editUserObj.UserInput.Branch).FirstOrDefault();
                    newAppuserData.BranchName = userBranch.BranchName;
                    newAppuserData.BranchSize = userBranch.BranchSize;
                    newAppuserData.BranchCode = userBranch.BraanchCode;
                    newAppuserData.Status = editUserObj.UserInput.Status;
                }
                else
                {
                    newAppuserData.Department = editUserObj.UserInput.Department;
                    var userBranch = _db.BranchesTable.Where(b => b.BranchName == editUserObj.UserInput.Branch).FirstOrDefault();

                    newAppuserData.BranchName = null;
                    newAppuserData.BranchSize = null;
                    newAppuserData.BranchCode = null;
                    newAppuserData.Status = editUserObj.UserInput.Status;


                }
                await _userManager.UpdateAsync(newAppuserData);
                TempData["success"] = "Succesfully updated user!";
                _errorLogs.LogUserActivity(User.Identity!.Name!, "User Updated Successfully");
                return RedirectToAction("userManagement");
            }
            catch (Exception ex)
            {
                _errorLogs.LogExceptions(ex, "User Update was unsuccessful");
                TempData["error"] = "User update was not successful";
                return RedirectToAction("UserManagement");
            }
        }

        public IActionResult ErrorPage()
        {
            return View();
        }

        //[Authorize(Roles = "Admin")]
        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<IdentityUser>)_userStore;
        }
    }
}
