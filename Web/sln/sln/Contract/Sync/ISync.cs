using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Michal.Project.Contract.View
{
    public interface ISync
    {
        Guid CurrentUserId { get; set; }
        string DeviceId { get; set; } //optional TODO
        string ClientId { get; set; } //optional TODO
    }
}
