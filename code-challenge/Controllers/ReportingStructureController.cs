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
    [Route("api/ReportingStructure")]
    public class ReportingStructureController : Controller
    {

        private readonly ILogger _logger;
        private readonly IEmployeeService _employeeService;

        public ReportingStructureController(ILogger<EmployeeController> logger, IEmployeeService employeeService) 
        {
            _logger = logger;
            _employeeService = employeeService;
        }

        // GET api/<ReportingStructureController>/5
        [HttpGet("{id}")]
        public IActionResult Get(String id)
        {
            //Get the it's current employee object
            var employeeFromService = _employeeService.GetById(id);
            //Check if it exists
            if (employeeFromService == null)
            {
                return NotFound();
            }

            //Get the report structure
            HashSet<string> directReportEmployeeIDsAndEmployeeID = _employeeService.GetListOfReportsWithSelf(id,new HashSet<string>());
            int reportsCount = directReportEmployeeIDsAndEmployeeID.Count - 1;// minus one for itself
            if (reportsCount < 0 ) 
            {// if it doesn't exist it will return a empty list
                reportsCount = 0;
            }
            //Fill in the contents
            ReportingStructure resultReport = new ReportingStructure();
            resultReport.employee = employeeFromService;
            resultReport.numberOfReports = reportsCount;

            return Ok(resultReport);
        }

       

    }
}
