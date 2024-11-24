using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnswerSmith.DTOs;
using AnswerSmith.Enums;
using AnswerSmith.Model;

namespace AnswerSmith.Interfaces
{
    public interface ISubjectService
    {
        public Task<bool> AddSubject(DTO_Subject dto_Subject);
        public Task<bool> EditSubject(DTO_Subject dto_Subject);
        public Task<DTO_Subject_Detail> GetSubject(string subjectCode);
        // public Task<Tuple<List<DTO_Subject_Detail>,Model_Pagination_CurrentPage>> GetSubjects(
        //     string searchKeyword = "%",
        //     int? searchColumn = null,
        //     string filterKeyword = "%",
        //     string? filterColumn = null,
        //     List<Model_WhereClause>? where_Clause = null,
        //     int orderBy = (int) Enum_Class_OrderBy.Name, 
        //     int orderMode = (int) Enum_Any_OrderMode.ASC,
        //     string? orderBy_Clause = null,

        //     int pageNo = 1,
        //     int rowsPerPage = 20
        // );

        public Task<Tuple<List<DTO_Subject_Detail>, Model_Pagination_CurrentPage>> GetSubjects(Model_PaginatedClientRequest clientRequest);

        public Task<List<Model_KeyValue>> GetKeyValuePair(string parentCode);



    }
}