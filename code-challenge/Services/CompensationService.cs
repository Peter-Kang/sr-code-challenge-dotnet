using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using challenge.Models;
using challenge.Data;
using challenge.Repositories;
using Microsoft.Extensions.Logging;

namespace challenge.Services
{
    public class CompensationService : ICompensationService
    {
        private readonly ICompensationRepository _compensationRepository;
        private readonly ILogger<CompensationService> _logger;

        public CompensationService(ILogger<CompensationService> logger, ICompensationRepository compensationRepository) 
        {
            _compensationRepository = compensationRepository;
            _logger = logger;
        }

        public Compensation Create(Compensation compensation) 
        {
            if (compensation != null ) 
            {
                _compensationRepository.Add(compensation);
                _compensationRepository.SaveAsync().Wait();
            }
            return compensation;
        }

        public Compensation GetById(string id) 
        {
            if (!String.IsNullOrEmpty(id))
            {
                return _compensationRepository.GetById(id);
            }
            return null;
        }

        public List<Compensation> GetByEmployeeID(string employeeID) 
        {
            if (!String.IsNullOrEmpty(employeeID))
            {
                return _compensationRepository.GetByEmployeeID(employeeID);
            }
            return null;
        }

    }
}
