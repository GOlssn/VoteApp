using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VoteApp.Models
{
    public class Option
    {
        public int Id { get; set; }

        public string Value { get; set; }

        public int TimesSelected { get; set; }
        public int PollId { get; set; }
        public Poll Poll { get; set; }
    }
}
