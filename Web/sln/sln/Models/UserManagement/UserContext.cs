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
        Guid? _grantUserid;
        public Guid? GrantUserId
        {
            get
            {
                return _grantUserid;
            }
            set
            {
                _grantUserid = value;
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
        Michal.Project.DataModel.Address _address;
        public Michal.Project.DataModel.Address Address
        {
            get
            {
                return _address;
            }
        }

        public UserContext()
        {
            _address = new DataModel.Address();
        }
        public UserContext(IAuthenticationManager authenticationManager)
        {
            _address = new DataModel.Address();
            ClaimsIdentity claimsIdentity = authenticationManager.User.Identity as ClaimsIdentity;
            foreach (var claim in claimsIdentity.Claims)
            {
                if (claim.Type == CustomClaimTypes.CityCode)
                {
                    _address.CityCode = claim.Value; continue;
                }
                if (claim.Type == CustomClaimTypes.City)
                {
                    _address.CityName = claim.Value; continue;
                }
                if (claim.Type == CustomClaimTypes.StreetCode)
                {
                    _address.StreetCode = claim.Value; continue;
                }
                if (claim.Type == CustomClaimTypes.Street)
                {
                    _address.StreetName = claim.Value; continue;
                }
                if (claim.Type == CustomClaimTypes.Num)
                {
                    _address.StreetNum = claim.Value; continue;
                }
                if (claim.Type == CustomClaimTypes.External)
                {
                    _address.ExtraDetail = claim.Value; continue;
                }
                if (claim.Type == CustomClaimTypes.Lat)
                {
                    _address.Lat = double.Parse(claim.Value); continue;
                }
                if (claim.Type == CustomClaimTypes.Lng)
                {
                    _address.Lng = double.Parse(claim.Value); continue;
                }
                if (claim.Type == CustomClaimTypes.UID)
                {
                    _address.UID = int.Parse(claim.Value); continue;
                }
                if (claim.Type == CustomClaimTypes.Tel)
                {
                    _tel = claim.Value; continue;
                }
                if (claim.Type == CustomClaimTypes.GrantUser)
                {
                    var tempId = Guid.Parse(claim.Value);
                    if (tempId != Guid.Empty)
                        _grantUserid = tempId; continue;
                }
                if (claim.Type == ClaimTypes.GroupSid)
                {
                    _orgId = Guid.Parse(claim.Value); continue;
                }

                if (claim.Type == ClaimTypes.NameIdentifier)
                {
                    _userid = Guid.Parse(claim.Value); continue;
                }
                if (claim.Type == ClaimTypes.Surname)
                {
                    _fullname = claim.Value; continue;
                }
                if (claim.Type == ClaimTypes.SerialNumber)
                {
                    _empId = claim.Value; continue;
                }
                if (claim.Type == CustomClaimTypes.DefaultView)
                {
                    if (String.IsNullOrEmpty(claim.Value))
                        _defaultView = 1;
                    else _defaultView = int.Parse(claim.Value);
                    continue;
                }
                if (claim.Type == CustomClaimTypes.ShowAllView)
                {
                    if (String.IsNullOrEmpty(claim.Value))
                        _showAll = false;
                    else _showAll = bool.Parse(claim.Value);
                    continue;
                }
                if (claim.Type == CustomClaimTypes.JobType)
                {
                    if (String.IsNullOrEmpty(claim.Value))
                        _jobType = "";
                    else _jobType = claim.Value;
                    continue;
                }
                if (claim.Type == CustomClaimTypes.JobTitle)
                {
                    if (String.IsNullOrEmpty(claim.Value))
                        _jobTitle = "";
                    else _jobTitle = claim.Value; continue;
                }
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