﻿using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace sln.Models
{
    public class UserContext
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
                return _fullname;
            }
        }
        Guid _orgId, _userid = Guid.Empty; string _fullname; string _empId;
        public UserContext(IAuthenticationManager authenticationManager )
        {
            ClaimsIdentity claimsIdentity = authenticationManager.User.Identity as ClaimsIdentity;
            foreach (var claim in claimsIdentity.Claims)
            {
                if (claim.Type == ClaimTypes.GroupSid)
                    _orgId = Guid.Parse(claim.Value);

                if (claim.Type == ClaimTypes.NameIdentifier)
                    _userid = Guid.Parse(claim.Value);
                if (claim.Type == ClaimTypes.Surname)
                    _fullname = claim.Value;
                if (claim.Type == ClaimTypes.SerialNumber)
                    _empId = claim.Value;
            }

        }

    }
}