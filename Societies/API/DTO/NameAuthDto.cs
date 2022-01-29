using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTO
{
    public class NameAuthDto
    {
        public string accountId { get; set; }
        public List<string> files { get; set; }
        public string societyType { get; set; }
        public string societyNames { get; set; }
        public string objectives { get; set; }
    }
}
