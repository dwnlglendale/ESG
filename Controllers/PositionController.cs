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
    public class PositionController : Controller
    {

        private readonly ApplicationDbContext? _db;
        private readonly UserManager<IdentityUser>? _userManager;
        private readonly SignInManager<IdentityUser>? _signInManager;
        private readonly RoleManager<IdentityRole>? _roleManager;
        private readonly ISessionManagerService _sessionManagerService;
        private readonly ErrorLogs _errorLogs;

        public PositionController(

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

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> PositionManagement()
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
                IEnumerable<Positions> getPositions = _db.PositionsTable.ToList();
                return View(getPositions);
            }
            catch (Exception ex)
            {
                return View(ex);
            }
        }

        //GET POSITION METHOD
        [Authorize(Roles = "Administrator")]
        public IActionResult CreatePosition()
        {
            PositionVM position = new()
            {
                PositionInput = new(),
                RoleList = _roleManager.Roles.Select(x => x.Name).Select(i => new SelectListItem
                {
                    Text = i,
                    Value = i
                }).ToList()
            };

            return View(position);
        }

        //POST METHOD
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> CreatePosition(PositionVM objPostion)
        {
            try
            {//Check is session is active
                var sessionActiveState = _sessionManagerService.CheckBrowserSession();
                if (sessionActiveState == false)
                {
                    await _signInManager.SignOutAsync();
                    return RedirectToAction("Index", "Home");
                }

                if (ModelState.IsValid)
                {
                    //Get list of positions in Db
                    var isPositionAva = _db.PositionsTable.Where(p => p.Position_Name == objPostion.PositionInput.Position_Name).FirstOrDefault();
                    if (isPositionAva == null)
                    {
                        Positions popo = new Positions();
                        popo.Position_Name = objPostion.PositionInput.Position_Name;
                        popo.Position_Description = objPostion.PositionInput.Position_Description;
                        popo.DateCreated = DateTime.Now;
                        popo.CreatedBy = User.Identity?.Name;
                        popo.Position_Status = "Active";
                        _db.PositionsTable.Add(popo);

                        var rolesForPosition = objPostion.RoleList.Where(x => x.Selected).Select(y => y.Value).ToList();

                        foreach (var role in rolesForPosition)
                        {
                            PositionRoles pos = new PositionRoles();
                            pos.Postion = objPostion.PositionInput.Position_Name;
                            pos.Roles = role;
                            _db.PositionRolesTable.Add(pos);
                        }

                        //Save changes
                        _db.SaveChanges();
                        TempData["success"] = "User Role created successfully";
                        return RedirectToAction("PositionManagement");
                    }
                    else
                    {
                        TempData["error"] = "Sorry Position already exists";
                        return RedirectToAction("PositionManagement");
                    }
                }
                return View(objPostion);
            }
            catch (Exception ex)
            {
                _errorLogs.LogExceptions(ex, "User not Created");
                return View();
            }
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
        private Positions CreatePositions()
        {
            try
            {
                return Activator.CreateInstance<Positions>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        //GET METHOD FOR EDITING POSITION
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> EditPosition(int? id)
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

            //Find Position in Position table
            var positionInDb = _db.PositionsTable.Where(p => p.Position_Id == id).FirstOrDefault();

            if (positionInDb == null)
            {
                return NotFound();
            }


            var model = new PositionInputModel
            {
                Position_ID = positionInDb.Position_Id,
                Position_Name = positionInDb.Position_Name,
                Position_Description = positionInDb.Position_Description
            };

            //Find roles assigned to Posiion
            var positionRole = _db.PositionRolesTable.Where(p => p.Postion == positionInDb.Position_Name).Select(x => x.Roles).ToList();
            var item = await _roleManager.Roles.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Name,
                Selected = positionRole.Contains(x.Name)
            }).ToListAsync();

            PositionVM position = new PositionVM()
            {
                RoleList = item,
                PositionInput = model
            };

            return View(position);
        }
        //POST METHOD
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> EditPosition(PositionVM editObj)
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

                if (ModelState.IsValid)
                {
                    //var positionRoles = _db.PositionRoleTable.Where(p => p.Position == editObj.PositionInput.Position_Name);
                    List<PositionRoles> positionlist = _db.PositionRolesTable.Where(p => p.Postion == editObj.PositionInput.Position_Name).ToList();
                    List<string> rolesInSystem = new List<string>();
                    List<string> assignedRolesToPosition = new List<string>();

                    foreach (var obj in _roleManager.Roles.ToList())
                    {
                        rolesInSystem.Add(obj.Name);
                    }
                    foreach (var obj in positionlist)
                    {
                        assignedRolesToPosition.Add(obj.Roles);
                    }

                    var rolesToAdd = new List<string>();
                    var rolesToRemove = new List<string>();

                    foreach (var role in editObj.RoleList)
                    {
                        var rolesForPosition = assignedRolesToPosition.FirstOrDefault(x => x == role.Text);
                        if (role.Selected)
                        {
                            var r = _db.Roles.Where(m => m.Id == role.Text);
                            if (rolesForPosition == null)
                            {
                                rolesToAdd.Add(role.Text);
                            }
                        }
                        else
                        {
                            if (rolesForPosition != null)
                            {
                                rolesToRemove.Add(role.Text);
                            }
                        }
                    }

                    if (rolesToAdd.Any())
                    {
                        //Add new roles to the position
                        foreach (var role in rolesToAdd)
                        {
                            PositionRoles pos = new PositionRoles();
                            pos.Postion = editObj.PositionInput.Position_Name;
                            pos.Roles = role;
                            _db.PositionRolesTable.Add(pos);

                            //Add roles to the users in that position
                            foreach (var user in _db.ApplicationUserTable.Where(p => p.UserPosition == editObj.PositionInput.Position_Name).ToList())
                            {
                                await _userManager.AddToRoleAsync(user, role);
                            }
                        }
                    }

                    if (rolesToRemove.Any())
                    {
                        //Remove new roles to the users
                        foreach (var role in rolesToRemove)
                        {
                            //Find the position with name and value to remove from db
                            var pos = _db.PositionRolesTable.Where(p => p.Postion == editObj.PositionInput.Position_Name && p.Roles == role).FirstOrDefault();
                            _db.PositionRolesTable.Remove(pos);

                            //Remove roles to the users in that position
                            foreach (var user in _db.ApplicationUserTable.Where(p => p.UserPosition == editObj.PositionInput.Position_Name).ToList())
                            {
                                await _userManager.RemoveFromRoleAsync(user, role);
                            }
                        }
                    }

                    var posi = _db.PositionsTable.Where(p => p.Position_Id == editObj.PositionInput.Position_ID).FirstOrDefault();
                    posi.Position_Name = editObj.PositionInput.Position_Name;
                    posi.Position_Description = editObj.PositionInput.Position_Description;

                    _db.PositionsTable.Update(posi);

                    _db.SaveChanges();
                    _errorLogs.LogUserActivity(User.Identity!.Name!, "Role Updated Successfully");
                    TempData["success"] = "Position edited successfully";
                    return RedirectToAction("PositionManagement");
                }

                else
                {
                    TempData["error"] = "Role name and description should be letters only";
                    return RedirectToAction("PositionManagement");
                }
            }
        catch (Exception ex)
            {
                _errorLogs.LogExceptions(ex, "Role Update was unsuccessful");
                TempData["error"] = "Role was not updated";
                return RedirectToAction("Index", "Home");
            }
       
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeletePosition(int? id)
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

            //Find Position in Position table
            var positionInDb = _db.PositionsTable.Where(p => p.Position_Id == id).FirstOrDefault();

            if (positionInDb == null)
            {
                return NotFound();
            }

            var model = new PositionInputModel
            {
                Position_ID = positionInDb.Position_Id,
                Position_Name = positionInDb.Position_Name,
                Position_Description = positionInDb.Position_Description
            };

            var positionRole = _db.PositionRolesTable.Where(p => p.Postion == positionInDb.Position_Name).Select(x => x.Roles).ToList();
            var item = await _roleManager.Roles.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Name,
                Selected = positionRole.Contains(x.Name)
            }).ToListAsync();

            PositionVM position = new PositionVM()
            {
                RoleList = item,
                PositionInput = model
            };

            return View(position);
        }
    }
}
