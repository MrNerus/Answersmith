using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnswerSmith.Model
{
    public class Model_WhereClause
    {
        public string Key {get; set;} = string.Empty;
        public string Value {get; set;} = string.Empty;
        public bool IsFuzzy {get; set;} = false;
    }
}