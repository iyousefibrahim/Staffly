using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Staffly.BLL.Interfaces;
using Staffly.DAL.Dtos;
using Staffly.DAL.Models;

namespace Staffly.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        public EmployeeController(IEmployeeRepository employeeRepository,IDepartmentRepository departmentRepository,IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            this._departmentRepository = departmentRepository;
            this._mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index(string? SearchInput)
        {
            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(SearchInput))
            {
                employees = _employeeRepository.GetAll();
            }
            else
            {
                employees = _employeeRepository.FindByName(SearchInput);
            }
            return View(employees);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var departments = _departmentRepository.GetAll();
            ViewData["Departments"] = departments;
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateEmployeeDto employeeDto)
        {
            if (ModelState.IsValid)
            {
                // Map DTO to Entity
                var employee = _mapper.Map<Employee>(employeeDto);
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
            var departments = _departmentRepository.GetAll();
            ViewData["Departments"] = departments;

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
            var employeeDto = _mapper.Map<UpdateEmployeeDto>(employee);
            var departments = _departmentRepository.GetAll();
            ViewData["Departments"] = departments;

            return View(employeeDto);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update([FromRoute] int id, UpdateEmployeeDto employeeDto)
        {
            if (ModelState.IsValid)
            {
                // Map DTO to Entity
                 var employee = _mapper.Map<Employee>(employeeDto);

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
