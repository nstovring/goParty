using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace goParty.Abstractions
{
    public interface IRefreshable
    {
        Task Refresh();
    }
}
