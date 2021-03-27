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
    [Route("api/Reports")]
    public class ReportingStructureController : Controller
    {

        private readonly ILogger _logger;
        private readonly IEmployeeService _employeeService;

        public ReportingStructureController(ILogger<EmployeeController> logger, IEmployeeService employeeService) 
        {
            _logger = logger;
            _employeeService = employeeService;
        }

        // GET api/<Reports>/5
        [HttpGet("{id}")]
        public IActionResult Get(String id)
        {
            _logger.LogDebug($"Received Reporting get request for '{id}'");
            //Get the it's current employee object
            var employeeFromService = _employeeService.GetById(id);
            //Check if it exists
            if (employeeFromService == null)
            {
                return NotFound();
            }
            //Fill in the contents
            ReportingStructure resultReport = new ReportingStructure()
            {
                employee = employeeFromService,
                numberOfReports = _employeeService.GetCountOfReports(id)
            };
            return Ok(resultReport);
        }

       

    }
}
