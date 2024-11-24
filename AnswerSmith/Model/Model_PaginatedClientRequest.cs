using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnswerSmith.Enums;

namespace AnswerSmith.Model
{
    public class Model_PaginatedClientRequest {
        public string? Search {get; set;} = string.Empty;
        public List<Model_WhereClause>? SearchFilter {get; set;}
        public List<Model_OrderBy>? Order {get; set;}
        public Model_PageRequest Pagination {get; set;} = new();

    }
}