using Michal.Project.DataModel;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;

namespace Michal.Project.Helper
{
    public class HelperSecurity
    {
        public static string GetHash(string input)
        {
            HashAlgorithm hashAlgorithm = new SHA256CryptoServiceProvider();

            byte[] byteValue = System.Text.Encoding.UTF8.GetBytes(input);

            byte[] byteHash = hashAlgorithm.ComputeHash(byteValue);

            return Convert.ToBase64String(byteHash);
        }

        public static void SetClaims(ClaimsIdentity identity, ApplicationUser user, Organization org, Helper.JobType jobType, string jobTitle)
        {
            identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
            identity.AddClaim(new Claim(CustomClaimTypes.JobTitle, jobTitle));
            identity.AddClaim(new Claim(CustomClaimTypes.JobType, ((int)jobType).ToString()));

            identity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
            identity.AddClaim(new Claim(ClaimTypes.GroupSid, org.OrgId.ToString()));
            identity.AddClaim(new Claim(ClaimTypes.SerialNumber, String.IsNullOrEmpty(user.EmpId) ? "אן מספר עובד" : user.EmpId));
            identity.AddClaim(new Claim(ClaimTypes.Surname, user.FirstName + " " + user.LastName));

            identity.AddClaim(new Claim(CustomClaimTypes.ShowAllView, user.ViewAll.ToString()));
            identity.AddClaim(new Claim(CustomClaimTypes.DefaultView, user.DefaultView.ToString()));
            identity.AddClaim(new Claim(CustomClaimTypes.Tel, user.Tel.ToString()));

            identity.AddClaim(new Claim(CustomClaimTypes.City, user.AddressUser.CityName.ToString()));
            identity.AddClaim(new Claim(CustomClaimTypes.CityCode, user.AddressUser.CityCode.ToString()));
            identity.AddClaim(new Claim(CustomClaimTypes.Street, user.AddressUser.StreetName.ToString()));
            identity.AddClaim(new Claim(CustomClaimTypes.StreetCode, user.AddressUser.StreetCode.ToString()));
            identity.AddClaim(new Claim(CustomClaimTypes.Num, user.AddressUser.StreetNum.ToString()));
            identity.AddClaim(new Claim(CustomClaimTypes.External, String.IsNullOrEmpty(user.AddressUser.ExtraDetail) ? "" : user.AddressUser.ExtraDetail.ToString()));
            identity.AddClaim(new Claim(CustomClaimTypes.UID, user.AddressUser.UID.ToString()));
            identity.AddClaim(new Claim(CustomClaimTypes.Lat, user.AddressUser.Lat.ToString()));
            identity.AddClaim(new Claim(CustomClaimTypes.Lng, user.AddressUser.Lng.ToString()));
            identity.AddClaim(new Claim(CustomClaimTypes.GrantUser, user.GrantUserManager.GetValueOrDefault().ToString()));

        }
    }
}