using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnswerSmith.Enums;

namespace AnswerSmith.Model
{
    public class Model_OrderBy
    {
        public string Column_Name {get; set;} = String.Empty;
        public Enum_Any_OrderMode Order_Mode {get; set;} = Enum_Any_OrderMode.NOT_IMPLEMENTED;
        public string Miscelleneous {get; set;} = string.Empty;

    }
}