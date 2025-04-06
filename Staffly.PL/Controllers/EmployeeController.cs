using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Staffly.BLL.Interfaces;
using Staffly.DAL.Dtos;
using Staffly.DAL.Models;

namespace Staffly.PL.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeController(IMapper mapper,IUnitOfWork unitOfWork)
        {
            this._mapper = mapper;
            this._unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? SearchInput)
        {
            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(SearchInput))
            {
                employees = await _unitOfWork.EmployeeRepository.GetAllAsync();
            }
            else
            {
                employees = await _unitOfWork.EmployeeRepository.FindByNameAsync(SearchInput);
            }
            return View(employees);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var departments =await _unitOfWork.DepartmentRepository.GetAllAsync();
            ViewData["Departments"] = departments;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeDto employeeDto)
        {
            if (ModelState.IsValid)
            {
                // Map DTO to Entity
                var employee = _mapper.Map<Employee>(employeeDto);
                await _unitOfWork.EmployeeRepository.AddAsync(employee);
                // Save changes to the database
                await _unitOfWork.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(employeeDto);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
            {
                return BadRequest("Invalid Id!");
            }

            var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(id.Value);

            if (employee is null)
            {
                return NotFound(new { statusCode = 404, message = $"Employee With Id:{id.Value} Is Not Found!" });
            }
            var departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
            ViewData["Departments"] = departments;

            return View(employee);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id is null)
            {
                return BadRequest("Invalid Id!");
            }

            var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(id.Value);

            if (employee is null)
            {
                return NotFound(new { statusCode = 404, message = $"Employee With Id:{id.Value} Is Not Found!" });
            }
            var employeeDto = _mapper.Map<UpdateEmployeeDto>(employee);
            var departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
            ViewData["Departments"] = departments;

            return View(employeeDto);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([FromRoute] int id, UpdateEmployeeDto employeeDto)
        {
            if (ModelState.IsValid)
            {
                // Map DTO to Entity
                 var employee = _mapper.Map<Employee>(employeeDto);

                _unitOfWork.EmployeeRepository.Update(employee);
                // Save changes to the database
                await _unitOfWork.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(employeeDto);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(id);
            if (employee is null)
            {
                return NotFound(new { statusCode = 404, message = $"Employee With Id:{id} Is Not Found!" });
            }
            _unitOfWork.EmployeeRepository.Delete(employee);
            // Save changes to the database
            await _unitOfWork.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
