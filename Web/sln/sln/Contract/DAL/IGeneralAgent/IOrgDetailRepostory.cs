using Michal.Project.Dal;
using Michal.Project.DataModel;
using System;
using System.Collections.Generic;
namespace Michal.Project.Contract.DAL
{
    public interface IOrgDetailRepostory
    {
        List<Distance> GetDistancesPerOrg(Guid orgId);

        List<ShippingCompany> GetShippingCompaniesByOrgId(Guid orgId);

        List<Organization> GetOrgs();

        List<ShipType> GetShipType();

        Guid GetOrg();

        Organization GetOrgEntity();
    }
}
