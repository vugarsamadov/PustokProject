using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using PustokProject.CoreModels;
using PustokProject.ViewModels.Auth;

namespace PustokProject.Controllers
{
    public class AuthController : Controller
    {
		private readonly SignInManager<User> signInManager;
		private readonly UserManager<User> userManager;

		public AuthController(SignInManager<User> signInManager, UserManager<User> userManager)
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
                new User()
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
    }
}
