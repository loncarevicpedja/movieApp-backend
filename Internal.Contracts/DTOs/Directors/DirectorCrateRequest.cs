using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internal.Contracts.DTOs.Directors
{
    public class DirectorCreateRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
