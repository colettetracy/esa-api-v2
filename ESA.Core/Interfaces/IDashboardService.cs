using ESA.Core.Models.Payment;
using GV.DomainModel.SharedKernel.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESA.Core.Interfaces
{
    public interface IDashboardService
    {
        Task<Result<DashboardInfo>> GetInfo();
    }
}
