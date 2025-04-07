using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Staffly.DAL.Dtos;
using Staffly.PL.Helpers;

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

        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordDto forgetPasswordDto)
        {
            if (ModelState.IsValid) {

                var user = await _userManager.FindByEmailAsync(forgetPasswordDto.Email);
                if (user is not null)
                {
                    // Generate Token
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                    // Create URL
                    var URL = Url.Action("ResetPassword", "Auth", new { email = forgetPasswordDto.Email, token }, Request.Scheme);

                    // Send Email
                    var flag = EmailSettings.SendEmail(new Email
                    {
                        To = forgetPasswordDto.Email,
                        Subject = "Forget Password",
                        Body = URL
                    });

                    if (flag)
                    {
                        return RedirectToAction("CheckYourInbox", "Auth");
                    }

                }
                ModelState.AddModelError("", "Invalid Email!");
            }

            return View(forgetPasswordDto);
        }

        [HttpGet]
        public IActionResult CheckYourInbox()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ResetPassword(string email,string token)
        {
            TempData["email"] = email;
            TempData["token"] = token;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            if (ModelState.IsValid)
            {
                var email = TempData["email"]?.ToString();
                var token = TempData["token"]?.ToString();
                if (email is not null && token is not null)
                {
                    var user = await _userManager.FindByEmailAsync(email);
                    if (user is not null)
                    {
                        var result = await _userManager.ResetPasswordAsync(user, token, resetPasswordDto.NewPassword);
                        if (result.Succeeded)
                        {
                            return RedirectToAction("SignIn", "Auth");
                        }
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
            }
            
            return View(resetPasswordDto);
        }

    }
}
