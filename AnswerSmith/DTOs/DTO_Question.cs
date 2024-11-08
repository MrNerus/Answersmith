using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnswerSmith.DTOs
{
    public class DTO_Question_Summary
    {
        public string Code { get; set; } = string.Empty;
        public string C_Code { get; set; } = string.Empty;
        public string Question {get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
    public class DTO_Question
    {
        public string Code { get; set; } = string.Empty;
        public string C_Code { get; set; } = string.Empty;
        public string? Parent { get; set; } = string.Empty;
        public string? Parent_Code { get; set; } = string.Empty;
        public string? Parent_Name { get; set; } = string.Empty;
        public string Question {get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
    public class DTO_Question_Detail
    {
        public string Code { get; set; } = string.Empty;
        public string C_Code { get; set; } = string.Empty;
        public string? Parent { get; set; } = string.Empty;
        public string? Parent_Code { get; set; } = string.Empty;
        public string? Parent_Name { get; set; } = string.Empty;
        public string Question {get; set; } = string.Empty;
        public short? Bias_Importance {get; set; }
        public short? Bias_AnswerSize {get; set; }
        public DateTime? Bias_Date { get; set; }
        public short? Bias_Weight {get; set; }
        public bool IsActive { get; set; }
    }
}