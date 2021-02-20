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
    [Route("api/ReportingStructureController")]
    public class ReportingStructureController : ControllerBase
    {

        private readonly ILogger _logger;
        private readonly IEmployeeService _employeeService;

        public ReportingStructureController(ILogger<EmployeeController> logger, IEmployeeService employeeService) 
        {
            _logger = logger;
            _employeeService = employeeService;
        }

        // GET: api/<ReportingStructureController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ReportingStructureController>/5
        [HttpGet("{id}")]
        public IActionResult Get(String id)
        {
            IActionResult result;
            //Get the it's current employee object
            var employee_from_service = _employeeService.GetById(id);
            //Check if it exists
            if (employee_from_service == null)
            {
                return NotFound();
            }

            //Get the report structure
            List<string> direct_report_employee_ids_and_employee_id = GetListOfReportsWithSelf(id,new List<string>());
            int reports_count = direct_report_employee_ids_and_employee_id.Count - 1;// minus one for itself

            //Fill in the contents
            ReportingStructure result_report = new ReportingStructure();
            result_report.employee = employee_from_service;
            result_report.numberOfReports = reports_count;

            return Ok(result_report);
        }

        private List<string> GetListOfReportsWithSelf(string id, List<string> vistedIDs) 
        {
            var current_employee = _employeeService.GetById(id);
            List<string> current_list_of_reports = vistedIDs;
            if ( current_employee != null  )
            {// employee exists
             //Check if we already have the employee in the list
                bool already_in_list = current_list_of_reports.Contains(id);
                //prevent cycling
                if (!already_in_list) 
                {//add itself to the list
                    current_list_of_reports.Add(id);
                    //recusively call other employees in a DFS manner
                    if (current_employee.DirectReports != null) 
                    {
                        List<Employee> current_list_of_direct_reports = current_employee.DirectReports;
                        for (int i = 0; i < current_list_of_direct_reports.Count; i++)
                        {
                            Employee direct_report = current_list_of_direct_reports[i];
                            current_list_of_reports = GetListOfReportsWithSelf(direct_report.EmployeeId, current_list_of_reports);
                        }
                    }
                }
            }
            return current_list_of_reports;
        }

    }
}
