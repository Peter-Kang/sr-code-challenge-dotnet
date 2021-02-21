using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using challenge.Services;
using challenge.Models;

namespace challenge.Controllers
{
    [Route("api/compensation")]
    public class CompensationController : Controller
    {
        private readonly ILogger _logger;
        private readonly ICompensationService _compensationService;

        public CompensationController(ILogger<CompensationController> logger, ICompensationService compensationService) 
        {
            _logger = logger;
            _compensationService = compensationService;
        }

        [HttpPost]
        public IActionResult CreateCompensation([FromBody] Compensation compensation)
        {
            if ( compensation.employeeID == null && compensation.employee.EmployeeId != null) 
            {
                compensation.employeeID = compensation.employee.EmployeeId;
            }
            _compensationService.Create(compensation);
            return CreatedAtRoute("getCompensationById", new { id = compensation.employeeID }, compensation);
        }

        [HttpGet("{id}", Name = "getCompensationById")]
        public IActionResult GetCompensationById(String id)
        {
            var compensation = _compensationService.GetById(id);
            if (compensation == null)
            {
                return NotFound();
            }
            return Ok(compensation);
        }


    }
}
