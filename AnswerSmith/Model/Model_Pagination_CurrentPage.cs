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
        public int Rows_Count { get; set; }
        public int Rows_Per_Page { get; set; }
        public int Total_Rows { get; set; }

        public static Model_Pagination_CurrentPage Empty() {
            return new Model_Pagination_CurrentPage {
                Page_Number = 1,
                Max_Page = 1,
                Rows_Count = 0,
                Rows_Per_Page = 20,
                Total_Rows = 0
            };
        }
    }
}