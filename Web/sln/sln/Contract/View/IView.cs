﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Michal.Project.Contract.View
{
    public interface IView
    {
         Guid Id { get; set; }


         string Name { get; set; }

    }
}
