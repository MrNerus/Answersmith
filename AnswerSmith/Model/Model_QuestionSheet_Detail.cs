using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnswerSmith.Model
{
    public class Model_QuestionSheet_Detail
    {
        public int Id { get; set; }
        public int Serial_No {get; set;}
        public int Order_No {get; set; }
        public int Question_Id { get; set; }
        public int Group_Id { get; set; }
        public int Group_Serial { get; set; }
        public string Group_Note { get; set; } = string.Empty;
        public string Parent { get; set; } = string.Empty;
        public int Parent_Id { get; set; }
        public float Marks { get; set; } = 0.0f;
        public bool IsActive { get; set; }
    }
}