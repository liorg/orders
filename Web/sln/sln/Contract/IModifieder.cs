﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sln.Contract
{
    public interface IModifieder
    {
         DateTime? CreatedOn { get; set; }
         DateTime? ModifiedOn { get; set; }
         Guid? CreatedBy { get; set; }
         Guid? ModifiedBy { get; set; }
         bool IsActive { get; set; }
    }
}
