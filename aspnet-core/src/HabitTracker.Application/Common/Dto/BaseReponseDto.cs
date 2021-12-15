using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Common.Dto
{
    public class BaseReponseDto
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}
