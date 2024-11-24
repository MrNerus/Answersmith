using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnswerSmith.DTOs;
using AnswerSmith.Enums;
using AnswerSmith.Model;

namespace AnswerSmith.Interfaces
{
    public interface IChapterHandler
    {
        public Task<bool> AddChapter(DTO_Chapter dTO_Chapter);
        public Task<bool> EditChapter(DTO_Chapter dTO_Chapter);
        public Task<DTO_Chapter_Detail> GetChapter(string chapterCode);

        public Task<Tuple<List<DTO_Chapter_Detail>, Model_Pagination_CurrentPage>> GetChapters(Model_PaginatedClientRequest paginationRequest);

        public Task<List<Model_KeyValue>> GetKeyValuePair(string parentCode);

    }
}