using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Staffly.DAL.Dtos;

namespace Staffly.PL.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthController(UserManager<ApplicationUser> userManager,IMapper mapper,SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            this._mapper = mapper;
            this._signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpDto signUpDto)
        {
            if (ModelState.IsValid)
            {
                // Check if the user already exists

                var user = await _userManager.FindByNameAsync(signUpDto.UserName);

                if (user is null)
                {
                    user = await _userManager.FindByEmailAsync(signUpDto.Email);
                    if (user is null)
                    {
                        user = _mapper.Map<ApplicationUser>(signUpDto);
                        var result = await _userManager.CreateAsync(user, signUpDto.Password);
                        if (result.Succeeded)
                        {
                            return RedirectToAction("SignIn","Auth");
                        }
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
                
                ModelState.AddModelError("", "Invalid SignUp!");

            }

            return View(signUpDto);
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInDto signInDto)
        {

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(signInDto.Email);
                if (user is not null)
                {
                    var password = await _userManager.CheckPasswordAsync(user, signInDto.Password);
                    if (password)
                    {
                        // Sign in the user
                        var result = await _signInManager.PasswordSignInAsync(user, signInDto.Password,signInDto.RememberMe,false);
                        if (result.Succeeded)
                        {
                            // Redirect to the home page or any other page
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
                ModelState.AddModelError("", "Invalid Email Or Password!");
            }
            return View(signInDto);

        }

        [HttpGet]
        public new async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("SignIn", "Auth");
        }
    }
}
