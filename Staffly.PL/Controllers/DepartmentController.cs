﻿using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        public IActionResult Details(int? id)
        {
            if(id is null)
            {
                return BadRequest("Invalid Id!");
            }

            var department = _departmentRepository.GetById(id.Value);

            if(department is null)
            {
                return NotFound(new {statusCode = 404, message = $"Department With Id:{id.Value} Is Not Found!"});
            }

            return View(department);
        }

        [HttpGet]
        public IActionResult Update(int? id)
        {
            if (id is null)
            {
                return BadRequest("Invalid Id!");
            }

            var department = _departmentRepository.GetById(id.Value);

            if (department is null)
            {
                return NotFound(new { statusCode = 404, message = $"Department With Id:{id.Value} Is Not Found!" });
            }

            var departmentDto = new UpdateDepartmentDto
            {
                Code = department.Code,
                Name = department.Name,
                CreateAt = department.CreateAt
            };

            return View(departmentDto);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update([FromRoute]int id,UpdateDepartmentDto departmentDto)
        {
            if (ModelState.IsValid)
            {

                var department = new Department
                {
                    Id = id,
                    Code = departmentDto.Code,
                    Name = departmentDto.Name,
                    CreateAt = departmentDto.CreateAt
                };

                _departmentRepository.Update(department);
                return RedirectToAction("Index");
            }
            return View(departmentDto);
        }
    }
}
