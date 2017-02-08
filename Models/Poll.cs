using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VoteApp.Models
{
    public class Poll
    {
        public int Id { get; set; }

        public string Question { get; set; }
        public string ApplicationUserId { get; set; }
        public int TotalVotes { get; set; }
        public DateTime created { get; set; }
        public ApplicationUser User { get; set; }
        public List<Option> Options { get; set; }
    }
}
