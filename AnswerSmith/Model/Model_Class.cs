using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace AnswerSmith.Model
{
    public class Model_Class
    {
        public int Id {get; set;}
        public string Code {get; set;} = String.Empty;
        public string Name {get; set;} = String.Empty;
        public bool IsActive {get; set;} = true;
    }
}