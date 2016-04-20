using Michal.Project.Mechanism.Sync.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Michal.Project.Contract.View
{
   public interface IExcuteRegisterTransaction
    {
       Task<IExcuteRegisterTransaction> ExcuteRegisterTransaction(ISync request, RegisterAdaptor registerAdaptor);
       Task CommitRegister();
    }
}
