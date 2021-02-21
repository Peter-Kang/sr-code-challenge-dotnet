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
            _compensationContext.Compensations.Add(compensation);
            return compensation;
        }

        public Compensation GetById(string id) 
        {
            return _compensationContext.Compensations.Include(m => m.employee).SingleOrDefault(e => e.employeeID == id);
        }

        public Task SaveAsync() 
        {
            return _compensationContext.SaveChangesAsync();
        }

    }
}
