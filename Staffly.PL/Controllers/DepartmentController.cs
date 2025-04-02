using Microsoft.AspNetCore.Mvc;
using Staffly.BLL.Interfaces;
using Staffly.DAL.Dtos;
using Staffly.DAL.Models;

namespace Staffly.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _departmentRepository;
        public DepartmentController(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var departments = _departmentRepository.GetAll();
            return View(departments);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateDepartmentDto departmentDto)
        {
            if (ModelState.IsValid)
            {
                // Mapping CreateDepartmentDto to Department
                var department = new Department
                {
                    Code = departmentDto.Code,
                    Name = departmentDto.Name,
                    CreateAt = departmentDto.CreateAt
                };

                _departmentRepository.Add(department);
                return RedirectToAction("Index");
            }

            return View(departmentDto);
        }
    }
}
