using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Michal.Project.Models
{
    public class Result
    {
        public string ErrCode { get; set; }
        public bool IsError { get; set; }
        public string    ErrDesc { get; set; }
    }

    public class Result<T> : Result
    {
        public T Model { get; set; }
    }

    public class ResponseBase<T>: Result<T> 
    {
        public bool IsAuthenticated { get; set; }
    }
}