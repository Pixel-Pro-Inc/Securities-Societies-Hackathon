using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTO
{
    public class EditAppDto
    {
        public int Id { get; set; }
        public bool accepted { get; set; }
        public bool corrections { get; set; }
        public string feedback { get; set; }
        public int MyProperty { get; set; }
        public List<bool> nameApproved { get; set; }
        public bool rejected { get; set; }
        public bool review { get; set; }
        public string stage { get; set; }
        public string admin { get; set; }
        public string certificate { get; set; }
    }
}
