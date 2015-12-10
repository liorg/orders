using Michal.Project.Contract.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Michal.Project.Models
{
    public class CompanyVm : IView
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ContractTel { get; set; }
        public string ContactFullName { get; set; }
        public AddressEditorViewModel Address{ get; set; }


        public string Desc { get; set; }
    }
}