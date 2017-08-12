using System;
using System.Collections.Generic;
using System.Text;

namespace goParty.Abstractions
{
    interface ICloudService
    {
        ICloudTable<T> GetTable<T>() where T : TableData;
    }
}
