using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnswerSmith.DTOs;
using AnswerSmith.Enums;
using AnswerSmith.Model;

namespace AnswerSmith.Interfaces
{
    public interface IClassHandler
    {
        public Task<bool> AddClass(DTO_Class dto_Class);
        public Task<bool> EditClass(DTO_Class dto_Class);
        public Task<DTO_Class> GetClass(string classCode);
        public Task<Tuple<List<DTO_Class>,Model_Pagination_CurrentPage>> GetClasses(
            int pageNo = 1,
            int rowsPerPage = 20,
            string filter = "%", 
            Enum_Class_OrderBy orderBy = Enum_Class_OrderBy.Name, 
            Enum_Any_OrderMode orderMode = Enum_Any_OrderMode.ASC
        );
    }
}