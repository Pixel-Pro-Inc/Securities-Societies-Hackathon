using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class NameAuth
    {
        public int Id { get; set; }
        public List<string> Names { get; set; }
        public List<string> FileUrls { get; set; }
        public string SocietyType { get; set; }
        public List<string> Objectives { get; set; }
        public User User { get; set; }
        public User Admin { get; set; }
        public bool Corrections { get; set; }
        public bool Rejected { get; set; }
        public bool Accepted { get; set; }
        public bool Review { get; set; }
        public List<string> Feedback { get; set; }
        public DateTime DateSubmitted { get; set; }
        public DateTime DateReviewed { get; set; }
        public List<string> FileNames { get; set; }
        public List<bool> AcceptedNames { get; set; }
    }
}
