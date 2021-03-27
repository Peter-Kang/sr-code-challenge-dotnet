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
            _logger.LogDebug($"Received compensation create request for compensation ID: '{compensation.CompensationID}'");
            ActionResult result;
            if (compensation.EmployeeID != null)
            {
                _compensationService.Create(compensation);
                result = CreatedAtRoute("getCompensationById", new { id = compensation.EmployeeID }, compensation);
            }
            else 
            {
                result = BadRequest("Employee ID missing");
            }
            return result;
        }

        [HttpGet("getCompensationById/{id}", Name = "getCompensationById")]
        public IActionResult GetCompensationById(String id)
        {
            _logger.LogDebug($"Received compensation get request for ID: '{id}'");
            var compensation = _compensationService.GetById(id);
            if (compensation == null)
            {
                return NotFound();
            }
            return Ok(compensation);
        }

        [HttpGet("getCompensationByEmployeeID/{id}", Name = "getCompensationByEmployeeID")]
        public IActionResult GetCompensationByEmployeeId(String id)
        {
            _logger.LogDebug($"Received compensation get request for Employee ID: '{id}'");
            var compensation = _compensationService.GetByEmployeeID(id);
            if (compensation == null)
            {
                return NotFound();
            }
            return Ok(compensation);
        }
    }
}
