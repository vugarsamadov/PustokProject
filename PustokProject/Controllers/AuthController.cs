using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using PustokProject.CoreModels;
using PustokProject.Migrations;
using PustokProject.ViewModels.Auth;

namespace PustokProject.Controllers
{
    public class AuthController : Controller
    {
		private readonly SignInManager<ApplicationUser> signInManager;
		private readonly UserManager<ApplicationUser> userManager;

		public AuthController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
			this.signInManager = signInManager;
			this.userManager = userManager;
		}

        public IActionResult LoginRegisterIndex()
        {
			if (TempData["Errors"] != null)
			{
				var errors = (IEnumerable<string>)TempData["Errors"];
                foreach (var error in errors)
                {
                    ModelState.AddModelError("", error);
                }
            }
			var a = ModelState;
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(VM_Register registermodel)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            var result = await userManager.CreateAsync(
                new ApplicationUser()
                {
                    FullName = registermodel.FullName,
                    Email = registermodel.Email,
                    UserName = registermodel.Username
                },registermodel.Password);

            if(!result.Succeeded)
            {				
				TempData["Errors"] = result.Errors.Select(a=>a.Description).ToList();
            }
            return RedirectToAction(nameof(LoginRegisterIndex)); 
        }

        [HttpPost]
        public async Task<IActionResult> Login(VM_Login loginModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var user = await userManager.FindByEmailAsync(loginModel.Email);
            if (user != null)
            {   
                var result = await signInManager.PasswordSignInAsync(user,
                loginModel.Password, false, lockoutOnFailure: false);

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("","Something went wrong...");
                    return View("LoginRegisterIndex");
                }
            }
            ModelState.AddModelError("", "Something went wrong...");
            return View("LoginRegisterIndex");
            return RedirectToAction(nameof(LoginRegisterIndex));
        }


        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            if (User.Identity == null)
            {
                return NotFound();
            }
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            return View();
        }

    }
}
