using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnswerSmith.Model
{
    public class Model_PageRequest
    {
        public int Page_Number { get; set; } = 1;
        public int Rows_Per_Page { get; set; } = 20;
    }
}