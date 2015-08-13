using System;
namespace Michal.Project.Contract
{
    public interface IJob
    {
         string JobType { get; set; }
         string JobTitle { get; set; }

    }

    public interface IRole
    {
        bool IsAcceptOrder { get; set; }
        bool IsAdmin { get; set; }
        bool IsCreateOrder { get; set; }
        bool IsOrgMangager { get; set; }
        bool IsRunner { get; set; }
    }
    public interface IViewerUser
    {

         int DefaultView { get; set; }

         bool ViewAll { get; set; }
    }
}
