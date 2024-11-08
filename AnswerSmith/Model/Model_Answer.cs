using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnswerSmith.Model
{
    public class Model_Answer
    {
        public int Id { get; set; }
        public string Question_C_Code {get; set;} = string.Empty;
        public string Answer {get; set; } = string.Empty;
        public string Flags {get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}