using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Staffly.DAL.Dtos;
using Staffly.DAL.Models;

namespace Staffly.PL.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public UserController(UserManager<ApplicationUser> userManager,IMapper mapper)
        {
            this._userManager = userManager;
            this._mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string? SearchInput)
        {
            IEnumerable<UserToReturnDto> users;
            if (string.IsNullOrEmpty(SearchInput))
            {
                users = _userManager.Users.Select(U => new UserToReturnDto
                {
                    Id = U.Id,
                    UserName = U.UserName,
                    FirstName = U.FirstName,
                    LastName = U.LastName,
                    Email = U.Email,
                    Roles = _userManager.GetRolesAsync(U).Result
                }).ToList();
            }
            else
            {
                users = _userManager.Users.Select(U => new UserToReturnDto
                {
                    Id = U.Id,
                    UserName = U.UserName,
                    FirstName = U.FirstName,
                    LastName = U.LastName,
                    Email = U.Email,
                    Roles = _userManager.GetRolesAsync(U).Result
                }).Where(U => U.FirstName.ToLower().Contains(SearchInput.ToLower()));
            }
            return View(users);
        }
        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("Invalid User Id!");

            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
                return NotFound(new { statusCode = 404, message = $"User with Id: {id} not found!" });

            var userDto = _mapper.Map<UserToReturnDto>(user);
            userDto.Roles = await _userManager.GetRolesAsync(user);

            return View(userDto);
        }

        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("Invalid User Id!");

            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
                return NotFound(new { statusCode = 404, message = $"User with Id: {id} not found!" });

            var userDto = _mapper.Map<UpdateUserDto>(user);
            return View(userDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(string id, UpdateUserDto userDto)
        {
            if (!ModelState.IsValid)
                return View(userDto);

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            _userManager.UpdateAsync(user).Wait();

            _mapper.Map(userDto, user);

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
                return RedirectToAction("Index");

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(userDto);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound(new { statusCode = 404, message = $"User with Id: {id} not found!" });

            await _userManager.DeleteAsync(user);
            return RedirectToAction("Index");
        }
    }
}
