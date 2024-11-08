using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnswerSmith.DTOs
{
    public class DTO_Marks
    {
        public int Id { get; set; }
        public int QS_Id { get; set; }
        public string QS_Code { get; set; } = string.Empty;
        public float Marks { get; set; } = 0.0f;
        public bool IsActive {get; set; }
    }
}