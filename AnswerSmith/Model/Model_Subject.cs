using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnswerSmith.Model
{
    public class Model_Subject
    {
        public int Id {get; set;}
        public string Code {get; set;} = String.Empty;
        public int Class_Id {get; set;}
        public string Name {get; set;} = String.Empty;
        public bool IsActive {get; set;} = true;
    }
}