using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ONE.DTOs;

namespace ONE.Controllers
{

    // [Authorize (Roles ="Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager) {
            this.roleManager = roleManager;
        }
        public IActionResult Index()
        {
            return View("Index");
        }
        [HttpPost]
        public async Task<IActionResult> SaveRole(RoleDTOs roleDTOs)
        {
            if (ModelState.IsValid)
            {
                IdentityRole role = new IdentityRole();
                role.Name= roleDTOs.RoleName;
                IdentityResult result = await roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    ViewBag.success = true;
                    return View("Index");
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
            return View("Index",roleDTOs);
        }

    }
}
