using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sln.Helper
{
    public static class HelperAutorize
    {
        public const string RoleAdmin = "Admin";
        public const string RoleUser = "User";
        public const string RoleRunner = "Runner";
        public const string RoleOrgManager = "OrgManager";
        public const string RoleAccept = "Accept";
    }

    public class RolesAttribute : System.Web.Mvc.AuthorizeAttribute
    {
        public RolesAttribute(params string[] roles)
        {
            Roles = String.Join(",", roles);
        }
    }

}