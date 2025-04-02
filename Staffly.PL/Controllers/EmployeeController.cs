using Microsoft.AspNetCore.Mvc;
using Staffly.BLL.Interfaces;
using Staffly.DAL.Dtos;
using Staffly.DAL.Models;

namespace Staffly.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var employees = _employeeRepository.GetAll();
            return View(employees);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateEmployeeDto employeeDto)
        {
            if (ModelState.IsValid)
            {
                // Mapping CreateDepartmentDto to Department
                var employee = new Employee
                {
                    Name = employeeDto.Name,
                    Age = employeeDto.Age,
                    Email = employeeDto.Email,
                    Address = employeeDto.Address,
                    Phone = employeeDto.Phone,
                    Salary = employeeDto.Salary,
                    isActive = employeeDto.isActive,
                    isDeleted = employeeDto.isDeleted,
                    HiringDate = employeeDto.HiringDate,
                    CreateAt = employeeDto.CreateAt
                };

                _employeeRepository.Add(employee);
                return RedirectToAction("Index");
            }

            return View(employeeDto);
        }

        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id is null)
            {
                return BadRequest("Invalid Id!");
            }

            var employee = _employeeRepository.GetById(id.Value);

            if (employee is null)
            {
                return NotFound(new { statusCode = 404, message = $"Employee With Id:{id.Value} Is Not Found!" });
            }

            return View(employee);
        }

        [HttpGet]
        public IActionResult Update(int? id)
        {
            if (id is null)
            {
                return BadRequest("Invalid Id!");
            }

            var employee = _employeeRepository.GetById(id.Value);

            if (employee is null)
            {
                return NotFound(new { statusCode = 404, message = $"Employee With Id:{id.Value} Is Not Found!" });
            }

            var employeeDto = new UpdateEmployeeDto
            {
                Name = employee.Name,
                Age = employee.Age,
                Email = employee.Email,
                Address = employee.Address,
                Phone = employee.Phone,
                Salary = employee.Salary,
                isActive = employee.isActive,
                isDeleted = employee.isDeleted,
                HiringDate = employee.HiringDate,
                CreateAt = employee.CreateAt
            };

            return View(employeeDto);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update([FromRoute] int id, UpdateEmployeeDto employeeDto)
        {
            if (ModelState.IsValid)
            {

                var employee = new Employee
                {
                    Id = id,
                    Name = employeeDto.Name,
                    Age = employeeDto.Age,
                    Email = employeeDto.Email,
                    Address = employeeDto.Address,
                    Phone = employeeDto.Phone,
                    Salary = employeeDto.Salary,
                    isActive = employeeDto.isActive,
                    isDeleted = employeeDto.isDeleted,
                    HiringDate = employeeDto.HiringDate,
                    CreateAt = employeeDto.CreateAt
                };

                _employeeRepository.Update(employee);
                return RedirectToAction("Index");
            }
            return View(employeeDto);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var employee = _employeeRepository.GetById(id);
            if (employee is null)
            {
                return NotFound(new { statusCode = 404, message = $"Employee With Id:{id} Is Not Found!" });
            }
            _employeeRepository.Delete(employee);
            return RedirectToAction("Index");
        }
    }
}
