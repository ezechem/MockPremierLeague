using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MockPremierLeague.API.Enumerators
{
    public enum ResponseCode
    {
        Success = 1,
        Failed = 2,
        Error = 3,
        RecordNotFound = 4
    }
}
