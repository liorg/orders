using Microsoft.Owin.Security;
using Michal.Project.Contract;
using Michal.Project.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace Michal.Project.Models
{
    public class UserContext : IUserContext
    {
        public Guid OrgId
        {
            get
            {
                return _orgId;
            }
        }

        public Guid UserId
        {
            get
            {
                return _userid;
            }
            set
            {
                _userid = value;
            }
        }

        public string FullName
        {
            get
            {
                return _fullname;
            }
        }

        public string EmpId
        {
            get
            {
                return _empId;
            }
        }

        public int DefaultView
        {
            get
            {
                return _defaultView;
            }
        }

        public bool ShowAll
        {
            get
            {
                return _showAll;
            }
        }

        public string Tel
        {
            get
            {
                return _tel;
            }
        }
        Guid _orgId, _userid = Guid.Empty; string _fullname; string _empId; string _tel;
        bool _showAll; int _defaultView;

        public UserContext()
        {
        }
        public UserContext(IAuthenticationManager authenticationManager)
        {
            ClaimsIdentity claimsIdentity = authenticationManager.User.Identity as ClaimsIdentity;
            foreach (var claim in claimsIdentity.Claims)
            {
                if (claim.Type == CustomClaimTypes.Tel)
                    _tel = claim.Value;

                if (claim.Type == ClaimTypes.GroupSid)
                    _orgId = Guid.Parse(claim.Value);

                if (claim.Type == ClaimTypes.NameIdentifier)
                    _userid = Guid.Parse(claim.Value);
                if (claim.Type == ClaimTypes.Surname)
                    _fullname = claim.Value;
                if (claim.Type == ClaimTypes.SerialNumber)
                    _empId = claim.Value;
                if (claim.Type == CustomClaimTypes.DefaultView)
                    if (String.IsNullOrEmpty(claim.Value))
                        _defaultView = 1;
                    else _defaultView = int.Parse(claim.Value);

                if (claim.Type == CustomClaimTypes.ShowAllView)
                    if (String.IsNullOrEmpty(claim.Value))
                        _showAll = false;
                    else _showAll = bool.Parse(claim.Value);

                if (claim.Type == CustomClaimTypes.JobType)
                    if (String.IsNullOrEmpty(claim.Value))
                        _jobType = "";
                    else _jobType = claim.Value;
                if (claim.Type == CustomClaimTypes.JobTitle)
                    if (String.IsNullOrEmpty(claim.Value))
                        _jobTitle = "";
                    else _jobTitle = claim.Value;
            }

        }

        string _jobType, _jobTitle;
        public string JobType
        {
            get
            {
                return _jobType;
            }
            set
            {
                _jobType = value;
            }
        }

        public string JobTitle
        {
            get
            {
                return _jobTitle;
            }
            set
            {
                _jobTitle = value;
            }
        }
    }
}