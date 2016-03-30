using Michal.Project.Contract.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Michal.Project.Models
{
	public class ResponseBase<T>: Result<T> 
    {
        public bool IsAuthenticated { get; set; }
    }
}