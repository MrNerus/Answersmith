using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnswerSmith.Model
{
    public class Model_QuestionSheet
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public int NumberOfQs { get; set; }
        public string Parent { get; set; } = string.Empty;
        public int Parent_Id { get; set; }
        public float FullMarks { get; set; } = 0.0f;
        public TimeOnly FullTime { get; set; }
        public DateTime UpdatedOn { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsActive { get; set; }
    }
}