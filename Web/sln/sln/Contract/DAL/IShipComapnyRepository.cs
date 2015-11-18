﻿using Michal.Project.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Michal.Project.Contract.DAL
{
    public interface IShipComapnyRepository
    {
        
        Task<ShippingCompany> GetAsync(Guid companyId);
    }
}
