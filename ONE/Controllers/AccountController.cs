using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using ONE.DTOs;
using ONE.Models;
using System.Security.Claims;

namespace ONE.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            this.signInManager = signInManager;
        }
        public async Task<IActionResult> Profile()
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login");
            }
            return View(user);
        }
            [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveRegister(RegisterDTOs registerDTOs )
        {
            if (ModelState.IsValid)
            
            {

                //Mapping RegisterDTOs to ApplicationUser
                ApplicationUser user = new ApplicationUser
                {
                    UserName = registerDTOs.UserName,
                    Email = registerDTOs.Email,
                    PasswordHash = registerDTOs.Password
                };
                if(registerDTOs.PictureUser.Length>0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await registerDTOs.PictureUser.CopyToAsync(memoryStream);
                        user.PictureSource = memoryStream.ToArray();
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Please upload a picture");
                    return View("Register", registerDTOs);
                }
                //save user to database
                IdentityResult result = await _userManager.CreateAsync(user,registerDTOs.Password);
                if (result.Succeeded)
                {
                    //Role
                    await _userManager.AddToRoleAsync(user, "User");
                    if (User.Identity.AuthenticationType=="Admin")
                    {
    

                        return RedirectToAction("Index", "Video");
                    }
                    await signInManager.SignInAsync(user,false);
                    return RedirectToAction("Index", "Home");

                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("",item.Description);

                }
            }
            return View("Register",registerDTOs);
        }
        [Authorize]
        public async Task<IActionResult> SiginOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> Login()
        {
            return View("Login");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveLogin(LoginDTOs loginDTOs)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser applicationUser= await _userManager.FindByNameAsync(loginDTOs.UserName);
                if(applicationUser == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
                    return View("Login", loginDTOs);
                }
                bool found= await _userManager.CheckPasswordAsync(applicationUser, loginDTOs.Password);
                if (found)
                {
                    List<Claim> claims = new List<Claim>();
                   
                    await signInManager.SignInWithClaimsAsync(applicationUser,loginDTOs.RememberMe,claims);
                    //await signInManager.SignInAsync(applicationUser, loginDTOs.RememberMe);
                    return RedirectToAction("Index", "Video");
                }
                var result = await signInManager.PasswordSignInAsync(loginDTOs.UserName, loginDTOs.Password, false, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Video");
                }
                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            }
            return View("Login", loginDTOs);
        }
    }
}
