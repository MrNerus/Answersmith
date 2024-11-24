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
    public class Service_Subject(ISubjectHandler subjectHandler): ISubjectService {
        private readonly ISubjectHandler _subjectHandler = subjectHandler;

        // public async Task<bool> AddClass(DTO_Class dto_Class) {
        //     if (dto_Class.Code.IsNullOrEmpty()) throw new InvalidDataException("Code is Empty");
        //     if (dto_Class.Name.IsNullOrEmpty()) throw new InvalidDataException("name is Empty");

        //     try { return await _subjectHandler.AddClass(dto_Class); }
        //     catch (Exception) { throw; }
        // }

        public async Task<bool> AddSubject(DTO_Subject dto_Subject)
        {
            if (dto_Subject.Class_Code.IsNullOrEmpty()) throw new InvalidDataException("Class Code is empty.");
            if (dto_Subject.Code.IsNullOrEmpty()) throw new InvalidDataException("Subject Code is empty.");
            if (dto_Subject.Name.IsNullOrEmpty()) throw new InvalidDataException("Subject Name is empty.");

            try { return await _subjectHandler.AddSubject(dto_Subject); }
            catch (Exception) { throw; }

        }

        // public async Task<bool> EditClass(DTO_Class dto_Class) {
        //     if (dto_Class.Code.IsNullOrEmpty()) throw new InvalidDataException("Code is Empty");
        //     if (dto_Class.Name.IsNullOrEmpty()) throw new InvalidDataException("name is Empty");

        //     try { return await _subjectHandler.EditClass(dto_Class); }
        //     catch (Exception) { throw; }
        // }

        public async Task<bool> EditSubject(DTO_Subject dto_Subject)
        {
            if (dto_Subject.Class_Code.IsNullOrEmpty()) throw new InvalidDataException("Class Code is empty.");
            if (dto_Subject.Code.IsNullOrEmpty()) throw new InvalidDataException("Subject Code is empty.");
            if (dto_Subject.Name.IsNullOrEmpty()) throw new InvalidDataException("Subject Name is empty.");

            try { return await _subjectHandler.EditSubject(dto_Subject); }
            catch (Exception) { throw; }
        }

        // public async Task<DTO_Class> GetClass(string classCode) {
        //     if (classCode.IsNullOrEmpty()) throw new InvalidDataException("Code is Empty");

        //     try { return await _subjectHandler.GetClass(classCode); }
        //     catch (Exception) { throw; }
        // }

        // public async Task<Tuple<List<DTO_Class>,Model_Pagination_CurrentPage>> GetClasses(
        //     string search_Keyword = "%",
        //     int? search_Column = null,
        //     string filter_Keyword = "%",
        //     string? filter_Column = null,
        //     string? where_clause = null,
        //     int order_By = (int) Enum_Class_OrderBy.Name, 
        //     int order_Mode = (int) Enum_Any_OrderMode.ASC,
        //     string? order_By_Clause = null,

        //     int page_No = 1,
        //     int rows_Per_Page = 20
        // ) {
        //     Enum_Class_OrderBy enum_OrderBy = (Enum_Class_OrderBy) order_By;
        //     Enum_Any_OrderMode enum_OrderMode = (Enum_Any_OrderMode) order_Mode;
        //     try { return await _subjectHandler.GetClasses(
        //         searchKeyword: search_Keyword, 
        //         searchColumn: search_Column, 
        //         filterKeyword: filter_Keyword, 
        //         filterColumn: filter_Column,
        //         where_Clause: where_clause,
        //         orderBy: enum_OrderBy, 
        //         orderMode: enum_OrderMode,
        //         orderBy_Clause: order_By_Clause,

        //         pageNo: page_No,
        //         rowsPerPage: rows_Per_Page
        //         ); 
        //     } catch (Exception) { throw; }
        // }

        public async Task<DTO_Subject_Detail> GetSubject(string subjectCode)
        {
            if (subjectCode.IsNullOrEmpty()) throw new InvalidDataException("Code is Empty");

            try { return await _subjectHandler.GetSubject(subjectCode); }
            catch (Exception) { throw; }
        }

        public async Task<Tuple<List<DTO_Subject_Detail>, Model_Pagination_CurrentPage>> GetSubjects(Model_PaginatedClientRequest clientRequest)
        {
            Dictionary<string,string> KeyMap = new() {
                {"ClassCode", "Class_Code"},
                {"ClassName", "Class_Name"},
                {"ClassStatus", "Class_IsActive"},
                {"Code", "Code"},
                {"Name", "Name"},
                {"Status", "IsActive"},
            };
            Dictionary<string,Dictionary<string,string>> ValueMap = new() {
                {"ClassStatus", new Dictionary<string, string>() {{"active", "1"}, {"inactive", "0"}, {"_", "%"}}},
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

            return await _subjectHandler.GetSubjects(translatedMeaning);
        }

        // public async Task<Tuple<List<DTO_Subject_Detail>, Model_Pagination_CurrentPage>> GetSubjects()
        // {
        //     try { return await _subjectHandler.GetSubjects(); }
        //     catch (Exception) { throw; }
        // }

        public async Task<List<Model_KeyValue>> GetKeyValuePair(string parentCode) {
            if (parentCode.IsNullOrEmpty()) {throw new InvalidDataException("Parent Code is not provided."); }
            try { return await _subjectHandler.GetKeyValuePair(parentCode); }
            catch (Exception) { throw; }
        }


    }
}