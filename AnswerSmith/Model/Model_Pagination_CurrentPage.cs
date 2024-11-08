using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnswerSmith.Model
{
    public class Model_Pagination_CurrentPage
    {
        public int Page_Number { get; set; }
        public int Max_Page { get; set; }
        public int Data_Count { get; set; }
        public int Total_Count { get; set; }

        public static Model_Pagination_CurrentPage Empty() {
            return new Model_Pagination_CurrentPage {
                Page_Number = 0,
                Max_Page = 0,
                Data_Count = 0,
                Total_Count = 0
            };
        }
    }
}