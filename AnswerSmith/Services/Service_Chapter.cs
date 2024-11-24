using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AnswerSmith.Data;
using AnswerSmith.DTOs;
using AnswerSmith.Enums;
using AnswerSmith.Interfaces;
using AnswerSmith.Mapper;
using AnswerSmith.Model;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;

namespace AnswerSmith.Services {
    public class Service_Chapter(IChapterHandler chapterHandler): IChapterService {
        private readonly IChapterHandler _chapterHandler = chapterHandler;
        public async Task<bool> AddChapter(DTO_Chapter dto_Chapter)
        {
            if (dto_Chapter.Subject_Code.IsNullOrEmpty()) throw new InvalidDataException("Subject Code is empty.");
            if (dto_Chapter.Code.IsNullOrEmpty()) throw new InvalidDataException("Chapter Code is empty.");
            if (dto_Chapter.Name.IsNullOrEmpty()) throw new InvalidDataException("Chapter Name is empty.");

            try { return await _chapterHandler.AddChapter(dto_Chapter); }
            catch (Exception) { throw; }

        }

        public async Task<bool> EditChapter(DTO_Chapter dto_Chapter)
        {
            if (dto_Chapter.Subject_Code.IsNullOrEmpty()) throw new InvalidDataException("Subject Code is empty.");
            if (dto_Chapter.Code.IsNullOrEmpty()) throw new InvalidDataException("Chapter Code is empty.");
            if (dto_Chapter.Name.IsNullOrEmpty()) throw new InvalidDataException("Chapter Name is empty.");

            try { return await _chapterHandler.EditChapter(dto_Chapter); }
            catch (Exception) { throw; }
        }

        public async Task<DTO_Chapter_Detail> GetChapter(string chapterCode)
        {
            if (chapterCode.IsNullOrEmpty()) throw new InvalidDataException("Code is Empty");

            try { return await _chapterHandler.GetChapter(chapterCode); }
            catch (Exception) { throw; }
        }

        public async Task<Tuple<List<DTO_Chapter_Detail>, Model_Pagination_CurrentPage>> GetChapters(Model_PaginatedClientRequest clientRequest)
        {
            Dictionary<string,string> KeyMap = new() {
                {"ClassCode", "Class_Code"},
                {"ClassName", "Class_Name"},
                {"ClassStatus", "Class_IsActive"},
                {"SubjectCode", "SubjectClass_Code"},
                {"SubjectName", "SubjectClass_Name"},
                {"SubjectStatus", "SubjectClass_IsActive"},
                {"Code", "Code"},
                {"Name", "Name"},
                {"Status", "IsActive"},
            };
            Dictionary<string,Dictionary<string,string>> ValueMap = new() {
                {"ClassStatus", new Dictionary<string, string>() {{"active", "1"}, {"inactive", "0"}, {"_", "%"}}},
                {"SubjectStatus", new Dictionary<string, string>() {{"active", "1"}, {"inactive", "0"}, {"_", "%"}}},
                {"Status", new Dictionary<string, string>() {{"active", "1"}, {"inactive", "0"}, {"_", "%"}}}
            };

            Model_PaginatedClientRequest translatedMeaning = new() {
                Search = clientRequest.Search,
                SearchFilter = (clientRequest.SearchFilter ?? []).Select(x => new Model_WhereClause { 
                    Key = KeyMap.ContainsKey(x.Key) ? KeyMap[x.Key] : x.Key,
                    Value = ValueMap.ContainsKey(x.Key) ? ValueMap[x.Key].ContainsKey(x.Value) ? ValueMap[x.Key][x.Value] : x.Value : x.Value,
                    IsFuzzy = x.IsFuzzy
                }).ToList(),
                Order = (clientRequest.Order ?? []).Select(x => new Model_OrderBy {
                    Column_Name = KeyMap.ContainsKey(x.Column_Name) ? KeyMap[x.Column_Name] : x.Column_Name,
                    Order_Mode = x.Order_Mode,
                    Miscelleneous = x.Miscelleneous
                }).ToList(),
                Pagination = clientRequest.Pagination
            };

            return await _chapterHandler.GetChapters(translatedMeaning);
        }

        public async Task<List<Model_KeyValue>> GetKeyValuePair(string parentCode) {
            if (parentCode.IsNullOrEmpty()) {throw new InvalidDataException("Parent Code is not provided."); }
            try { return await _chapterHandler.GetKeyValuePair(parentCode); }
            catch (Exception) { throw; }
        }


    }
}