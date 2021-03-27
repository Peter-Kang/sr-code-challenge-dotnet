using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using challenge.Models;
using Microsoft.Extensions.Logging;
using challenge.Repositories;

namespace challenge.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(ILogger<EmployeeService> logger, IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        public Employee Create(Employee employee)
        {
            if(employee != null)
            {
                _employeeRepository.Add(employee);
                _employeeRepository.SaveAsync().Wait();
            }

            return employee;
        }

        public Employee GetById(string id)
        {
            if(!String.IsNullOrEmpty(id))
            {
                return _employeeRepository.GetById(id);
            }

            return null;
        }

        public Employee Replace(Employee originalEmployee, Employee newEmployee)
        {
            if(originalEmployee != null)
            {
                _employeeRepository.Remove(originalEmployee);
                if (newEmployee != null)
                {
                    // ensure the original has been removed, otherwise EF will complain another entity w/ same id already exists
                    _employeeRepository.SaveAsync().Wait();

                    _employeeRepository.Add(newEmployee);
                    // overwrite the new id with previous employee id
                    newEmployee.EmployeeId = originalEmployee.EmployeeId;
                }
                _employeeRepository.SaveAsync().Wait();
            }

            return newEmployee;
        }

        public HashSet<string> GetListOfReportsWithSelf(string id, HashSet<string> vistedIDs)
        {
            var currentEmployee = GetById(id);
            HashSet<string> currentListOfReports = vistedIDs;
            if (currentEmployee != null)
            {// employee exists
             //Check if we already have the employee in the list
                bool alreadyInList = currentListOfReports.Contains(id);
                //prevent cycling
                if (!alreadyInList)
                {//add itself to the list
                    currentListOfReports.Add(id);
                    //recusively call other employees in a DFS manner
                    if (currentEmployee.DirectReports != null)
                    {
                        List<Employee> current_list_of_direct_reports = currentEmployee.DirectReports;
                        for (int i = 0; i < current_list_of_direct_reports.Count; i++)
                        {
                            Employee direct_report = current_list_of_direct_reports[i];
                            currentListOfReports = GetListOfReportsWithSelf(direct_report.EmployeeId, currentListOfReports);
                        }
                    }
                }
            }
            return currentListOfReports;
        }

    }
}
