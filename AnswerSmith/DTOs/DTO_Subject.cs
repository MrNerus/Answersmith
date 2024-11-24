using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnswerSmith.DTOs
{
    public class DTO_Subject_Summary
    {
        public int SN {get; set;} = -1;
        public string Code {get; set;} = String.Empty;
        public string Name {get; set;} = String.Empty;
        public bool IsActive {get; set;} = true;
    }

    public class DTO_Subject
    {
        public int SN {get; set;} = -1;
        public string Code {get; set;} = String.Empty;
        public string Class_Code {get; set;} = String.Empty;
        public string Name {get; set;} = String.Empty;
        public bool IsActive {get; set;} = true;
    }

    public class DTO_Subject_Detail {
        public int SN {get; set;} = -1;
        public string Code {get; set;} = String.Empty;
        public string Class_Code {get; set;} = String.Empty;
        public string Class_Name {get; set;} = String.Empty;
        public string Name {get; set;} = String.Empty;
        public bool IsActive {get; set;} = true;
    }
}