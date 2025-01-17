﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using challenge.Models;

namespace challenge.Repositories
{
    public interface ICompensationRepository
    {
        Compensation Add(Compensation compensation);

        Compensation GetById(string id);

        List<Compensation> GetByEmployeeID(string id);

        Task SaveAsync();
    }
}
