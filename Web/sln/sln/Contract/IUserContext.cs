using System;
namespace sln.Contract
{
    public interface IUserContext
    {
        int DefaultView { get; }
        string EmpId { get; }
        string FullName { get; }
        Guid OrgId { get; }
        bool ShowAll { get; }
        Guid UserId { get; }
    }
}
