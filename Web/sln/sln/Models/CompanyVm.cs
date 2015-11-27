using Michal.Project.Contract.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Michal.Project.Models
{
    public class CompanyVm : IComapnyView
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}