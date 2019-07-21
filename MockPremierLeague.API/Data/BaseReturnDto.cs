using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MockPremierLeague.API.Data
{
    public class BaseReturnDto
    {
        public string Status { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public object ModelToReturn { get; set; }
    }
}
