using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Staffly.DAL.Dtos;
using Staffly.DAL.Models;

namespace Staffly.PL.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleController(RoleManager<IdentityRole> roleManager, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            this._roleManager = roleManager;
            this._mapper = mapper;
            this._userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string? SearchInput)
        {
            IEnumerable<RoleToRetrurnDto> roles;
            if (string.IsNullOrEmpty(SearchInput))
            {
                roles = _roleManager.Roles.Select(U => new RoleToRetrurnDto
                {
                    Id = U.Id,
                    Name = U.Name
                }).ToList();
            }
            else
            {
                roles = _roleManager.Roles.Select(U => new RoleToRetrurnDto
                {
                    Id = U.Id,
                    Name = U.Name
                }).Where(U => U.Name.ToLower().Contains(SearchInput.ToLower()));
            }
            return View(roles);
        }
        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("Invalid Role Id!");

            var role = await _roleManager.FindByIdAsync(id);

            if (role is null)
                return NotFound(new { statusCode = 404, message = $"Role with Id: {id} not found!" });

            var roleDto = _mapper.Map<RoleToRetrurnDto>(role);

            return View(roleDto);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var roleDto = new RoleToRetrurnDto();
            ViewData["Roles"] = await _roleManager.Roles.ToListAsync();
            return View(roleDto);
        }


        [HttpPost]
        public async Task<IActionResult> Create(RoleToRetrurnDto roleDto)
        {
            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByNameAsync(roleDto.Name);
                if (role != null)
                {
                    ModelState.AddModelError("", "Role already exists!");
                    return View(roleDto);
                }

                var newRole = new IdentityRole
                {
                    Name = roleDto.Name,
                    NormalizedName = roleDto.Name.ToUpper()
                };

                var result = await _roleManager.CreateAsync(newRole);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return View(roleDto);
        }

        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("Invalid Role Id!");

            var role = await _roleManager.FindByIdAsync(id);
            if (role is null)
                return NotFound(new { statusCode = 404, message = $"Role with Id: {id} not found!" });

            var roleDto = _mapper.Map<RoleToRetrurnDto>(role);
            return View(roleDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(string id, RoleToRetrurnDto roleDto)
        {
            if (!ModelState.IsValid)
                return View(roleDto);

            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
                return NotFound();

            if (role.Name != roleDto.Name)
            {
                role.Name = roleDto.Name;
            }

            var result = await _roleManager.UpdateAsync(role);

            if (result.Succeeded)
                return RedirectToAction("Index");

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(roleDto);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
                return NotFound(new { statusCode = 404, message = $"Role with Id: {id} not found!" });

            await _roleManager.DeleteAsync(role);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> AddOrRemoveUser(string roleId)
        {
            var roles = await _roleManager.FindByIdAsync(roleId);
            if (roles == null)
                return NotFound();

            var usersInRole = new List<UsersInRoleDto>();
            var users = await _userManager.Users.ToListAsync();
            foreach (var user in users)
            {
                var userInRole = new UsersInRoleDto()
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    IsSelected = await _userManager.IsInRoleAsync(user, roles.Name)
                };
                usersInRole.Add(userInRole);
            }

            ViewData["RoleId"] = roleId;
            return View(usersInRole);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrRemoveUser(string roleId, List<UsersInRoleDto> users)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                foreach (var user in users)
                {
                    var userInDb = await _userManager.FindByIdAsync(user.UserId);
                    if (userInDb != null)
                    {
                        if (user.IsSelected && !await _userManager.IsInRoleAsync(userInDb, role.Name))
                        {
                            await _userManager.AddToRoleAsync(userInDb, role.Name);
                        }
                        else if (!user.IsSelected && await _userManager.IsInRoleAsync(userInDb, role.Name))
                        {
                            await _userManager.RemoveFromRoleAsync(userInDb, role.Name);
                        }
                    }
                }
                return RedirectToAction("Update", "Role", new { id = roleId });
            }

            ViewData["RoleId"] = roleId;
            return View(users);  // إعادة إرسال البيانات بعد التحديث
        }


    }


}
