using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnswerSmith.Model
{
    public class Model_Question
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string C_Code { get; set; } = string.Empty;
        public string? Parent { get; set; } = string.Empty;
        public int? Parent_Id { get; set; }
        public string Question {get; set; } = string.Empty;
        public short? Bias_Importance {get; set; }
        public short? Bias_AnswerSize {get; set; }
        public DateTime? Bias_Date { get; set; }
        public short? Bias_Weight {get; set; }
        public bool IsActive { get; set; }
    }
}