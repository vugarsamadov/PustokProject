using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using PustokProject.CoreModels;
using PustokProject.Migrations;
using PustokProject.Persistance;
using PustokProject.ViewModels.Auth;
using PustokProject.ViewModels.Profile;

namespace PustokProject.Controllers
{
    public class AuthController : Controller
    {
		private readonly SignInManager<ApplicationUser> signInManager;
		private readonly UserManager<ApplicationUser> userManager;
		private readonly RoleManager<IdentityRole> roleManager;

		public ApplicationDbContext _dbContext { get; }

		public AuthController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext, RoleManager<IdentityRole> roleManager)
		{
			this.signInManager = signInManager;
			this.userManager = userManager;
			_dbContext = dbContext;
			this.roleManager = roleManager;
		}

		public IActionResult Login()
        {
            return View();
        }
		public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(VM_Register registermodel)
        {
			if (!ModelState.IsValid)
			{
				return View(registermodel);
			}
			var user = new ApplicationUser()
            {
                FullName = registermodel.FullName,
                Email = registermodel.Email,
                UserName = registermodel.Username
            };

			var result = await userManager.CreateAsync(user
                ,registermodel.Password);

            if(!result.Succeeded)
            {				
				result.Errors.Select(a=>a.Description).ToList().ForEach(a=> ModelState.AddModelError("",a));
				return View(registermodel);

			}

			var roleResult = await userManager.AddToRoleAsync(user,Roles.Member.ToString());
            if (!roleResult.Succeeded)
                    {
				result.Errors.Select(a => a.Description).ToList().ForEach(a => ModelState.AddModelError("", a));
				return View(registermodel);
			}

			return RedirectToAction(nameof(Login)); 
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromQuery]string? returnUrl,VM_Login loginModel)
        {
			if (!ModelState.IsValid)
			{
				return View(nameof(Login));
			}
			var user = await userManager.FindByEmailAsync(loginModel.Email);
            if (user != null)
            {   
                var result = await signInManager.PasswordSignInAsync(user,
                loginModel.Password, false, lockoutOnFailure: false);

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Email or password is wrong!");
					return View(loginModel);
				}
			}
            else
            {
                ModelState.AddModelError("", "Email or password is wrong!");
            return View(loginModel);
            }
			if (returnUrl != null && Url.IsLocalUrl(returnUrl))
			{
				return Redirect(returnUrl);
			}
			return RedirectToAction("Index","Home");
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
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            VM_UserProfile model=new();
            if(user != null)
            {
                model = new VM_UserProfile()
                {
                    FirstName = user.FullName.Split(" ")[0],
                    LastName= user.FullName.Split(" ")[1],
                    DisplayName = user.UserName,
                    Email = user.Email,
                    ProfileImageUrl = user.ProfileImageUrl
                };
            }

            return View(model);
        }

        [HttpPost]
		public async Task<IActionResult> Profile(VM_UserProfile model)
		{
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            if(User.Identity != null)
            {
    			var user = await userManager.FindByNameAsync(User.Identity.Name);
				user.FullName = model.FirstName+" "+model.LastName;
                user.Email = model.Email;
                user.UserName = model.DisplayName;
                if(model.ProfileImage != null)
                {
                    var imagepath= await model.ProfileImage.SaveToRootWithUniqueNameAsync(true);
                    user.ProfileImageUrl = imagepath;
                }
				var result = await userManager.UpdateAsync(user);
				if (model.CurrentPassword != null)
				{
					await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
				}

			}
			return RedirectToAction(nameof(Profile));
		}

        [HttpGet]
        public async Task<bool> CreateRoles()
        {
            foreach(var item in Enum.GetValues(typeof(Roles)))
            {
                if(!await roleManager.RoleExistsAsync(item.ToString()))
                {
                    var r = await roleManager.CreateAsync(new IdentityRole
                    {
                        Name = item.ToString()
					});

                    if(!r.Succeeded)
                    {
                        return false;
                    }

                }
            }
                return true;
        }

        public string AccessDenied()
        {
            return "Access Denied";
        }


	}
}
