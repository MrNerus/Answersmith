using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnswerSmith.DTOs
{
    public class DTO_Class
    {
        public int SN {get; set;} = -1;
        public string Code {get; set;} = String.Empty;
        public string Name {get; set;} = String.Empty;
        public bool IsActive {get; set;} = true;
    }
}