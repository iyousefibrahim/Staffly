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

        public RoleController(RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            this._roleManager = roleManager;
            this._mapper = mapper;
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
            var roles = await _roleManager.Roles.ToListAsync();
            ViewData["Roles"] = roles;
            return View();
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
    }
}
