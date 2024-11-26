using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnswerSmith.DTOs
{
    public class DTO_Chapter_Summary
    {
        public int SN {get; set;} = -1;
        public string Code {get; set;} = String.Empty;
        public string Name {get; set;} = String.Empty;
        public bool IsActive {get; set;} = true;
    }

    public class DTO_Chapter
    {
        public int SN {get; set;} = -1;
        public string Code {get; set;} = String.Empty;
        public string Subject_Code {get; set;} = String.Empty;
        public string Name {get; set;} = String.Empty;
        public bool IsActive {get; set;} = true;
    }

    public class DTO_Chapter_Detail {
        public int SN {get; set;} = -1;
        public string Code {get; set;} = String.Empty;
        public string Subject_Code {get; set;} = String.Empty;
        public string Subject_Name {get; set;} = String.Empty;
        public string Class_Code {get; set;} = String.Empty;
        public string Class_Name {get; set;} = String.Empty;
        public string Name {get; set;} = String.Empty;
        public bool IsActive {get; set;} = true;
    }
}