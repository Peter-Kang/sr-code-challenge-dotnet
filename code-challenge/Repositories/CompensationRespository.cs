using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using challenge.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using challenge.Data;

namespace challenge.Repositories
{
    public class CompensationRespository : ICompensationRepository
    {
        private readonly CompensationContext _compensationContext;
        private readonly ILogger<ICompensationRepository> _logger;

        public CompensationRespository(ILogger<ICompensationRepository> logger, CompensationContext compensationContext)
        {
            _compensationContext = compensationContext;
            _logger = logger;
        }

        public Compensation Add(Compensation compensation) 
        {
            compensation.CompensationID = Guid.NewGuid().ToString();
            _compensationContext.Compensations.Add(compensation);
            return compensation;
        }

        public Compensation GetById(string id) 
        {
            var compensation_result = _compensationContext.Compensations.SingleOrDefault(e => e.CompensationID == id);
            return compensation_result;
        }

        public List<Compensation> GetByEmployeeID(string employeeID) 
        {
            List<Compensation> compensation_result = _compensationContext.Compensations.Where( e=>e.EmployeeID == employeeID ).ToList();
            return compensation_result;
        }

        public Task SaveAsync() 
        {
            return _compensationContext.SaveChangesAsync();
        }

    }
}
