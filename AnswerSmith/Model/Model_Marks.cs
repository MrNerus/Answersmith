using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnswerSmith.Model {
    public class Model_Marks {
        public int Id { get; set; }
        public int QS_Id { get; set; }
        public float Marks { get; set; } = 0.0f;
        public bool IsActive {get; set; }
    }
}